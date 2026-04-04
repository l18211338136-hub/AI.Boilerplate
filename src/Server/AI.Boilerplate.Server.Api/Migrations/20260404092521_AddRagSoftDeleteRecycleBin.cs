using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI.Boilerplate.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRagSoftDeleteRecycleBin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "RagKnowledgeBases",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RagKnowledgeBases",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "RagDocuments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RagDocuments",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "RagKnowledgeBases");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RagKnowledgeBases");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "RagDocuments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RagDocuments");
        }
    }
}
