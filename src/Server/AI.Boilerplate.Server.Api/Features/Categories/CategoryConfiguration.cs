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
        
        builder.Property(p => p.CreatedOn).HasComment("创建时间");
        builder.Property(p => p.CreatedBy).HasComment("创建人ID");
        builder.Property(p => p.ModifiedOn).HasComment("最后修改时间");
        builder.Property(p => p.ModifiedBy).HasComment("最后修改人ID");
        builder.Property(p => p.IsDeleted).HasComment("是否删除");
        builder.Property(p => p.DeletedOn).HasComment("删除时间");
        builder.Property(p => p.DeletedBy).HasComment("删除人ID");

        builder.HasIndex(p => p.Name).IsUnique();
    }
}

