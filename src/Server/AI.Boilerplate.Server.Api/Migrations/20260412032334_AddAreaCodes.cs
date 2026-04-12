using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AI.Boilerplate.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddAreaCodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AreaCodes",
                columns: table => new
                {
                    Code = table.Column<long>(type: "bigint", nullable: false, comment: "区划代码")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, defaultValue: "", comment: "名称"),
                    Level = table.Column<short>(type: "smallint", nullable: false, comment: "级别1-5,省市县镇村"),
                    Pcode = table.Column<long>(type: "bigint", nullable: true, comment: "父级区划代码"),
                    Category = table.Column<int>(type: "integer", nullable: false, comment: "城乡分类")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaCodes", x => x.Code);
                    table.ForeignKey(
                        name: "FK_AreaCode_ParentCode",
                        column: x => x.Pcode,
                        principalTable: "AreaCodes",
                        principalColumn: "Code");
                },
                comment: "行政区划代码表");

            migrationBuilder.CreateIndex(
                name: "IX_AreaCodes_Pcode",
                table: "AreaCodes",
                column: "Pcode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AreaCodes");
        }
    }
}
