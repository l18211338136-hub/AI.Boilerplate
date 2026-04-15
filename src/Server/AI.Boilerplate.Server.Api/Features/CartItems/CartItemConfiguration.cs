using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.Boilerplate.Server.Api.Features.CartItems;

public partial class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable(t => t.HasComment("购物车项表"));
        builder.Property(p => p.Id).HasComment("主键ID");
        builder.Property(p => p.UserId).HasComment("用户ID");
        builder.Property(p => p.ProductId).HasComment("产品ID");
        builder.Property(p => p.Quantity).HasComment("数量");
        builder.Property(p => p.Selected).HasComment("结算是否勾选");
        builder.Property(p => p.CreatedOn).HasComment("创建时间");
        builder.Property(p => p.CreatedBy).HasComment("创建人ID");
        builder.Property(p => p.ModifiedOn).HasComment("最后修改时间");
        builder.Property(p => p.ModifiedBy).HasComment("最后修改人ID");
        builder.Property(p => p.IsDeleted).HasComment("是否删除");
        builder.Property(p => p.DeletedOn).HasComment("删除时间");
        builder.Property(p => p.DeletedBy).HasComment("删除人ID");

        // 每个用户对每个产品只能有一个购物车项
        builder.HasIndex(p => new { p.UserId, p.ProductId }).IsUnique();
        // 用户ID索引，用于查询用户的购物车
        builder.HasIndex(p => p.UserId);
    }
}
