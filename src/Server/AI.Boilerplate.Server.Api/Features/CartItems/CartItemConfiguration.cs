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
        builder.Property(p => p.AddedOn).HasComment("加入购物车时间");
        builder.Property(p => p.UpdatedOn).HasComment("更新时间");

        // 每个用户对每个产品只能有一个购物车项
        builder.HasIndex(p => new { p.UserId, p.ProductId }).IsUnique();
        // 用户ID索引，用于查询用户的购物车
        builder.HasIndex(p => p.UserId);
    }
}
