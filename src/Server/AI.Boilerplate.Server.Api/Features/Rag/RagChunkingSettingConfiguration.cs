namespace AI.Boilerplate.Server.Api.Features.Rag;

public class RagChunkingSettingConfiguration : IEntityTypeConfiguration<RagChunkingSetting>
{
    public void Configure(EntityTypeBuilder<RagChunkingSetting> builder)
    {
        builder.ToTable(t => t.HasComment("RAG 分片规则配置"));
        builder.Property(x => x.MaxChunkLength).HasComment("最大分片长度");
        builder.Property(x => x.PreferParagraphFirst).HasComment("是否优先按段落分片");
        builder.Property(x => x.MinChunkCount).HasComment("最小分片数");
        builder.Property(x => x.UpdatedAt).HasComment("更新时间");
        builder.Property(x => x.Version).HasComment("并发版本");
    }
}
