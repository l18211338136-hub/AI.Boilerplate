using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI.Boilerplate.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCategoryColorCommentFormat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Categories",
                type: "text",
                nullable: true,
                comment: "类别颜色（HEX，例如 #FFCD56）",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "类别颜色");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Categories",
                type: "text",
                nullable: true,
                comment: "类别颜色",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "类别颜色（HEX，例如 #FFCD56）");
        }
    }
}
