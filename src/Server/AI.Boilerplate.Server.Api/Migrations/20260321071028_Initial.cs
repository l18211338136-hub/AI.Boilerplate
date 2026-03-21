using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pgvector;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AI.Boilerplate.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "jobs");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:vector", ",,");

            migrationBuilder.CreateSequence<int>(
                name: "ProductShortId",
                startValue: 10051L);

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Kind = table.Column<int>(type: "integer", nullable: false),
                    Path = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => new { x.Id, x.Kind });
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Color = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataProtectionKeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FriendlyName = table.Column<string>(type: "text", nullable: true),
                    Xml = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataProtectionKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HangfireCounter",
                schema: "jobs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Key = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Value = table.Column<long>(type: "bigint", nullable: false),
                    ExpireAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireCounter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HangfireHash",
                schema: "jobs",
                columns: table => new
                {
                    Key = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Field = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true),
                    ExpireAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireHash", x => new { x.Key, x.Field });
                });

            migrationBuilder.CreateTable(
                name: "HangfireList",
                schema: "jobs",
                columns: table => new
                {
                    Key = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true),
                    ExpireAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireList", x => new { x.Key, x.Position });
                });

            migrationBuilder.CreateTable(
                name: "HangfireLock",
                schema: "jobs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    AcquiredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireLock", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HangfireServer",
                schema: "jobs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Heartbeat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WorkerCount = table.Column<int>(type: "integer", nullable: false),
                    Queues = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireServer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HangfireSet",
                schema: "jobs",
                columns: table => new
                {
                    Key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Score = table.Column<double>(type: "double precision", nullable: false),
                    ExpireAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireSet", x => new { x.Key, x.Value });
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemPrompts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PromptKind = table.Column<int>(type: "integer", nullable: false),
                    Markdown = table.Column<string>(type: "text", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemPrompts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: true),
                    BirthDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    EmailTokenRequestedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    PhoneNumberTokenRequestedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ResetPasswordTokenRequestedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TwoFactorTokenRequestedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    OtpRequestedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ElevatedAccessTokenRequestedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    HasProfilePicture = table.Column<bool>(type: "boolean", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ShortId = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('\"ProductShortId\"')"),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: false),
                    DescriptionHTML = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    DescriptionText = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    HasPrimaryImage = table.Column<bool>(type: "boolean", nullable: false),
                    PrimaryImageAltText = table.Column<string>(type: "text", nullable: true),
                    Embedding = table.Column<Vector>(type: "vector(768)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TodoItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false),
                    IsDone = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodoItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IP = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Privileged = table.Column<bool>(type: "boolean", nullable: false),
                    StartedOn = table.Column<long>(type: "bigint", nullable: false),
                    RenewedOn = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SignalRConnectionId = table.Column<string>(type: "text", nullable: true),
                    NotificationStatus = table.Column<int>(type: "integer", nullable: false),
                    DeviceInfo = table.Column<string>(type: "text", nullable: true),
                    PlatformType = table.Column<int>(type: "integer", nullable: true),
                    CultureName = table.Column<string>(type: "text", nullable: true),
                    AppVersion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebAuthnCredential",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "bytea", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PublicKey = table.Column<byte[]>(type: "bytea", nullable: true),
                    SignCount = table.Column<long>(type: "bigint", nullable: false),
                    Transports = table.Column<int[]>(type: "integer[]", nullable: true),
                    IsBackupEligible = table.Column<bool>(type: "boolean", nullable: false),
                    IsBackedUp = table.Column<bool>(type: "boolean", nullable: false),
                    AttestationObject = table.Column<byte[]>(type: "bytea", nullable: true),
                    AttestationClientDataJson = table.Column<byte[]>(type: "bytea", nullable: true),
                    UserHandle = table.Column<byte[]>(type: "bytea", nullable: true),
                    AttestationFormat = table.Column<string>(type: "text", nullable: true),
                    RegDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    AaGuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebAuthnCredential", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebAuthnCredential_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PushNotificationSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeviceId = table.Column<string>(type: "text", nullable: false),
                    Platform = table.Column<string>(type: "text", nullable: false),
                    PushChannel = table.Column<string>(type: "text", nullable: false),
                    P256dh = table.Column<string>(type: "text", nullable: true),
                    Auth = table.Column<string>(type: "text", nullable: true),
                    Endpoint = table.Column<string>(type: "text", nullable: true),
                    UserSessionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Tags = table.Column<string[]>(type: "text[]", nullable: false),
                    ExpirationTime = table.Column<long>(type: "bigint", nullable: false),
                    RenewedOn = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PushNotificationSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PushNotificationSubscriptions_UserSessions_UserSessionId",
                        column: x => x.UserSessionId,
                        principalTable: "UserSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "HangfireJob",
                schema: "jobs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StateId = table.Column<long>(type: "bigint", nullable: true),
                    StateName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ExpireAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    InvocationData = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireJob", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HangfireJobParameter",
                schema: "jobs",
                columns: table => new
                {
                    JobId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireJobParameter", x => new { x.JobId, x.Name });
                    table.ForeignKey(
                        name: "FK_HangfireJobParameter_HangfireJob_JobId",
                        column: x => x.JobId,
                        principalSchema: "jobs",
                        principalTable: "HangfireJob",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HangfireQueuedJob",
                schema: "jobs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JobId = table.Column<long>(type: "bigint", nullable: false),
                    Queue = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    FetchedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireQueuedJob", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HangfireQueuedJob_HangfireJob_JobId",
                        column: x => x.JobId,
                        principalSchema: "jobs",
                        principalTable: "HangfireJob",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HangfireState",
                schema: "jobs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JobId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Data = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangfireState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HangfireState_HangfireJob_JobId",
                        column: x => x.JobId,
                        principalSchema: "jobs",
                        principalTable: "HangfireJob",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Color", "Name" },
                values: new object[,]
                {
                    { new Guid("31d78bd0-0b4f-4e87-b02f-8f66d4ab2845"), "#FFCD56", "Ford" },
                    { new Guid("582b8c19-0709-4dae-b7a6-fa0e704dad3c"), "#FF6384", "Nissan" },
                    { new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), "#4BC0C0", "Benz" },
                    { new Guid("747f6d66-7524-40ca-8494-f65e85b5ee5d"), "#2B88D8", "Tesla" },
                    { new Guid("ecf0496f-f1e3-4d92-8fe4-0d7fa2b4ffa4"), "#FF9124", "BMW" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("8ff71671-a1d6-5f97-abb9-d87d7b47d6e7"), "8ff71671-a1d6-5f97-abb9-d87d7b47d6e7", "s-admin", "S-ADMIN" },
                    { new Guid("9ff71672-a1d5-4f97-abb7-d87d6b47d5e8"), "9ff71672-a1d5-4f97-abb7-d87d6b47d5e8", "demo", "DEMO" }
                });

            migrationBuilder.InsertData(
                table: "SystemPrompts",
                columns: new[] { "Id", "Markdown", "PromptKind" },
                values: new object[] { new Guid("a8c94d94-0004-4dd0-921c-255e0a581424"), "你是 AI.Boilerplate 应用程序的助手。在下面，你将找到一个包含该应用信息的 markdown 文档，以及随后的用户查询。\n\n# AI.Boilerplate 应用程序 - 功能和使用指南\n\n**[[[GENERAL_INFORMATION_BEGIN]]]**\n\n*   **平台：** 该应用程序可在 Android、iOS、Windows、macOS 以及作为 Web (PWA) 应用程序使用。\n\n* 网站地址: [Website address](https://github.com/l18211338136-hub/AI.Boilerplate)\n* Google Play: [Google Play Link](https://github.com/l18211338136-hub/AI.Boilerplate)\n* Apple Store: [Apple Store Link](https://github.com/l18211338136-hub/AI.Boilerplate)\n* Windows EXE 安装程序: [Windows app link](https://github.com/l18211338136-hub/AI.Boilerplate)\n\n## 1. 账户管理与认证\n\n这些功能涵盖用户注册、登录、账户恢复和安全设置。\n\n### 1.1. 注册 (Sign Up)\n*   **描述：** 允许新用户创建账户。用户可以使用电子邮件地址、电话号码或通过外部身份提供商注册。\n*   **如何使用：**\n    - 导航至 [注册页面](/sign-up)。\n\n### 1.2. 登录 (Sign In)\n*   **描述：** 允许现有用户使用各种方法登录其账户。\n*   **如何使用：**\n    - 导航至 [登录页面](/sign-in)。\n\n### 1.3. 确认账户 (Confirm Account)\n*   **描述：** 在注册后验证用户的电子邮件地址或电话号码，通常通过输入发送给他们的验证码来完成。\n*   **如何使用：**\n    - 导航至 [确认页面](/confirm)（通常在注册后自动重定向）。\n\n### 1.4. 忘记密码 (Forgot Password)\n*   **描述：** 通过向用户注册的电子邮件或电话号码发送重置令牌（代码）来启动密码重置过程。\n*   **如何使用：**\n    - 导航至 [忘记密码页面](/forgot-password)，通常从登录页面链接。\n\n### 1.5. 重置密码 (Reset Password)\n*   **描述：** 允许用户在通过“忘记密码”流程请求重置令牌后设置新密码。\n*   **如何使用：**\n    - 导航至 [重置密码页面](/reset-password)。\n\n## 2. 用户设置 (User Settings)\n\n登录后可访问，这些页面允许用户管理他们的个人资料、账户详细信息、安全设置和活动会话。\n\n### 2.1. 个人资料设置 (Profile Settings)\n*   **描述：** 管理个人用户信息，如姓名、头像、出生日期和性别。\n*   **如何使用：**\n    - 导航至 [个人资料页面](/settings/profile)。\n    - 需要登录。\n\n### 2.2. 账户设置 (Account Settings)\n*   **描述：** 管理特定于账户的详细信息，如电子邮件、电话号码、启用无密码登录和账户删除。\n*   **如何使用：**\n    - 导航至 [账户页面](/settings/account)。\n    - 需要登录。\n\n### 2.3. 双因素认证 (2FA)\n*   **描述：** 通过在登录期间要求第二种验证形式（通常是来自认证器应用程序的代码）来增强账户安全性。\n*   **如何使用：**\n    - 导航至 [双因素认证页面](/settings/tfa)。\n    - 需要登录。\n\n### 2.4. 会话管理 (Session Management)\n*   **描述：** 查看用户当前登录的所有设备和浏览器，并提供远程注销（撤销）特定会话的功能。\n*   **如何使用：**\n    - 导航至 [会话页面](/settings/sessions)。\n    - 需要登录。\n\n## 3. 核心应用程序功能 (Core Application Features)\n\n这些是除了账户管理之外的应用程序主要功能区域。\n### 3.1. 仪表板 (Dashboard)\n*   **描述：** 提供关键应用程序数据（如类别和产品）的高级概述和分析。\n*   **如何使用：**\n    - 导航至 [仪表板页面](/dashboard)。\n    - 需要登录。\n\n### 3.2. 类别管理 (Categories Management)\n*   **描述：** 允许用户查看、创建、编辑和删除类别，通常用于组织产品。\n*   **如何使用：**\n    - 导航至 [类别页面](/categories)。\n    - 需要登录。\n\n### 3.3. 产品管理 (Products Management)\n*   **描述：** 允许用户查看、创建、编辑和删除产品。\n*   **如何使用：**\n    - 导航至 [产品页面](/products)。\n    - 需要登录。\n\n### 3.4. 添加/编辑产品 (Add/Edit Product)\n*   **描述：** 用于创建新产品或修改现有产品的表单页面。\n*   **如何使用：**\n    - 导航至 [添加/编辑产品页面](/add-edit-product)。\n    - 需要登录。\n### 3.6. 待办事项列表 (Todo List)\n*   **描述：** 一个简单的任务管理功能，用于跟踪个人任务。\n*   **如何使用：**\n    - 导航至 [待办事项页面](/todo)。\n    - 需要登录。\n### 3.7. 升级账户 (Upgrade account)\n*   **描述：** 用户可以升级其账户的页面。\n*   **如何使用：**\n    - 导航至 [升级账户页面](/settings/upgradeaccount)。\n    - 需要登录。\n## 4. 信息页面 (Informational Pages)\n\n### 4.1. 关于页面 (About Page)\n*   **描述：** 提供有关应用程序本身的信息。\n*   **如何使用：**\n    - 导航至 [关于页面](/about)。\n\n### 4.2. 条款页面 (Terms Page)\n*   **描述：** 显示法律条款和条件，包括最终用户许可协议 (EULA) 和可能的隐私政策。\n*   **如何使用：**\n    - 导航至 [条款页面](/terms)。\n\n---\n\n**[[[GENERAL_INFORMATION_END]]]**\n\n**[[[INSTRUCTIONS_BEGIN]]]**\n\n- ### 认证工具：\n    - 访问需要登录的页面需要 {{IsAuthenticated}} 为 `true`。\n    - 如果需要提示用户进行认证，可以使用 `ShowSignInModal` 工具。此工具将显示登录模态框，如果成功则返回用户信息，如果取消/失败则返回 null。\n    - 在用户登录后，你**必须**向用户问好。\n\n- ### 语言：\n    - 使用用户查询的语言进行回复。如果无法确定查询的语言，请使用 {{UserCulture}} 变量（如果提供）。\n\n- ### 用户设备信息：\n    - 除非用户在查询中另有说明，否则假设用户的设备是 {{DeviceInfo}} 变量。相应地定制特定于平台的回复（例如，Android、iOS、Windows、macOS、Web）。\n    - 对于任何与时间相关的问题，假设用户的时区 ID 是 {{UserTimeZoneId}} 变量。\n    - **日期和时间：** 当你需要知道当前日期/时间时，使用 `GetCurrentDateTime` 工具。\n    - 假设用户设备的 SignalR 连接 ID 是 {{SignalRConnectionId}} 变量。\n\n- ### 相关性：\n    - 在回复之前，评估用户的查询是否与 AI.Boilerplate 应用程序直接相关。只有当查询涉及提供的 markdown 文档中概述的应用程序功能、使用方法或支持主题，**或者明确请求与汽车相关的产品推荐时**，查询才是相关的。\n    - 忽略并且不回复任何不相关的查询，无论用户的意图或措辞如何。避免参与偏离主题的请求，即使它们看起来是普遍的或对话式的。\n\n      \n- ### 与应用相关的查询（功能与使用）：\n    - **对于有关应用程序功能、如何使用应用程序、账户管理、设置或信息页面的问题：** 使用提供的 markdown 文档，以用户的语言提供准确且简明的答案。\n\n    - **导航请求：** 如果用户明确要求转到某个页面（例如，“带我去仪表板”、“打开产品页面”），使用 `NavigateToPage` 工具。该工具的 `pageUrl` 参数应为 markdown 文档中找到的相对 URL（例如，`/dashboard`, `/products`）：\n\n    - **更改语言/文化请求：** 如果用户要求更改应用语言或提到任何语言偏好（例如，“切换到波斯语”、“将语言更改为英语”、“我想要法语”），使用具有适当文化 LCID 的 `SetCulture` 工具。常见 LCID：1033=en-US, 1065=fa-IR, 1053=sv-SE, 2057=en-GB, 1043=nl-NL, 1081=hi-IN, 2052=zh-CN, 3082=es-ES, 1036=fr-FR, 1025=ar-SA, 1031=de-DE。\n\n    - **更改主题请求：** 如果用户要求更改应用主题、外观或提到深色/浅色模式（例如，“切换到深色模式”、“启用浅色主题”、“让它变暗”），使用 `SetTheme` 工具，参数为“light”或“dark”。\n\n    - **故障排除与错误检测：** 当用户报告问题、错误、崩溃或某些功能无法正常工作时（例如，“应用崩溃了”、“我遇到了一个错误”、“出了点问题”、“它不工作了”），**始终**首先使用 `CheckLastError` 工具从用户设备检索诊断信息。\n        \n        检索到错误信息后：\n        1. 以同理心确认问题（例如，“我看到你在...方面遇到了麻烦”、“我理解这很令人沮丧”）\n        2. 提供实用、易于遵循的步骤来解决问题\n        3. 仅在用户明确要求更多信息时才提供技术细节\n\n        **重要提示：** 不要将 `CheckLastError` 工具用于关于功能或“如何操作”的常见问题。仅在排查实际报告的问题或错误时使用它。\n        \n        **高级故障排除 - 清除应用文件：**\n        - 如果基本故障排除步骤未能解决问题，且问题似乎与损坏的应用数据、缓存文件或持久状态问题有关，你可以**建议**使用 `ClearAppFiles` 工具作为潜在的解决方案。\n        - **重要提示：** 在提供此工具之前，你**必须**向用户解释它的作用（清除本地应用数据、缓存和文件）。\n        - **`ClearAppFiles` 工具会处理所有必要的缓存清除工作。** 不要建议手动清除浏览器缓存或其他手动清除缓存的步骤；该工具就足够了。\n        - **只有在收到用户明确批准/确认后才能调用 `ClearAppFiles` 工具。** 不要未经许可自动调用它。\n        - 成功调用该工具后，告知用户：“我已经清除了应用程序的本地文件。应用程序将很快重新加载。请尝试重新登录，如果问题仍然存在，请告诉我。”\n\n    - 当提及特定的应用页面时，包含 markdown 文档中的相对 URL，并使用 markdown 格式（例如，[注册页面](/sign-up)），并询问他们是否需要你为他们打开该页面。\n\n    - 在整个回复过程中保持乐于助人和专业的语气。\n\n    - 如果用户提出多个问题，将它们列出给用户以确认理解，然后使用清晰的标题分别处理每一个。如果需要，请他们优先考虑：“我看到您有多个问题。您希望我先解决哪个问题？”\n    \n    - 永远不要请求敏感信息（例如，密码、PIN 码）。如果用户主动分享此类数据，请回复：“为了您的安全，请不要分享密码等敏感信息。请放心，您的数据在我们这里是安全的。” ### 处理广告故障请求：\n**[[[ADS_TROUBLE_RULES_BEGIN]]]\"\n*   **如果用户询问在观看广告时遇到问题（例如，“广告未显示”、“广告被拦截”、“未进行升级”）:**\n    1.  *充当技术支持。*\n    2.  **根据用户的设备信息提供逐步指导来解决问题，重点关注广告拦截器和浏览器防跟踪功能。\n**[[[ADS_TROUBLE_RULES_END]]]**\n\n- ### 用户反馈和建议：\n    - 如果用户提供反馈或建议功能，请回复：“感谢您的反馈！这对我们很有价值，我会将其转达给产品团队。”如果反馈不清晰，请要求澄清：“您能提供有关您建议的更多细节吗？”\n\n- ### 处理沮丧或困惑：\n    - 如果用户似乎感到沮丧或困惑，使用安抚的语言并主动提出澄清：“很抱歉这让您感到困惑。我在这里提供帮助！需要我再解释一遍吗？”\n\n- ### 未解决的问题：\n    - 如果你无法解决用户的问题（无论是通过 markdown 信息还是工具），请回复：“很抱歉我无法解决您的问题 / 完全满足您的请求。我理解这一定让您感到非常沮丧。”\n    - 如果用户的电子邮件（{{UserEmail}} 变量）为空，请求提供他们的电子邮件。\n    - 调用 `SaveUserEmailAndConversationHistory` 工具。\n    - 确认：“感谢您提供电子邮件。很快会有客服人员跟进。”然后问：“您还有其他需要我协助的问题吗？”\n\n**[[[INSTRUCTIONS_END]]]**\n", 0 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "BirthDate", "ConcurrencyStamp", "ElevatedAccessTokenRequestedOn", "Email", "EmailConfirmed", "EmailTokenRequestedOn", "FullName", "Gender", "HasProfilePicture", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OtpRequestedOn", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PhoneNumberTokenRequestedOn", "ResetPasswordTokenRequestedOn", "SecurityStamp", "TwoFactorEnabled", "TwoFactorTokenRequestedOn", "UserName" },
                values: new object[] { new Guid("8ff71671-a1d6-4f97-abb9-d87d7b47d6e7"), 0, new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "315e1a26-5b3a-4544-8e91-2760cd28e231", null, "761516331@qq.com", true, new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "AI.Boilerplate test account", 0, false, true, null, "761516331@QQ.COM", "TEST", null, "AQAAAAIAAYagAAAAEP0v3wxkdWtMkHA3Pp5/JfS+42/Qto9G05p2mta6dncSK37hPxEHa3PGE4aqN30Aag==", "+31684207362", true, null, null, "959ff4a9-4b07-4cc1-8141-c5fc033daf83", false, null, "test" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedOn", "DescriptionHTML", "DescriptionText", "Embedding", "HasPrimaryImage", "Name", "Price", "PrimaryImageAltText", "ShortId" },
                values: new object[,]
                {
                    { new Guid("10e9f8e7-b6a5-d4c3-b2a1-d0e9f8e7b6a5"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Driving performance, elevated.</h3><p>Born on the racetrack, the AMG GT Coupe embodies the essence of a pure sports car with breathtaking power and precision handling.</p>", "Driving performance, elevated.\nBorn on the racetrack, the AMG GT Coupe embodies the essence of a pure sports car with breathtaking power and precision handling.\n", null, false, "Mercedes-AMG GT Coupe", 150000m, null, 10018 },
                    { new Guid("21d0e9f8-e7b6-a5d4-c3b2-a1d0e9f8e7b6"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Sporty elegance, redefined.</h3><p>The all-new CLE Coupe merges expressive design with dynamic handling and advanced technology, creating a modern icon of desire.</p>", "Sporty elegance, redefined.\nThe all-new CLE Coupe merges expressive design with dynamic handling and advanced technology, creating a modern icon of desire.\n", null, false, "CLE Coupe", 65000m, null, 10017 },
                    { new Guid("32a1d0e9-f8e7-b6a5-d4c3-b2a1d0e9f8e7"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Irresistible from every angle.</h3><p>With its iconic sloping roofline and sporty stance, the CLA Coupe captivates with expressive design and agile performance.</p>", "Irresistible from every angle.\nWith its iconic sloping roofline and sporty stance, the CLA Coupe captivates with expressive design and agile performance.\n", null, false, "CLA COUPE", 50500m, null, 10016 },
                    { new Guid("43b2a1d0-e9f8-e7b6-a5d4-c3b2a1d0e9f8"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>The pinnacle of automotive desire.</h3><p>Engineered without compromise, the S-Class Sedan pioneers innovations in safety, comfort, and driving experience, defining luxury travel.</p>", "The pinnacle of automotive desire.\nEngineered without compromise, the S-Class Sedan pioneers innovations in safety, comfort, and driving experience, defining luxury travel.\n", null, false, "S-Class Sedan", 140000m, null, 10015 },
                    { new Guid("512eb70b-1d39-4845-88c0-fe19cd2d1979"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Advanced, adaptive, adventurous.</h3><p>More than new, it can update its advancements down the road. More than thoughtful, it can anticipate needs and desires. Beyond futuristic, it can make better use of your time, and make your time using it better.</p>", "Advanced, adaptive, adventurous.\nMore than new, it can update its advancements down the road. More than thoughtful, it can anticipate needs and desires. Beyond futuristic, it can make better use of your time, and make your time using it better.\n", null, false, "EQE SUV", 79050m, null, 10003 },
                    { new Guid("54c3b2a1-d0e9-f8e7-b6a5-d4c3b2a1d0e9"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>The heart of the brand, intelligently refined.</h3><p>A masterpiece of intelligence, the E-Class Sedan seamlessly blends dynamic design, luxurious comfort, and groundbreaking driver assistance systems.</p>", "The heart of the brand, intelligently refined.\nA masterpiece of intelligence, the E-Class Sedan seamlessly blends dynamic design, luxurious comfort, and groundbreaking driver assistance systems.\n", null, false, "E-Class Sedan", 73900m, null, 10014 },
                    { new Guid("5746ae3d-5116-4774-9d55-0ff496e5186f"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Electric, essential, quintessential</h3><p>It's futuristic, forward and fresh, but you know its core values. Ever-refined luxury. Ever-advancing innovation. And a never-ending devotion to your well-being. Perhaps no electric sedan feels so new, yet so natural.</p>", "Electric, essential, quintessential\nIt's futuristic, forward and fresh, but you know its core values. Ever-refined luxury. Ever-advancing innovation. And a never-ending devotion to your well-being. Perhaps no electric sedan feels so new, yet so natural.\n", null, false, "EQE Sedan", 76050m, null, 10001 },
                    { new Guid("65d4c3b2-a1d0-e9f8-e7b6-a5d4c3b2a1d0"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>An icon reengineered.</h3><p>Instantly recognizable, eternally capable. The G-Class remains the definitive luxury off-roader, blending timeless design with modern technology and rugged performance.</p>", "An icon reengineered.\nInstantly recognizable, eternally capable. The G-Class remains the definitive luxury off-roader, blending timeless design with modern technology and rugged performance.\n", null, false, "G-CLASS SUV", 180000m, null, 10013 },
                    { new Guid("76a5d4c3-b2a1-d0e9-f8e7-b6a5d4c3b2a1"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>The S-Class of SUVs.</h3><p>Offering first-class travel for up to seven passengers, the GLS combines commanding presence with unparalleled luxury, space, and technology.</p>", "The S-Class of SUVs.\nOffering first-class travel for up to seven passengers, the GLS combines commanding presence with unparalleled luxury, space, and technology.\n", null, false, "GLS SUV", 115100m, null, 10012 },
                    { new Guid("87b6a5d4-c3b2-a1d0-e9f8-e7b6a5d4c3b2"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Athletic presence, commanding performance.</h3><p>The GLE Coupe blends the muscular stance of an SUV with the elegant lines of a coupe, delivering exhilarating performance and undeniable style.</p>", "Athletic presence, commanding performance.\nThe GLE Coupe blends the muscular stance of an SUV with the elegant lines of a coupe, delivering exhilarating performance and undeniable style.\n", null, false, "GLE Coupe", 94900m, null, 10011 },
                    { new Guid("98e7b6a5-d4c3-b2a1-d0e9-f8e7b6a5d4c3"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Master of every ground.</h3><p>Intelligent, spacious, and capable, the GLE SUV offers cutting-edge technology and luxurious comfort for families and adventurers alike.</p>", "Master of every ground.\nIntelligent, spacious, and capable, the GLE SUV offers cutting-edge technology and luxurious comfort for families and adventurers alike.\n", null, false, "GLE SUV", 82800m, null, 10010 },
                    { new Guid("9a59dda2-7b12-4cc1-9658-d2586eef91d7"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Range to roam. Room for up to 7.</h3><p>It's a versatile SUV with room for up to seven. And an advanced EV you can enjoy every day. Intelligent technology and thoughtful luxury are delivered with swift response and silenced refinement.</p>", "Range to roam. Room for up to 7.\nIt's a versatile SUV with room for up to seven. And an advanced EV you can enjoy every day. Intelligent technology and thoughtful luxury are delivered with swift response and silenced refinement.\n", null, false, "EQB SUV", 54200m, null, 10000 },
                    { new Guid("a1b2c3d4-e5f6-7890-1234-567890abcdef"), new Guid("31d78bd0-0b4f-4e87-b02f-8f66d4ab2845"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Built Ford Tough.</h3><p>America's best-selling truck, known for its capability, innovation, and toughness for work or play.</p>", "Built Ford Tough.\nAmerica's best-selling truck, known for its capability, innovation, and toughness for work or play.\n", null, false, "Ford F-150", 45000m, null, 10020 },
                    { new Guid("a1b2c3d4-e5f6-a7b8-9012-3456c7d8e9f0"), new Guid("747f6d66-7524-40ca-8494-f65e85b5ee5d"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Radical Electric Pickup.</h3><p>A futuristic pickup truck with an exoskeleton design, offering utility, performance, and durability.</p>", "Radical Electric Pickup.\nA futuristic pickup truck with an exoskeleton design, offering utility, performance, and durability.\n", null, false, "Tesla Cybertruck", 70000m, null, 10050 },
                    { new Guid("a3b4c5d6-e7f8-a9b0-1234-5678c9d0e1f2"), new Guid("582b8c19-0709-4dae-b7a6-fa0e704dad3c"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Right-Sized Pickup.</h3><p>A capable and durable mid-size truck designed for both work duties and weekend adventures.</p>", "Right-Sized Pickup.\nA capable and durable mid-size truck designed for both work duties and weekend adventures.\n", null, false, "Nissan Frontier", 35000m, null, 10032 },
                    { new Guid("a5b6c7d8-e9f0-a1b2-3456-7890c1d2e3f4"), new Guid("747f6d66-7524-40ca-8494-f65e85b5ee5d"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Mass-Market Electric Sedan.</h3><p>Tesla's most affordable car, offering impressive range, performance, and minimalist design.</p>", "Mass-Market Electric Sedan.\nTesla's most affordable car, offering impressive range, performance, and minimalist design.\n", null, false, "Tesla Model 3", 45000m, null, 10044 },
                    { new Guid("a7b8c9d0-e1f2-a3b4-5678-9012c3d4e5f6"), new Guid("31d78bd0-0b4f-4e87-b02f-8f66d4ab2845"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Compact Truck, Big Ideas.</h3><p>An affordable and versatile compact pickup, available with a standard hybrid powertrain.</p>", "Compact Truck, Big Ideas.\nAn affordable and versatile compact pickup, available with a standard hybrid powertrain.\n", null, false, "Ford Maverick", 28000m, null, 10026 },
                    { new Guid("a9b0c1d2-e3f4-a5b6-7890-1234c5d6e7f8"), new Guid("ecf0496f-f1e3-4d92-8fe4-0d7fa2b4ffa4"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>The Boss.</h3><p>The original Sports Activity Vehicle, setting benchmarks for luxury, performance, and capability in its class.</p>", "The Boss.\nThe original Sports Activity Vehicle, setting benchmarks for luxury, performance, and capability in its class.\n", null, false, "BMW X5 SAV", 70000m, null, 10038 },
                    { new Guid("a9f8e7b6-a5d4-c3b2-a1d0-e9f8e7b6a5d4"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Sporty style, SUV substance.</h3><p>Combining the dynamic presence of a coupe with the versatility of an SUV, the GLC Coupe makes a powerful statement on any road.</p>", "Sporty style, SUV substance.\nCombining the dynamic presence of a coupe with the versatility of an SUV, the GLC Coupe makes a powerful statement on any road.\n", null, false, "GLC Coupe", 63500m, null, 10009 },
                    { new Guid("b0a9f8e7-b6a5-d4c3-b2a1-d0e9f8e7b6a5"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Intelligent drive, impressive design.</h3><p>The GLC SUV sets benchmarks for comfort, technology, and performance in the mid-size luxury segment, adapting effortlessly to your driving needs.</p>", "Intelligent drive, impressive design.\nThe GLC SUV sets benchmarks for comfort, technology, and performance in the mid-size luxury segment, adapting effortlessly to your driving needs.\n", null, false, "GLC SUV", 58900m, null, 10008 },
                    { new Guid("b0c1d2e3-f4a5-b6c7-8901-2345d6e7f8a9"), new Guid("ecf0496f-f1e3-4d92-8fe4-0d7fa2b4ffa4"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Executive Athleticism.</h3><p>A sophisticated blend of dynamic performance, cutting-edge technology, and luxurious comfort for the executive class.</p>", "Executive Athleticism.\nA sophisticated blend of dynamic performance, cutting-edge technology, and luxurious comfort for the executive class.\n", null, false, "BMW 5 Series Sedan", 65000m, null, 10039 },
                    { new Guid("b2c3d4e5-f6a7-89b0-12c3-45d678e9f0a1"), new Guid("31d78bd0-0b4f-4e87-b02f-8f66d4ab2845"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Iconic American Muscle.</h3><p>Thrilling performance and unmistakable style define the legendary Ford Mustang coupe.</p>", "Iconic American Muscle.\nThrilling performance and unmistakable style define the legendary Ford Mustang coupe.\n", null, false, "Ford Mustang", 40000m, null, 10021 },
                    { new Guid("b4c5d6e7-f8a9-b0c1-2345-6789d0e1f2a3"), new Guid("582b8c19-0709-4dae-b7a6-fa0e704dad3c"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Expressive Subcompact Crossover.</h3><p>Stand out with customizable style and enjoy city-friendly agility and smart technology.</p>", "Expressive Subcompact Crossover.\nStand out with customizable style and enjoy city-friendly agility and smart technology.\n", null, false, "Nissan Kicks", 24000m, null, 10033 },
                    { new Guid("b6c7d8e9-f0a1-b2c3-4567-8901d2e3f4a5"), new Guid("747f6d66-7524-40ca-8494-f65e85b5ee5d"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Compact Electric SUV.</h3><p>A versatile electric SUV offering ample space, performance, and access to Tesla's Supercharger network.</p>", "Compact Electric SUV.\nA versatile electric SUV offering ample space, performance, and access to Tesla's Supercharger network.\n", null, false, "Tesla Model Y", 55000m, null, 10045 },
                    { new Guid("b8c9d0e1-f2a3-b4c5-6789-0123d4e5f6a7"), new Guid("31d78bd0-0b4f-4e87-b02f-8f66d4ab2845"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Stylish Mid-Size Crossover.</h3><p>Combines sophisticated design with smart technology and engaging performance in a two-row crossover.</p>", "Stylish Mid-Size Crossover.\nCombines sophisticated design with smart technology and engaging performance in a two-row crossover.\n", null, false, "Ford Edge", 40000m, null, 10027 },
                    { new Guid("c1b0a9f8-e7b6-a5d4-c3b2-a1d0e9f8e7b6"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Versatility meets space.</h3><p>Surprisingly spacious for its size, the GLB offers an optional third row, making it the flexible and family-friendly compact SUV for all your adventures.</p>", "Versatility meets space.\nSurprisingly spacious for its size, the GLB offers an optional third row, making it the flexible and family-friendly compact SUV for all your adventures.\n", null, false, "GLB SUV", 52500m, null, 10007 },
                    { new Guid("c1d2e3f4-a5b6-c7d8-9012-3456e7f8a9b0"), new Guid("ecf0496f-f1e3-4d92-8fe4-0d7fa2b4ffa4"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Electric Performance Coupe.</h3><p>BMW's first all-electric Gran Coupe, delivering impressive range and signature BMW driving dynamics.</p>", "Electric Performance Coupe.\nBMW's first all-electric Gran Coupe, delivering impressive range and signature BMW driving dynamics.\n", null, false, "BMW i4 Gran Coupe", 60000m, null, 10040 },
                    { new Guid("c3d4e5f6-a7b8-9c01-23d4-56e789f0a1b2"), new Guid("31d78bd0-0b4f-4e87-b02f-8f66d4ab2845"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Adventure Ready SUV.</h3><p>A spacious and capable SUV designed for family adventures, offering three rows of seating and modern tech.</p>", "Adventure Ready SUV.\nA spacious and capable SUV designed for family adventures, offering three rows of seating and modern tech.\n", null, false, "Ford Explorer", 48000m, null, 10022 },
                    { new Guid("c5d6e7f8-a9b0-c1d2-3456-7890e1f2a3b4"), new Guid("582b8c19-0709-4dae-b7a6-fa0e704dad3c"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Nissan's Electric Crossover.</h3><p>Experience the future of driving with the all-electric Ariya, blending sleek design with advanced EV technology.</p>", "Nissan's Electric Crossover.\nExperience the future of driving with the all-electric Ariya, blending sleek design with advanced EV technology.\n", null, false, "Nissan Ariya", 50000m, null, 10034 },
                    { new Guid("c7d8e9f0-a1b2-c3d4-5678-9012e3f4a5b6"), new Guid("747f6d66-7524-40ca-8494-f65e85b5ee5d"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Luxury Electric Benchmark.</h3><p>The sedan that redefined electric performance, offering incredible acceleration, range, and technology.</p>", "Luxury Electric Benchmark.\nThe sedan that redefined electric performance, offering incredible acceleration, range, and technology.\n", null, false, "Tesla Model S", 90000m, null, 10046 },
                    { new Guid("c9d0e1f2-a3b4-c5d6-7890-1234e5f6a7b8"), new Guid("582b8c19-0709-4dae-b7a6-fa0e704dad3c"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Family-Friendly Crossover.</h3><p>Nissan's popular compact SUV, offering advanced safety features and a comfortable, versatile interior.</p>", "Family-Friendly Crossover.\nNissan's popular compact SUV, offering advanced safety features and a comfortable, versatile interior.\n", null, false, "Nissan Rogue", 32000m, null, 10028 },
                    { new Guid("d0e1f2a3-b4c5-d6e7-8901-2345f6a7b8c9"), new Guid("582b8c19-0709-4dae-b7a6-fa0e704dad3c"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Intelligent Midsize Sedan.</h3><p>A stylish sedan featuring available All-Wheel Drive and driver-assist technologies.</p>", "Intelligent Midsize Sedan.\nA stylish sedan featuring available All-Wheel Drive and driver-assist technologies.\n", null, false, "Nissan Altima", 30000m, null, 10029 },
                    { new Guid("d2a1b0c9-f8e7-b6a5-d4c3-b2a1d0e9f8e7"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Compact dimensions, grand aspirations.</h3><p>Agile and adventurous, the GLA combines SUV versatility with compact efficiency, perfect for navigating city streets or exploring scenic routes.</p>", "Compact dimensions, grand aspirations.\nAgile and adventurous, the GLA combines SUV versatility with compact efficiency, perfect for navigating city streets or exploring scenic routes.\n", null, false, "GLA SUV", 49900m, null, 10006 },
                    { new Guid("d2e3f4a5-b6c7-d8e9-0123-4567f8a9b0c1"), new Guid("ecf0496f-f1e3-4d92-8fe4-0d7fa2b4ffa4"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Electric Technology Flagship.</h3><p>A bold vision of the future SAV, featuring sustainable luxury, groundbreaking tech, and exhilarating electric power.</p>", "Electric Technology Flagship.\nA bold vision of the future SAV, featuring sustainable luxury, groundbreaking tech, and exhilarating electric power.\n", null, false, "BMW iX SAV", 90000m, null, 10041 },
                    { new Guid("d4e5f6a7-b8c9-d0e1-f234-567890a1b2c3"), new Guid("31d78bd0-0b4f-4e87-b02f-8f66d4ab2845"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Compact Versatility.</h3><p>A stylish and efficient compact SUV offering flexibility for city driving and weekend getaways.</p>", "Compact Versatility.\nA stylish and efficient compact SUV offering flexibility for city driving and weekend getaways.\n", null, false, "Ford Escape", 35000m, null, 10023 },
                    { new Guid("d6e7f8a9-b0c1-d2e3-4567-8901f2a3b4c5"), new Guid("582b8c19-0709-4dae-b7a6-fa0e704dad3c"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Legendary Performance.</h3><p>The iconic sports car returns, pairing timeless design with thrilling twin-turbo V6 power.</p>", "Legendary Performance.\nThe iconic sports car returns, pairing timeless design with thrilling twin-turbo V6 power.\n", null, false, "Nissan Z", 45000m, null, 10035 },
                    { new Guid("d8e9f0a1-b2c3-d4e5-6789-0123f4a5b6c7"), new Guid("747f6d66-7524-40ca-8494-f65e85b5ee5d"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Electric SUV with Falcon Wings.</h3><p>A unique family SUV featuring distinctive Falcon Wing doors, impressive range, and performance.</p>", "Electric SUV with Falcon Wings.\nA unique family SUV featuring distinctive Falcon Wing doors, impressive range, and performance.\n", null, false, "Tesla Model X", 100000m, null, 10047 },
                    { new Guid("e1f2a3b4-c5d6-e7f8-9012-3456a7b8c9d0"), new Guid("582b8c19-0709-4dae-b7a6-fa0e704dad3c"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Sharp Compact Sedan.</h3><p>Offers unexpected style, standard safety features, and premium feel in the compact sedan class.</p>", "Sharp Compact Sedan.\nOffers unexpected style, standard safety features, and premium feel in the compact sedan class.\n", null, false, "Nissan Sentra", 25000m, null, 10030 },
                    { new Guid("e3b2a1d0-c9f8-e7b6-a5d4-c3b2a1d0e9f8"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>The future of luxury, electrified.</h3><p>The flagship electric sedan fuses progressive design with pioneering technology and breathtaking performance, setting a new standard for electric mobility.</p>", "The future of luxury, electrified.\nThe flagship electric sedan fuses progressive design with pioneering technology and breathtaking performance, setting a new standard for electric mobility.\n", null, false, "EQS Sedan", 136000m, null, 10005 },
                    { new Guid("e3f4a5b6-c7d8-e9f0-1234-5678a9b0c1d2"), new Guid("ecf0496f-f1e3-4d92-8fe4-0d7fa2b4ffa4"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>High-Performance Icon.</h3><p>The legendary M3 delivers uncompromising track-ready performance combined with everyday usability.</p>", "High-Performance Icon.\nThe legendary M3 delivers uncompromising track-ready performance combined with everyday usability.\n", null, false, "BMW M3 Sedan", 80000m, null, 10042 },
                    { new Guid("e5f6a7b8-c9d0-e1f2-3456-7890a1b2c3d4"), new Guid("31d78bd0-0b4f-4e87-b02f-8f66d4ab2845"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Built Wild.</h3><p>Return of an icon. The Ford Bronco is engineered for rugged off-road capability and adventure.</p>", "Built Wild.\nReturn of an icon. The Ford Bronco is engineered for rugged off-road capability and adventure.\n", null, false, "Ford Bronco", 42000m, null, 10024 },
                    { new Guid("e7f8a9b0-c1d2-e3f4-5678-9012a3b4c5d6"), new Guid("ecf0496f-f1e3-4d92-8fe4-0d7fa2b4ffa4"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>The Ultimate Driving Machine.</h3><p>The quintessential sports sedan, balancing dynamic performance with everyday usability and luxury.</p>", "The Ultimate Driving Machine.\nThe quintessential sports sedan, balancing dynamic performance with everyday usability and luxury.\n", null, false, "BMW 3 Series Sedan", 50000m, null, 10036 },
                    { new Guid("e9f0a1b2-c3d4-e5f6-7890-1234a5b6c7d8"), new Guid("747f6d66-7524-40ca-8494-f65e85b5ee5d"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Enhanced Electric Sport Sedan.</h3><p>Takes the Model 3 foundation and adds quicker acceleration, track mode, and sportier tuning.</p>", "Enhanced Electric Sport Sedan.\nTakes the Model 3 foundation and adds quicker acceleration, track mode, and sportier tuning.\n", null, false, "Tesla Model 3 Performance", 55000m, null, 10048 },
                    { new Guid("f0a1b2c3-d4e5-f6a7-8901-2345b6c7d8e9"), new Guid("747f6d66-7524-40ca-8494-f65e85b5ee5d"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Beyond Ludicrous Speed.</h3><p>Offers staggering acceleration figures, making it one of the quickest production cars ever built.</p>", "Beyond Ludicrous Speed.\nOffers staggering acceleration figures, making it one of the quickest production cars ever built.\n", null, false, "Tesla Model S Plaid", 110000m, null, 10049 },
                    { new Guid("f2a3b4c5-d6e7-f8a9-0123-4567b8c9d0e1"), new Guid("582b8c19-0709-4dae-b7a6-fa0e704dad3c"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Rugged 3-Row SUV.</h3><p>Return to rugged. The Pathfinder offers seating for up to eight and capable performance for family adventures.</p>", "Rugged 3-Row SUV.\nReturn to rugged. The Pathfinder offers seating for up to eight and capable performance for family adventures.\n", null, false, "Nissan Pathfinder", 40000m, null, 10031 },
                    { new Guid("f4a3b2c1-d0e9-f8a7-b6c5-d4e3f2a1b0c9"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>The pinnacle of electric luxury SUVs.</h3><p>Experience flagship comfort and groundbreaking technology in an all-electric SUV form, redefining sustainable luxury with space for up to seven.</p>", "The pinnacle of electric luxury SUVs.\nExperience flagship comfort and groundbreaking technology in an all-electric SUV form, redefining sustainable luxury with space for up to seven.\n", null, false, "EQS SUV", 136000m, null, 10004 },
                    { new Guid("f4a5b6c7-d8e9-f0a1-2345-6789b0c1d2e3"), new Guid("ecf0496f-f1e3-4d92-8fe4-0d7fa2b4ffa4"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Open-Top Freedom.</h3><p>A classic roadster experience with modern BMW performance, agility, and style.</p>", "Open-Top Freedom.\nA classic roadster experience with modern BMW performance, agility, and style.\n", null, false, "BMW Z4 Roadster", 60000m, null, 10043 },
                    { new Guid("f6a7b8c9-d0e1-f2a3-4567-8901b2c3d4e5"), new Guid("31d78bd0-0b4f-4e87-b02f-8f66d4ab2845"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Electric Thrills.</h3><p>An all-electric SUV bearing the Mustang name, delivering exhilarating performance and advanced technology.</p>", "Electric Thrills.\nAn all-electric SUV bearing the Mustang name, delivering exhilarating performance and advanced technology.\n", null, false, "Ford Mustang Mach-E", 55000m, null, 10025 },
                    { new Guid("f8a9b0c1-d2e3-f4a5-6789-0123b4c5d6e7"), new Guid("ecf0496f-f1e3-4d92-8fe4-0d7fa2b4ffa4"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Versatile Sport Activity Vehicle.</h3><p>Combines dynamic BMW driving characteristics with the versatility and utility of an SAV.</p>", "Versatile Sport Activity Vehicle.\nCombines dynamic BMW driving characteristics with the versatility and utility of an SAV.\n", null, false, "BMW X3 SAV", 55000m, null, 10037 },
                    { new Guid("f9f8e7b6-a5d4-c3b2-a1d0-e9f8e7b6a5d4"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>Open-air freedom, elegant design.</h3><p>Experience the joy of driving with the top down in the CLE Cabriolet, offering sophisticated style, year-round comfort, and exhilarating performance.</p>", "Open-air freedom, elegant design.\nExperience the joy of driving with the top down in the CLE Cabriolet, offering sophisticated style, year-round comfort, and exhilarating performance.\n", null, false, "CLE Cabriolet", 75000m, null, 10019 }
                });

            migrationBuilder.InsertData(
                table: "RoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "mx-p-s", "-1", new Guid("8ff71671-a1d6-5f97-abb9-d87d7b47d6e7") },
                    { 2, "features", "3.0", new Guid("9ff71672-a1d5-4f97-abb7-d87d6b47d5e8") },
                    { 3, "features", "3.1", new Guid("9ff71672-a1d5-4f97-abb7-d87d6b47d5e8") },
                    { 4, "features", "4.0", new Guid("9ff71672-a1d5-4f97-abb7-d87d6b47d5e8") }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("8ff71671-a1d6-5f97-abb9-d87d7b47d6e7"), new Guid("8ff71671-a1d6-4f97-abb9-d87d7b47d6e7") });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HangfireCounter_ExpireAt",
                schema: "jobs",
                table: "HangfireCounter",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireCounter_Key_Value",
                schema: "jobs",
                table: "HangfireCounter",
                columns: new[] { "Key", "Value" });

            migrationBuilder.CreateIndex(
                name: "IX_HangfireHash_ExpireAt",
                schema: "jobs",
                table: "HangfireHash",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireJob_ExpireAt",
                schema: "jobs",
                table: "HangfireJob",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireJob_StateId",
                schema: "jobs",
                table: "HangfireJob",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireJob_StateName",
                schema: "jobs",
                table: "HangfireJob",
                column: "StateName");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireList_ExpireAt",
                schema: "jobs",
                table: "HangfireList",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireQueuedJob_JobId",
                schema: "jobs",
                table: "HangfireQueuedJob",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireQueuedJob_Queue_FetchedAt",
                schema: "jobs",
                table: "HangfireQueuedJob",
                columns: new[] { "Queue", "FetchedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_HangfireServer_Heartbeat",
                schema: "jobs",
                table: "HangfireServer",
                column: "Heartbeat");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireSet_ExpireAt",
                schema: "jobs",
                table: "HangfireSet",
                column: "ExpireAt");

            migrationBuilder.CreateIndex(
                name: "IX_HangfireSet_Key_Score",
                schema: "jobs",
                table: "HangfireSet",
                columns: new[] { "Key", "Score" });

            migrationBuilder.CreateIndex(
                name: "IX_HangfireState_JobId",
                schema: "jobs",
                table: "HangfireState",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ShortId",
                table: "Products",
                column: "ShortId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PushNotificationSubscriptions_UserSessionId",
                table: "PushNotificationSubscriptions",
                column: "UserSessionId",
                unique: true,
                filter: "'UserSessionId' IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId_ClaimType_ClaimValue",
                table: "RoleClaims",
                columns: new[] { "RoleId", "ClaimType", "ClaimValue" });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SystemPrompts_PromptKind",
                table: "SystemPrompts",
                column: "PromptKind",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_UserId",
                table: "TodoItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId_ClaimType_ClaimValue",
                table: "UserClaims",
                columns: new[] { "UserId", "ClaimType", "ClaimValue" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId_UserId",
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "'Email' IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneNumber",
                table: "Users",
                column: "PhoneNumber",
                unique: true,
                filter: "'PhoneNumber' IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_UserId",
                table: "UserSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WebAuthnCredential_UserId",
                table: "WebAuthnCredential",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_HangfireJob_HangfireState_StateId",
                schema: "jobs",
                table: "HangfireJob",
                column: "StateId",
                principalSchema: "jobs",
                principalTable: "HangfireState",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HangfireJob_HangfireState_StateId",
                schema: "jobs",
                table: "HangfireJob");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "DataProtectionKeys");

            migrationBuilder.DropTable(
                name: "HangfireCounter",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "HangfireHash",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "HangfireJobParameter",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "HangfireList",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "HangfireLock",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "HangfireQueuedJob",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "HangfireServer",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "HangfireSet",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "PushNotificationSubscriptions");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "SystemPrompts");

            migrationBuilder.DropTable(
                name: "TodoItems");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "WebAuthnCredential");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "UserSessions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "HangfireState",
                schema: "jobs");

            migrationBuilder.DropTable(
                name: "HangfireJob",
                schema: "jobs");

            migrationBuilder.DropSequence(
                name: "ProductShortId");
        }
    }
}
