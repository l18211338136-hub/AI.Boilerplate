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
        builder.Property(t => t.UpdatedAt).HasComment("更新时间");

        builder.HasOne(t => t.User)
            .WithMany(u => u.TodoItems)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
