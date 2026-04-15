using AI.Boilerplate.Server.Api.Infrastructure.Data.Audit;
using AI.Boilerplate.Server.Api.Features.Identity.Models;
using StackExchange.Redis;

namespace AI.Boilerplate.Server.Api.Features.Addresses;

public partial class Address : AuditEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    public Guid? UserId { get; set; }

    [MaxLength(64)]
    [Comment("收件人姓名")]
    public string? RecipientName { get; set; }

    [MaxLength(20)]
    [Comment("联系电话")]
    public string? PhoneNumber { get; set; }

    [MaxLength(32)]
    [Comment("省")]
    public string? Province { get; set; }

    [MaxLength(32)]
    [Comment("市")]
    public string? City { get; set; }

    [MaxLength(32)]
    [Comment("区/县")]
    public string? District { get; set; }

    [MaxLength(256)]
    [Comment("详细地址")]
    public string? StreetAddress { get; set; }

    [MaxLength(10)]
    [Comment("邮政编码")]
    public string? PostalCode { get; set; }

    [Comment("是否默认地址")]
    public bool? IsDefault { get; set; }

    // 导航属性
    public IList<Order> Orders { get; set; } = [];
}
