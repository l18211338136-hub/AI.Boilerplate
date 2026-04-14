namespace AI.Boilerplate.Server.Api.Features.Categories;

public partial class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable(t => t.HasComment("产品类别表"));
        builder.Property(p => p.Id).HasComment("主键ID");
        builder.Property(p => p.Name).HasComment("类别名称");
        builder.Property(p => p.Color).HasComment("类别颜色（HEX，例如 #FFCD56）");
        builder.Property(p => p.Version).HasComment("并发版本");

        builder.HasIndex(p => p.Name).IsUnique();
    }
}

