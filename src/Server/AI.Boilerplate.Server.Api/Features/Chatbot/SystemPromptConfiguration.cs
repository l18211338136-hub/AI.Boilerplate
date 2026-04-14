using AI.Boilerplate.Shared.Features.Chatbot;

namespace AI.Boilerplate.Server.Api.Features.Chatbot;

public class SystemPromptConfiguration : IEntityTypeConfiguration<SystemPrompt>
{
    public void Configure(EntityTypeBuilder<SystemPrompt> builder)
    {
        builder.ToTable(t => t.HasComment("系统提示词配置表"));
        builder.Property(sp => sp.Id).HasComment("主键ID");
        builder.Property(sp => sp.PromptKind).HasComment("提示词类型");
        builder.Property(sp => sp.Markdown).HasComment("提示词内容");
        builder.Property(sp => sp.Version).HasComment("并发版本");

        builder.HasIndex(sp => sp.PromptKind)
            .IsUnique();
    }
}
