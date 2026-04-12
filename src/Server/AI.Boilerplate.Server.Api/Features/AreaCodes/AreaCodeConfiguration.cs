using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.Boilerplate.Server.Api.Features.AreaCodes;

public class AreaCodeConfiguration : IEntityTypeConfiguration<AreaCode>
{
    public void Configure(EntityTypeBuilder<AreaCode> builder)
    {
        // 表名与表注释
        builder.ToTable("AreaCodes", t => t.HasComment("行政区划代码表"));

        // 字段映射、约束与注释
        builder.Property(a => a.Code)
               .HasColumnName("Code")
               .HasComment("区划代码");

        builder.Property(a => a.Name)
               .HasColumnName("Name")
               .HasMaxLength(128)
               .IsRequired()
               .HasDefaultValue(string.Empty)
               .HasComment("名称");

        builder.Property(a => a.Level)
               .HasColumnName("Level")
               .HasComment("级别1-5,省市县镇村");

        builder.Property(a => a.Pcode)
               .HasColumnName("Pcode")
               .HasComment("父级区划代码");

        builder.Property(a => a.Category)
               .HasColumnName("Category")
               .HasComment("城乡分类");

        // 主键
        builder.HasKey(a => a.Code);

        // 💡 可选：配置自引用层级关系（推荐，便于后续树形查询）
        builder.HasOne<AreaCode>()
               .WithMany()
               .HasForeignKey(a => a.Pcode)
               .HasConstraintName("FK_AreaCode_ParentCode");
    }
}
