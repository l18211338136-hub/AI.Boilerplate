using ImageMagick;
using FluentStorage.Blobs;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.SignalR;
using AI.Boilerplate.Server.Api.Infrastructure.SignalR;
using AI.Boilerplate.Server.Api.Features.Identity;
using AI.Boilerplate.Server.Api.Features.Identity.Models;
using AI.Boilerplate.Shared.Features.Attachments;
using AI.Boilerplate.Server.Api.Infrastructure.Services;

namespace AI.Boilerplate.Server.Api.Features.Attachments;

[ApiController]
[ApiVersion(1)]
[Route("api/v{v:apiVersion}/[controller]/[action]")]
public partial class AttachmentController : AppControllerBase, IAttachmentController
{
    [AutoInject] private IBlobStorage blobStorage = default!;
    [AutoInject] private UserManager<User> userManager = default!;

    [AutoInject] private IServiceProvider serviceProvider = default!;
    [AutoInject] private ILogger<AttachmentController> logger = default!;

    [AutoInject] private IHubContext<AppHub> appHubContext = default!;

    [AutoInject] private ResponseCacheService responseCacheService = default!;

    [AutoInject] private IConfiguration configuration = default!;

    // For open telemetry metrics
    private static readonly Histogram<double> updateResizeDurationHistogram = Meter.Current.CreateHistogram<double>("attachment.resize_duration", "ms", "Elapsed time to resize and persist an uploaded image");

    [HttpPost]
    [RequestSizeLimit(11 * 1024 * 1024 /*11MB*/)]
    public async Task<IActionResult> UploadUserProfilePicture(IFormFile? file, CancellationToken cancellationToken)
    {
        return await UploadAttachment(
             User.GetUserId(),
             [AttachmentKind.UserProfileImageSmall, AttachmentKind.UserProfileImageOriginal],
             file,
             cancellationToken);
    }

    [HttpPost("{productId}")]
    [RequestSizeLimit(11 * 1024 * 1024 /*11MB*/)]
    public async Task<IActionResult> UploadProductPrimaryImage(Guid productId, IFormFile? file, CancellationToken cancellationToken)
    {
        return await UploadAttachment(
            productId,
            [AttachmentKind.ProductPrimaryImageMedium, AttachmentKind.ProductPrimaryImageOriginal],
            file,
            cancellationToken);
    }

    [AllowAnonymous]
    [HttpGet("{attachmentId}/{kind}")]
    [AppResponseCache(MaxAge = 3600 * 24 * 7, UserAgnostic = true)]
    public async Task<IActionResult> GetAttachment(Guid attachmentId, AttachmentKind kind, CancellationToken cancellationToken = default)
    {
        var filePath = GetFilePath(attachmentId, kind);

        if (await blobStorage.ExistsAsync(filePath, cancellationToken) is false)
            throw new ResourceNotFoundException();

        var mimeType = kind switch
        {
            _ => "image/webp" // Currently, all attachment types are images.
        };

        return File(await blobStorage.OpenReadAsync(filePath, cancellationToken), mimeType, enableRangeProcessing: true);
    }

    [HttpDelete]
    public async Task DeleteUserProfilePicture(CancellationToken cancellationToken)
    {
        await DeleteAttachment(User.GetUserId(), [AttachmentKind.UserProfileImageSmall, AttachmentKind.UserProfileImageOriginal], cancellationToken);
    }

    [HttpDelete("{productId}"), Authorize(Policy = AppFeatures.AdminPanel.ManageProductCatalog)]
    public async Task DeleteProductPrimaryImage(Guid productId, CancellationToken cancellationToken)
    {
        await DeleteAttachment(productId, [AttachmentKind.ProductPrimaryImageMedium, AttachmentKind.ProductPrimaryImageOriginal], cancellationToken);
    }

    private async Task PublishUserProfileUpdated(User user, CancellationToken cancellationToken)
    {
        // Notify other sessions of the user that user's info has been updated, so they'll update their UI.
        var currentUserSessionId = User.GetSessionId();
        var userSessionIdsExceptCurrentUserSessionId = await DbContext.UserSessions
            .Where(us => us.UserId == user.Id && us.Id != currentUserSessionId && us.SignalRConnectionId != null)
            .Select(us => us.SignalRConnectionId!)
            .ToArrayAsync(cancellationToken);
        await appHubContext.Clients.Clients(userSessionIdsExceptCurrentUserSessionId).Publish(SharedAppMessages.PROFILE_UPDATED, user.Map(), cancellationToken);
    }

