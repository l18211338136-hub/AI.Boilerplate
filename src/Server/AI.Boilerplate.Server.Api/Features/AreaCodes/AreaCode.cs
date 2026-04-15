using AI.Boilerplate.Server.Api.Infrastructure.Data.Audit;

namespace AI.Boilerplate.Server.Api.Features.AreaCodes;

public class AreaCode : AuditEntity
{
    /// <summary>
    /// 区划代码
    /// </summary>
    public long Code { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 级别1-5,省市县镇村
    /// </summary>
    public short Level { get; set; }

    /// <summary>
    /// 父级区划代码
    /// </summary>
    public long? Pcode { get; set; }

    /// <summary>
    /// 城乡分类
    /// </summary>
    public int Category { get; set; }
}
