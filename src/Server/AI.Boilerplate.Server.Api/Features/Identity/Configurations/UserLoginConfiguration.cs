using AI.Boilerplate.Server.Api.Features.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.Boilerplate.Server.Api.Features.Identity.Configurations;

public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
{
    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        builder.ToTable(t => t.HasComment("外部用户登录表(存储第三方OAuth授权记录)"));

        builder.Property(l => l.LoginProvider).HasComment("登录提供商名称(如: Google, GitHub)");
        builder.Property(l => l.ProviderKey).HasComment("提供商分配的唯一标识(ProviderKey)");
        builder.Property(l => l.ProviderDisplayName).HasComment("提供商显示名称");
        builder.Property(l => l.UserId).HasComment("关联的用户ID");
        
        builder.Property(l => l.CreatedOn).HasComment("创建时间");
        builder.Property(l => l.CreatedBy).HasComment("创建人ID");
        builder.Property(l => l.ModifiedOn).HasComment("最后修改时间");
        builder.Property(l => l.ModifiedBy).HasComment("最后修改人ID");
        builder.Property(l => l.IsDeleted).HasComment("是否删除");
        builder.Property(l => l.DeletedOn).HasComment("删除时间");
        builder.Property(l => l.DeletedBy).HasComment("删除人ID");
    }
}
