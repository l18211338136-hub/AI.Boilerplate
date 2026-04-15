using AI.Boilerplate.Server.Api.Features.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.Boilerplate.Server.Api.Features.Identity.Configurations;

public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.ToTable(t => t.HasComment("用户会话表"));

        builder.Property(s => s.Id).HasComment("会话ID");
        builder.Property(s => s.IP).HasComment("登录IP地址");
        builder.Property(s => s.Address).HasComment("登录地理位置");
        builder.Property(s => s.Privileged).HasComment("是否提权(提权访问)");
        builder.Property(s => s.StartedOn).HasComment("会话开始时间(Unix时间戳秒)");
        builder.Property(s => s.RenewedOn).HasComment("会话续期时间(Unix时间戳秒)");
        builder.Property(s => s.UserId).HasComment("关联的用户ID");
        builder.Property(s => s.SignalRConnectionId).HasComment("SignalR连接ID");
        builder.Property(s => s.NotificationStatus).HasComment("推送通知状态");
        builder.Property(s => s.DeviceInfo).HasComment("设备信息");
        builder.Property(s => s.PlatformType).HasComment("平台类型(App/Web等)");
        builder.Property(s => s.CultureName).HasComment("用户选择的语言文化");
        builder.Property(s => s.AppVersion).HasComment("应用程序版本号");
        
        builder.Property(s => s.CreatedOn).HasComment("创建时间");
        builder.Property(s => s.CreatedBy).HasComment("创建人ID");
        builder.Property(s => s.ModifiedOn).HasComment("最后修改时间");
        builder.Property(s => s.ModifiedBy).HasComment("最后修改人ID");
        builder.Property(s => s.IsDeleted).HasComment("是否删除");
        builder.Property(s => s.DeletedOn).HasComment("删除时间");
        builder.Property(s => s.DeletedBy).HasComment("删除人ID");
    }
}
