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
        
        builder.Property(sp => sp.CreatedOn).HasComment("创建时间");
        builder.Property(sp => sp.CreatedBy).HasComment("创建人ID");
        builder.Property(sp => sp.ModifiedOn).HasComment("最后修改时间");
        builder.Property(sp => sp.ModifiedBy).HasComment("最后修改人ID");
        builder.Property(sp => sp.IsDeleted).HasComment("是否删除");
        builder.Property(sp => sp.DeletedOn).HasComment("删除时间");
        builder.Property(sp => sp.DeletedBy).HasComment("删除人ID");

        builder.HasIndex(sp => sp.PromptKind)
            .IsUnique();
    }
}
