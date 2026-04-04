using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI.Boilerplate.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class PersistRagChunkingSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RagChunkingSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MaxChunkLength = table.Column<int>(type: "integer", nullable: false, comment: "最大分片长度"),
                    PreferParagraphFirst = table.Column<bool>(type: "boolean", nullable: false, comment: "是否优先按段落分片"),
                    MinChunkCount = table.Column<int>(type: "integer", nullable: false, comment: "最小分片数"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "更新时间"),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false, comment: "并发版本")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RagChunkingSettings", x => x.Id);
                },
                comment: "RAG 分片规则配置");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RagChunkingSettings");
        }
    }
}
