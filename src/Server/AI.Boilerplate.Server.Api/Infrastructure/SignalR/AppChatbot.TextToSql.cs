﻿using System.ComponentModel;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;
using AI.Boilerplate.Server.Api.Infrastructure.Services;
using ModelContextProtocol.Server;
using Npgsql;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace AI.Boilerplate.Server.Api.Infrastructure.SignalR;

public partial class AppChatbot
{
    private static readonly System.Text.Json.JsonSerializerOptions _jsonSerializerOptions = new()
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    [Description("将自然语言报表需求转换为SQL并执行。仅执行只读 SELECT/WITH；失败时自动带错误上下文重试一次。注意：如果用户明确要求“可视化”、“图表”、“图”、“界面”或“生成报表界面”，【绝对不要】调用此工具，必须直接调用 `PgGenerateDashboard`！拿到执行结果后，你必须将执行结果的所有列和数据原样使用 markdown 表格展示给用户，并将表格的列名（表头）翻译为易懂的中文。绝对不要擅自总结、解释、截断或篡改任何字段的数据值！必须严格按照返回的 JSON 里的 rows 字段的值来展示！如果数据超过了返回限制，请在末尾提示用户“数据已截断”。不要输出其他多余的自然语言。")]
    [McpServerTool(Name = nameof(PgTextToSqlReport))]
    private async Task<string> PgTextToSqlReport(
        [Required, Description("报表需求自然语言描述，例如：统计近30天各分类产品数量")] string reportRequirement,
        [Description("最大返回行数。除非用户指定，否则不要传此参数")] int limit = 1000)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(PgTextToSqlReport)}");
        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            var userId = await EnsureCurrentUserIdIsPresent();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var connection = await OpenAppDbConnectionAsync(db, CancellationToken.None);
            //var schemaSummary = await BuildSchemaSummaryAsync(connection, CancellationToken.None);
            var schemaSummary = await RetrieveSchemaContextAsync(reportRequirement, CancellationToken.None);

            var firstSql = await GenerateTextToSqlAsync(reportRequirement, schemaSummary, userId, null, CancellationToken.None);

            try
            {
                var firstResult = await ExecuteReportQueryAsync(connection, firstSql, limit, CancellationToken.None);
                var jsonResult = JsonSerializer.Serialize(new
                {
                    sql = firstResult.sql,
                    retry = false,
                    total = firstResult.total,
                    rows = firstResult.rows
                }, _jsonSerializerOptions);
                Console.WriteLine($"\n[PgTextToSqlReport Result]: {jsonResult}");
                return $"【系统强制指令：查询成功！】请严格根据以下 JSON 数据(rows)原样生成 markdown 表格。绝对不允许擅自联想、伪造、翻译或篡改任何字段的**数据值**！\n{jsonResult}";
            }
            catch (Exception firstExp)
            {
                Console.WriteLine($"\n[PgTextToSqlReport Execution Failed, Retrying...]: {firstExp.Message}");
                var secondSql = await GenerateTextToSqlAsync(reportRequirement, schemaSummary, userId, firstExp.Message, CancellationToken.None);
                var secondResult = await ExecuteReportQueryAsync(connection, secondSql, limit, CancellationToken.None);

                var jsonResult = JsonSerializer.Serialize(new
                {
                    sql = secondResult.sql,
                    retry = true,
                    previousError = firstExp.Message,
                    total = secondResult.total,
                    rows = secondResult.rows
                }, _jsonSerializerOptions);
                Console.WriteLine($"\n[PgTextToSqlReport Result (Retry)]: {jsonResult}");
                return $"【系统强制指令：查询成功！】(经过自动重试后生效) 请严格根据以下 JSON 数据(rows)原样生成 markdown 表格。绝对不允许擅自联想、伪造、翻译或篡改任何字段的**数据值**！\n{jsonResult}";
            }
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return JsonSerializer.Serialize(new { error = exp.Message }, _jsonSerializerOptions);
        }
    }

    [Description("根据自然语言需求，查询数据库并自动生成交互式单文件 HTML 数据大屏（仪表盘）。当用户要求“生成图表”、“可视化”、“报表页面”等需求时，【必须优先且仅调用此工具】。拿到执行结果后，请将生成的 HTML 代码块原样反馈给用户，不要输出多余解释。")]
    [McpServerTool(Name = nameof(PgGenerateDashboard))]
    private async Task<string> PgGenerateDashboard(
        [Required, Description("报表需求自然语言描述，例如：统计各分类产品数量及价格分布")] string requirement,
        [Description("提取用户关于页面长相的所有视觉描述词，严禁遗漏。输出必须包含以下三类，且保留原文中的修饰细节：1.视觉风格：提取整体氛围（如：赛博朋克、科技感）及具体的视觉修饰词（如：霓虹、全息、透明、磨砂等）。2.布局与形态：提取整体架构（如：大屏、仪表盘）及组件的具体形状（如：六边形、圆形、卡片、网格、悬浮等）。注意：如果用户指定了特殊形状（如六边形等），必须原样提取，不可忽略！3.动画特效：提取所有动态描述（如：流光、过渡动画、展开动画、波形）。输出规则：请将所有提取到的关键词用逗号拼接;保持原文用词（例如用户说“霓虹波形”，就提取“霓虹波形”等）；若无相关描述，返回空字符串。")] string? visualStyle = null,
        [Description("最大返回行数。除非用户指定，否则不要传此参数")] int limit = 1000)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(PgGenerateDashboard)}");
        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            var userId = await EnsureCurrentUserIdIsPresent();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var connection = await OpenAppDbConnectionAsync(db, CancellationToken.None);
            //var schemaSummary = await BuildSchemaSummaryAsync(connection, CancellationToken.None);
            var schemaSummary = await RetrieveSchemaContextAsync(requirement, CancellationToken.None);

            var firstSql = await GenerateTextToSqlAsync(requirement, schemaSummary, userId, null, CancellationToken.None);
            
            string jsonResult;
            try
            {
                var firstResult = await ExecuteReportQueryAsync(connection, firstSql, limit, CancellationToken.None);
                jsonResult = JsonSerializer.Serialize(firstResult.rows, _jsonSerializerOptions);
            }
            catch (Exception firstExp)
            {
                Console.WriteLine($"\n[PgGenerateDashboard Execution Failed, Retrying...]: {firstExp.Message}");
                var secondSql = await GenerateTextToSqlAsync(requirement, schemaSummary, userId, firstExp.Message, CancellationToken.None);
                var secondResult = await ExecuteReportQueryAsync(connection, secondSql, limit, CancellationToken.None);
                jsonResult = JsonSerializer.Serialize(secondResult.rows, _jsonSerializerOptions);
            }

            // 生成 HTML 时，将风格描述拼接到提示词中
            var htmlPrompt = string.IsNullOrWhiteSpace(visualStyle)
                ? requirement
                : $"{requirement}。视觉风格要求：{visualStyle}";

            var html = await GenerateDashboardHtmlAsync(htmlPrompt, jsonResult, CancellationToken.None);
            return $"```html\n{html}\n```";
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return JsonSerializer.Serialize(new { error = exp.Message }, _jsonSerializerOptions);
        }
    }

    private async Task<string> GenerateDashboardHtmlAsync(string requirement, string dataJson, CancellationToken cancellationToken)
    {
        if (serviceProvider.GetService<IChatClient>() is not IChatClient chatClient)
            throw new ResourceNotFoundException("IChatClient is not available.");

        var systemPrompt = """
            你是一位拥有10年经验的数据可视化专家和高级前端工程师，擅长将枯燥的数据转化为极具视觉冲击力的交互式仪表盘。 
            你的核心任务是根据用户输入的自然语义需求和提供的 JSON 数据，编写一个单文件的、自包含的 HTML 代码。

            【🔴 极其重要的强制约束 🔴】
            1. **图表容器高度强制约束**：必须为每一个 ECharts 图表容器（div）设置明确的内联宽度和高度！例如 `<div id="main-chart" style="width: 100%; height: 400px; min-height: 400px;"></div>`。如果你不设置具体高度，ECharts 图表将渲染为 0 像素高，导致页面完全看不到图表！
            2. **严禁 Blazor 脚本**：绝对禁止在图表中引入任何和 Blazor 相关的脚本。
            3. **图标库引入规范**：如果需要引入图标库，必须使用 `<link rel="stylesheet">` 引入 CSS 文件，绝对不要用 `<script>` 引入 CSS 文件！
            4. **窗口自适应**：必须在 `window.onresize` 时调用所有 echart 实例的 `resize()` 方法以确保自适应。
            5. **数据真实性与动态解析（绝对强制）**：绝对不能在生成的 HTML 代码中通过硬编码伪造数据（如写死假数字 `$10,000,000` 或假名称 `BMW`）。你必须在生成的 `<script>` 中将提供的原始 JSON 数据原样声明为一个 JavaScript 变量（例如 `const rawData = [真实的JSON数据];`），然后通过 JavaScript 动态计算并渲染出总数、总金额等指标卡片内容，并通过 DOM 操作（如 `document.getElementById('total-amount').innerText = ...`）写入卡片。同理，ECharts 的 `xAxis.data` 和 `series.data` 也必须通过对 `rawData` 的动态解析生成。请确保最终图表和卡片显示的数据与传入的 JSON 完全一致！

            【🛠️ 技术栈规范】
            *   **核心框架**：原生 HTML5。
            *   **样式系统**：必须使用 Tailwind CSS (通过 CDN 引入)，用于快速构建现代化的响应式布局、卡片式设计、阴影和排版。请直接在 HTML 标签上使用 Tailwind 的 class（例如 `<body class="bg-gray-900 text-white">`），**绝对不要在 `<style>` 中手写无效的 CSS 伪类（例如 `background-color: bg-gray-900;` 是错误的！）**。
            *   **可视化引擎**：必须使用 Apache ECharts (通过 CDN 引入)，因为它提供了丰富的图表类型和炫酷的交互效果。如果需要 3D 效果，请额外引入 `echarts-gl`。
            *   **图标库**：使用 FontAwesome 或 RemixIcon (CDN) 增强 UI 细节。

            【🎨 视觉与交互要求】
            **设计风格与用户自定义**：
            *   默认情况下，页面推荐使用“高级感、科技感或赛博朋克风”的**深色模式（Dark Mode）**，使用深邃的暗色背景（必须直接使用 tailwind 类，如 `<body class="bg-gray-900 text-white min-h-[100vh]">`，绝对不要使用 `min-h-screen;` 这种错误的 CSS 语法）。
            *   **【极其重要】：如果用户在需求中明确指定了其他风格（如“极简风”、“明亮模式”、“可爱风”、“手绘风”等），必须无条件遵循用户的指定风格！** 此时请灵活调整背景色、字体颜色、卡片样式和 ECharts 的配色方案。
            *   在科技感/深色模式下，建议使用半透明的**玻璃拟态（Glassmorphism）**效果，搭配细腻的边框高光和背景网格/噪点纹理。卡片使用带透明度的背景（如 `class="bg-white/10 backdrop-blur-md border border-white/20 rounded-xl shadow-2xl"`）。

            **布局逻辑**：
            *   **顶部**：醒目的报表标题和关键指标卡片（可以带有图标）。如果数据中有最大、最小、平均、总和等维度，应当优先在这里使用数字指标卡片展示，排成一排或多列。
            *   **主体**：采用栅格布局，根据数据维度自动规划图表位置。布局不要使用太挤的栅格，可以考虑使用一列或两列布局，并为每个图表卡片增加足够的内边距(`p-6`)和外边距(`mb-6`)。
            *   **图表样式**：所有图表必须开启 ToolTip 提示框，支持数据缩放，并具备鼠标悬停时的动画反馈。
            *   **3D 效果与动画**：强烈建议在 ECharts 配置中使用 3D 图表（如 `bar3D`, `scatter3D`, `pie3D` 等）或者通过 `itemStyle` 配置动态渐变色、阴影特效（如 `shadowBlur: 20, shadowColor: 'rgba(0, 255, 255, 0.8)'`）来营造强烈的 3D 动画与科幻效果。除非用户明确要求不要 3D 或特效。

            **智能图表映射逻辑**：
            *   **时间序列数据** → 带有渐变色填充的**折线图**或**面积图**（Area）。
            *   **分类对比数据** → 带有 3D 效果或**圆角**的柱状图（Bar），使用动态渐变色。
            *   **占比数据** → 带有 3D 质感的**环形图**（Doughnut）或 南丁格尔玫瑰图（Pie）。
            *   **关系/分布数据** → 3D 散点图或热力图。
            *   **长文本处理**：如果数据项名称过长，请配置 X 轴标签倾斜（`rotate: -30`）或换行展示。

            **【极致高级排版与赛博朋克动画增强（核心要求）】**：
            1.  **动态星空/粒子背景（强制要求）**：绝对不能只用纯色背景！你**必须**使用 `particles.js` 生成动态粒子背景。
                *   在 `<body>` 第一行添加 `<div id="particles-js" class="fixed inset-0 z-0 pointer-events-none"></div>`。
                *   主内容容器必须加上 `relative z-10` 以确保显示在粒子上方，例如 `<div class="relative z-10 min-h-screen p-4 md:p-8">`。
                *   背景颜色使用深色渐变，例如 `body` 加上 `class="bg-gradient-to-br from-gray-900 via-[#0b0c10] to-black text-white overflow-x-hidden"`。
                *   在 `<script>` 中必须包含粒子初始化代码：`particlesJS("particles-js", {"particles":{"number":{"value":80},"color":{"value":"#00ffff"},"shape":{"type":"circle"},"opacity":{"value":0.5},"size":{"value":3},"line_linked":{"enable":true,"distance":150,"color":"#00ffff","opacity":0.4,"width":1},"move":{"enable":true,"speed":2}},"interactivity":{"events":{"onhover":{"enable":true,"mode":"repulse"}}}});`。
            2.  **玻璃拟态与全息投影边框**：所有卡片必须带有极其强烈的毛玻璃质感（`backdrop-blur-2xl bg-white/5` 或 `bg-gradient-to-b from-white/10 to-transparent`），同时外层包裹一层霓虹发光边框（`border border-cyan-500/50 shadow-[0_0_20px_rgba(0,255,255,0.3)]`），当鼠标悬停时（`hover:`），发光强度和卡片透明度要发生平滑且夸张的过渡变化（`transition-all duration-500 hover:shadow-[0_0_40px_rgba(0,255,255,0.6)]`），制造全息投影的交互感。
            3.  **赛博朋克风数字指标卡片**：指标卡片（例如总数、总金额）绝对不能干瘪。必须设计为独立的悬浮小卡片，排成网格（`grid-cols-2 md:grid-cols-4 gap-6`）。数字必须极其巨大（`text-6xl` 或 `text-7xl`），并且应用动态的渐变流光文字特效（`bg-clip-text text-transparent bg-gradient-to-r from-cyan-400 via-purple-500 to-pink-500`），最好配有 CSS 关键帧动画让数字带有“呼吸灯”效果或轻微浮动效果。
            4.  **ECharts 图表究极美化与发光**：
                *   **背景透明**：`backgroundColor: 'transparent'`。
                *   **数据列科幻发光（核心）**：柱状图、折线图、饼图必须在 `itemStyle` 中配置极其强烈的发光特效！例如 `shadowBlur: 50`, `shadowColor: 'rgba(0, 255, 255, 1)'`。这是体现科技感的灵魂！
                *   **折线图动态流光**：如果是折线图，必须开启 `smooth: true`，线条本身要细且极度发光，同时使用 `areaStyle` 填充带有高透明度渐变（如从 0.8 到 0）的色彩到底部。强烈建议开启 `animationDuration: 3000`, `animationEasing: 'cubicOut'` 和带有轨迹感的动画配置。
                *   **柱状图3D圆角与霓虹渐变**：柱状图必须配置圆角 `itemStyle: { borderRadius: [12, 12, 0, 0] }`，并且每个柱子必须是从深紫到高亮青色的线性霓虹渐变（`LinearGradient`）。
                *   **深色系网格线**：坐标轴的分割线（`splitLine`）必须是极细的 `rgba(255,255,255,0.05)` 虚线。
                *   **ToolTip 赛博化**：把 ToolTip 的背景也设置为半透明玻璃拟态，边框为青色发光。

            【🚀 执行步骤】
            1.  **分析数据**：理解数据的结构、字段含义及数据之间的关系。
            2.  **规划布局**：设计一个充满未来感的 HTML 结构。
            3.  **编写代码**：
                *   在 `<head>` 中必须精确引入以下 CDN 链接（绝不能错）：
                    - Tailwind CSS: `<script src="https://cdn.tailwindcss.com"></script>`
                    - ECharts: `<script src="https://cdn.jsdelivr.net/npm/echarts@5.5.0/dist/echarts.min.js"></script>`
                    - Particles.js: `<script src="https://cdn.jsdelivr.net/particles.js/2.0.0/particles.min.js"></script>`
                    - (如果使用了 3D 图表) ECharts-GL: `<script src="https://cdn.jsdelivr.net/npm/echarts-gl@2.0.9/dist/echarts-gl.min.js"></script>`
                *   在 `<style>` 中，务必手写一段极其炫酷的 CSS 关键帧动画（Keyframes），比如背景流光、卡片浮动呼吸、或者边框霓虹灯跑马灯效果，并应用到主要的容器上。请确保所有 CSS 都是合法且原生的 CSS3 语法，**绝对不要在 `<style>` 标签中混入 Tailwind 的工具类（例如不要写出 `min-h-screen;` 或 `backdrop-blur-2xl` 这种非法 CSS）**。
                *   在 `<body>` 中编写语义化的 HTML 结构，大量使用 Tailwind 的毛玻璃、渐变和发光 class。注意：Tailwind v3 的所有 JIT 语法（如 `bg-[#0b0c10]`, `shadow-[0_0_20px_#0ff]`）都可以直接使用！请确保 `<body class="...">` 中包含 `bg-black` 或深色渐变等能够明确覆盖背景颜色的 class，以免出现白色背景导致白色文字看不见的问题。
                *   **【极其重要】**：所有自定义的 JavaScript 代码（包括 particlesJS 和 ECharts 的初始化代码）必须放在 `</body>` 标签之前，或者包裹在 `window.onload = function() { ... }` 中，以确保图表容器和粒子容器 DOM 元素已经完全加载！
                *   初始化 ECharts 时配置极其炫酷、带有伪 3D 质感或科幻感的 `option` 参数（大量使用 `linearGradient`、阴影 `shadowBlur` 等）。
                *   **【极其重要】**：严禁在未引入 `echarts-gl` 时使用带有 `3D` 后缀的图表类型（如 `bar3D`）！强烈建议直接使用普通的 2D 图表，通过 `itemStyle` 配置极其夸张的渐变色和阴影特效来**模拟出极品科幻发光质感**！

            【✅ 输出约束】
            *   必须让页面看起来极其昂贵、充满未来科技感、并且充满流畅的 CSS 或 JS 动画！
            *   严禁使用外部 CSS/JS 文件，所有代码必须内联或在 CDN 中获取。
            *   确保代码没有语法错误，且在不同分辨率下均能自适应显示。
            *   如果数据量较大，请自动添加滚动条或分页样式。
            *   默认配色方案应和谐统一，避免使用高饱和度的刺眼颜色，推荐使用科技蓝、紫罗兰或霓虹色系。
            *   **仅输出 HTML 代码**，绝对不要包含 markdown 代码块符号 (```html) 或任何其他解释性文字。
            """;

        var prompt = $"""
            用户需求：
            {requirement}

            查询到的数据 (JSON 格式)：
            {dataJson}
            """;

        ChatOptions chatOptions = new()
        {
            ResponseFormat = ChatResponseFormat.Text,
            MaxOutputTokens = 8192
        };
        configuration.GetRequiredSection("AI:ChatOptions").Bind(chatOptions);
        var coderModelId = configuration["AI:OpenAI:CoderModel"] ?? configuration["AI:AzureOpenAI:CoderModel"];
        if (!string.IsNullOrWhiteSpace(coderModelId))
        {
            chatOptions.ModelId = coderModelId;
        }

        var coderEndpoint = configuration["AI:OpenAI:CoderEndpoint"] ?? configuration["AI:AzureOpenAI:CoderEndpoint"];
        if (!string.IsNullOrWhiteSpace(coderEndpoint))
        {
            chatOptions.AdditionalProperties ??= new();
            chatOptions.AdditionalProperties["Endpoint"] = new Uri(coderEndpoint);
        }

        Console.WriteLine($"\n[AI Tool Calling]: {nameof(PgGenerateDashboard)} | Model: {chatOptions.ModelId ?? "default"} | Endpoint: {coderEndpoint ?? "default"}");

        var response = await chatClient.GetResponseAsync(
            messages: [
                new(Microsoft.Extensions.AI.ChatRole.System, systemPrompt),
                new(Microsoft.Extensions.AI.ChatRole.User, prompt)
            ],
            options: chatOptions,
            cancellationToken: cancellationToken);

        var html = response.Text?.Trim();
        Console.WriteLine($"\n[Generated HTML]: {html}");
        if (html != null && html.StartsWith("```html", StringComparison.OrdinalIgnoreCase))
        {
            html = html.Substring(7);
            if (html.EndsWith("```"))
            {
                html = html.Substring(0, html.Length - 3);
            }
        }
        else if (html != null && html.StartsWith("```", StringComparison.OrdinalIgnoreCase))
        {
            html = html.Substring(3);
            if (html.EndsWith("```"))
            {
                html = html.Substring(0, html.Length - 3);
            }
        }

        return html?.Trim() ?? string.Empty;
    }

    [Description("将自然语言数据变更需求转换为 SQL 并执行（INSERT/UPDATE/DELETE）。注意：该工具会自动提交到数据库并生效。执行成功后，你【必须立即停止调用本工具】，且只对用户输出“执行成功！”。绝对不允许自己构建表格！绝对不要尝试读取、翻译或提取数据字段！")]
    [McpServerTool(Name = nameof(PgTextToSqlWrite))]
    private async Task<string> PgTextToSqlWrite(
        [Required, Description("自然语言数据变更需求，例如：把标题为A的任务标记完成")] string writeRequirement,
        [Description("最大允许影响行数，默认50，最大200")] int maxAffectedRows = 50)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(PgTextToSqlWrite)} (maxAffectedRows: {maxAffectedRows})");
        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            var userId = await EnsureCurrentUserIdIsPresent();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var connection = await OpenAppDbConnectionAsync(db, CancellationToken.None);
            //var schemaSummary = await BuildSchemaSummaryAsync(connection, CancellationToken.None);
            var schemaSummary = await RetrieveSchemaContextAsync(writeRequirement, CancellationToken.None);
            var accessibleTables = await LoadAccessibleTablesAsync(connection, CancellationToken.None);
            var safeMaxAffectedRows = Math.Clamp(maxAffectedRows, 1, 200);

            var firstSql = await GenerateTextToWriteSqlAsync(writeRequirement, schemaSummary, userId, null, CancellationToken.None);

            try
            {
                var firstResult = await ExecuteWriteSqlAsync(connection, firstSql, safeMaxAffectedRows, accessibleTables, CancellationToken.None);
                var jsonResult = JsonSerializer.Serialize(new
                {
                    sql = firstResult.sql,
                    retry = false,
                    affectedRows = firstResult.affectedRows
                }, _jsonSerializerOptions);
                Console.WriteLine($"\n[PgTextToSqlWrite Result]: {jsonResult}");
                return $"【系统强制指令：执行成功！】数据已生效。请立即停止调用此工具，并且只对用户输出这句话：'执行成功！数据已更新，请刷新页面。'。绝对不要输出 markdown 表格，绝对不要提取、联想或翻译任何字段！";
            }
            catch (Exception firstExp)
            {
                Console.WriteLine($"\n[PgTextToSqlWrite Execution Failed, Retrying...]: {firstExp.Message}");
                var secondSql = await GenerateTextToWriteSqlAsync(writeRequirement, schemaSummary, userId, firstExp.Message, CancellationToken.None);
                var secondResult = await ExecuteWriteSqlAsync(connection, secondSql, safeMaxAffectedRows, accessibleTables, CancellationToken.None);
                var jsonResult = JsonSerializer.Serialize(new
                {
                    sql = secondResult.sql,
                    retry = true,
                    previousError = firstExp.Message,
                    affectedRows = secondResult.affectedRows
                }, _jsonSerializerOptions);
                Console.WriteLine($"\n[PgTextToSqlWrite Result (Retry)]: {jsonResult}");
                return $"【系统强制指令：执行成功！】(经过自动重试后生效) 数据已生效。请立即停止调用此工具，并且只对用户输出这句话：'执行成功！数据已更新，请刷新页面。'。绝对不要输出 markdown 表格，绝对不要提取、联想或翻译任何字段！";
            }
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return "[]";
        }
    }

    private static string QuoteIdentifier(string identifier) => $"\"{identifier.Replace("\"", "\"\"")}\"";

    private static async Task<(string sql, int total, List<Dictionary<string, object?>> rows)> ExecuteReportQueryAsync(
        DbConnection connection,
        string sql,
        int limit,
        CancellationToken cancellationToken)
    {
        var sanitizedSql = ValidateReadOnlySql(sql);
        var safeLimit = Math.Clamp(limit, 1, 10000);

        await using var command = connection.CreateCommand();
        command.CommandText = $"SELECT * FROM ({sanitizedSql}) AS report_result LIMIT @p0;";
        AddParameter(command, "@p0", safeLimit);

        try
        {
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var rows = new List<Dictionary<string, object?>>();
            while (await reader.ReadAsync(cancellationToken))
            {
                rows.Add(ReadRow(reader));
            }

            return (sanitizedSql, rows.Count, rows);
        }
        catch (PostgresException pgEx)
        {
            throw new ValidationException($"数据库查询失败: {pgEx.MessageText} (Position: {pgEx.Position})", pgEx);
        }
    }

    private static async Task<(string sql, int affectedRows, Dictionary<string, object?>? firstRow)> ExecuteWriteSqlAsync(
        DbConnection connection,
        string sql,
        int maxAffectedRows,
        List<PgTableNameInfo> accessibleTables,
        CancellationToken cancellationToken)
    {
        var (normalizedSql, _) = PrepareWriteSql(sql, accessibleTables);
        var executableSql = EnsureReturningAllColumns(normalizedSql);

        await using var command = connection.CreateCommand();
        command.CommandText = executableSql;

        try
        {
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var affectedRows = 0;
            Dictionary<string, object?>? firstRow = null;
            while (await reader.ReadAsync(cancellationToken))
            {
                affectedRows++;
                firstRow ??= ReadRow(reader);
            }

            if (affectedRows > maxAffectedRows)
                throw new ValidationException($"Affected rows {affectedRows} exceeded safety limit {maxAffectedRows}.");

            return (normalizedSql, affectedRows, firstRow);
        }
        catch (PostgresException pgEx)
        {
            throw new ValidationException($"数据库执行失败: {pgEx.MessageText} (Position: {pgEx.Position})", pgEx);
        }
    }

    [GeneratedRegex(@"\bRETURNING\b", RegexOptions.IgnoreCase)]
    private static partial Regex ReturningRegex();

    private static string EnsureReturningAllColumns(string sql)
    {
        if (ReturningRegex().IsMatch(sql))
            return sql;

        return $"{sql.Trim().TrimEnd(';')} RETURNING *;";
    }

    private static (string normalizedSql, PgTableNameInfo targetTable) PrepareWriteSql(
        string sql,
        List<PgTableNameInfo> accessibleTables)
    {
        var sanitizedSql = ValidateWriteSql(sql);
        var tableNormalizedSql = NormalizeWriteSqlTableIdentifiers(sanitizedSql, accessibleTables);
        var targetTable = ResolveWriteTargetTableInfo(tableNormalizedSql, accessibleTables);
        var columnNormalizedSql = NormalizeWriteSqlColumnIdentifiers(tableNormalizedSql, targetTable);
        var normalizedSql = EnrichInsertSqlWithRequiredColumns(columnNormalizedSql, targetTable);
        return (normalizedSql, targetTable);
    }

    private static void AddParameter(DbCommand command, string name, object? value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value ?? DBNull.Value;
        command.Parameters.Add(parameter);
    }
    private static object? JsonElementToDbValue(JsonElement value)
    {
        return value.ValueKind switch
        {
            JsonValueKind.Null => null,
            JsonValueKind.String => value.GetString(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Number when value.TryGetInt32(out var v) => v,
            JsonValueKind.Number when value.TryGetInt64(out var v) => v,
            JsonValueKind.Number when value.TryGetDecimal(out var v) => v,
            JsonValueKind.Number => value.GetDouble(),
            _ => value.GetRawText()
        };
    }
    private static Dictionary<string, object?> ReadRow(DbDataReader reader)
    {
        var row = new Dictionary<string, object?>(reader.FieldCount, StringComparer.OrdinalIgnoreCase);
        for (var i = 0; i < reader.FieldCount; i++)
        {
            var value = reader.GetValue(i);
            row[reader.GetName(i)] = value == DBNull.Value ? null : value;
        }
        return row;
    }
    private static string ValidateReadOnlySql(string sql)
    {
        var normalized = sql.Trim();
        if (string.IsNullOrWhiteSpace(normalized))
            throw new ValidationException("sql is required.");

        if (normalized.EndsWith(';'))
            normalized = normalized[..^1].TrimEnd();

        if (normalized.Contains(';'))
            throw new ValidationException("Only single SQL statement is allowed.");

        if (ReadOnlySqlRegex.IsMatch(normalized) is false)
            throw new ValidationException("Only SELECT/WITH read-only SQL is allowed.");

        if (ForbiddenSqlRegex.IsMatch(normalized))
            throw new ValidationException("Write/DDL SQL is not allowed in report query.");

        if (DangerousSqlRegex.IsMatch(normalized))
            throw new ValidationException("Dangerous SQL function is not allowed.");

        return normalized;
    }
    private static string ValidateWriteSql(string sql)
    {
        var normalized = sql.Trim();
        if (string.IsNullOrWhiteSpace(normalized))
            throw new ValidationException("sql is required.");

        if (normalized.EndsWith(';'))
            normalized = normalized[..^1].TrimEnd();

        if (normalized.Contains(';'))
            throw new ValidationException("Only single SQL statement is allowed.");

        if (WriteSqlRegex.IsMatch(normalized) is false)
            throw new ValidationException("Only INSERT/UPDATE/DELETE SQL is allowed.");

        if (DdlSqlRegex.IsMatch(normalized))
            throw new ValidationException("DDL SQL is not allowed.");

        if (DangerousSqlRegex.IsMatch(normalized))
            throw new ValidationException("Dangerous SQL function is not allowed.");

        if (UpdateDeleteWithoutWhereRegex.IsMatch(normalized))
            throw new ValidationException("UPDATE/DELETE must include WHERE clause.");

        return normalized;
    }
    private async Task<string> GenerateTextToSqlAsync(
        string reportRequirement,
        string schemaSummary,
        Guid currentUserId,
        string? previousError,
        CancellationToken cancellationToken)
    {
        if (serviceProvider.GetService<IChatClient>() is not IChatClient chatClient)
            throw new ResourceNotFoundException("IChatClient is not available.");

        var sqlAgent = chatClient.AsAIAgent(
            instructions: """
            你是 PostgreSQL 报表 SQL 生成器。
            目标：根据用户需求和给定 schema 生成可执行、只读的 SQL。

            约束：
            1) 只允许 SELECT 或 WITH 开头语句；
            2) 只生成一条 SQL，不允许分号分隔多语句；
            3) 禁止 INSERT/UPDATE/DELETE/DDL；
            4) 如果表中有比较耗时的字段（如大文本 DescriptionHTML, 向量 Embedding 等），请绝对不要使用 SELECT *，必须明确指定需要查询的核心字段（如主键、名称、状态、日期等）。仅当表结构简单且无大文本字段时，才可使用 SELECT *。
            5) 表名和字段名必须严格来自 schema，绝对不要凭空捏造或联想不存在的字段。如果要查询的字段不在主表中，必须通过 JOIN 关联；如果用户条件涉及的字段在当前查询的表中不存在，绝对不要将该条件强加到该表上，直接忽略该条件或寻找合理的替代方案。
            6) 如果需求不清晰，做最保守可执行推断；
            7) 所有的表名和列名（包括 SELECT 列表、WHERE 条件、GROUP BY 和 ORDER BY 中的列名）都必须使用双引号包裹，以保证大小写精确匹配。当使用表别名时，表别名不要加双引号，但列名必须加双引号，格式为：别名."列名"（例如：SELECT p."Id", c."Name" FROM "public"."Products" p JOIN "public"."Categories" c ON p."CategoryId" = c."Id" WHERE p."IsDone" = true）。绝对不要使用未带双引号的列名，也不要将别名和列名一起放在一个双引号内（如 "p.Id" 是错误的，必须是 p."Id"）。
            8) 用户提供的业务值必须原样保留，不允许擅自改写、替换、联想或翻译。
            9) 虽然我们在上下文提供了当前用户的 UserId，但你必须检查你要查询或操作的表是否真的有 UserId 字段，如果没有，请绝对不要在 WHERE 条件中加入 UserId 的过滤！
            10) 严禁在 WHERE 子句中使用窗口函数（如 ROW_NUMBER() OVER ()）。PostgreSQL 不允许在 WHERE 中直接使用窗口函数。如果需要限制行数，请使用 LIMIT 关键字；如果需要基于排序限制，请使用 ORDER BY 结合 LIMIT，或者使用子查询。
            11) 必须根据提供的 schema 数据类型生成正确的语法：对于 uuid 类型请勿使用 LIKE/ILIKE 进行模糊匹配；对于 varchar/text 类型请优先使用 ILIKE 进行忽略大小写的匹配；处理时间类型时请使用正确的时间函数。
            12) 当需要查询总数或者统计信息时，请使用正确的聚合函数（如 COUNT, SUM, MAX, MIN, AVG），并使用明确的列名。在生成统计报表时，应尽可能提供极为丰富的数据维度（例如除了简单的分组数量，还必须同时计算价格/金额的总和、最大值、最小值、平均值等相关统计，甚至按时间维度（如天、月）进行多维度 Group By），从而让报表数据极其饱满。

            输出格式：
            仅返回 JSON 对象：{"sql":"..."}，不要附加解释文本。
            """,
            name: "PgTextToSqlAgent",
            description: "Converts natural language report requirement into safe PostgreSQL read-only SQL");

        ChatOptions chatOptions = new()
        {
            ResponseFormat = ChatResponseFormat.Json,
            AdditionalProperties = new()
            {
                ["response_format"] = new { type = "json_object" }
            }
        };
        configuration.GetRequiredSection("AI:ChatOptions").Bind(chatOptions);
        var coderModelId = configuration["AI:OpenAI:CoderModel"] ?? configuration["AI:AzureOpenAI:CoderModel"];
        if (!string.IsNullOrWhiteSpace(coderModelId))
        {
            chatOptions.ModelId = coderModelId;
        }

        var coderEndpoint = configuration["AI:OpenAI:CoderEndpoint"] ?? configuration["AI:AzureOpenAI:CoderEndpoint"];
        if (!string.IsNullOrWhiteSpace(coderEndpoint))
        {
            chatOptions.AdditionalProperties ??= new();
            chatOptions.AdditionalProperties["Endpoint"] = new Uri(coderEndpoint);
        }

        Console.WriteLine($"\n[AI Tool Calling]: {nameof(PgTextToSqlReport)} | Model: {chatOptions.ModelId ?? "default"} | Endpoint: {coderEndpoint ?? "default"}");

        var prompt = $"""
            当前用户的 UserId 为：{currentUserId}

            用户报表需求：
            {reportRequirement}

            可用数据库 schema（仅这些表和字段可用）：
            {schemaSummary}
            """;

        if (string.IsNullOrWhiteSpace(previousError) is false)
        {
            prompt += $"""

                上一次SQL执行错误：
                {previousError}

                请修正SQL并重新输出。
                """;
        }

        var response = await sqlAgent.RunAsync<TextToSqlResponse>(
            messages: [new(ChatRole.User, prompt)],
            cancellationToken: cancellationToken,
            options: new Microsoft.Agents.AI.ChatClientAgentRunOptions(chatOptions));

        var sql = response.Result.Sql?.Trim();
        if (string.IsNullOrWhiteSpace(sql))
            throw new ValidationException("Text-to-SQL model returned empty SQL.");

        Console.WriteLine($"\n[prompt]: {prompt}");
        Console.WriteLine($"\n[Generated SQL]: {sql}");
        return sql!;
    }

    private async Task<string> GenerateTextToWriteSqlAsync(
        string writeRequirement,
        string schemaSummary,
        Guid currentUserId,
        string? previousError,
        CancellationToken cancellationToken)
    {
        if (serviceProvider.GetService<IChatClient>() is not IChatClient chatClient)
            throw new ResourceNotFoundException("IChatClient is not available.");

        var sqlAgent = chatClient.AsAIAgent(
            instructions: """
            你是 PostgreSQL 写入 SQL 生成器。
            目标：根据用户需求和给定 schema 生成可执行的数据变更 SQL。

            约束：
            1) 只允许 INSERT/UPDATE/DELETE；
            2) 只生成一条 SQL，不允许分号分隔多语句；
            3) 严禁 DDL（CREATE/ALTER/DROP/TRUNCATE 等）；
            4) UPDATE/DELETE 必须带 WHERE；
            5) 表名和字段名必须严格、100%来自给定的 schema。绝对不要凭空捏造字段名！如果用户说“类别是BMW”，而表中只有 `CategoryId`，你绝对不能生成 `CategoryName` 这个字段！必须使用子查询去查出对应的 ID 并赋给 `CategoryId` 字段。
            6) 若需求不清晰，做最保守的精确变更，不做全表操作。
            7) 所有的表名和列名（包括 INSERT 列表、UPDATE SET 列表和 WHERE 条件中的列名）都必须使用双引号包裹，以保证大小写精确匹配（例如：UPDATE "public"."TodoItems" SET "IsDone" = true WHERE "Id" = '...'）。不要使用未带双引号的标识符。
            8) 用户提供的业务值必须原样保留，不允许擅自改写、替换、联想或翻译。
            9) 虽然我们在上下文提供了当前用户的 UserId，但你必须检查你要查询或操作的表是否真的有 UserId 字段，如果没有，请不要在 WHERE 条件中加入 UserId 的过滤！
            10) 必须根据提供的 schema 数据类型生成正确的语法：更新或插入 uuid、字符串、日期字段时必须使用单引号（'）包裹值；更新布尔值请使用 true/false；更新数值请直接写数字。
            11) 严禁凭空伪造外键 UUID 值！如果需要关联其他表的数据，必须使用子查询去获取真实的 ID。注意：在使用子查询匹配外键时，如果目标字段可能是外键（比如 Categories 表的 Name 字段），请尽量使用 ILIKE 模糊匹配（例如 ILIKE '%BMW%'），以防止因用户输入的名称不完全精确而导致子查询返回 NULL，进而引发外键字段 not-null constraint 错误！

            输出格式：
            仅返回 JSON 对象：{"sql":"..."}，不要附加解释文本。
            """,
            name: "PgTextToWriteSqlAgent",
            description: "Converts natural language write requirement into safe PostgreSQL DML SQL");

        ChatOptions chatOptions = new()
        {
            ResponseFormat = ChatResponseFormat.Json,
            AdditionalProperties = new()
            {
                ["response_format"] = new { type = "json_object" }
            }
        };
        configuration.GetRequiredSection("AI:ChatOptions").Bind(chatOptions);
        var coderModelId = configuration["AI:OpenAI:CoderModel"] ?? configuration["AI:AzureOpenAI:CoderModel"];
        if (!string.IsNullOrWhiteSpace(coderModelId))
        {
            chatOptions.ModelId = coderModelId;
        }

        var coderEndpoint = configuration["AI:OpenAI:CoderEndpoint"] ?? configuration["AI:AzureOpenAI:CoderEndpoint"];
        if (!string.IsNullOrWhiteSpace(coderEndpoint))
        {
            chatOptions.AdditionalProperties ??= new();
            chatOptions.AdditionalProperties["Endpoint"] = new Uri(coderEndpoint);
        }

        Console.WriteLine($"\n[AI Tool Calling]: {nameof(PgTextToSqlWrite)} | Model: {chatOptions.ModelId ?? "default"} | Endpoint: {coderEndpoint ?? "default"}");

        var prompt = $"""
            当前用户的 UserId 为：{currentUserId}

            用户写入需求：
            {writeRequirement}

            重要指示：如果用户需要插入外键关联的数据（例如在产品表中插入带有类别的记录），你必须先生成子查询去目标表查找正确的 ID，而不是自己瞎编一个 UUID 或直接插入文字。
            例如：INSERT INTO "public"."Products" ("Name", "CategoryId") VALUES ('新产品', (SELECT "Id" FROM "public"."Categories" WHERE "Name" ILIKE '%类别名称%' LIMIT 1));

            注意：请只生成你认为必须的业务字段即可。像 `Id`、`CreatedOn`、`HasPrimaryImage` 这种系统级别或非空的默认字段，如果用户没有显式提到，你不要主动去补全它们（交由后端的自动补全机制处理），除非它们是你业务逻辑的一部分。

            可用数据库 schema（仅这些表和字段可用）：
            {schemaSummary}
            """;

        if (string.IsNullOrWhiteSpace(previousError) is false)
        {
            prompt += $"""

                上一次SQL执行错误：
                {previousError}

                请修正SQL并重新输出。
                """;
        }

        var response = await sqlAgent.RunAsync<TextToSqlResponse>(
            messages: [new(ChatRole.User, prompt)],
            cancellationToken: cancellationToken,
            options: new Microsoft.Agents.AI.ChatClientAgentRunOptions(chatOptions));

        var sql = response.Result.Sql?.Trim();
        if (string.IsNullOrWhiteSpace(sql))
            throw new ValidationException("Text-to-SQL model returned empty SQL.");

        Console.WriteLine($"\n[schemaSummary]: {schemaSummary}");
        Console.WriteLine($"\n[Generated SQL]: {sql}");
        return sql!;
    }

    private static string? BuildGeneratedColumnValue(PgTableColumnInfo column)
    {
        if (column.Name.Equals("Id", StringComparison.OrdinalIgnoreCase))
        {
            if (column.DataType.Equals("uuid", StringComparison.OrdinalIgnoreCase) ||
                column.UdtName.Equals("uuid", StringComparison.OrdinalIgnoreCase))
                return $"'{Guid.CreateVersion7()}'";

            return $"'{Guid.CreateVersion7()}'";
        }

        if (column.Name.Equals("Version", StringComparison.OrdinalIgnoreCase))
            return "1";

        if (column.Name.Equals("CreatedOn", StringComparison.OrdinalIgnoreCase) ||
            column.Name.Equals("CreatedAt", StringComparison.OrdinalIgnoreCase) ||
            column.Name.Equals("UpdatedAt", StringComparison.OrdinalIgnoreCase))
            return "NOW()";

        if (column.DataType.Equals("bool", StringComparison.OrdinalIgnoreCase) || 
            column.UdtName.Equals("bool", StringComparison.OrdinalIgnoreCase))
            return "FALSE";

        return null;
    }

    private static string BuildOrderByClause(string? orderBy, PgTableInfo tableInfo)
    {
        if (string.IsNullOrWhiteSpace(orderBy))
            return string.Empty;

        var segments = orderBy.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var orders = new List<string>();
        foreach (var segment in segments)
        {
            var parts = segment.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (parts.Length == 0)
                continue;

            var column = ResolveColumnName(tableInfo, parts[0]);
            var direction = parts.Length > 1 && parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase)
                ? "DESC"
                : "ASC";
            orders.Add($"{QuoteIdentifier(column)} {direction}");
        }

        return orders.Count == 0 ? string.Empty : $" ORDER BY {string.Join(", ", orders)}";
    }

    private static string BuildWhereClause(
        string? whereJson,
        PgTableInfo tableInfo,
        DbCommand command,
        ref int parameterIndex)
    {
        if (string.IsNullOrWhiteSpace(whereJson))
            return string.Empty;

        using var doc = JsonDocument.Parse(whereJson);
        if (doc.RootElement.ValueKind is not JsonValueKind.Object)
            throw new ValidationException("whereJson must be a JSON object.");

        var whereConditions = new List<string>();
        foreach (var property in doc.RootElement.EnumerateObject())
        {
            var column = ResolveColumnName(tableInfo, property.Name);
            if (property.Value.ValueKind is JsonValueKind.Null)
            {
                whereConditions.Add($"{QuoteIdentifier(column)} IS NULL");
                continue;
            }

            var parameterName = $"@p{parameterIndex++}";
            whereConditions.Add($"{QuoteIdentifier(column)} = {parameterName}");
            AddParameter(command, parameterName, JsonElementToDbValue(property.Value));
        }

        if (whereConditions.Count == 0)
            return string.Empty;

        return $" WHERE {string.Join(" AND ", whereConditions)}";
    }

    private static List<(string Column, object? Value)> ExtractColumnsAndValues(JsonElement root, PgTableInfo tableInfo)
    {
        var pairs = new List<(string Column, object? Value)>();
        foreach (var property in root.EnumerateObject())
        {
            var column = ResolveColumnName(tableInfo, property.Name);
            pairs.Add((column, JsonElementToDbValue(property.Value)));
        }
        return pairs;
    }

    private static string ResolveColumnName(PgTableInfo tableInfo, string inputColumnName)
    {
        if (tableInfo.Columns.TryGetValue(inputColumnName, out var resolvedColumn))
            return resolvedColumn;

        throw new ValidationException($"Column '{inputColumnName}' does not exist in table '{tableInfo.FullName}'.");
    }

    private static bool IsTodoTable(PgTableInfo tableInfo)
        => tableInfo.Schema.Equals("public", StringComparison.OrdinalIgnoreCase)
           && tableInfo.Name.Equals("TodoItems", StringComparison.OrdinalIgnoreCase);

    private async Task<DbConnection> OpenAppDbConnectionAsync(AppDbContext db, CancellationToken cancellationToken)
    {
        var connection = db.Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync(cancellationToken);
        return connection;
    }

    private async Task<PgTableInfo> ResolveTableInfoAsync(DbConnection connection, string tableInput, CancellationToken cancellationToken)
    {
        var input = tableInput.Trim().Replace("\"", "");
        if (string.IsNullOrWhiteSpace(input))
            throw new ValidationException("table is required.");

        var hasSchema = input.Contains('.', StringComparison.Ordinal);
        var parts = hasSchema ? input.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) : [];
        if (hasSchema && parts.Length != 2)
            throw new ValidationException("table format must be table or schema.table.");

        var schemaParam = hasSchema ? parts[0] : null;
        var tableParam = hasSchema ? parts[1] : input;

        await using var command = connection.CreateCommand();
        var sql = """
                  SELECT table_schema, table_name, column_name
                  FROM information_schema.columns
                  WHERE table_schema NOT IN ('pg_catalog', 'information_schema', 'jobs')
                    AND table_name = @p0
                  """;
        if (hasSchema)
        {
            sql += " AND table_schema = @p1";
        }
        sql += " ORDER BY table_schema, table_name, ordinal_position;";

        command.CommandText = sql;
        AddParameter(command, "@p0", tableParam);
        if (hasSchema)
        {
            AddParameter(command, "@p1", schemaParam);
        }

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var tables = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        while (await reader.ReadAsync(cancellationToken))
        {
            var schema = reader.GetString(0);
            var table = reader.GetString(1);
            var column = reader.GetString(2);
            var fullName = $"{schema}.{table}";

            if (!tables.TryGetValue(fullName, out var columns))
            {
                columns = [];
                tables[fullName] = columns;
            }

            columns.Add(column);
        }

        if (tables.Count == 0)
            throw new ResourceNotFoundException($"Table '{input}' not found.");

        if (tables.Count > 1)
            throw new ValidationException($"Table name '{input}' is ambiguous. Use schema.table format.");

        var selectedFullName = tables.Keys.First();
        var fullNameParts = selectedFullName.Split('.');
        var schemaName = fullNameParts[0];
        var tableName = fullNameParts[1];
        var columnLookup = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var column in tables[selectedFullName])
        {
            columnLookup[column] = column;
        }

        return new PgTableInfo(
            schemaName,
            tableName,
            columnLookup,
            $"{QuoteIdentifier(schemaName)}.{QuoteIdentifier(tableName)}",
            $"{schemaName}.{tableName}");
    }

    private sealed record PgTableInfo(
        string Schema,
        string Name,
        Dictionary<string, string> Columns,
        string QuotedName,
        string FullName);

    private sealed record PgTableNameInfo(string Schema, string Table, Dictionary<string, PgTableColumnInfo> Columns);
    private sealed record PgTableColumnInfo(string Name, string DataType, string UdtName, bool IsNullable, string ColumnDefault);

    private sealed class TextToSqlResponse
    {
        public string? Sql { get; init; }
    }

    [Description("列出 PostgreSQL 中可访问的所有业务表（schema.table）。")]
    [McpServerTool(Name = nameof(PgListTables))]
    private async Task<string[]> PgListTables()
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(PgListTables)}");
        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            _ = await EnsureCurrentUserIdIsPresent();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var connection = await OpenAppDbConnectionAsync(db, CancellationToken.None);
            await using var command = connection.CreateCommand();
            command.CommandText = """
                                  SELECT table_schema, table_name
                                  FROM information_schema.tables
                                  WHERE table_type = 'BASE TABLE'
                                    AND table_schema NOT IN ('pg_catalog', 'information_schema', 'jobs')
                                  ORDER BY table_schema, table_name;
                                  """;

            await using var reader = await command.ExecuteReaderAsync(CancellationToken.None);
            var tables = new List<string>();
            while (await reader.ReadAsync(CancellationToken.None))
            {
                tables.Add($"{reader.GetString(0)}.{reader.GetString(1)}");
            }

            return [.. tables];
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return [];
        }
    }

    [Description("查询指定表的字段信息。table 支持 table 或 schema.table。")]
    [McpServerTool(Name = nameof(PgGetTableColumns))]
    private async Task<string> PgGetTableColumns(
        [Required, Description("表名，支持 table 或 schema.table")] string table)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(PgGetTableColumns)} (table: {table})");
        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            _ = await EnsureCurrentUserIdIsPresent();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var connection = await OpenAppDbConnectionAsync(db, CancellationToken.None);
            var tableInfo = await ResolveTableInfoAsync(connection, table, CancellationToken.None);
            return JsonSerializer.Serialize(new
            {
                table = tableInfo.FullName,
                columns = tableInfo.Columns.Values.OrderBy(c => c).ToArray()
            }, _jsonSerializerOptions);
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return "[]";
        }
    }

    [Description("查询指定表的数据。whereJson 为 JSON 对象，仅支持等值过滤；orderBy 例如: UpdatedAt desc, Title asc。拿到执行结果后，必须严格按照返回的 JSON 里的 rows 字段的数据值来展示，绝对不要篡改或翻译数据值！")]
    [McpServerTool(Name = nameof(PgSelectRows))]
    private async Task<string> PgSelectRows(
        [Required, Description("表名，支持 table 或 schema.table")] string table,
        [Description("过滤条件 JSON，例如 {\"Title\":\"任务A\",\"IsDone\":false}")] string? whereJson = null,
        [Description("排序字段，例如 UpdatedAt desc, Title asc")] string? orderBy = null,
        [Description("返回行数。除非用户指定，否则不要传此参数")] int limit = 1000,
        [Description("分页偏移，默认0")] int offset = 0)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(PgSelectRows)} (table: {table}, limit: {limit}, offset: {offset})");
        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            _ = await EnsureCurrentUserIdIsPresent();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var connection = await OpenAppDbConnectionAsync(db, CancellationToken.None);
            var tableInfo = await ResolveTableInfoAsync(connection, table, CancellationToken.None);

            await using var command = connection.CreateCommand();
            var parameterIndex = 0;
            var whereClause = BuildWhereClause(whereJson, tableInfo, command, ref parameterIndex);
            var orderByClause = BuildOrderByClause(orderBy, tableInfo);
            var safeLimit = Math.Clamp(limit, 1, 10000);
            var safeOffset = Math.Max(offset, 0);

            command.CommandText = $"SELECT * FROM {tableInfo.QuotedName}{whereClause}{orderByClause} LIMIT @p{parameterIndex++} OFFSET @p{parameterIndex};";
            AddParameter(command, $"@p{parameterIndex - 1}", safeLimit);
            AddParameter(command, $"@p{parameterIndex}", safeOffset);

            await using var reader = await command.ExecuteReaderAsync(CancellationToken.None);
            var rows = new List<Dictionary<string, object?>>();
            while (await reader.ReadAsync(CancellationToken.None))
            {
                rows.Add(ReadRow(reader));
            }

            var jsonResult = JsonSerializer.Serialize(new
            {
                table = tableInfo.FullName,
                total = rows.Count,
                rows
            }, _jsonSerializerOptions);
            Console.WriteLine($"\n[PgSelectRows Result]: {jsonResult}");
            return $"【系统强制指令：查询成功！】请严格根据以下 JSON 数据(rows)原样生成 markdown 表格。绝对不允许擅自联想、伪造、翻译或篡改任何字段的**数据值**！\n{jsonResult}";
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return JsonSerializer.Serialize(new { error = exp.Message }, _jsonSerializerOptions);
        }
    }

    [Description("向指定表插入一条数据。dataJson 为 JSON 对象。")]
    [McpServerTool(Name = nameof(PgInsertRow))]
    private async Task<string> PgInsertRow(
        [Required, Description("表名，支持 table 或 schema.table")] string table,
        [Required, Description("要插入的数据 JSON，例如 {\"Title\":\"任务A\",\"IsDone\":false}")] string dataJson)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(PgInsertRow)} (table: {table})");
        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            _ = await EnsureCurrentUserIdIsPresent();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var connection = await OpenAppDbConnectionAsync(db, CancellationToken.None);
            var tableInfo = await ResolveTableInfoAsync(connection, table, CancellationToken.None);

            using var doc = JsonDocument.Parse(dataJson);
            if (doc.RootElement.ValueKind is not JsonValueKind.Object)
                throw new ValidationException("dataJson must be a JSON object.");

            var pairs = ExtractColumnsAndValues(doc.RootElement, tableInfo);
            if (pairs.Count == 0)
                throw new ValidationException("dataJson must contain at least one field.");

            await using var command = connection.CreateCommand();
            var cols = new List<string>();
            var vals = new List<string>();
            for (var i = 0; i < pairs.Count; i++)
            {
                var p = $"@p{i}";
                cols.Add(QuoteIdentifier(pairs[i].Column));
                vals.Add(p);
                AddParameter(command, p, pairs[i].Value);
            }

            command.CommandText = $"INSERT INTO {tableInfo.QuotedName} ({string.Join(", ", cols)}) VALUES ({string.Join(", ", vals)}) RETURNING *;";

            await using var reader = await command.ExecuteReaderAsync(CancellationToken.None);
            Dictionary<string, object?>? row = null;
            if (await reader.ReadAsync(CancellationToken.None))
            {
                row = ReadRow(reader);
            }

            if (IsTodoTable(tableInfo))
                await NotifyTodoItemsChanged();

            var jsonResult = JsonSerializer.Serialize(new
            {
                table = tableInfo.FullName,
                affectedRows = 1,
                row
            }, _jsonSerializerOptions);
            return $"【系统强制指令：执行成功！】插入已生效，请停止调用工具。并且只输出这句话：'执行成功！数据已更新。'。绝对不要输出 markdown 表格，绝对不要提取或翻译任何字段！原始数据为：\n{jsonResult}";
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return "[]";
        }
    }

    [Description("更新指定表数据。dataJson 为更新字段；whereJson 为等值过滤。默认禁止无条件全表更新。")]
    [McpServerTool(Name = nameof(PgUpdateRows))]
    private async Task<string> PgUpdateRows(
        [Required, Description("表名，支持 table 或 schema.table")] string table,
        [Required, Description("要更新的数据 JSON，例如 {\"IsDone\":true}")] string dataJson,
        [Description("过滤条件 JSON，例如 {\"Title\":\"任务A\"}")] string? whereJson = null,
        [Description("是否允许无条件更新全部数据，默认 false")] bool allowAffectAll = false)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(PgUpdateRows)} (table: {table}, allowAffectAll: {allowAffectAll})");
        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            _ = await EnsureCurrentUserIdIsPresent();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var connection = await OpenAppDbConnectionAsync(db, CancellationToken.None);
            var tableInfo = await ResolveTableInfoAsync(connection, table, CancellationToken.None);

            using var dataDoc = JsonDocument.Parse(dataJson);
            if (dataDoc.RootElement.ValueKind is not JsonValueKind.Object)
                throw new ValidationException("dataJson must be a JSON object.");

            var updates = ExtractColumnsAndValues(dataDoc.RootElement, tableInfo);
            if (updates.Count == 0)
                throw new ValidationException("dataJson must contain at least one field.");

            if (string.IsNullOrWhiteSpace(whereJson) && allowAffectAll is false)
                throw new ValidationException("whereJson is required unless allowAffectAll is true.");

            await using var command = connection.CreateCommand();
            var setClauses = new List<string>();
            var parameterIndex = 0;
            foreach (var update in updates)
            {
                var p = $"@p{parameterIndex++}";
                setClauses.Add($"{QuoteIdentifier(update.Column)} = {p}");
                AddParameter(command, p, update.Value);
            }

            var whereClause = BuildWhereClause(whereJson, tableInfo, command, ref parameterIndex);
            command.CommandText = $"UPDATE {tableInfo.QuotedName} SET {string.Join(", ", setClauses)}{whereClause};";
            var affectedRows = await command.ExecuteNonQueryAsync(CancellationToken.None);

            if (affectedRows > 0 && IsTodoTable(tableInfo))
                await NotifyTodoItemsChanged();

            var jsonResult = JsonSerializer.Serialize(new
            {
                table = tableInfo.FullName,
                affectedRows
            }, _jsonSerializerOptions);
            return $"【系统强制指令：执行成功！】更新已生效，请停止调用工具。并且只输出这句话：'执行成功！数据已更新。'。绝对不要输出 markdown 表格，绝对不要提取或翻译任何字段！原始数据为：\n{jsonResult}";
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return "[]";
        }
    }

    [Description("删除指定表数据。whereJson 为等值过滤。默认禁止无条件全表删除。")]
    [McpServerTool(Name = nameof(PgDeleteRows))]
    private async Task<string> PgDeleteRows(
        [Required, Description("表名，支持 table 或 schema.table")] string table,
        [Description("过滤条件 JSON，例如 {\"Title\":\"任务A\"}")] string? whereJson = null,
        [Description("是否允许无条件删除全部数据，默认 false")] bool allowAffectAll = false)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(PgDeleteRows)} (table: {table}, allowAffectAll: {allowAffectAll})");
        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            _ = await EnsureCurrentUserIdIsPresent();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var connection = await OpenAppDbConnectionAsync(db, CancellationToken.None);
            var tableInfo = await ResolveTableInfoAsync(connection, table, CancellationToken.None);

            if (string.IsNullOrWhiteSpace(whereJson) && allowAffectAll is false)
                throw new ValidationException("whereJson is required unless allowAffectAll is true.");

            await using var command = connection.CreateCommand();
            var parameterIndex = 0;
            var whereClause = BuildWhereClause(whereJson, tableInfo, command, ref parameterIndex);
            command.CommandText = $"DELETE FROM {tableInfo.QuotedName}{whereClause};";
            var affectedRows = await command.ExecuteNonQueryAsync(CancellationToken.None);

            if (affectedRows > 0 && IsTodoTable(tableInfo))
                await NotifyTodoItemsChanged();

            var jsonResult = JsonSerializer.Serialize(new
            {
                table = tableInfo.FullName,
                affectedRows
            }, _jsonSerializerOptions);
            return $"【系统强制指令：执行成功！】删除已生效，请停止调用工具。并且只输出这句话：'执行成功！数据已更新。'。绝对不要输出 markdown 表格，绝对不要提取或翻译任何字段！原始数据为：\n{jsonResult}";
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return "[]";
        }
    }

    [Description("执行报表查询SQL，支持多表关联，仅允许只读 SELECT/WITH 语句。拿到执行结果后，必须严格按照返回的 JSON 里的 rows 字段的数据值来展示，绝对不要篡改或翻译数据值！")]
    [McpServerTool(Name = nameof(PgQueryReport))]
    private async Task<string> PgQueryReport(
        [Required, Description("报表SQL，仅支持 SELECT/WITH")] string sql,
        [Description("最大返回行数。除非用户指定，否则不要传此参数")] int limit = 1000)
    {
        Console.WriteLine($"\n[AI Tool Called]: {nameof(PgQueryReport)}");
        Console.WriteLine($"[PgQueryReport SQL]: {sql}");
        await using var scope = serviceProvider.CreateAsyncScope();

        try
        {
            _ = await EnsureCurrentUserIdIsPresent();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var connection = await OpenAppDbConnectionAsync(db, CancellationToken.None);
            var reportResult = await ExecuteReportQueryAsync(connection, sql, limit, CancellationToken.None);
            var jsonResult = JsonSerializer.Serialize(new
            {
                sql = reportResult.sql,
                total = reportResult.total,
                rows = reportResult.rows
            }, _jsonSerializerOptions);
            Console.WriteLine($"\n[PgQueryReport Result]: {jsonResult}");
            return $"【系统强制指令：查询成功！】请严格根据以下 JSON 数据(rows)原样生成 markdown 表格。绝对不允许擅自联想、伪造、翻译或篡改任何字段的**数据值**！\n{jsonResult}";
        }
        catch (Exception exp)
        {
            serviceProvider.GetRequiredService<ServerExceptionHandler>().Handle(exp);
            return JsonSerializer.Serialize(new { error = exp.Message }, _jsonSerializerOptions);
        }
    }


    private static async Task<string> BuildSchemaSummaryAsync(DbConnection connection, CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = """
                              SELECT n.nspname AS table_schema,
                                     c.relname AS table_name,
                                     a.attname AS column_name,
                                     t.typname AS data_type,
                                     COALESCE(d_table.description, '') AS table_comment,
                                     COALESCE(d_col.description, '') AS column_comment
                              FROM pg_class c
                              JOIN pg_namespace n ON n.oid = c.relnamespace
                              JOIN pg_attribute a ON a.attrelid = c.oid
                              JOIN pg_type t ON t.oid = a.atttypid
                              LEFT JOIN pg_description d_table ON d_table.objoid = c.oid AND d_table.objsubid = 0
                              LEFT JOIN pg_description d_col ON d_col.objoid = c.oid AND d_col.objsubid = a.attnum
                              WHERE c.relkind = 'r'
                                AND n.nspname NOT IN ('pg_catalog', 'information_schema', 'jobs')
                                AND a.attnum > 0
                                AND NOT a.attisdropped
                              ORDER BY n.nspname, c.relname, a.attnum;
                              """;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var grouped = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        var tableComments = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        while (await reader.ReadAsync(cancellationToken))
        {
            var schema = reader.GetString(0);
            var table = reader.GetString(1);
            var column = reader.GetString(2);
            var dataType = reader.GetString(3);
            var tableComment = reader.GetString(4);
            var columnComment = reader.GetString(5);
            var key = $"{schema}.{table}";
            
            if (!grouped.TryGetValue(key, out var cols))
            {
                cols = [];
                grouped[key] = cols;
                tableComments[key] = tableComment;
            }

            var resolvedColumnComment = string.IsNullOrWhiteSpace(columnComment) ? "" : $" /* {columnComment} */";
            cols.Add($"- `{column}` ({dataType}){resolvedColumnComment}");
        }

        var sb = new System.Text.StringBuilder();
        foreach (var kv in grouped.Take(100))
        {
            var full = kv.Key.Split('.');
            var tableComment = tableComments.TryGetValue(kv.Key, out var comment) && !string.IsNullOrWhiteSpace(comment)
                ? comment
                : full[1];
            sb.Append("### Table: `").Append(full[0]).Append("`.`").Append(full[1]).Append("`\n")
              .Append("**Description**: ").Append(tableComment).Append("\n")
              .Append("**Columns**:\n")
              .Append(string.Join("\n", kv.Value)).AppendLine("\n");
        }

        return sb.ToString();
    }

    private static async Task<List<PgTableNameInfo>> LoadAccessibleTablesAsync(DbConnection connection, CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = """
                              SELECT n.nspname AS table_schema,
                                     c.relname AS table_name,
                                     a.attname AS column_name,
                                     t.typname AS data_type,
                                     t.typname AS udt_name,
                                     CASE WHEN a.attnotnull THEN 'NO' ELSE 'YES' END AS is_nullable,
                                     COALESCE(pg_get_expr(ad.adbin, ad.adrelid), '') AS column_default
                              FROM pg_class c
                              JOIN pg_namespace n ON n.oid = c.relnamespace
                              JOIN pg_attribute a ON a.attrelid = c.oid
                              JOIN pg_type t ON t.oid = a.atttypid
                              LEFT JOIN pg_attrdef ad ON ad.adrelid = c.oid AND ad.adnum = a.attnum
                              WHERE c.relkind = 'r'
                                AND n.nspname NOT IN ('pg_catalog', 'information_schema', 'jobs')
                                AND a.attnum > 0
                                AND NOT a.attisdropped
                              ORDER BY n.nspname, c.relname, a.attnum;
                              """;
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var grouped = new Dictionary<string, PgTableNameInfo>(StringComparer.OrdinalIgnoreCase);
        while (await reader.ReadAsync(cancellationToken))
        {
            var schema = reader.GetString(0);
            var table = reader.GetString(1);
            var column = reader.GetString(2);
            var dataType = reader.GetString(3);
            var udtName = reader.GetString(4);
            var isNullable = reader.GetString(5).Equals("YES", StringComparison.OrdinalIgnoreCase);
            var columnDefault = reader.GetString(6);
            var key = $"{schema}.{table}";

            if (grouped.TryGetValue(key, out var info) is false)
            {
                info = new PgTableNameInfo(schema, table, new Dictionary<string, PgTableColumnInfo>(StringComparer.OrdinalIgnoreCase));
                grouped[key] = info;
            }

            info.Columns[column] = new PgTableColumnInfo(column, dataType, udtName, isNullable, columnDefault);
        }

        return grouped.Values.ToList();
    }

    /// <summary>
    /// 基于 RAG 检索相关的数据库 Schema 上下文。
    /// 使用混合检索（向量+关键词）获取最相关的表结构。
    /// </summary>
    private async Task<string> RetrieveSchemaContextAsync(string userQuestion, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(userQuestion))
            throw new ArgumentException("问题不能为空", nameof(userQuestion));

        // 1. 检查知识库是否存在 (参考代码逻辑)
        var exists = await dbContext.RagKnowledgeBases
            .AnyAsync(k => k.IsDeleted == false, cancellationToken);

        if (!exists)
            throw new ResourceNotFoundException($"知识库不存在");

        // 2. 生成向量与分词 (参考代码逻辑)
        var questionEmbedding = await embeddingGenerator.GenerateAsync(userQuestion, cancellationToken: cancellationToken);
        var queryVector = new Vector(Normalize(questionEmbedding.Vector.ToArray()));
        var queryKeywords = Tokenize(userQuestion);

        // 3. 配置检索参数 (这里假设你有类似的配置对象，或者直接硬编码默认值)
        // 为了演示，我直接定义默认值，你可以根据实际情况替换为 serverApiSettings.RagRetrieval
        var ragOptions = new
        {
            MaxTopK = 10,
            CandidateMultiplier = 4, // 候选集倍数
            CandidateCap = 50,       // 候选集上限
            VectorWeight = 0.85,     // 向量默认权重
            KeywordWeight = 0.15     // 关键词默认权重
        };

        // 假设我们固定取 Top 3 个表结构作为上下文，或者你可以将其作为参数传入
        var topK = 5;
        var candidateCount = Math.Clamp(topK * ragOptions.CandidateMultiplier, topK, ragOptions.CandidateCap);

        var vectorWeight = ragOptions.VectorWeight;
        var keywordWeight = ragOptions.KeywordWeight;

        // 4. 获取候选集 (先取 CandidateCount 个向量最相似的)
        var candidates = await dbContext.RagChunks
            .AsNoTracking()
            .Where(c => c.Document.IsDeleted == false
                        && c.Embedding != null)
            .OrderBy(c => c.Embedding!.CosineDistance(queryVector))
            .ThenBy(c => c.ChunkIndex) // 保持原有的排序稳定性
            .Take(candidateCount)
            .Select(c => new
            {
                c.Id,
                c.Content,
                Distance = c.Embedding!.CosineDistance(queryVector)
            })
            .ToListAsync(cancellationToken);

        // 5. 混合打分与重排序 (参考代码逻辑：完全复刻打分公式)
        var hits = candidates
            .Select(c =>
            {
                var vectorScore = Math.Clamp(1 - c.Distance, 0, 1);
                var keywordScore = ComputeKeywordScore(c.Content, queryKeywords);

                // 核心公式：加权总分
                var finalScore = (vectorScore * vectorWeight) + (keywordScore * keywordWeight);

                return new
                {
                    c.Content,
                    Score = finalScore
                };
            })
            .OrderByDescending(h => h.Score) // 按加权分数重新排序
            .Take(topK) // 最终只取 Top K
            .ToList();

        // 6. 构建最终上下文字符串
        if (!hits.Any())
        {
            return "未找到相关的表结构信息。";
        }

        var contextBuilder = new StringBuilder();
        foreach (var hit in hits)
        {
            Console.WriteLine($"hit:{ hit.Content}");
            contextBuilder.AppendLine("---");
            contextBuilder.AppendLine(hit.Content);
        }

        var schema = await FilterSchemaAsync(userQuestion, contextBuilder.ToString(), cancellationToken);

        return schema;
    }

    /// <summary>
    /// 根据用户问题，利用 AI 筛选出相关的数据库 Schema 信息。
    /// 移除无关的表，只保留生成 SQL 所需的上下文。
    /// </summary>
    private async Task<string> FilterSchemaAsync(
        string userQuestion,
        string fullSchemaMarkdown,
        CancellationToken cancellationToken)
    {
        if (serviceProvider.GetService<IChatClient>() is not IChatClient chatClient)
            throw new ResourceNotFoundException("IChatClient is not available.");

        var filterAgent = chatClient.AsAIAgent(
            instructions: """
        你是一个严格的数据库上下文裁剪工具。
        你的任务是根据用户问题，从提供的“全量 Schema”中**筛选**出相关的表，并**原样保留**这些表的完整定义。

        ### 核心指令（必须遵守）：
        1. **禁止摘要/重写**：绝对不要把 Markdown 表格转换成自然语言描述。输出的表结构必须与输入中的 Markdown 格式**完全一致**。
        2. **原文摘录**：输出内容必须是输入内容的**子集**。只删除不需要的表，保留需要的表的**所有**原始文本。
        3. **外键级联（强制执行）**：
           - 在决定是否保留某个表（如 `Products`）时，**必须**检查该表下方的 `### 关联关系` 部分。
           - 如果该表通过外键引用了其他表，**必须无条件保留被引用的表**。
        4. **中间表/关联表保留（关键）**：
           - 如果用户的问题涉及两个实体（例如“查询订单包含哪些产品”），且这两个实体通过一个**中间表**（如 `OrderDetails`）进行连接，**必须保留该中间表**。
           - **识别特征**：中间表通常包含多个外键，分别指向两个主要实体（例如 `OrderId` 指向 `Orders`，`ProductId` 指向 `Products`）。
           - **规则**：如果你保留了表 A 和表 B，且存在一个表 C 同时关联了 A 和 B，请务必保留表 C，即使问题中没有明确提到表 C 的名字。
        5. **格式保持**：不要修改表名前的 `public"."` 前缀，不要修改字段名的大小写。

        ### 错误示例（绝对不要这样做）：
        - 错误：保留了 `Orders` 和 `Products`，但删除了 `OrderDetails`（导致无法 JOIN）。
        - 错误：保留了 `Orders` 表，但删除了 `Users` 表，即使 `Orders` 有 `UserId` 外键。
        - 错误：将表结构总结为“产品表包含ID和名称”。

        ### 正确示例：
        （直接输出类似以下的完整 Markdown 块）
        # 数据库表结构: public.Orders
        ...
        ### 关联关系 (外键)
         - `UserId` 关联到 * *public.Users**

        # 数据库表结构: public.Users
        ...

        # 数据库表结构: public.OrderDetails
        ...
        ### 关联关系 (外键)
         - `OrderId` 关联到 * *public.Orders**
         - `ProductId` 关联到 * *public.Products**
        """,
            name: "SchemaFilterAgent",
            description: "Filters irrelevant tables from schema context based on user question");

        ChatOptions chatOptions = new()
        {
            Temperature = 0.0f, // 极低温度，确保它不敢乱改表名
        };

        // 绑定配置
        configuration.GetRequiredSection("AI:ChatOptions").Bind(chatOptions);

        // 使用通用的 Chat 模型
        var chatModelId = configuration["AI:OpenAI:ChatModel"] ?? configuration["AI:AzureOpenAI:ChatModel"];
        if (!string.IsNullOrWhiteSpace(chatModelId))
        {
            chatOptions.ModelId = chatModelId;
        }

        var prompt = $"""
        用户问题：
        {userQuestion}

        全量数据库 Schema（注意：保留的表必须包含 `public"."` 前缀和双引号）：
        {fullSchemaMarkdown}
        """;
        Console.WriteLine($"fullSchemaMarkdown:{fullSchemaMarkdown}");
        // 执行 AI 调用
        var response = await filterAgent.RunAsync<string>(
            messages: [new(ChatRole.User, prompt)],
            cancellationToken: cancellationToken,
            options: new Microsoft.Agents.AI.ChatClientAgentRunOptions(chatOptions));

        var filteredSchema = response.Result?.Trim();

        if (string.IsNullOrWhiteSpace(filteredSchema))
        {
            // 如果 AI 认为没有表相关（极少见），可以返回一个提示，或者抛出异常
            // 这里为了健壮性，如果 AI 返回空，我们可以选择返回原 Schema 或者空字符串让后续步骤报错
            Console.WriteLine("[Schema Filter]: AI returned empty schema.");
            return fullSchemaMarkdown;
        }

        Console.WriteLine( $"\n[Schema Filter]: Successfully filtered schema. Length: {filteredSchema.Length}");
        return filteredSchema;
    }

    [GeneratedRegex(@"\bINSERT\s+INTO\s+(?<id>(?:(?:""[^""]+""|\w+)\.)?(?:""[^""]+""|\w+))", RegexOptions.IgnoreCase)]
    private static partial Regex InsertTableRegex();

    [GeneratedRegex(@"\bUPDATE\s+(?<id>(?:(?:""[^""]+""|\w+)\.)?(?:""[^""]+""|\w+))", RegexOptions.IgnoreCase)]
    private static partial Regex UpdateTableRegex();

    [GeneratedRegex(@"\bDELETE\s+FROM\s+(?<id>(?:(?:""[^""]+""|\w+)\.)?(?:""[^""]+""|\w+))", RegexOptions.IgnoreCase)]
    private static partial Regex DeleteTableRegex();

    private static string NormalizeWriteSqlTableIdentifiers(string sql, List<PgTableNameInfo> accessibleTables)
    {
        return WriteSqlRegex.IsMatch(sql) switch
        {
            true when sql.TrimStart().StartsWith("insert", StringComparison.OrdinalIgnoreCase) =>
                InsertTableRegex().Replace(sql, m => $"INSERT INTO {ResolveAndQuoteTableIdentifier(m.Groups["id"].Value, accessibleTables)}"),
            true when sql.TrimStart().StartsWith("update", StringComparison.OrdinalIgnoreCase) =>
                UpdateTableRegex().Replace(sql, m => $"UPDATE {ResolveAndQuoteTableIdentifier(m.Groups["id"].Value, accessibleTables)}"),
            true when sql.TrimStart().StartsWith("delete", StringComparison.OrdinalIgnoreCase) =>
                DeleteTableRegex().Replace(sql, m => $"DELETE FROM {ResolveAndQuoteTableIdentifier(m.Groups["id"].Value, accessibleTables)}"),
            _ => sql
        };
    }

    private static string ResolveAndQuoteTableIdentifier(string input, List<PgTableNameInfo> accessibleTables)
    {
        var raw = input.Replace("\"", string.Empty);
        var parts = raw.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (parts.Length == 2)
        {
            var match = accessibleTables.FirstOrDefault(t =>
                t.Schema.Equals(parts[0], StringComparison.OrdinalIgnoreCase) &&
                t.Table.Equals(parts[1], StringComparison.OrdinalIgnoreCase));
            if (match is null)
                throw new ValidationException($"Table '{input}' is not accessible.");
            return $"{QuoteIdentifier(match.Schema)}.{QuoteIdentifier(match.Table)}";
        }

        if (parts.Length == 1)
        {
            var candidates = accessibleTables
                .Where(t => t.Table.Equals(parts[0], StringComparison.OrdinalIgnoreCase))
                .ToArray();
            if (candidates.Length == 0)
                throw new ValidationException($"Table '{input}' is not accessible.");
            if (candidates.Length > 1)
                throw new ValidationException($"Table '{input}' is ambiguous. Use schema-qualified table name.");
            return $"{QuoteIdentifier(candidates[0].Schema)}.{QuoteIdentifier(candidates[0].Table)}";
        }

        throw new ValidationException($"Invalid table identifier '{input}'.");
    }

    private static PgTableNameInfo ResolveWriteTargetTableInfo(string sql, List<PgTableNameInfo> accessibleTables)
    {
        Match match;
        if (sql.TrimStart().StartsWith("insert", StringComparison.OrdinalIgnoreCase))
            match = InsertTableRegex().Match(sql);
        else if (sql.TrimStart().StartsWith("update", StringComparison.OrdinalIgnoreCase))
            match = UpdateTableRegex().Match(sql);
        else
            match = DeleteTableRegex().Match(sql);

        if (match.Success is false)
            throw new ValidationException("Unable to resolve target table from SQL.");

        return ResolveTableInfoFromIdentifier(match.Groups["id"].Value, accessibleTables);
    }

    private static PgTableNameInfo ResolveTableInfoFromIdentifier(string input, List<PgTableNameInfo> accessibleTables)
    {
        var raw = input.Replace("\"", string.Empty);
        var parts = raw.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (parts.Length == 2)
        {
            return accessibleTables.FirstOrDefault(t =>
                       t.Schema.Equals(parts[0], StringComparison.OrdinalIgnoreCase) &&
                       t.Table.Equals(parts[1], StringComparison.OrdinalIgnoreCase))
                   ?? throw new ValidationException($"Table '{input}' is not accessible.");
        }

        if (parts.Length == 1)
        {
            var candidates = accessibleTables
                .Where(t => t.Table.Equals(parts[0], StringComparison.OrdinalIgnoreCase))
                .ToArray();
            if (candidates.Length == 0)
                throw new ValidationException($"Table '{input}' is not accessible.");
            if (candidates.Length > 1)
                throw new ValidationException($"Table '{input}' is ambiguous. Use schema-qualified table name.");
            return candidates[0];
        }

        throw new ValidationException($"Invalid table identifier '{input}'.");
    }

    [GeneratedRegex(@"\bINSERT\s+INTO\s+(?<table>(?:(?:""[^""]+""|\w+)\.)?(?:""[^""]+""|\w+))\s*\((?<cols>[^)]*)\)", RegexOptions.IgnoreCase)]
    private static partial Regex InsertColsRegex();

    [GeneratedRegex(@"\bSET\s+(?<set>[\s\S]*?)(?<tail>\s+WHERE\s+[\s\S]*$|$)", RegexOptions.IgnoreCase)]
    private static partial Regex UpdateSetRegex();

    private static string NormalizeWriteSqlColumnIdentifiers(string sql, PgTableNameInfo tableInfo)
    {
        if (sql.TrimStart().StartsWith("insert", StringComparison.OrdinalIgnoreCase))
        {
            return InsertColsRegex().Replace(sql,
                m =>
                {
                    var cols = m.Groups["cols"].Value
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .Select(c => QuoteIdentifier(ResolveColumnName(tableInfo, c).Name))
                        .ToArray();
                    return $"INSERT INTO {m.Groups["table"].Value} ({string.Join(", ", cols)})";
                });
        }

        if (sql.TrimStart().StartsWith("update", StringComparison.OrdinalIgnoreCase))
        {
            var normalized = UpdateSetRegex().Replace(sql,
                m =>
                {
                    var assignments = m.Groups["set"].Value
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .Select(a =>
                        {
                            var idx = a.IndexOf('=');
                            if (idx <= 0) return a;
                            var left = a[..idx].Trim();
                            var right = a[(idx + 1)..].Trim();
                            return $"{QuoteIdentifier(ResolveColumnName(tableInfo, left).Name)} = {right}";
                        });
                    return $"SET {string.Join(", ", assignments)}{m.Groups["tail"].Value}";
                });
            return NormalizeWhereColumns(normalized, tableInfo);
        }

        return NormalizeWhereColumns(sql, tableInfo);
    }

    [GeneratedRegex(@"(?<lhs>(?:""[^""]+""|\w+)(?:\.(?:""[^""]+""|\w+))?)\s*(?<op>=|<>|!=|<=|>=|<|>|\bLIKE\b|\bIN\b|\bIS\b)", RegexOptions.IgnoreCase)]
    private static partial Regex WhereColumnsRegex();

    private static string NormalizeWhereColumns(string sql, PgTableNameInfo tableInfo)
    {
        return WhereColumnsRegex().Replace(sql,
            m =>
            {
                var left = m.Groups["lhs"].Value;
                if (left.Contains('.', StringComparison.Ordinal))
                    return $"{left} {m.Groups["op"].Value}";

                if (tableInfo.Columns.ContainsKey(UnwrapIdentifier(left)))
                {
                    return $"{QuoteIdentifier(ResolveColumnName(tableInfo, left).Name)} {m.Groups["op"].Value}";
                }

                return m.Value;
            });
    }

    private static PgTableColumnInfo ResolveColumnName(PgTableNameInfo tableInfo, string inputColumnName)
    {
        var key = UnwrapIdentifier(inputColumnName);
        if (key.Contains('.', StringComparison.Ordinal))
            key = key.Split('.').Last();

        if (tableInfo.Columns.TryGetValue(key, out var resolved))
            return resolved;

        throw new ValidationException($"Column '{inputColumnName}' does not exist in table '{tableInfo.Schema}.{tableInfo.Table}'.");
    }

    private static string UnwrapIdentifier(string input) => input.Replace("\"", string.Empty).Trim();

    [GeneratedRegex(@"\bINSERT\s+INTO\s+(?<table>(?:(?:""[^""]+""|\w+)\.)?(?:""[^""]+""|\w+))\s*\((?<cols>[^)]*)\)\s*VALUES\s*\((?<vals>[\s\S]*)\)(?:\s*RETURNING[\s\S]*)?(?:\s*;)?\s*$", RegexOptions.IgnoreCase)]
    private static partial Regex InsertFullRegex();

    private static string EnrichInsertSqlWithRequiredColumns(string sql, PgTableNameInfo tableInfo)
    {
        if (sql.TrimStart().StartsWith("insert", StringComparison.OrdinalIgnoreCase) is false)
            return sql;

        // 【移除子查询短路逻辑】：因为我们已经升级了 SplitSqlCsv 方法，它现在能够完美解析带有子查询的复杂 VALUES。
        // 所以我们允许程序继续执行，为 AI 生成的带子查询的 SQL 自动补全像 CreatedOn 这样的非空字段。

        var match = InsertFullRegex().Match(sql);
        if (match.Success is false)
            return sql;

        var cols = SplitSqlCsv(match.Groups["cols"].Value).Select(UnwrapIdentifier).ToList();
        var vals = SplitSqlCsv(match.Groups["vals"].Value).ToList();
        
        if (cols.Count != vals.Count)
        {
            Console.WriteLine($"[EnrichInsertSqlWithRequiredColumns] Column count ({cols.Count}) does not match value count ({vals.Count}). SQL: {sql}");
            return sql;
        }

        foreach (var column in tableInfo.Columns.Values)
        {
            if (cols.Contains(column.Name, StringComparer.OrdinalIgnoreCase))
                continue;

            // 如果列允许为空，或者数据库层面已经有默认值，或者它是由数据库自动生成的主键（如 serial, identity），跳过
            if (column.IsNullable || string.IsNullOrWhiteSpace(column.ColumnDefault) is false || column.ColumnDefault.Contains("nextval", StringComparison.OrdinalIgnoreCase))
                continue;

            var generated = BuildGeneratedColumnValue(column);
            if (generated is null)
                continue;

            cols.Add(column.Name);
            vals.Add(generated);
        }

        var rebuilt = $"INSERT INTO {match.Groups["table"].Value} ({string.Join(", ", cols.Select(QuoteIdentifier))}) VALUES ({string.Join(", ", vals)});";
        Console.WriteLine($"EnrichInsertSqlWithRequiredColumns-rebuilt:{rebuilt}");
        return rebuilt;
    }

    private static List<string> SplitSqlCsv(string input)
    {
        var result = new List<string>();
        var sb = new System.Text.StringBuilder();
        var depth = 0;
        var inSingleQuote = false;
        var inDoubleQuote = false;

        foreach (var ch in input)
        {
            if (ch == '\'' && !inDoubleQuote && (sb.Length == 0 || sb[^1] != '\\'))
                inSingleQuote = !inSingleQuote;
                
            if (ch == '"' && !inSingleQuote && (sb.Length == 0 || sb[^1] != '\\'))
                inDoubleQuote = !inDoubleQuote;

            if (!inSingleQuote && !inDoubleQuote)
            {
                if (ch == '(') depth++;
                if (ch == ')') depth--;
                if (ch == ',' && depth == 0)
                {
                    result.Add(sb.ToString().Trim());
                    sb.Clear();
                    continue;
                }
            }

            sb.Append(ch);
        }

        if (sb.Length > 0)
            result.Add(sb.ToString().Trim());

        return result;
    }

    private static float[] Normalize(float[] vector)
    {
        var sum = 0d;
        for (var i = 0; i < vector.Length; i++)
        {
            sum += vector[i] * vector[i];
        }

        var norm = Math.Sqrt(sum);
        if (norm <= 0)
            return vector;

        for (var i = 0; i < vector.Length; i++)
        {
            vector[i] = (float)(vector[i] / norm);
        }

        return vector;
    }

    private static HashSet<string> Tokenize(string text)
    {
        return [.. text.Split([' ', ',', '.', ';', ':', '-', '_', '/', '\\', '\n', '\r', '\t'], StringSplitOptions.RemoveEmptyEntries)
                       .Select(t => t.Trim().ToLowerInvariant())
                       .Where(t => t.Length > 1)];
    }

    private static double ComputeKeywordScore(string content, HashSet<string> queryKeywords)
    {
        if (queryKeywords.Count == 0 || string.IsNullOrWhiteSpace(content))
            return 0;

        var chunkKeywords = Tokenize(content);
        if (chunkKeywords.Count == 0)
            return 0;

        var matched = queryKeywords.Count(chunkKeywords.Contains);
        return (double)matched / queryKeywords.Count;
    }
}
