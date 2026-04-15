namespace AI.Boilerplate.Server.Api.Features.Todo;

public class TodoConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.ToTable(t => t.HasComment("待办事项表"));
        builder.Property(t => t.Id).HasComment("主键ID");
        builder.Property(t => t.Title).HasComment("待办标题");
        builder.Property(t => t.IsDone).HasComment("是否完成");
        builder.Property(t => t.UserId).HasComment("所属用户ID");
        builder.Property(t => t.CreatedOn).HasComment("创建时间");
        builder.Property(t => t.CreatedBy).HasComment("创建人ID");
        builder.Property(t => t.ModifiedOn).HasComment("最后修改时间");
        builder.Property(t => t.ModifiedBy).HasComment("最后修改人ID");
        builder.Property(t => t.IsDeleted).HasComment("是否删除");
        builder.Property(t => t.DeletedOn).HasComment("删除时间");
        builder.Property(t => t.DeletedBy).HasComment("删除人ID");

        builder.HasOne(t => t.User)
            .WithMany(u => u.TodoItems)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
