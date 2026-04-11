using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Pgvector;

#nullable disable

namespace AI.Boilerplate.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRagTableComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "RagKnowledgeBases",
                comment: "RAG 知识库表");

            migrationBuilder.AlterTable(
                name: "RagDocuments",
                comment: "RAG 文档表");

            migrationBuilder.AlterTable(
                name: "RagChunks",
                comment: "RAG 文本分片表");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "RagKnowledgeBases",
                type: "timestamp with time zone",
                nullable: false,
                comment: "更新时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RagKnowledgeBases",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                comment: "知识库名称",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<bool>(
                name: "IsEnabled",
                table: "RagKnowledgeBases",
                type: "boolean",
                nullable: false,
                comment: "是否启用",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RagKnowledgeBases",
                type: "boolean",
                nullable: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "EmbeddingModel",
                table: "RagKnowledgeBases",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                comment: "嵌入模型",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<int>(
                name: "EmbeddingDimension",
                table: "RagKnowledgeBases",
                type: "integer",
                nullable: false,
                comment: "嵌入维度",
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "RagKnowledgeBases",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "RagKnowledgeBases",
                type: "timestamp with time zone",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "RagKnowledgeBases",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                comment: "知识库编码",
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "RagKnowledgeBases",
                type: "uuid",
                nullable: false,
                comment: "主键ID",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "RagDocuments",
                type: "timestamp with time zone",
                nullable: false,
                comment: "更新时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "RagDocuments",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                comment: "文档标题",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "SourceType",
                table: "RagDocuments",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                comment: "来源类型",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "SourceId",
                table: "RagDocuments",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                comment: "来源ID",
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastIndexedAt",
                table: "RagDocuments",
                type: "timestamp with time zone",
                nullable: false,
                comment: "最后索引时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "KnowledgeBaseId",
                table: "RagDocuments",
                type: "uuid",
                nullable: false,
                comment: "知识库ID",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RagDocuments",
                type: "boolean",
                nullable: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "RagDocuments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "RagDocuments",
                type: "timestamp with time zone",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "RagDocuments",
                type: "uuid",
                nullable: false,
                comment: "主键ID",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "RagChunks",
                type: "timestamp with time zone",
                nullable: false,
                comment: "更新时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<int>(
                name: "TokenCount",
                table: "RagChunks",
                type: "integer",
                nullable: false,
                comment: "令牌数量",
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Vector>(
                name: "Embedding",
                table: "RagChunks",
                type: "vector(768)",
                nullable: true,
                comment: "向量嵌入",
                oldClrType: typeof(Vector),
                oldType: "vector(768)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DocumentId",
                table: "RagChunks",
                type: "uuid",
                nullable: false,
                comment: "文档ID",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "RagChunks",
                type: "timestamp with time zone",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "RagChunks",
                type: "text",
                nullable: false,
                comment: "分片内容",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "ChunkIndex",
                table: "RagChunks",
                type: "integer",
                nullable: false,
                comment: "分片索引",
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "RagChunks",
                type: "uuid",
                nullable: false,
                comment: "主键ID",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "RagChunkingSettings",
                type: "uuid",
                nullable: false,
                comment: "主键ID",
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "RagKnowledgeBases",
                oldComment: "RAG 知识库表");

            migrationBuilder.AlterTable(
                name: "RagDocuments",
                oldComment: "RAG 文档表");

            migrationBuilder.AlterTable(
                name: "RagChunks",
                oldComment: "RAG 文本分片表");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "RagKnowledgeBases",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "更新时间");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RagKnowledgeBases",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldComment: "知识库名称");

            migrationBuilder.AlterColumn<bool>(
                name: "IsEnabled",
                table: "RagKnowledgeBases",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否启用");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RagKnowledgeBases",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<string>(
                name: "EmbeddingModel",
                table: "RagKnowledgeBases",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldComment: "嵌入模型");

            migrationBuilder.AlterColumn<int>(
                name: "EmbeddingDimension",
                table: "RagKnowledgeBases",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "嵌入维度");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "RagKnowledgeBases",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "RagKnowledgeBases",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "RagKnowledgeBases",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldComment: "知识库编码");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "RagKnowledgeBases",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "主键ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "RagDocuments",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "更新时间");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "RagDocuments",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldComment: "文档标题");

            migrationBuilder.AlterColumn<string>(
                name: "SourceType",
                table: "RagDocuments",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldComment: "来源类型");

            migrationBuilder.AlterColumn<string>(
                name: "SourceId",
                table: "RagDocuments",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512,
                oldComment: "来源ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastIndexedAt",
                table: "RagDocuments",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "最后索引时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "KnowledgeBaseId",
                table: "RagDocuments",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "知识库ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RagDocuments",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "RagDocuments",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "删除时间");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "RagDocuments",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "RagDocuments",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "主键ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "RagChunks",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "更新时间");

            migrationBuilder.AlterColumn<int>(
                name: "TokenCount",
                table: "RagChunks",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "令牌数量");

            migrationBuilder.AlterColumn<Vector>(
                name: "Embedding",
                table: "RagChunks",
                type: "vector(768)",
                nullable: true,
                oldClrType: typeof(Vector),
                oldType: "vector(768)",
                oldNullable: true,
                oldComment: "向量嵌入");

            migrationBuilder.AlterColumn<Guid>(
                name: "DocumentId",
                table: "RagChunks",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "文档ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "RagChunks",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "RagChunks",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "分片内容");

            migrationBuilder.AlterColumn<int>(
                name: "ChunkIndex",
                table: "RagChunks",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "分片索引");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "RagChunks",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "主键ID");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "RagChunkingSettings",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "主键ID");
        }
    }
}
