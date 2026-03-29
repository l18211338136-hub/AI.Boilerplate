using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pgvector;

#nullable disable

namespace AI.Boilerplate.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTableAndColumnComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "WebAuthnCredential",
                comment: "WebAuthn凭据表");

            migrationBuilder.AlterTable(
                name: "Users",
                comment: "用户表");

            migrationBuilder.AlterTable(
                name: "UserRoles",
                comment: "用户角色关联表");

            migrationBuilder.AlterTable(
                name: "UserClaims",
                comment: "用户声明表");

            migrationBuilder.AlterTable(
                name: "TodoItems",
                comment: "待办事项表");

            migrationBuilder.AlterTable(
                name: "SystemPrompts",
                comment: "系统提示词配置表");

            migrationBuilder.AlterTable(
                name: "Roles",
                comment: "角色表");

            migrationBuilder.AlterTable(
                name: "RoleClaims",
                comment: "角色声明表");

            migrationBuilder.AlterTable(
                name: "PushNotificationSubscriptions",
                comment: "推送订阅表");

            migrationBuilder.AlterTable(
                name: "Products",
                comment: "产品表");

            migrationBuilder.AlterTable(
                name: "Categories",
                comment: "产品类别表");

            migrationBuilder.AlterTable(
                name: "Attachments",
                comment: "附件表");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "WebAuthnCredential",
                type: "uuid",
                nullable: false,
                comment: "用户ID",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<byte[]>(
                name: "UserHandle",
                table: "WebAuthnCredential",
                type: "bytea",
                nullable: true,
                comment: "用户句柄",
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldNullable: true);

            migrationBuilder.AlterColumn<int[]>(
                name: "Transports",
                table: "WebAuthnCredential",
                type: "integer[]",
                nullable: true,
                comment: "传输通道",
                oldClrType: typeof(int[]),
                oldType: "integer[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SignCount",
                table: "WebAuthnCredential",
                type: "bigint",
                nullable: false,
                comment: "签名计数器",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "RegDate",
                table: "WebAuthnCredential",
                type: "timestamp with time zone",
                nullable: false,
                comment: "注册时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PublicKey",
                table: "WebAuthnCredential",
                type: "bytea",
                nullable: true,
                comment: "公钥",
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsBackupEligible",
                table: "WebAuthnCredential",
                type: "boolean",
                nullable: false,
                comment: "是否支持备份",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsBackedUp",
                table: "WebAuthnCredential",
                type: "boolean",
                nullable: false,
                comment: "是否已备份",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<byte[]>(
                name: "AttestationObject",
                table: "WebAuthnCredential",
                type: "bytea",
                nullable: true,
                comment: "认证对象",
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AttestationFormat",
                table: "WebAuthnCredential",
                type: "text",
                nullable: true,
                comment: "认证格式",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "AttestationClientDataJson",
                table: "WebAuthnCredential",
                type: "bytea",
                nullable: true,
                comment: "认证客户端数据",
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AaGuid",
                table: "WebAuthnCredential",
                type: "uuid",
                nullable: false,
                comment: "认证器AAGUID",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Id",
                table: "WebAuthnCredential",
                type: "bytea",
                nullable: false,
                comment: "主键ID",
                oldClrType: typeof(byte[]),
                oldType: "bytea");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "TwoFactorTokenRequestedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "双因子验证码请求时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ResetPasswordTokenRequestedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "重置密码令牌请求时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "PhoneNumberTokenRequestedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "手机号验证码请求时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "OtpRequestedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "一次性密码请求时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "HasProfilePicture",
                table: "Users",
                type: "boolean",
                nullable: false,
                comment: "是否有头像",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "Users",
                type: "integer",
                nullable: true,
                comment: "性别",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Users",
                type: "text",
                nullable: true,
                comment: "姓名",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "EmailTokenRequestedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "邮箱验证码请求时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ElevatedAccessTokenRequestedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "提权令牌请求时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "BirthDate",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "出生日期",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Users",
                type: "uuid",
                nullable: false,
                comment: "用户ID",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                table: "UserRoles",
                type: "uuid",
                nullable: false,
                comment: "角色ID",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserRoles",
                type: "uuid",
                nullable: false,
                comment: "用户ID",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserClaims",
                type: "uuid",
                nullable: false,
                comment: "用户ID",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimValue",
                table: "UserClaims",
                type: "text",
                nullable: true,
                comment: "声明值",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClaimType",
                table: "UserClaims",
                type: "text",
                nullable: true,
                comment: "声明类型",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UserClaims",
                type: "integer",
                nullable: false,
                comment: "主键ID",
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TodoItems",
                type: "uuid",
                nullable: false,
                comment: "所属用户ID",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "TodoItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "更新时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "TodoItems",
                type: "text",
                nullable: false,
                comment: "待办标题",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDone",
                table: "TodoItems",
                type: "boolean",
                nullable: false,
                comment: "是否完成",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "TodoItems",
                type: "text",
                nullable: false,
                comment: "主键ID",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<uint>(
                name: "xmin",
                table: "SystemPrompts",
                type: "xid",
                rowVersion: true,
                nullable: false,
                comment: "并发版本",
                oldClrType: typeof(uint),
                oldType: "xid",
                oldRowVersion: true);

            migrationBuilder.AlterColumn<int>(
                name: "PromptKind",
                table: "SystemPrompts",
                type: "integer",
                nullable: false,
                comment: "提示词类型",
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Markdown",
                table: "SystemPrompts",
                type: "text",
                nullable: false,
                comment: "提示词内容",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "SystemPrompts",
                type: "uuid",
                nullable: false,
                comment: "主键ID",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedName",
                table: "Roles",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                comment: "规范化角色名称",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Roles",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                comment: "角色名称",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "Roles",
                type: "text",
                nullable: true,
                comment: "并发戳",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Roles",
                type: "uuid",
                nullable: false,
                comment: "角色ID",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                table: "RoleClaims",
                type: "uuid",
                nullable: false,
                comment: "角色ID",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimValue",
                table: "RoleClaims",
                type: "text",
                nullable: true,
                comment: "声明值",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClaimType",
                table: "RoleClaims",
                type: "text",
                nullable: true,
                comment: "声明类型",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "RoleClaims",
                type: "integer",
                nullable: false,
                comment: "主键ID",
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserSessionId",
                table: "PushNotificationSubscriptions",
                type: "uuid",
                nullable: true,
                comment: "用户会话ID",
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string[]>(
                name: "Tags",
                table: "PushNotificationSubscriptions",
                type: "text[]",
                nullable: false,
                comment: "订阅标签",
                oldClrType: typeof(string[]),
                oldType: "text[]");

            migrationBuilder.AlterColumn<long>(
                name: "RenewedOn",
                table: "PushNotificationSubscriptions",
                type: "bigint",
                nullable: false,
                comment: "续订时间(Unix秒)",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "PushChannel",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: false,
                comment: "推送通道",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Platform",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: false,
                comment: "平台类型",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "P256dh",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: true,
                comment: "WebPush公钥",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ExpirationTime",
                table: "PushNotificationSubscriptions",
                type: "bigint",
                nullable: false,
                comment: "过期时间(Unix秒)",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Endpoint",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: true,
                comment: "推送端点",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeviceId",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: false,
                comment: "设备ID",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Auth",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: true,
                comment: "WebPush认证值",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PushNotificationSubscriptions",
                type: "integer",
                nullable: false,
                comment: "主键ID",
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<uint>(
                name: "xmin",
                table: "Products",
                type: "xid",
                rowVersion: true,
                nullable: false,
                comment: "并发版本",
                oldClrType: typeof(uint),
                oldType: "xid",
                oldRowVersion: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShortId",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValueSql: "nextval('\"ProductShortId\"')",
                comment: "短编号",
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValueSql: "nextval('\"ProductShortId\"')");

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryImageAltText",
                table: "Products",
                type: "text",
                nullable: true,
                comment: "主图替代文本",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                comment: "产品价格",
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                comment: "产品名称",
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<bool>(
                name: "HasPrimaryImage",
                table: "Products",
                type: "boolean",
                nullable: false,
                comment: "是否有主图",
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<Vector>(
                name: "Embedding",
                table: "Products",
                type: "vector(768)",
                nullable: true,
                comment: "语义向量",
                oldClrType: typeof(Vector),
                oldType: "vector(768)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionText",
                table: "Products",
                type: "character varying(4096)",
                maxLength: 4096,
                nullable: true,
                comment: "纯文本描述",
                oldClrType: typeof(string),
                oldType: "character varying(4096)",
                oldMaxLength: 4096,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionHTML",
                table: "Products",
                type: "character varying(4096)",
                maxLength: 4096,
                nullable: true,
                comment: "HTML描述",
                oldClrType: typeof(string),
                oldType: "character varying(4096)",
                oldMaxLength: 4096,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Products",
                type: "timestamp with time zone",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Products",
                type: "uuid",
                nullable: false,
                comment: "类别ID",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Products",
                type: "uuid",
                nullable: false,
                comment: "主键ID",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<uint>(
                name: "xmin",
                table: "Categories",
                type: "xid",
                rowVersion: true,
                nullable: false,
                comment: "并发版本",
                oldClrType: typeof(uint),
                oldType: "xid",
                oldRowVersion: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                comment: "类别名称",
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Categories",
                type: "text",
                nullable: true,
                comment: "类别颜色",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Categories",
                type: "uuid",
                nullable: false,
                comment: "主键ID",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "Attachments",
                type: "text",
                nullable: true,
                comment: "附件路径",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Kind",
                table: "Attachments",
                type: "integer",
                nullable: false,
                comment: "附件类型",
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Attachments",
                type: "uuid",
                nullable: false,
                comment: "附件ID",
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "WebAuthnCredential",
                oldComment: "WebAuthn凭据表");

            migrationBuilder.AlterTable(
                name: "Users",
                oldComment: "用户表");

            migrationBuilder.AlterTable(
                name: "UserRoles",
                oldComment: "用户角色关联表");

            migrationBuilder.AlterTable(
                name: "UserClaims",
                oldComment: "用户声明表");

            migrationBuilder.AlterTable(
                name: "TodoItems",
                oldComment: "待办事项表");

            migrationBuilder.AlterTable(
                name: "SystemPrompts",
                oldComment: "系统提示词配置表");

            migrationBuilder.AlterTable(
                name: "Roles",
                oldComment: "角色表");

            migrationBuilder.AlterTable(
                name: "RoleClaims",
                oldComment: "角色声明表");

            migrationBuilder.AlterTable(
                name: "PushNotificationSubscriptions",
                oldComment: "推送订阅表");

            migrationBuilder.AlterTable(
                name: "Products",
                oldComment: "产品表");

            migrationBuilder.AlterTable(
                name: "Categories",
                oldComment: "产品类别表");

            migrationBuilder.AlterTable(
                name: "Attachments",
                oldComment: "附件表");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "WebAuthnCredential",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "用户ID");

            migrationBuilder.AlterColumn<byte[]>(
                name: "UserHandle",
                table: "WebAuthnCredential",
                type: "bytea",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldNullable: true,
                oldComment: "用户句柄");

            migrationBuilder.AlterColumn<int[]>(
                name: "Transports",
                table: "WebAuthnCredential",
                type: "integer[]",
                nullable: true,
                oldClrType: typeof(int[]),
                oldType: "integer[]",
                oldNullable: true,
                oldComment: "传输通道");

            migrationBuilder.AlterColumn<long>(
                name: "SignCount",
                table: "WebAuthnCredential",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "签名计数器");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "RegDate",
                table: "WebAuthnCredential",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "注册时间");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PublicKey",
                table: "WebAuthnCredential",
                type: "bytea",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldNullable: true,
                oldComment: "公钥");

            migrationBuilder.AlterColumn<bool>(
                name: "IsBackupEligible",
                table: "WebAuthnCredential",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否支持备份");

            migrationBuilder.AlterColumn<bool>(
                name: "IsBackedUp",
                table: "WebAuthnCredential",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否已备份");

            migrationBuilder.AlterColumn<byte[]>(
                name: "AttestationObject",
                table: "WebAuthnCredential",
                type: "bytea",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldNullable: true,
                oldComment: "认证对象");

            migrationBuilder.AlterColumn<string>(
                name: "AttestationFormat",
                table: "WebAuthnCredential",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "认证格式");

            migrationBuilder.AlterColumn<byte[]>(
                name: "AttestationClientDataJson",
                table: "WebAuthnCredential",
                type: "bytea",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldNullable: true,
                oldComment: "认证客户端数据");

            migrationBuilder.AlterColumn<Guid>(
                name: "AaGuid",
                table: "WebAuthnCredential",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "认证器AAGUID");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Id",
                table: "WebAuthnCredential",
                type: "bytea",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldComment: "主键ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "TwoFactorTokenRequestedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "双因子验证码请求时间");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ResetPasswordTokenRequestedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "重置密码令牌请求时间");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "PhoneNumberTokenRequestedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "手机号验证码请求时间");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "OtpRequestedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "一次性密码请求时间");

            migrationBuilder.AlterColumn<bool>(
                name: "HasProfilePicture",
                table: "Users",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否有头像");

            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "Users",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "性别");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "姓名");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "EmailTokenRequestedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "邮箱验证码请求时间");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ElevatedAccessTokenRequestedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "提权令牌请求时间");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "BirthDate",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "出生日期");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Users",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "用户ID");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                table: "UserRoles",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "角色ID");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserRoles",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "用户ID");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserClaims",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "用户ID");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimValue",
                table: "UserClaims",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "声明值");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimType",
                table: "UserClaims",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "声明类型");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UserClaims",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "主键ID")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TodoItems",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "所属用户ID");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "TodoItems",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldComment: "更新时间");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "TodoItems",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "待办标题");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDone",
                table: "TodoItems",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否完成");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "TodoItems",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "主键ID");

            migrationBuilder.AlterColumn<uint>(
                name: "xmin",
                table: "SystemPrompts",
                type: "xid",
                rowVersion: true,
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "xid",
                oldRowVersion: true,
                oldComment: "并发版本");

            migrationBuilder.AlterColumn<int>(
                name: "PromptKind",
                table: "SystemPrompts",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "提示词类型");

            migrationBuilder.AlterColumn<string>(
                name: "Markdown",
                table: "SystemPrompts",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "提示词内容");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "SystemPrompts",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "主键ID");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedName",
                table: "Roles",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true,
                oldComment: "规范化角色名称");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Roles",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "角色名称");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "Roles",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "并发戳");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Roles",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "角色ID");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                table: "RoleClaims",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "角色ID");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimValue",
                table: "RoleClaims",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "声明值");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimType",
                table: "RoleClaims",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "声明类型");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "RoleClaims",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "主键ID")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserSessionId",
                table: "PushNotificationSubscriptions",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true,
                oldComment: "用户会话ID");

            migrationBuilder.AlterColumn<string[]>(
                name: "Tags",
                table: "PushNotificationSubscriptions",
                type: "text[]",
                nullable: false,
                oldClrType: typeof(string[]),
                oldType: "text[]",
                oldComment: "订阅标签");

            migrationBuilder.AlterColumn<long>(
                name: "RenewedOn",
                table: "PushNotificationSubscriptions",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "续订时间(Unix秒)");

            migrationBuilder.AlterColumn<string>(
                name: "PushChannel",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "推送通道");

            migrationBuilder.AlterColumn<string>(
                name: "Platform",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "平台类型");

            migrationBuilder.AlterColumn<string>(
                name: "P256dh",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "WebPush公钥");

            migrationBuilder.AlterColumn<long>(
                name: "ExpirationTime",
                table: "PushNotificationSubscriptions",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "过期时间(Unix秒)");

            migrationBuilder.AlterColumn<string>(
                name: "Endpoint",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "推送端点");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceId",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "设备ID");

            migrationBuilder.AlterColumn<string>(
                name: "Auth",
                table: "PushNotificationSubscriptions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "WebPush认证值");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PushNotificationSubscriptions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "主键ID")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<uint>(
                name: "xmin",
                table: "Products",
                type: "xid",
                rowVersion: true,
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "xid",
                oldRowVersion: true,
                oldComment: "并发版本");

            migrationBuilder.AlterColumn<int>(
                name: "ShortId",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValueSql: "nextval('\"ProductShortId\"')",
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValueSql: "nextval('\"ProductShortId\"')",
                oldComment: "短编号");

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryImageAltText",
                table: "Products",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "主图替代文本");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
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
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldComment: "产品名称");

            migrationBuilder.AlterColumn<bool>(
                name: "HasPrimaryImage",
                table: "Products",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否有主图");

            migrationBuilder.AlterColumn<Vector>(
                name: "Embedding",
                table: "Products",
                type: "vector(768)",
                nullable: true,
                oldClrType: typeof(Vector),
                oldType: "vector(768)",
                oldNullable: true,
                oldComment: "语义向量");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionText",
                table: "Products",
                type: "character varying(4096)",
                maxLength: 4096,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(4096)",
                oldMaxLength: 4096,
                oldNullable: true,
                oldComment: "纯文本描述");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionHTML",
                table: "Products",
                type: "character varying(4096)",
                maxLength: 4096,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(4096)",
                oldMaxLength: 4096,
                oldNullable: true,
                oldComment: "HTML描述");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Products",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Products",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "类别ID");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Products",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "主键ID");

            migrationBuilder.AlterColumn<uint>(
                name: "xmin",
                table: "Categories",
                type: "xid",
                rowVersion: true,
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "xid",
                oldRowVersion: true,
                oldComment: "并发版本");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldComment: "类别名称");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Categories",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "类别颜色");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Categories",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "主键ID");

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "Attachments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "附件路径");

            migrationBuilder.AlterColumn<int>(
                name: "Kind",
                table: "Attachments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "附件类型");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Attachments",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldComment: "附件ID");
        }
    }
}
