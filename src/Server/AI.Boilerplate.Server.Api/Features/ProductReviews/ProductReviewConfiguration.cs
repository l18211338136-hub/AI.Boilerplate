using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.Boilerplate.Server.Api.Features.ProductReviews;

public partial class ProductReviewConfiguration : IEntityTypeConfiguration<ProductReview>
{
    public void Configure(EntityTypeBuilder<ProductReview> builder)
    {
        builder.ToTable(t => t.HasComment("产品评价表"));
        builder.Property(p => p.Id).HasComment("主键ID");
        builder.Property(p => p.OrderId).HasComment("订单ID");
        builder.Property(p => p.ProductId).HasComment("产品ID");
        builder.Property(p => p.UserId).HasComment("用户ID");
        builder.Property(p => p.Rating).HasComment("评分(1-5)");
        builder.Property(p => p.Comment).HasComment("评价内容");
        builder.Property(p => p.IsAnonymous).HasComment("是否匿名显示");
        builder.Property(p => p.CreatedOn).HasComment("评价时间");

        // 订单ID和产品ID的组合唯一索引，确保每个订单中的每个产品只能有一个评价
        builder.HasIndex(p => new { p.OrderId, p.ProductId }).IsUnique();
        // 产品ID索引，用于查询产品的所有评价
        builder.HasIndex(p => p.ProductId);
        // 用户ID索引，用于查询用户的所有评价
        builder.HasIndex(p => p.UserId);
        // 评分索引，用于按评分筛选评价
        builder.HasIndex(p => p.Rating);
    }
}
