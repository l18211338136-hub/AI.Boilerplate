using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI.Boilerplate.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class MakePropertiesNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Users_UserId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Users_UserId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Products_ProductId",
                table: "Inventories");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Addresses_AddressId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Orders_OrderId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_UserId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_Products_ProductId",
                table: "ProductImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Orders_OrderId",
                table: "ProductReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Products_ProductId",
                table: "ProductReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Users_UserId",
                table: "ProductReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSessions_Users_UserId",
                table: "UserSessions");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "WebAuthnCredential",
                type: "uuid",
                nullable: true,
                comment: "用户ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "用户ID");

            migrationBuilder.AlterColumn<long>(
                name: "SignCount",
                table: "WebAuthnCredential",
                type: "bigint",
                nullable: true,
                comment: "签名计数器",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "签名计数器");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "WebAuthnCredential",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<bool>(
                name: "IsBackupEligible",
                table: "WebAuthnCredential",
                type: "boolean",
                nullable: true,
                comment: "是否支持备份",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否支持备份");

            migrationBuilder.AlterColumn<bool>(
                name: "IsBackedUp",
                table: "WebAuthnCredential",
                type: "boolean",
                nullable: true,
                comment: "是否已备份",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否已备份");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "WebAuthnCredential",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "AaGuid",
                table: "WebAuthnCredential",
                type: "uuid",
                nullable: true,
                comment: "认证器AAGUID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "认证器AAGUID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserTokens",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "UserTokens",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserSessions",
                type: "uuid",
                nullable: true,
                comment: "关联的用户ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "关联的用户ID");

            migrationBuilder.AlterColumn<long>(
                name: "StartedOn",
                table: "UserSessions",
                type: "bigint",
                nullable: true,
                comment: "会话开始时间(Unix时间戳秒)",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "会话开始时间(Unix时间戳秒)");

            migrationBuilder.AlterColumn<bool>(
                name: "Privileged",
                table: "UserSessions",
                type: "boolean",
                nullable: true,
                comment: "是否提权(提权访问)",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否提权(提权访问)");

            migrationBuilder.AlterColumn<int>(
                name: "NotificationStatus",
                table: "UserSessions",
                type: "integer",
                nullable: true,
                comment: "推送通知状态",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "推送通知状态");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserSessions",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "UserSessions",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Users",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<bool>(
                name: "HasProfilePicture",
                table: "Users",
                type: "boolean",
                nullable: true,
                comment: "是否有头像",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否有头像");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserRoles",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "UserRoles",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserLogins",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "UserLogins",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserClaims",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "UserClaims",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TodoItems",
                type: "uuid",
                nullable: true,
                comment: "所属用户ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "所属用户ID");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "TodoItems",
                type: "text",
                nullable: true,
                comment: "待办标题",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "待办标题");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDone",
                table: "TodoItems",
                type: "boolean",
                nullable: true,
                comment: "是否完成",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否完成");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "TodoItems",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "TodoItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<int>(
                name: "PromptKind",
                table: "SystemPrompts",
                type: "integer",
                nullable: true,
                comment: "提示词类型",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "提示词类型");

            migrationBuilder.AlterColumn<string>(
                name: "Markdown",
                table: "SystemPrompts",
                type: "text",
                nullable: true,
                comment: "提示词内容",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "提示词内容");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SystemPrompts",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "SystemPrompts",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Roles",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Roles",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RoleClaims",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "RoleClaims",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RagKnowledgeBases",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "知识库名称",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldComment: "知识库名称");

            migrationBuilder.AlterColumn<bool>(
                name: "IsEnabled",
                table: "RagKnowledgeBases",
                type: "boolean",
                nullable: true,
                comment: "是否启用",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否启用");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RagKnowledgeBases",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<string>(
                name: "EmbeddingModel",
                table: "RagKnowledgeBases",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "嵌入模型",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldComment: "嵌入模型");

            migrationBuilder.AlterColumn<int>(
                name: "EmbeddingDimension",
                table: "RagKnowledgeBases",
                type: "integer",
                nullable: true,
                comment: "嵌入维度",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "嵌入维度");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "RagKnowledgeBases",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "RagKnowledgeBases",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true,
                comment: "知识库编码",
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldComment: "知识库编码");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "RagDocuments",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                comment: "文档标题",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldComment: "文档标题");

            migrationBuilder.AlterColumn<string>(
                name: "SourceType",
                table: "RagDocuments",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "来源类型",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldComment: "来源类型");

            migrationBuilder.AlterColumn<string>(
                name: "SourceId",
                table: "RagDocuments",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true,
                comment: "来源ID",
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512,
                oldComment: "来源ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastIndexedAt",
                table: "RagDocuments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后索引时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "最后索引时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "KnowledgeBaseId",
                table: "RagDocuments",
                type: "uuid",
                nullable: true,
                comment: "知识库ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "知识库ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RagDocuments",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "RagDocuments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<int>(
                name: "TokenCount",
                table: "RagChunks",
                type: "integer",
                nullable: true,
                comment: "令牌数量",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "令牌数量");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RagChunks",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<Guid>(
                name: "DocumentId",
                table: "RagChunks",
                type: "uuid",
                nullable: true,
                comment: "文档ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "文档ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "RagChunks",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "RagChunks",
                type: "text",
                nullable: true,
                comment: "分片内容",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "分片内容");

            migrationBuilder.AlterColumn<int>(
                name: "ChunkIndex",
                table: "RagChunks",
                type: "integer",
                nullable: true,
                comment: "分片索引",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "分片索引");

            migrationBuilder.AlterColumn<bool>(
                name: "PreferParagraphFirst",
                table: "RagChunkingSettings",
                type: "boolean",
                nullable: true,
                comment: "是否优先按段落分片",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否优先按段落分片");

            migrationBuilder.AlterColumn<int>(
                name: "MinChunkCount",
                table: "RagChunkingSettings",
                type: "integer",
                nullable: true,
                comment: "最小分片数",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "最小分片数");

            migrationBuilder.AlterColumn<int>(
                name: "MaxChunkLength",
                table: "RagChunkingSettings",
                type: "integer",
                nullable: true,
                comment: "最大分片长度",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "最大分片长度");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RagChunkingSettings",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "RagChunkingSettings",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<long>(
                name: "RenewedOn",
                table: "PushNotificationSubscriptions",
                type: "bigint",
                nullable: true,
                comment: "续订时间(Unix秒)",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "续订时间(Unix秒)");

            migrationBuilder.AlterColumn<string>(
                name: "PushChannel",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: true,
                comment: "推送通道",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "推送通道");

            migrationBuilder.AlterColumn<string>(
                name: "Platform",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: true,
                comment: "平台类型",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "平台类型");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PushNotificationSubscriptions",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<long>(
                name: "ExpirationTime",
                table: "PushNotificationSubscriptions",
                type: "bigint",
                nullable: true,
                comment: "过期时间(Unix秒)",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "过期时间(Unix秒)");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceId",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: true,
                comment: "设备ID",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "设备ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "PushNotificationSubscriptions",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<int>(
                name: "ShortId",
                table: "Products",
                type: "integer",
                nullable: true,
                defaultValueSql: "nextval('\"产品短编号\"')",
                comment: "产品短编号",
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValueSql: "nextval('\"产品短编号\"')",
                oldComment: "产品短编号");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true,
                comment: "产品价格",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldComment: "产品价格");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true,
                comment: "产品名称",
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldComment: "产品名称");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Products",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<bool>(
                name: "HasPrimaryImage",
                table: "Products",
                type: "boolean",
                nullable: true,
                comment: "是否有主图",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否有主图");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Products",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Products",
                type: "uuid",
                nullable: true,
                comment: "类别 ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "类别 ID");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "ProductReviews",
                type: "uuid",
                nullable: true,
                comment: "用户ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "用户ID");

            migrationBuilder.AlterColumn<short>(
                name: "Rating",
                table: "ProductReviews",
                type: "smallint",
                nullable: true,
                comment: "评分(1-5)",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldComment: "评分(1-5)");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "ProductReviews",
                type: "uuid",
                nullable: true,
                comment: "产品ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "产品ID");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "ProductReviews",
                type: "uuid",
                nullable: true,
                comment: "订单ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "订单ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ProductReviews",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAnonymous",
                table: "ProductReviews",
                type: "boolean",
                nullable: true,
                comment: "是否匿名显示",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否匿名显示");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "ProductReviews",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "ProductImages",
                type: "integer",
                nullable: true,
                comment: "排序权重(升序)",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "排序权重(升序)");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "ProductImages",
                type: "uuid",
                nullable: true,
                comment: "产品ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "产品ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                table: "ProductImages",
                type: "boolean",
                nullable: true,
                comment: "是否为主图",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否为主图");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ProductImages",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "ProductImages",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true,
                comment: "图片存储URL",
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512,
                oldComment: "图片存储URL");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "ProductImages",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Payments",
                type: "uuid",
                nullable: true,
                comment: "用户ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "用户ID");

            migrationBuilder.AlterColumn<short>(
                name: "Status",
                table: "Payments",
                type: "smallint",
                nullable: true,
                comment: "支付状态(0:待支付 1:成功 2:失败 3:已退款)",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldComment: "支付状态(0:待支付 1:成功 2:失败 3:已退款)");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "Payments",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "支付渠道(Alipay, WeChatPay, UnionPay等)",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldComment: "支付渠道(Alipay, WeChatPay, UnionPay等)");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "Payments",
                type: "uuid",
                nullable: true,
                comment: "订单ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "订单ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Payments",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Payments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Payments",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true,
                comment: "实际支付金额",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldComment: "实际支付金额");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Orders",
                type: "uuid",
                nullable: true,
                comment: "用户ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "用户ID");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                table: "Orders",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true,
                comment: "商品原价总额",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldComment: "商品原价总额");

            migrationBuilder.AlterColumn<short>(
                name: "Status",
                table: "Orders",
                type: "smallint",
                nullable: true,
                comment: "订单状态(0:待付款 1:已付款 2:已发货 3:已完成 4:已取消 5:退款中)",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldComment: "订单状态(0:待付款 1:已付款 2:已发货 3:已完成 4:已取消 5:退款中)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ShippingFee",
                table: "Orders",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true,
                comment: "运费",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldComment: "运费");

            migrationBuilder.AlterColumn<decimal>(
                name: "PayableAmount",
                table: "Orders",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true,
                comment: "实际应付金额",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldComment: "实际应付金额");

            migrationBuilder.AlterColumn<string>(
                name: "OrderNo",
                table: "Orders",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "订单业务编号(唯一)",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldComment: "订单业务编号(唯一)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Orders",
                type: "boolean",
                nullable: true,
                comment: "是否删除标识",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除标识");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountAmount",
                table: "Orders",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true,
                comment: "优惠抵扣金额",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldComment: "优惠抵扣金额");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "Orders",
                type: "uuid",
                nullable: true,
                comment: "收货地址ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "收货地址ID");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "OrderItems",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true,
                comment: "下单时单价快照",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldComment: "下单时单价快照");

            migrationBuilder.AlterColumn<decimal>(
                name: "SubTotal",
                table: "OrderItems",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true,
                comment: "行小计金额",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldComment: "行小计金额");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "OrderItems",
                type: "integer",
                nullable: true,
                comment: "购买数量",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "购买数量");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "OrderItems",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                comment: "下单时产品名称快照",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldComment: "下单时产品名称快照");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "OrderItems",
                type: "uuid",
                nullable: true,
                comment: "产品ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "产品ID");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "OrderItems",
                type: "uuid",
                nullable: true,
                comment: "订单ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "订单ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "OrderItems",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "OrderItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<int>(
                name: "StockQuantity",
                table: "Inventories",
                type: "integer",
                nullable: true,
                comment: "当前可用库存",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "当前可用库存");

            migrationBuilder.AlterColumn<int>(
                name: "ReservedQuantity",
                table: "Inventories",
                type: "integer",
                nullable: true,
                comment: "预占库存(已下单未支付)",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "预占库存(已下单未支付)");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "Inventories",
                type: "uuid",
                nullable: true,
                comment: "产品ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "产品ID");

            migrationBuilder.AlterColumn<int>(
                name: "LowStockThreshold",
                table: "Inventories",
                type: "integer",
                nullable: true,
                comment: "低库存预警阈值",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "低库存预警阈值");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Inventories",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Inventories",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true,
                comment: "类别名称",
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldComment: "类别名称");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Categories",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Categories",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "CartItems",
                type: "uuid",
                nullable: true,
                comment: "用户ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "用户ID");

            migrationBuilder.AlterColumn<bool>(
                name: "Selected",
                table: "CartItems",
                type: "boolean",
                nullable: true,
                comment: "结算是否勾选",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "结算是否勾选");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "CartItems",
                type: "integer",
                nullable: true,
                comment: "数量",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "数量");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "CartItems",
                type: "uuid",
                nullable: true,
                comment: "产品ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "产品ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "CartItems",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "CartItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Attachments",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Attachments",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<short>(
                name: "Level",
                table: "AreaCodes",
                type: "smallint",
                nullable: true,
                comment: "级别1-5,省市县镇村",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldComment: "级别1-5,省市县镇村");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "AreaCodes",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "AreaCodes",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<int>(
                name: "Category",
                table: "AreaCodes",
                type: "integer",
                nullable: true,
                comment: "城乡分类",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "城乡分类");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Addresses",
                type: "uuid",
                nullable: true,
                comment: "用户ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "用户ID");

            migrationBuilder.AlterColumn<string>(
                name: "StreetAddress",
                table: "Addresses",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                comment: "详细地址",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldComment: "详细地址");

            migrationBuilder.AlterColumn<string>(
                name: "RecipientName",
                table: "Addresses",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true,
                comment: "收件人姓名",
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldComment: "收件人姓名");

            migrationBuilder.AlterColumn<string>(
                name: "Province",
                table: "Addresses",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "省",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldComment: "省");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Addresses",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                comment: "联系电话",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldComment: "联系电话");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Addresses",
                type: "boolean",
                nullable: true,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDefault",
                table: "Addresses",
                type: "boolean",
                nullable: true,
                comment: "是否默认地址",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否默认地址");

            migrationBuilder.AlterColumn<string>(
                name: "District",
                table: "Addresses",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "区/县",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldComment: "区/县");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Addresses",
                type: "timestamp with time zone",
                nullable: true,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Addresses",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                comment: "市",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldComment: "市");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Users_UserId",
                table: "Addresses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Users_UserId",
                table: "CartItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Products_ProductId",
                table: "Inventories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Addresses_AddressId",
                table: "Orders",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Orders_OrderId",
                table: "Payments",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_UserId",
                table: "Payments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_Products_ProductId",
                table: "ProductImages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Orders_OrderId",
                table: "ProductReviews",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Products_ProductId",
                table: "ProductReviews",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Users_UserId",
                table: "ProductReviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSessions_Users_UserId",
                table: "UserSessions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Users_UserId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Users_UserId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Products_ProductId",
                table: "Inventories");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Addresses_AddressId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Orders_OrderId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_UserId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_Products_ProductId",
                table: "ProductImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Orders_OrderId",
                table: "ProductReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Products_ProductId",
                table: "ProductReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Users_UserId",
                table: "ProductReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSessions_Users_UserId",
                table: "UserSessions");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "WebAuthnCredential",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "用户ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "用户ID");

            migrationBuilder.AlterColumn<long>(
                name: "SignCount",
                table: "WebAuthnCredential",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "签名计数器",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "签名计数器");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "WebAuthnCredential",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<bool>(
                name: "IsBackupEligible",
                table: "WebAuthnCredential",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否支持备份",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否支持备份");

            migrationBuilder.AlterColumn<bool>(
                name: "IsBackedUp",
                table: "WebAuthnCredential",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否已备份",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否已备份");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "WebAuthnCredential",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "AaGuid",
                table: "WebAuthnCredential",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "认证器AAGUID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "认证器AAGUID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserTokens",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "UserTokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserSessions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "关联的用户ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "关联的用户ID");

            migrationBuilder.AlterColumn<long>(
                name: "StartedOn",
                table: "UserSessions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "会话开始时间(Unix时间戳秒)",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "会话开始时间(Unix时间戳秒)");

            migrationBuilder.AlterColumn<bool>(
                name: "Privileged",
                table: "UserSessions",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否提权(提权访问)",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否提权(提权访问)");

            migrationBuilder.AlterColumn<int>(
                name: "NotificationStatus",
                table: "UserSessions",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "推送通知状态",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "推送通知状态");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserSessions",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "UserSessions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<bool>(
                name: "HasProfilePicture",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否有头像",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否有头像");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserRoles",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "UserRoles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserLogins",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "UserLogins",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "UserClaims",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "UserClaims",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TodoItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "所属用户ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "所属用户ID");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "TodoItems",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "待办标题",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "待办标题");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDone",
                table: "TodoItems",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否完成",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否完成");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "TodoItems",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "TodoItems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<int>(
                name: "PromptKind",
                table: "SystemPrompts",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "提示词类型",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "提示词类型");

            migrationBuilder.AlterColumn<string>(
                name: "Markdown",
                table: "SystemPrompts",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "提示词内容",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "提示词内容");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SystemPrompts",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "SystemPrompts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Roles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RoleClaims",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "RoleClaims",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RagKnowledgeBases",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "",
                comment: "知识库名称",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "知识库名称");

            migrationBuilder.AlterColumn<bool>(
                name: "IsEnabled",
                table: "RagKnowledgeBases",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否启用",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否启用");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RagKnowledgeBases",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<string>(
                name: "EmbeddingModel",
                table: "RagKnowledgeBases",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "",
                comment: "嵌入模型",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "嵌入模型");

            migrationBuilder.AlterColumn<int>(
                name: "EmbeddingDimension",
                table: "RagKnowledgeBases",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "嵌入维度",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "嵌入维度");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "RagKnowledgeBases",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "RagKnowledgeBases",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "",
                comment: "知识库编码",
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true,
                oldComment: "知识库编码");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "RagDocuments",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                comment: "文档标题",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true,
                oldComment: "文档标题");

            migrationBuilder.AlterColumn<string>(
                name: "SourceType",
                table: "RagDocuments",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "",
                comment: "来源类型",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "来源类型");

            migrationBuilder.AlterColumn<string>(
                name: "SourceId",
                table: "RagDocuments",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "",
                comment: "来源ID",
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512,
                oldNullable: true,
                oldComment: "来源ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastIndexedAt",
                table: "RagDocuments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "最后索引时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "最后索引时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "KnowledgeBaseId",
                table: "RagDocuments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "知识库ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "知识库ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RagDocuments",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "RagDocuments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<int>(
                name: "TokenCount",
                table: "RagChunks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "令牌数量",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "令牌数量");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RagChunks",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<Guid>(
                name: "DocumentId",
                table: "RagChunks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "文档ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "文档ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "RagChunks",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "RagChunks",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "分片内容",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "分片内容");

            migrationBuilder.AlterColumn<int>(
                name: "ChunkIndex",
                table: "RagChunks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "分片索引",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "分片索引");

            migrationBuilder.AlterColumn<bool>(
                name: "PreferParagraphFirst",
                table: "RagChunkingSettings",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否优先按段落分片",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否优先按段落分片");

            migrationBuilder.AlterColumn<int>(
                name: "MinChunkCount",
                table: "RagChunkingSettings",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "最小分片数",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "最小分片数");

            migrationBuilder.AlterColumn<int>(
                name: "MaxChunkLength",
                table: "RagChunkingSettings",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "最大分片长度",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "最大分片长度");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RagChunkingSettings",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "RagChunkingSettings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<long>(
                name: "RenewedOn",
                table: "PushNotificationSubscriptions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "续订时间(Unix秒)",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "续订时间(Unix秒)");

            migrationBuilder.AlterColumn<string>(
                name: "PushChannel",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "推送通道",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "推送通道");

            migrationBuilder.AlterColumn<string>(
                name: "Platform",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "平台类型",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "平台类型");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PushNotificationSubscriptions",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<long>(
                name: "ExpirationTime",
                table: "PushNotificationSubscriptions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "过期时间(Unix秒)",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "过期时间(Unix秒)");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceId",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "设备ID",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "设备ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "PushNotificationSubscriptions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<int>(
                name: "ShortId",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValueSql: "nextval('\"产品短编号\"')",
                comment: "产品短编号",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldDefaultValueSql: "nextval('\"产品短编号\"')",
                oldComment: "产品短编号");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                defaultValue: 0m,
                comment: "产品价格",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true,
                oldComment: "产品价格");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "",
                comment: "产品名称",
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true,
                oldComment: "产品名称");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<bool>(
                name: "HasPrimaryImage",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否有主图",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否有主图");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Products",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Products",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "类别 ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "类别 ID");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "ProductReviews",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "用户ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "用户ID");

            migrationBuilder.AlterColumn<short>(
                name: "Rating",
                table: "ProductReviews",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0,
                comment: "评分(1-5)",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true,
                oldComment: "评分(1-5)");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "ProductReviews",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "产品ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "产品ID");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "ProductReviews",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "订单ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "订单ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ProductReviews",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAnonymous",
                table: "ProductReviews",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否匿名显示",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否匿名显示");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "ProductReviews",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<int>(
                name: "SortOrder",
                table: "ProductImages",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "排序权重(升序)",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "排序权重(升序)");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "ProductImages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "产品ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "产品ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                table: "ProductImages",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否为主图",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否为主图");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ProductImages",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "ProductImages",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "",
                comment: "图片存储URL",
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512,
                oldNullable: true,
                oldComment: "图片存储URL");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "ProductImages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Payments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "用户ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "用户ID");

            migrationBuilder.AlterColumn<short>(
                name: "Status",
                table: "Payments",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0,
                comment: "支付状态(0:待支付 1:成功 2:失败 3:已退款)",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true,
                oldComment: "支付状态(0:待支付 1:成功 2:失败 3:已退款)");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "Payments",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "",
                comment: "支付渠道(Alipay, WeChatPay, UnionPay等)",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "支付渠道(Alipay, WeChatPay, UnionPay等)");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "Payments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "订单ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "订单ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Payments",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Payments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Payments",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                defaultValue: 0m,
                comment: "实际支付金额",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true,
                oldComment: "实际支付金额");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Orders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "用户ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "用户ID");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                table: "Orders",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                defaultValue: 0m,
                comment: "商品原价总额",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true,
                oldComment: "商品原价总额");

            migrationBuilder.AlterColumn<short>(
                name: "Status",
                table: "Orders",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0,
                comment: "订单状态(0:待付款 1:已付款 2:已发货 3:已完成 4:已取消 5:退款中)",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true,
                oldComment: "订单状态(0:待付款 1:已付款 2:已发货 3:已完成 4:已取消 5:退款中)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ShippingFee",
                table: "Orders",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                defaultValue: 0m,
                comment: "运费",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true,
                oldComment: "运费");

            migrationBuilder.AlterColumn<decimal>(
                name: "PayableAmount",
                table: "Orders",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                defaultValue: 0m,
                comment: "实际应付金额",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true,
                oldComment: "实际应付金额");

            migrationBuilder.AlterColumn<string>(
                name: "OrderNo",
                table: "Orders",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "",
                comment: "订单业务编号(唯一)",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "订单业务编号(唯一)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Orders",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除标识",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除标识");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountAmount",
                table: "Orders",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                defaultValue: 0m,
                comment: "优惠抵扣金额",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true,
                oldComment: "优惠抵扣金额");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "AddressId",
                table: "Orders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "收货地址ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "收货地址ID");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "OrderItems",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                defaultValue: 0m,
                comment: "下单时单价快照",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true,
                oldComment: "下单时单价快照");

            migrationBuilder.AlterColumn<decimal>(
                name: "SubTotal",
                table: "OrderItems",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                defaultValue: 0m,
                comment: "行小计金额",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true,
                oldComment: "行小计金额");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "OrderItems",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "购买数量",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "购买数量");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "OrderItems",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "",
                comment: "下单时产品名称快照",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true,
                oldComment: "下单时产品名称快照");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "OrderItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "产品ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "产品ID");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "OrderItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "订单ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "订单ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "OrderItems",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "OrderItems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<int>(
                name: "StockQuantity",
                table: "Inventories",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "当前可用库存",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "当前可用库存");

            migrationBuilder.AlterColumn<int>(
                name: "ReservedQuantity",
                table: "Inventories",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "预占库存(已下单未支付)",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "预占库存(已下单未支付)");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "Inventories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "产品ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "产品ID");

            migrationBuilder.AlterColumn<int>(
                name: "LowStockThreshold",
                table: "Inventories",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "低库存预警阈值",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "低库存预警阈值");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Inventories",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Inventories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "",
                comment: "类别名称",
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true,
                oldComment: "类别名称");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Categories",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Categories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "CartItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "用户ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "用户ID");

            migrationBuilder.AlterColumn<bool>(
                name: "Selected",
                table: "CartItems",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "结算是否勾选",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "结算是否勾选");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "CartItems",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "数量",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "数量");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "CartItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "产品ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "产品ID");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "CartItems",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "CartItems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Attachments",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Attachments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "Level",
                table: "AreaCodes",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0,
                comment: "级别1-5,省市县镇村",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true,
                oldComment: "级别1-5,省市县镇村");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "AreaCodes",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "AreaCodes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Category",
                table: "AreaCodes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "城乡分类",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "城乡分类");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Addresses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "用户ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "用户ID");

            migrationBuilder.AlterColumn<string>(
                name: "StreetAddress",
                table: "Addresses",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                comment: "详细地址",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true,
                oldComment: "详细地址");

            migrationBuilder.AlterColumn<string>(
                name: "RecipientName",
                table: "Addresses",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "",
                comment: "收件人姓名",
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true,
                oldComment: "收件人姓名");

            migrationBuilder.AlterColumn<string>(
                name: "Province",
                table: "Addresses",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "",
                comment: "省",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "省");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Addresses",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                comment: "联系电话",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldComment: "联系电话");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Addresses",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否删除");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDefault",
                table: "Addresses",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否默认地址",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldComment: "是否默认地址");

            migrationBuilder.AlterColumn<string>(
                name: "District",
                table: "Addresses",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "",
                comment: "区/县",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "区/县");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Addresses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Addresses",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "",
                comment: "市",
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true,
                oldComment: "市");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Users_UserId",
                table: "Addresses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Users_UserId",
                table: "CartItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Products_ProductId",
                table: "Inventories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Addresses_AddressId",
                table: "Orders",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Orders_OrderId",
                table: "Payments",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_UserId",
                table: "Payments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_Products_ProductId",
                table: "ProductImages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Orders_OrderId",
                table: "ProductReviews",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Products_ProductId",
                table: "ProductReviews",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Users_UserId",
                table: "ProductReviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSessions_Users_UserId",
                table: "UserSessions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
