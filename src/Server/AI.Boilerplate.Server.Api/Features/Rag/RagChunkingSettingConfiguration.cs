namespace AI.Boilerplate.Server.Api.Features.Rag;

public class RagChunkingSettingConfiguration : IEntityTypeConfiguration<RagChunkingSetting>
{
    public void Configure(EntityTypeBuilder<RagChunkingSetting> builder)
    {
        builder.ToTable(t => t.HasComment("RAG 分片规则配置"));
        builder.Property(x => x.Id).HasComment("主键ID");
        builder.Property(x => x.MaxChunkLength).HasComment("最大分片长度");
        builder.Property(x => x.PreferParagraphFirst).HasComment("是否优先按段落分片");
        builder.Property(x => x.MinChunkCount).HasComment("最小分片数");
        builder.Property(x => x.CreatedOn).HasComment("创建时间");
        builder.Property(x => x.CreatedBy).HasComment("创建人ID");
        builder.Property(x => x.ModifiedOn).HasComment("最后修改时间");
        builder.Property(x => x.ModifiedBy).HasComment("最后修改人ID");
        builder.Property(x => x.IsDeleted).HasComment("是否删除");
        builder.Property(x => x.DeletedOn).HasComment("删除时间");
        builder.Property(x => x.DeletedBy).HasComment("删除人ID");
        builder.Property(x => x.Version).HasComment("并发版本");
    }
}