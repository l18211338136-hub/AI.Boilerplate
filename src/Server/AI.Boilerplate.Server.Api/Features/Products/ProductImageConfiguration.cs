using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.Boilerplate.Server.Api.Features.Products;

public partial class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable(t => t.HasComment("产品图片表"));
        builder.Property(p => p.Id).HasComment("主键ID");
        builder.Property(p => p.ProductId).HasComment("产品ID");
        builder.Property(p => p.ImageUrl).HasComment("图片存储URL");
        builder.Property(p => p.AltText).HasComment("图片替代文本");
        builder.Property(p => p.SortOrder).HasComment("排序权重(升序)");
        builder.Property(p => p.IsPrimary).HasComment("是否为主图");
        builder.Property(p => p.CreatedOn).HasComment("创建时间");
        builder.Property(p => p.CreatedBy).HasComment("创建人ID");
        builder.Property(p => p.ModifiedOn).HasComment("最后修改时间");
        builder.Property(p => p.ModifiedBy).HasComment("最后修改人ID");
        builder.Property(p => p.IsDeleted).HasComment("是否删除");
        builder.Property(p => p.DeletedOn).HasComment("删除时间");
        builder.Property(p => p.DeletedBy).HasComment("删除人ID");

        // 产品ID索引，用于查询产品的所有图片
        builder.HasIndex(p => p.ProductId);
        // 为每个产品配置主图的唯一性
        builder.HasIndex(p => new { p.ProductId, p.IsPrimary })
            .HasFilter("\"IsPrimary\" = true")
            .IsUnique();
    }
}
