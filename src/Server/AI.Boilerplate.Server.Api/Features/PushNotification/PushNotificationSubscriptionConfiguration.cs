namespace AI.Boilerplate.Server.Api.Features.PushNotification;

public class PushNotificationSubscriptionConfiguration : IEntityTypeConfiguration<PushNotificationSubscription>
{
    public void Configure(EntityTypeBuilder<PushNotificationSubscription> builder)
    {
        builder.ToTable(t => t.HasComment("推送订阅表"));
        builder.Property(sub => sub.Id).HasComment("主键ID");
        builder.Property(sub => sub.DeviceId).HasComment("设备ID");
        builder.Property(sub => sub.Platform).HasComment("平台类型");
        builder.Property(sub => sub.PushChannel).HasComment("推送通道");
        builder.Property(sub => sub.P256dh).HasComment("WebPush公钥");
        builder.Property(sub => sub.Auth).HasComment("WebPush认证值");
        builder.Property(sub => sub.Endpoint).HasComment("推送端点");
        builder.Property(sub => sub.UserSessionId).HasComment("用户会话ID");
        builder.Property(sub => sub.Tags).HasComment("订阅标签");
        builder.Property(sub => sub.ExpirationTime).HasComment("过期时间(Unix秒)");
        builder.Property(sub => sub.RenewedOn).HasComment("续订时间(Unix秒)");

        builder
            .HasOne(sub => sub.UserSession)
            .WithOne(us => us.PushNotificationSubscription)
            .HasForeignKey<PushNotificationSubscription>(sub => sub.UserSessionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .HasIndex(b => b.UserSessionId)
            .HasFilter($"'{nameof(PushNotificationSubscription.UserSessionId)}' IS NOT NULL")
            .IsUnique();
    }
}
