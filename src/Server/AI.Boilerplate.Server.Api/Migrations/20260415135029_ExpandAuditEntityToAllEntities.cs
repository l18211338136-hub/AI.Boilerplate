using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI.Boilerplate.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class ExpandAuditEntityToAllEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegDate",
                table: "WebAuthnCredential");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "RagKnowledgeBases");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "RagDocuments");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "RagChunks");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "RagChunkingSettings");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "AddedOn",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Addresses");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "WebAuthnCredential",
                type: "uuid",
                nullable: true,
                comment: "创建人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "WebAuthnCredential",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "WebAuthnCredential",
                type: "uuid",
                nullable: true,
                comment: "删除人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "WebAuthnCredential",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "WebAuthnCredential",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "WebAuthnCredential",
                type: "uuid",
                nullable: true,
                comment: "最后修改人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "WebAuthnCredential",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后修改时间");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "UserTokens",
                type: "uuid",
                nullable: true,
                comment: "创建人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "UserTokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "UserTokens",
                type: "uuid",
                nullable: true,
                comment: "删除人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "UserTokens",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserTokens",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "UserTokens",
                type: "uuid",
                nullable: true,
                comment: "最后修改人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "UserTokens",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后修改时间");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "UserSessions",
                type: "uuid",
                nullable: true,
                comment: "创建人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "UserSessions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "UserSessions",
                type: "uuid",
                nullable: true,
                comment: "删除人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "UserSessions",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserSessions",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "UserSessions",
                type: "uuid",
                nullable: true,
                comment: "最后修改人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "UserSessions",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后修改时间");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Users",
                type: "uuid",
                nullable: true,
                comment: "创建人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Users",
                type: "uuid",
                nullable: true,
                comment: "删除人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Users",
                type: "uuid",
                nullable: true,
                comment: "最后修改人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后修改时间");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "UserRoles",
                type: "uuid",
                nullable: true,
                comment: "创建人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "UserRoles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "UserRoles",
                type: "uuid",
                nullable: true,
                comment: "删除人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "UserRoles",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserRoles",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "UserRoles",
                type: "uuid",
                nullable: true,
                comment: "最后修改人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "UserRoles",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后修改时间");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "UserLogins",
                type: "uuid",
                nullable: true,
                comment: "创建人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "UserLogins",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "UserLogins",
                type: "uuid",
                nullable: true,
                comment: "删除人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "UserLogins",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserLogins",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "UserLogins",
                type: "uuid",
                nullable: true,
                comment: "最后修改人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "UserLogins",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后修改时间");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "UserClaims",
                type: "uuid",
                nullable: true,
                comment: "创建人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "UserClaims",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "UserClaims",
                type: "uuid",
                nullable: true,
                comment: "删除人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "UserClaims",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserClaims",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "UserClaims",
                type: "uuid",
                nullable: true,
                comment: "最后修改人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "UserClaims",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后修改时间");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "TodoItems",
                type: "uuid",
                nullable: true,
                comment: "创建人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "TodoItems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "TodoItems",
                type: "uuid",
                nullable: true,
                comment: "删除人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "TodoItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TodoItems",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "TodoItems",
                type: "uuid",
                nullable: true,
                comment: "最后修改人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "TodoItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后修改时间");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "SystemPrompts",
                type: "uuid",
                nullable: true,
                comment: "创建人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "SystemPrompts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "SystemPrompts",
                type: "uuid",
                nullable: true,
                comment: "删除人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "SystemPrompts",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SystemPrompts",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "SystemPrompts",
                type: "uuid",
                nullable: true,
                comment: "最后修改人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "SystemPrompts",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后修改时间");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Roles",
                type: "uuid",
                nullable: true,
                comment: "创建人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Roles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Roles",
                type: "uuid",
                nullable: true,
                comment: "删除人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Roles",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Roles",
                type: "uuid",
                nullable: true,
                comment: "最后修改人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Roles",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后修改时间");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "RoleClaims",
                type: "uuid",
                nullable: true,
                comment: "创建人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "RoleClaims",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "RoleClaims",
                type: "uuid",
                nullable: true,
                comment: "删除人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "RoleClaims",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RoleClaims",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "RoleClaims",
                type: "uuid",
                nullable: true,
                comment: "最后修改人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "RoleClaims",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后修改时间");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "RagKnowledgeBases",
                type: "uuid",
                nullable: true,
                comment: "创建人ID");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "RagKnowledgeBases",
                type: "uuid",
                nullable: true,
                comment: "删除人ID");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "RagKnowledgeBases",
                type: "uuid",
                nullable: true,
                comment: "最后修改人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "RagKnowledgeBases",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后修改时间");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "RagDocuments",
                type: "uuid",
                nullable: true,
                comment: "创建人ID");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "RagDocuments",
                type: "uuid",
                nullable: true,
                comment: "删除人ID");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "RagDocuments",
                type: "uuid",
                nullable: true,
                comment: "最后修改人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "RagDocuments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后修改时间");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "RagChunks",
                type: "uuid",
                nullable: true,
                comment: "创建人ID");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "RagChunks",
                type: "uuid",
                nullable: true,
                comment: "删除人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "RagChunks",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RagChunks",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "RagChunks",
                type: "uuid",
                nullable: true,
                comment: "最后修改人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "RagChunks",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后修改时间");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "RagChunkingSettings",
                type: "uuid",
                nullable: true,
                comment: "创建人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "RagChunkingSettings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "RagChunkingSettings",
                type: "uuid",
                nullable: true,
                comment: "删除人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "RagChunkingSettings",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RagChunkingSettings",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "RagChunkingSettings",
                type: "uuid",
                nullable: true,
                comment: "最后修改人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "RagChunkingSettings",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后修改时间");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "PushNotificationSubscriptions",
                type: "uuid",
                nullable: true,
                comment: "创建人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "PushNotificationSubscriptions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "PushNotificationSubscriptions",
                type: "uuid",
                nullable: true,
                comment: "删除人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "PushNotificationSubscriptions",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PushNotificationSubscriptions",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "PushNotificationSubscriptions",
                type: "uuid",
                nullable: true,
                comment: "最后修改人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "PushNotificationSubscriptions",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后修改时间");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Products",
                type: "uuid",
                nullable: true,
                comment: "创建人ID");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Products",
                type: "uuid",
                nullable: true,
                comment: "删除人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Products",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Products",
                type: "uuid",
                nullable: true,
                comment: "最后修改人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Products",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后修改时间");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "ProductReviews",
                type: "timestamp with time zone",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "评价时间");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "ProductReviews",
                type: "uuid",
                nullable: true,
                comment: "创建人ID");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "ProductReviews",
                type: "uuid",
                nullable: true,
                comment: "删除人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "ProductReviews",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProductReviews",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "ProductReviews",
                type: "uuid",
                nullable: true,
                comment: "最后修改人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "ProductReviews",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后修改时间");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "ProductImages",
                type: "uuid",
                nullable: true,
                comment: "创建人ID");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "ProductImages",
                type: "uuid",
                nullable: true,
                comment: "删除人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "ProductImages",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProductImages",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "ProductImages",
                type: "uuid",
                nullable: true,
                comment: "最后修改人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "ProductImages",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后修改时间");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Payments",
                type: "timestamp with time zone",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "记录创建时间");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Payments",
                type: "uuid",
                nullable: true,
                comment: "创建人ID");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Payments",
                type: "uuid",
                nullable: true,
                comment: "删除人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Payments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Payments",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Payments",
                type: "uuid",
                nullable: true,
                comment: "最后修改人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Payments",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后修改时间");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Orders",
                type: "boolean",
                nullable: false,
                comment: "是否删除标识",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "逻辑删除标识");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Orders",
                type: "uuid",
                nullable: true,
                comment: "创建人ID");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Orders",
                type: "uuid",
                nullable: true,
                comment: "删除人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Orders",
                type: "uuid",
                nullable: true,
                comment: "最后修改人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后修改时间");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "OrderItems",
                type: "uuid",
                nullable: true,
                comment: "创建人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "OrderItems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "OrderItems",
                type: "uuid",
                nullable: true,
                comment: "删除人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "OrderItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "OrderItems",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "OrderItems",
                type: "uuid",
                nullable: true,
                comment: "最后修改人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "OrderItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后修改时间");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Inventories",
                type: "uuid",
                nullable: true,
                comment: "创建人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Inventories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Inventories",
                type: "uuid",
                nullable: true,
                comment: "删除人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Inventories",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Inventories",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Inventories",
                type: "uuid",
                nullable: true,
                comment: "最后修改人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Inventories",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后修改时间");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Categories",
                type: "uuid",
                nullable: true,
                comment: "创建人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Categories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Categories",
                type: "uuid",
                nullable: true,
                comment: "删除人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Categories",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Categories",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Categories",
                type: "uuid",
                nullable: true,
                comment: "最后修改人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Categories",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后修改时间");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "CartItems",
                type: "uuid",
                nullable: true,
                comment: "创建人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "CartItems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "创建时间");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "CartItems",
                type: "uuid",
                nullable: true,
                comment: "删除人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "CartItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CartItems",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "CartItems",
                type: "uuid",
                nullable: true,
                comment: "最后修改人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "CartItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后修改时间");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Attachments",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Attachments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Attachments",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Attachments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Attachments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Attachments",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Attachments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "AreaCodes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "AreaCodes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "AreaCodes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "AreaCodes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AreaCodes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "AreaCodes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "AreaCodes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Addresses",
                type: "uuid",
                nullable: true,
                comment: "创建人ID");

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "Addresses",
                type: "uuid",
                nullable: true,
                comment: "删除人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedOn",
                table: "Addresses",
                type: "timestamp with time zone",
                nullable: true,
                comment: "删除时间");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Addresses",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                comment: "是否删除");

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Addresses",
                type: "uuid",
                nullable: true,
                comment: "最后修改人ID");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOn",
                table: "Addresses",
                type: "timestamp with time zone",
                nullable: true,
                comment: "最后修改时间");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "WebAuthnCredential");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "WebAuthnCredential");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "WebAuthnCredential");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "WebAuthnCredential");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "WebAuthnCredential");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "WebAuthnCredential");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "WebAuthnCredential");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "UserTokens");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "UserTokens");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "UserTokens");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "UserTokens");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserTokens");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "UserTokens");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "UserTokens");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "UserSessions");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "UserSessions");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "UserSessions");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "UserSessions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserSessions");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "UserSessions");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "UserSessions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "UserLogins");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "UserLogins");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "UserLogins");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "UserLogins");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserLogins");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "UserLogins");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "UserLogins");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "UserClaims");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "UserClaims");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "UserClaims");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "UserClaims");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserClaims");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "UserClaims");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "UserClaims");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SystemPrompts");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "SystemPrompts");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "SystemPrompts");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "SystemPrompts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SystemPrompts");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "SystemPrompts");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "SystemPrompts");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "RagKnowledgeBases");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "RagKnowledgeBases");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "RagKnowledgeBases");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "RagKnowledgeBases");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "RagDocuments");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "RagDocuments");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "RagDocuments");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "RagDocuments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "RagChunks");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "RagChunks");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "RagChunks");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RagChunks");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "RagChunks");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "RagChunks");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "RagChunkingSettings");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "RagChunkingSettings");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "RagChunkingSettings");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "RagChunkingSettings");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RagChunkingSettings");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "RagChunkingSettings");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "RagChunkingSettings");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PushNotificationSubscriptions");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "PushNotificationSubscriptions");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "PushNotificationSubscriptions");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "PushNotificationSubscriptions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PushNotificationSubscriptions");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "PushNotificationSubscriptions");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "PushNotificationSubscriptions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "ProductReviews");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AreaCodes");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "AreaCodes");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "AreaCodes");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "AreaCodes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AreaCodes");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "AreaCodes");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "AreaCodes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Addresses");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RegDate",
                table: "WebAuthnCredential",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "注册时间");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "TodoItems",
                type: "timestamp with time zone",
                nullable: true,
                comment: "更新时间");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "RagKnowledgeBases",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "更新时间");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "RagDocuments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "更新时间");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "RagChunks",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "更新时间");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "RagChunkingSettings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "更新时间");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "ProductReviews",
                type: "timestamp with time zone",
                nullable: false,
                comment: "评价时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Payments",
                type: "timestamp with time zone",
                nullable: false,
                comment: "记录创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Orders",
                type: "boolean",
                nullable: false,
                comment: "逻辑删除标识",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldComment: "是否删除标识");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedOn",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "更新时间");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedOn",
                table: "Inventories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "最后更新时间");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "AddedOn",
                table: "CartItems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "加入购物车时间");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedOn",
                table: "CartItems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "更新时间");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedOn",
                table: "Addresses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                comment: "更新时间");
        }
    }
}
