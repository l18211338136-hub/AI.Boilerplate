namespace AI.Boilerplate.Server.Api.Features.Attachments;

public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.ToTable(t => t.HasComment("附件表"));
        builder.Property(attachment => attachment.Id).HasComment("附件ID");
        builder.Property(attachment => attachment.Kind).HasComment("附件类型");
        builder.Property(attachment => attachment.Path).HasComment("附件路径");

        builder.HasKey(attachment => new { attachment.Id, attachment.Kind });
    }
}