    private async Task DeleteAttachment(Guid attachmentId, AttachmentKind[] kinds, CancellationToken cancellationToken)
    {
        var attachments = await DbContext.Attachments.Where(p => p.Id == attachmentId && p.Kind != null && kinds.Contains(p.Kind.Value)).ToArrayAsync(cancellationToken);

        foreach (var attachment in attachments)
        {
            var filePath = attachment.Path;

            if (await blobStorage.ExistsAsync(filePath, cancellationToken) is false)
                throw new ResourceNotFoundException(Localizer[nameof(AppStrings.ImageCouldNotBeFound)]);

            await blobStorage.DeleteAsync(filePath, cancellationToken);

            if (attachment.Kind is AttachmentKind.ProductPrimaryImageOriginal)
            {
                var product = await DbContext.Products.FindAsync([attachment.Id], cancellationToken);
                if (product is not null) // else means product is being added to the database.
                {
                    product.HasPrimaryImage = false;
                    product.PrimaryImageAltText = null;
                    await DbContext.SaveChangesAsync(cancellationToken);
                    await responseCacheService.PurgeProductCache(product.ShortId ?? 0);
                }
            }

            if (attachment.Kind is AttachmentKind.UserProfileImageOriginal)
            {
                var user = await userManager.FindByIdAsync(User.GetUserId().ToString());
                user!.HasProfilePicture = false;

                var result = await userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    throw new ResourceValidationException(result.Errors.Select(err => new LocalizedString(err.Code, err.Description)).ToArray());

                await PublishUserProfileUpdated(user, cancellationToken);
            }

            DbContext.Attachments.Remove(attachment);
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task<IActionResult> UploadAttachment(Guid attachmentId, AttachmentKind[] kinds, IFormFile? file, CancellationToken cancellationToken)
    {
        if (file is null)
            throw new BadRequestException();

        string? altText = null; // For future use, e.g., AI-generated alt text.

        await DbContext.Attachments.Where(att => att.Id == attachmentId).ExecuteDeleteAsync(cancellationToken);

        foreach (var kind in kinds)
        {
            var attachment = new Attachment
            {
                Id = attachmentId,
                Kind = kind,
                Path = GetFilePath(attachmentId, kind, file.FileName),
            };

            if (await blobStorage.ExistsAsync(attachment.Path, cancellationToken))
            {
                await blobStorage.DeleteAsync(attachment.Path, cancellationToken);
            }

            (bool NeedsResize, uint Width, uint Height) imageResizeContext = kind switch
            {
                AttachmentKind.UserProfileImageSmall => (true, 256, 256),
                AttachmentKind.ProductPrimaryImageMedium => (true, 512, 512),
                _ => (false, 0, 0)
            };

            byte[]? imageBytes = null;

            if (imageResizeContext.NeedsResize)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                using MagickImage sourceImage = new(file.OpenReadStream());

                if (sourceImage.Width < imageResizeContext.Width || sourceImage.Height < imageResizeContext.Height)
                    return BadRequest(Localizer[nameof(AppStrings.ImageTooSmall), imageResizeContext.Width, imageResizeContext.Height, sourceImage.Width, sourceImage.Height].ToString());

                sourceImage.Resize(new MagickGeometry(imageResizeContext.Width, imageResizeContext.Height));

                await blobStorage.WriteAsync(attachment.Path, imageBytes = sourceImage.ToByteArray(MagickFormat.WebP), cancellationToken: cancellationToken);

                updateResizeDurationHistogram.Record(stopwatch.Elapsed.TotalMilliseconds, new KeyValuePair<string, object?>("kind", kind.ToString()));
            }
            else
            {
                await blobStorage.WriteAsync(attachment.Path, file.OpenReadStream(), cancellationToken: cancellationToken);
            }

            await DbContext.Attachments.AddAsync(attachment, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);

            if (attachment.Kind is AttachmentKind.ProductPrimaryImageMedium)
            {
                if (serviceProvider.GetService<IChatClient>() is IChatClient chatClient)
                {
                    var imageAnalysisAgent = chatClient.AsAIAgent(
                        instructions: """
                    你是一个产品图像专家助手。你的职责是分析用于电子商务目录的任意产品图像。

                    分析流程：
                    1. 首先，仔细检查图像内容与质量
                    2. 确定图像是否展示了一个清晰、可识别的商业产品（非人物/风景/抽象图等）
                    3. 如果是有效产品，提供详细的、对 SEO 友好的中文描述，并提取关键属性（如品类、颜色、材质、适用场景等）
                    4. 如果不符合目录要求，清晰说明具体原因（如主体模糊、非商品、违规内容等）

                    响应格式：
                    只返回一个包含以下内容的 JSON 对象，并且所有文本字段必须使用中文返回：
                    - "isValidProduct": 布尔值（如果图像展示的是符合目录标准的有效产品则为 true，否则为 false）
                    - "confidence": 0-1 之间的数字，表示分类的确信度
                    - "alt": 字符串，用于辅助功能和 SEO 的详细中文描述（如：「黑色真皮商务双肩包，多隔层设计，适合通勤与短途旅行」）
                    - "reasoning": 字符串，简要解释你的分析决定的中文说明
                    - "suggestedCategory": 字符串，建议的产品一级分类（如「箱包」「数码配件」「家居用品」等，仅在 isValidProduct 为 true 时提供）

                    验证规则：
                    - 图像质量必须清晰，主体产品占比合理，无严重遮挡/模糊/水印
                    - 产品必须是可销售的实物商品，排除虚拟服务、人物肖像、纯文字图片等
                    - 描述需客观中立，避免主观评价或营销话术
                    """,
                        name: "ProductImageAnalystAgent",
                        description: "分析任意产品图像，确保其符合电商目录的质量、内容与合规标准");

                    ChatOptions chatOptions = new()
                    {
                        ResponseFormat = ChatResponseFormat.Json,
                        AdditionalProperties = new()
                        {
                            ["response_format"] = new { type = "json_object" }
                        }
                    };

                    configuration.GetRequiredSection("AI:ChatOptions").Bind(chatOptions);
                    var visionModelId = configuration["AI:OpenAI:VisionModel"] ?? configuration["AI:AzureOpenAI:VisionModel"];
                    if (!string.IsNullOrWhiteSpace(visionModelId))
                    {
                        chatOptions.ModelId = visionModelId;
                    }

                    var visionEndpoint = configuration["AI:OpenAI:VisionEndpoint"] ?? configuration["AI:AzureOpenAI:VisionEndpoint"];
                    if (!string.IsNullOrWhiteSpace(visionEndpoint))
                    {
                        chatOptions.AdditionalProperties ??= new();
                        chatOptions.AdditionalProperties["Endpoint"] = new Uri(visionEndpoint);
                    }

                    Console.WriteLine($"\n[AI VisionModel Calling]: {nameof(UploadAttachment)} | Model: {chatOptions.ModelId ?? "default"} | Endpoint: {visionEndpoint ?? "default"}");

                    var response = await imageAnalysisAgent.RunAsync<AIImageReviewResponse>(
                        messages: [
                            new ChatMessage(ChatRole.User,
                            "请为我们的电商目录分析此产品图像。这是一个符合质量和内容标准的有效商品图片吗？")
                        {
                            Contents = [new DataContent(imageBytes, "image/webp")]
                        }
                        ],
                        cancellationToken: cancellationToken,
                        options: new Microsoft.Agents.AI.ChatClientAgentRunOptions(chatOptions));

                    if (response.Result.IsValidProduct is false)
                    {
                        logger.LogWarning(
                            "Image validation failed - Not a valid product. Confidence: {Confidence}, Reasoning: {Reasoning}, SuggestedCategory: {Category}",
                            response.Result.Confidence,
                            response.Result.Reasoning,
                            response.Result.SuggestedCategory);
                        return BadRequest(Localizer[nameof(AppStrings.ImageNotValidProductError)].ToString());
                    }

                    if (response.Result.Confidence < 0.85)
                    {
                        logger.LogWarning(
                            "Image analysis low confidence ({Confidence}). Reasoning: {Reasoning}. Alt text: {AltText}. Category: {Category}",
                            response.Result.Confidence,
                            response.Result.Reasoning,
                            response.Result.Alt,
                            response.Result.SuggestedCategory);
                    }

                    altText = response.Result.Alt;

                    // 可选：记录建议的分类，用于后续自动归类
                    if (!string.IsNullOrEmpty(response.Result.SuggestedCategory))
                    {
                        logger.LogInformation(
                            "AI suggested category for product image: {Category} (confidence: {Confidence})",
                            response.Result.SuggestedCategory,
                            response.Result.Confidence);
                    }
                }
            }

            if (kind is AttachmentKind.UserProfileImageSmall)
            {
                var user = await userManager.FindByIdAsync(User.GetUserId().ToString());
                user!.HasProfilePicture = true;

                var result = await userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    throw new ResourceValidationException(result.Errors.Select(err => new LocalizedString(err.Code, err.Description)).ToArray());

                await PublishUserProfileUpdated(user, cancellationToken);
            }
        }

        return Ok(altText);
    }

    private string GetFilePath(Guid attachmentId, AttachmentKind kind, string? fileName = null)
    {
        var filePath = kind switch
        {
            AttachmentKind.ProductPrimaryImageMedium => $"{AppSettings.ProductImagesDir}{attachmentId}_{kind}.webp",
            AttachmentKind.ProductPrimaryImageOriginal => $"{AppSettings.ProductImagesDir}{attachmentId}_{kind}{Path.GetExtension(fileName)}",
            AttachmentKind.UserProfileImageSmall => $"{AppSettings.UserProfileImagesDir}{attachmentId}_{kind}.webp",
            AttachmentKind.UserProfileImageOriginal => $"{AppSettings.UserProfileImagesDir}{attachmentId}_{kind}{Path.GetExtension(fileName)}",
            _ => throw new NotImplementedException()
        };

        filePath = Environment.ExpandEnvironmentVariables(filePath);

        return filePath;
    }

    public record AIImageReviewResponse(bool IsValidProduct, double Confidence, string? Alt, string? Reasoning, string? SuggestedCategory = null);
}
