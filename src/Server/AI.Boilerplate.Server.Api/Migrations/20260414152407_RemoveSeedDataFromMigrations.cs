using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AI.Boilerplate.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSeedDataFromMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("10e9f8e7-b6a5-d4c3-b2a1-d0e9f8e7b6a5"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("21d0e9f8-e7b6-a5d4-c3b2-a1d0e9f8e7b6"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("32a1d0e9-f8e7-b6a5-d4c3-b2a1d0e9f8e7"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("43b2a1d0-e9f8-e7b6-a5d4-c3b2a1d0e9f8"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("512eb70b-1d39-4845-88c0-fe19cd2d1979"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("54c3b2a1-d0e9-f8e7-b6a5-d4c3b2a1d0e9"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("5746ae3d-5116-4774-9d55-0ff496e5186f"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("65d4c3b2-a1d0-e9f8-e7b6-a5d4c3b2a1d0"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("76a5d4c3-b2a1-d0e9-f8e7-b6a5d4c3b2a1"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("87b6a5d4-c3b2-a1d0-e9f8-e7b6a5d4c3b2"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("98e7b6a5-d4c3-b2a1-d0e9-f8e7b6a5d4c3"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("9a59dda2-7b12-4cc1-9658-d2586eef91d7"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-1234-567890abcdef"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-a7b8-9012-3456c7d8e9f0"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a3b4c5d6-e7f8-a9b0-1234-5678c9d0e1f2"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a5b6c7d8-e9f0-a1b2-3456-7890c1d2e3f4"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a7b8c9d0-e1f2-a3b4-5678-9012c3d4e5f6"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a9b0c1d2-e3f4-a5b6-7890-1234c5d6e7f8"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a9f8e7b6-a5d4-c3b2-a1d0-e9f8e7b6a5d4"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b0a9f8e7-b6a5-d4c3-b2a1-d0e9f8e7b6a5"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b0c1d2e3-f4a5-b6c7-8901-2345d6e7f8a9"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a7-89b0-12c3-45d678e9f0a1"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b4c5d6e7-f8a9-b0c1-2345-6789d0e1f2a3"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b6c7d8e9-f0a1-b2c3-4567-8901d2e3f4a5"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b8c9d0e1-f2a3-b4c5-6789-0123d4e5f6a7"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c1b0a9f8-e7b6-a5d4-c3b2-a1d0e9f8e7b6"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-c7d8-9012-3456e7f8a9b0"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c3d4e5f6-a7b8-9c01-23d4-56e789f0a1b2"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c5d6e7f8-a9b0-c1d2-3456-7890e1f2a3b4"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c7d8e9f0-a1b2-c3d4-5678-9012e3f4a5b6"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c9d0e1f2-a3b4-c5d6-7890-1234e5f6a7b8"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d0e1f2a3-b4c5-d6e7-8901-2345f6a7b8c9"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d2a1b0c9-f8e7-b6a5-d4c3-b2a1d0e9f8e7"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d2e3f4a5-b6c7-d8e9-0123-4567f8a9b0c1"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d4e5f6a7-b8c9-d0e1-f234-567890a1b2c3"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d6e7f8a9-b0c1-d2e3-4567-8901f2a3b4c5"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d8e9f0a1-b2c3-d4e5-6789-0123f4a5b6c7"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e1f2a3b4-c5d6-e7f8-9012-3456a7b8c9d0"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e3b2a1d0-c9f8-e7b6-a5d4-c3b2a1d0e9f8"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e3f4a5b6-c7d8-e9f0-1234-5678a9b0c1d2"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e5f6a7b8-c9d0-e1f2-3456-7890a1b2c3d4"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e7f8a9b0-c1d2-e3f4-5678-9012a3b4c5d6"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e9f0a1b2-c3d4-e5f6-7890-1234a5b6c7d8"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f0a1b2c3-d4e5-f6a7-8901-2345b6c7d8e9"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f2a3b4c5-d6e7-f8a9-0123-4567b8c9d0e1"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f4a3b2c1-d0e9-f8a7-b6c5-d4e3f2a1b0c9"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f4a5b6c7-d8e9-f0a1-2345-6789b0c1d2e3"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f6a7b8c9-d0e1-f2a3-4567-8901b2c3d4e5"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f8a9b0c1-d2e3-f4a5-6789-0123b4c5d6e7"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f9f8e7b6-a5d4-c3b2-a1d0-e9f8e7b6a5d4"));

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "SystemPrompts",
                keyColumn: "Id",
                keyValue: new Guid("a8c94d94-0004-4dd0-921c-255e0a581424"));

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("8ff71671-a1d6-5f97-abb9-d87d7b47d6e7"), new Guid("8ff71671-a1d6-4f97-abb9-d87d7b47d6e7") });

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("31d78bd0-0b4f-4e87-b02f-8f66d4ab2845"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("582b8c19-0709-4dae-b7a6-fa0e704dad3c"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("747f6d66-7524-40ca-8494-f65e85b5ee5d"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("ecf0496f-f1e3-4d92-8fe4-0d7fa2b4ffa4"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("8ff71671-a1d6-5f97-abb9-d87d7b47d6e7"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("9ff71672-a1d5-4f97-abb7-d87d6b47d5e8"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8ff71671-a1d6-4f97-abb9-d87d7b47d6e7"));

            migrationBuilder.AlterColumn<int>(
                name: "ShortId",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValueSql: "nextval('\"产品短编号\"')",
                comment: "产品短编号",
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValueSql: "nextval('\"产品短编号\"')",
                oldComment: "短编号");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ShortId",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValueSql: "nextval('\"产品短编号\"')",
                comment: "短编号",
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValueSql: "nextval('\"产品短编号\"')",
                oldComment: "产品短编号");

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
                values: new object[] { new Guid("a8c94d94-0004-4dd0-921c-255e0a581424"), "你是 AI.Boilerplate 应用程序的助手。在下面，你将找到一个包含该应用信息的 markdown 文档，以及随后的用户查询。\r\n\r\n# AI.Boilerplate 应用程序 - 功能和使用指南\r\n\r\n**[[[GENERAL_INFORMATION_BEGIN]]]**\r\n\r\n*   **平台：** 该应用程序可在 Android、iOS、Windows、macOS 以及作为 Web (PWA) 应用程序使用。\r\n\r\n* 网站地址: [Website address](https://github.com/l18211338136-hub/AI.Boilerplate)\r\n* Google Play: [Google Play Link](https://github.com/l18211338136-hub/AI.Boilerplate)\r\n* Apple Store: [Apple Store Link](https://github.com/l18211338136-hub/AI.Boilerplate)\r\n* Windows EXE 安装程序: [Windows app link](https://github.com/l18211338136-hub/AI.Boilerplate)\r\n\r\n## 1. 账户管理与认证\r\n\r\n这些功能涵盖用户注册、登录、账户恢复和安全设置。\r\n\r\n### 1.1. 注册 (Sign Up)\r\n*   **描述：** 允许新用户创建账户。用户可以使用电子邮件地址、电话号码或通过外部身份提供商注册。\r\n*   **如何使用：**\r\n    - 导航至 [注册页面](/sign-up)。\r\n\r\n### 1.2. 登录 (Sign In)\r\n*   **描述：** 允许现有用户使用各种方法登录其账户。\r\n*   **如何使用：**\r\n    - 导航至 [登录页面](/sign-in)。\r\n\r\n### 1.3. 确认账户 (Confirm Account)\r\n*   **描述：** 在注册后验证用户的电子邮件地址或电话号码，通常通过输入发送给他们的验证码来完成。\r\n*   **如何使用：**\r\n    - 导航至 [确认页面](/confirm)（通常在注册后自动重定向）。\r\n\r\n### 1.4. 忘记密码 (Forgot Password)\r\n*   **描述：** 通过向用户注册的电子邮件或电话号码发送重置令牌（代码）来启动密码重置过程。\r\n*   **如何使用：**\r\n    - 导航至 [忘记密码页面](/forgot-password)，通常从登录页面链接。\r\n\r\n### 1.5. 重置密码 (Reset Password)\r\n*   **描述：** 允许用户在通过“忘记密码”流程请求重置令牌后设置新密码。\r\n*   **如何使用：**\r\n    - 导航至 [重置密码页面](/reset-password)。\r\n\r\n## 2. 用户设置 (User Settings)\r\n\r\n登录后可访问，这些页面允许用户管理他们的个人资料、账户详细信息、安全设置和活动会话。\r\n\r\n### 2.1. 个人资料设置 (Profile Settings)\r\n*   **描述：** 管理个人用户信息，如姓名、头像、出生日期和性别。\r\n*   **如何使用：**\r\n    - 导航至 [个人资料页面](/settings/profile)。\r\n    - 需要登录。\r\n\r\n### 2.2. 账户设置 (Account Settings)\r\n*   **描述：** 管理特定于账户的详细信息，如电子邮件、电话号码、启用无密码登录和账户删除。\r\n*   **如何使用：**\r\n    - 导航至 [账户页面](/settings/account)。\r\n    - 需要登录。\r\n\r\n### 2.3. 双因素认证 (2FA)\r\n*   **描述：** 通过在登录期间要求第二种验证形式（通常是来自认证器应用程序的代码）来增强账户安全性。\r\n*   **如何使用：**\r\n    - 导航至 [双因素认证页面](/settings/tfa)。\r\n    - 需要登录。\r\n\r\n### 2.4. 会话管理 (Session Management)\r\n*   **描述：** 查看用户当前登录的所有设备和浏览器，并提供远程注销（撤销）特定会话的功能。\r\n*   **如何使用：**\r\n    - 导航至 [会话页面](/settings/sessions)。\r\n    - 需要登录。\r\n\r\n## 3. 核心应用程序功能 (Core Application Features)\r\n\r\n这些是除了账户管理之外的应用程序主要功能区域。\r\n### 3.1. 仪表板 (Dashboard)\r\n*   **描述：** 提供关键应用程序数据（如类别和产品）的高级概述和分析。\r\n*   **如何使用：**\r\n    - 导航至 [仪表板页面](/dashboard)。\r\n    - 需要登录。\r\n\r\n### 3.2. 类别管理 (Categories Management)\r\n*   **描述：** 允许用户查看、创建、编辑和删除类别，通常用于组织产品。\r\n*   **如何使用：**\r\n    - 导航至 [类别页面](/categories)。\r\n    - 需要登录。\r\n\r\n### 3.3. 产品管理 (Products Management)\r\n*   **描述：** 允许用户查看、创建、编辑和删除产品。\r\n*   **如何使用：**\r\n    - 导航至 [产品页面](/products)。\r\n    - 需要登录。\r\n\r\n### 3.4. 添加/编辑产品 (Add/Edit Product)\r\n*   **描述：** 用于创建新产品或修改现有产品的表单页面。\r\n*   **如何使用：**\r\n    - 导航至 [添加/编辑产品页面](/add-edit-product)。\r\n    - 需要登录。\r\n### 3.6. 待办事项列表 (Todo List)\r\n*   **描述：** 一个简单的任务管理功能，用于跟踪个人任务。\r\n*   **如何使用：**\r\n    - 导航至 [待办事项页面](/todo)。\r\n    - 需要登录。\r\n### 3.7. 升级账户 (Upgrade account)\r\n*   **描述：** 用户可以升级其账户的页面。\r\n*   **如何使用：**\r\n    - 导航至 [升级账户页面](/settings/upgradeaccount)。\r\n    - 需要登录。\r\n\r\n### 3.8. RAG 知识库管理 (RAG Management)\r\n*   **描述：** 管理 RAG 知识库、文档、切片与召回调试，可查看向量分、关键词分与综合分，支持知识库来源类型（数据库/文件/链接）。\r\n*   **如何使用：**\r\n    - 导航至 [RAG 管理页面](/rag-management)。\r\n    - 需要登录，并具备管理权限。\r\n\r\n## 4. 信息页面 (Informational Pages)\r\n\r\n### 4.1. 关于页面 (About Page)\r\n*   **描述：** 提供有关应用程序本身的信息。\r\n*   **如何使用：**\r\n    - 导航至 [关于页面](/about)。\r\n\r\n### 4.2. 条款页面 (Terms Page)\r\n*   **描述：** 显示法律条款和条件，包括最终用户许可协议 (EULA) 和可能的隐私政策。\r\n*   **如何使用：**\r\n    - 导航至 [条款页面](/terms)。\r\n\r\n---\r\n\r\n**[[[GENERAL_INFORMATION_END]]]**\r\n\r\n**[[[INSTRUCTIONS_BEGIN]]]**\r\n\r\n- ### 认证工具：\r\n    - 访问需要登录的页面需要 {{IsAuthenticated}} 为 `true`。\r\n    - 如果需要提示用户进行认证，可以使用 `ShowSignInModal` 工具。此工具将显示登录模态框，如果成功则返回用户信息，如果取消/失败则返回 null。\r\n    - 在用户登录后，你**必须**向用户问好。\r\n    - 当 {{IsAuthenticated}} 为 `true` 时，不要再次询问用户是否已登录，直接执行其明确的操作请求。\r\n    - **强制规则：** 每次用户明确要求打开特定页面（如“打开产品页面”、“去仪表板”）时，无论上下文如何，都必须调用 `NavigateToPage` 工具。\r\n\r\n- ### 自然语义数据操作规则：\r\n    - 业务查询与报表优先调用 `PgTextToSqlReport`。\r\n    - **【最优先级】** 如果用户的需求中包含“可视化”、“报表界面”、“仪表盘”、“图表”、“生成报表页面”、“数据大屏”等类似字眼，**绝对必须直接、且仅调用 `PgGenerateDashboard` 工具一次**。不允许询问用户确认，不允许生成确认按钮，更不允许分开调用 `PgTextToSqlReport` 和 `PgGenerateDashboard`！你只需要直接调用 `PgGenerateDashboard`。\r\n     - 注意：如果用户明确要求“可视化”、“图表”、“图”、“界面”或“生成报表界面”等需求，【绝对不要】调用 `PgTextToSqlReport` 工具，必须直接且仅调用 `PgGenerateDashboard` 工具！\r\n    - 业务新增/修改/删除优先调用 `PgTextToSqlWrite`，若用户意图明确则直接执行，不需要二次确认。\r\n    - 不要向用户索要表名、字段名、SQL、主键等技术细节，仅在业务语义不清时做业务澄清。\r\n    - 回复中不要提及工具名称、函数名或内部执行细节。\r\n    - 回复写入结果时，必须严格基于工具返回数据，不得编造或改写字段值。\r\n    - 如果工具返回 `resultMessage`，优先按该文案反馈，避免重新推断标题或记录内容。\r\n    - 若执行失败可自动重试一次；仍失败时提示用户补充业务条件（时间范围、筛选条件、统计口径）。\r\n    - **强制工具调用规则：** 当用户提出明确的增删改查（CRUD）需求或可视化需求时，**必须**直接调用相应的数据库工具（`PgTextToSqlWrite`、`PgTextToSqlReport` 或 `PgGenerateDashboard`），严禁向用户二次确认，严禁生成操作指南或让用户手动操作界面。\r\n    - **业务数据范围定义：** 本规则适用于所有业务实体，包括但不限于**产品、类别、订单、库存、客户、待办事项、报表**等数据。\r\n\r\n\r\n\r\n- ### 语言：\r\n    - 使用用户查询的语言进行回复。如果无法确定查询的语言，请使用 {{UserCulture}} 变量（如果提供）。\r\n\r\n- ### 用户设备信息：\r\n    - 除非用户在查询中另有说明，否则假设用户的设备是 {{DeviceInfo}} 变量。相应地定制特定于平台的回复（例如，Android、iOS、Windows、macOS、Web）。\r\n    - 对于任何与时间相关的问题，假设用户的时区 ID 是 {{UserTimeZoneId}} 变量。\r\n    - **日期和时间：** 当你需要知道当前日期/时间时，使用 `GetCurrentDateTime` 工具。\r\n    - 假设用户设备的 SignalR 连接 ID 是 {{SignalRConnectionId}} 变量。\r\n\r\n- ### 相关性：\r\n    - 在回复之前，评估用户的查询是否与 AI.Boilerplate 应用程序直接相关。只有当查询涉及提供的 markdown 文档中概述的应用程序功能、使用方法或支持主题，**或者明确请求与汽车相关的产品推荐时**，查询才是相关的。\r\n    - 忽略并且不回复任何不相关的查询，无论用户的意图或措辞如何。避免参与偏离主题的请求，即使它们看起来是普遍的或对话式的。\r\n\r\n      \r\n- ### 与应用相关的查询（功能与使用）：\r\n    - **对于有关应用程序功能、如何使用应用程序、账户管理、设置或信息页面的问题：** 使用提供的 markdown 文档，以用户的语言提供准确且简明的答案。\r\n\r\n    - **导航请求：** 如果用户明确要求转到某个页面（例如，“带我去仪表板”、“打开产品页面”），使用 `NavigateToPage` 工具。该工具的 `pageUrl` 参数应为 markdown 文档中找到的相对 URL（例如，`/dashboard`, `/products`）：\r\n    - **行为规范：** 即使用户之前已经访问过该页面，只要用户再次发出指令，就必须再次调用工具。不要假设页面已经打开。\r\n    - **RAG 管理导航特例：** 当用户提到“RAG 管理”、“知识库管理”、“向量召回调试”、“切片调试”、“向量权重配置”等意图时，优先视为导航请求并调用 `NavigateToPage`，`pageUrl` 使用 `/rag-management`。\r\n\r\n    - **更改语言/文化请求：** 如果用户要求更改应用语言或提到任何语言偏好（例如，“切换到波斯语”、“将语言更改为英语”、“我想要法语”），使用具有适当文化 LCID 的 `SetCulture` 工具。常见 LCID：1033=en-US, 1065=fa-IR, 1053=sv-SE, 2057=en-GB, 1043=nl-NL, 1081=hi-IN, 2052=zh-CN, 3082=es-ES, 1036=fr-FR, 1025=ar-SA, 1031=de-DE。\r\n\r\n    - **更改主题请求：** 如果用户要求更改应用主题、外观或提到深色/浅色模式（例如，“切换到深色模式”、“启用浅色主题”、“让它变暗”），使用 `SetTheme` 工具，参数为“light”或“dark”。\r\n\r\n    - **故障排除与错误检测：** 当用户报告问题、错误、崩溃或某些功能无法正常工作时（例如，“应用崩溃了”、“我遇到了一个错误”、“出了点问题”、“它不工作了”），**始终**首先使用 `CheckLastError` 工具从用户设备检索诊断信息。\r\n        \r\n        检索到错误信息后：\r\n        1. 以同理心确认问题（例如，“我看到你在...方面遇到了麻烦”、“我理解这很令人沮丧”）\r\n        2. 提供实用、易于遵循的步骤来解决问题\r\n        3. 仅在用户明确要求更多信息时才提供技术细节\r\n\r\n        **重要提示：** 不要将 `CheckLastError` 工具用于关于功能或“如何操作”的常见问题。仅在排查实际报告的问题或错误时使用它。\r\n        \r\n        **高级故障排除 - 清除应用文件：**\r\n        - 如果基本故障排除步骤未能解决问题，且问题似乎与损坏的应用数据、缓存文件或持久状态问题有关，你可以**建议**使用 `ClearAppFiles` 工具作为潜在的解决方案。\r\n        - **重要提示：** 在提供此工具之前，你**必须**向用户解释它的作用（清除本地应用数据、缓存和文件）。\r\n        - **`ClearAppFiles` 工具会处理所有必要的缓存清除工作。** 不要建议手动清除浏览器缓存或其他手动清除缓存的步骤；该工具就足够了。\r\n        - **只有在收到用户明确批准/确认后才能调用 `ClearAppFiles` 工具。** 不要未经许可自动调用它。\r\n        - 成功调用该工具后，告知用户：“我已经清除了应用程序的本地文件。应用程序将很快重新加载。请尝试重新登录，如果问题仍然存在，请告诉我。”\r\n\r\n    - 当提及特定的应用页面时，包含 markdown 文档中的相对 URL，并使用 markdown 格式（例如，[注册页面](/sign-up)），并询问他们是否需要你为他们打开该页面。\r\n\r\n    - 在整个回复过程中保持乐于助人和专业的语气。\r\n\r\n    - 如果用户提出多个问题，将它们列出给用户以确认理解，然后使用清晰的标题分别处理每一个。如果需要，请他们优先考虑：“我看到您有多个问题。您希望我先解决哪个问题？”\r\n    \r\n    - 永远不要请求敏感信息（例如，密码、PIN 码）。如果用户主动分享此类数据，请回复：“为了您的安全，请不要分享密码等敏感信息。请放心，您的数据在我们这里是安全的。” ### 处理广告故障请求：\r\n**[[[ADS_TROUBLE_RULES_BEGIN]]]**\r\n*   **如果用户询问在观看广告时遇到问题（例如，“广告未显示”、“广告被拦截”、“未进行升级”）:**\r\n    1.  *充当技术支持。*\r\n    2.  **根据用户的设备信息提供逐步指导来解决问题，重点关注广告拦截器和浏览器防跟踪功能。**\r\n\r\n**[[[ADS_TROUBLE_RULES_END]]]**\r\n\r\n- ### 用户反馈和建议：\r\n    - 如果用户提供反馈或建议功能，请回复：“感谢您的反馈！这对我们很有价值，我会将其转达给产品团队。”如果反馈不清晰，请要求澄清：“您能提供有关您建议的更多细节吗？”\r\n\r\n- ### 处理沮丧或困惑：\r\n    - 如果用户似乎感到沮丧或困惑，使用安抚的语言并主动提出澄清：“很抱歉这让您感到困惑。我在这里提供帮助！需要我再解释一遍吗？”\r\n\r\n- ### 未解决的问题：\r\n    - 如果你无法解决用户的问题（无论是通过 markdown 信息还是工具），请回复：“很抱歉我无法解决您的问题 / 完全满足您的请求。我理解这一定让您感到非常沮丧。”\r\n    - 如果用户的电子邮件（{{UserEmail}} 变量）为空，请求提供他们的电子邮件。\r\n    - 调用 `SaveUserEmailAndConversationHistory` 工具。\r\n    - 确认：“感谢您提供电子邮件。很快会有客服人员跟进。”然后问：“您还有其他需要我协助的问题吗？”\r\n\r\n**[[[INSTRUCTIONS_END]]]**\r\n", 0 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "BirthDate", "ConcurrencyStamp", "ElevatedAccessTokenRequestedOn", "Email", "EmailConfirmed", "EmailTokenRequestedOn", "FullName", "Gender", "HasProfilePicture", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "OtpRequestedOn", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PhoneNumberTokenRequestedOn", "ResetPasswordTokenRequestedOn", "SecurityStamp", "TwoFactorEnabled", "TwoFactorTokenRequestedOn", "UserName" },
                values: new object[] { new Guid("8ff71671-a1d6-4f97-abb9-d87d7b47d6e7"), 0, new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "315e1a26-5b3a-4544-8e91-2760cd28e231", null, "761516331@qq.com", true, new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "AI.Boilerplate test account", 0, false, true, null, "761516331@QQ.COM", "TEST", null, "AQAAAAIAAYagAAAAEP0v3wxkdWtMkHA3Pp5/JfS+42/Qto9G05p2mta6dncSK37hPxEHa3PGE4aqN30Aag==", "+31684207362", true, null, null, "959ff4a9-4b07-4cc1-8141-c5fc033daf83", false, null, "test" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedOn", "DescriptionHTML", "DescriptionText", "Embedding", "HasPrimaryImage", "Name", "Price", "PrimaryImageAltText", "ShortId" },
                values: new object[,]
                {
                    { new Guid("10e9f8e7-b6a5-d4c3-b2a1-d0e9f8e7b6a5"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>提升驾驶性能。</h3><p>生于赛道，AMG GT Coupe 体现了纯粹跑车的本质，拥有惊人的动力和精准的操控。</p>", "提升驾驶性能。\n生于赛道，AMG GT Coupe 体现了纯粹跑车的本质，拥有惊人的动力和精准的操控。\n", null, false, "Mercedes-AMG GT Coupe", 150000m, null, 10018 },
                    { new Guid("21d0e9f8-e7b6-a5d4-c3b2-a1d0e9f8e7b6"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>运动优雅，重新定义。</h3><p>全新 CLE Coupe 将富有表现力的设计与动态操控和先进技术相结合，创造出现代的欲望图标。</p>", "运动优雅，重新定义。\n全新 CLE Coupe 将富有表现力的设计与动态操控和先进技术相结合，创造出现代的欲望图标。\n", null, false, "CLE Coupe", 65000m, null, 10017 },
                    { new Guid("32a1d0e9-f8e7-b6a5-d4c3-b2a1d0e9f8e7"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>从每个角度都无法抗拒。</h3><p>凭借标志性的倾斜车顶线和运动姿态，CLA Coupe 以富有表现力的设计和灵活的性能吸引着人们的目光。</p>", "从每个角度都无法抗拒。\n凭借标志性的倾斜车顶线和运动姿态，CLA Coupe 以富有表现力的设计和灵活的性能吸引着人们的目光。\n", null, false, "CLA COUPE", 50500m, null, 10016 },
                    { new Guid("43b2a1d0-e9f8-e7b6-a5d4-c3b2a1d0e9f8"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>汽车欲望的巅峰。</h3><p>无懈可击的工程技术，S-Class Sedan 在安全、舒适和驾驶体验方面开创了创新，定义了奢华旅行。</p>", "汽车欲望的巅峰。\n无懈可击的工程技术，S-Class Sedan 在安全、舒适和驾驶体验方面开创了创新，定义了奢华旅行。\n", null, false, "S-Class Sedan", 140000m, null, 10015 },
                    { new Guid("512eb70b-1d39-4845-88c0-fe19cd2d1979"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>先进、适应性强、爱冒险。</h3><p>不仅仅是新，它可以在未来更新其先进性。不仅仅是贴心，它可以预测需求和愿望。超越未来，它可以更好地利用您的时间，并使您使用它的时间更美好。</p>", "先进、适应性强、爱冒险。\n不仅仅是新，它可以在未来更新其先进性。不仅仅是贴心，它可以预测需求和愿望。超越未来，它可以更好地利用您的时间，并使您使用它的时间更美好。\n", null, false, "EQE SUV", 79050m, null, 10003 },
                    { new Guid("54c3b2a1-d0e9-f8e7-b6a5-d4c3b2a1d0e9"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>品牌的核心，智能精炼。</h3><p>作为智能的杰作，E-Class Sedan 无缝融合了动感设计、奢华舒适和开创性的驾驶辅助系统。</p>", "品牌的核心，智能精炼。\n作为智能的杰作，E-Class Sedan 无缝融合了动感设计、奢华舒适和开创性的驾驶辅助系统。\n", null, false, "E-Class Sedan", 73900m, null, 10014 },
                    { new Guid("5746ae3d-5116-4774-9d55-0ff496e5186f"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>电动、必需、典范</h3><p>它是未来主义、前卫和新鲜的，但您知道它的核心价值观。不断完善的奢华。不断进步的创新。以及对您福祉的永恒奉献。也许没有哪款电动轿车能如此新颖而自然。</p>", "电动、必需、典范\n它是未来主义、前卫和新鲜的，但您知道它的核心价值观。不断完善的奢华。不断进步的创新。以及对您福祉的永恒奉献。也许没有哪款电动轿车能如此新颖而自然。\n", null, false, "EQE Sedan", 76050m, null, 10001 },
                    { new Guid("65d4c3b2-a1d0-e9f8-e7b6-a5d4c3b2a1d0"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>一个被重新工程的图标。</h3><p>一眼就能认出，永远充满能力。G-Class 仍然是定义奢华越野车的标杆，将永恒的设计与现代科技和强悍的性能相结合。</p>", "一个被重新工程的图标。\n一眼就能认出，永远充满能力。G-Class 仍然是定义奢华越野车的标杆，将永恒的设计与现代科技和强悍的性能相结合。\n", null, false, "G-CLASS SUV", 180000m, null, 10013 },
                    { new Guid("76a5d4c3-b2a1-d0e9-f8e7-b6a5d4c3b2a1"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>豪华 SUV 的 S 级。</h3><p>为多达七名乘客提供头等舱般的旅行体验，GLS 将强大的气场与无与伦比的奢华、空间和科技相结合。</p>", "豪华 SUV 的 S 级。\n为多达七名乘客提供头等舱般的旅行体验，GLS 将强大的气场与无与伦比的奢华、空间和科技相结合。\n", null, false, "GLS SUV", 115100m, null, 10012 },
                    { new Guid("87b6a5d4-c3b2-a1d0-e9f8-e7b6a5d4c3b2"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>运动气息，强劲性能。</h3><p>GLE Coupe 将 SUV 的肌肉感与轿跑的优雅线条相结合，带来令人振奋的性能和不容忽视的风格。</p>", "运动气息，强劲性能。\nGLE Coupe 将 SUV 的肌肉感与轿跑的优雅线条相结合，带来令人振奋的性能和不容忽视的风格。\n", null, false, "GLE Coupe", 94900m, null, 10011 },
                    { new Guid("98e7b6a5-d4c3-b2a1-d0e9-f8e7b6a5d4c3"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>每一种地形的主宰。</h3><p>智能、宽敞且强大的 GLE SUV 为家庭和冒险者提供尖端技术和奢华舒适。</p>", "每一种地形的主宰。\n智能、宽敞且强大的 GLE SUV 为家庭和冒险者提供尖端技术和奢华舒适。\n", null, false, "GLE SUV", 82800m, null, 10010 },
                    { new Guid("9a59dda2-7b12-4cc1-9658-d2586eef91d7"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>探索的范围。可容纳多达 7 人。</h3><p>这是一款多功能 SUV，可容纳多达七人。是一款您可以每天享受的先进电动车。智能技术和贴心的奢华以快速响应和静音精致呈现。</p>", "探索的范围。可容纳多达 7 人。\n这是一款多功能 SUV，可容纳多达七人。是一款您可以每天享受的先进电动车。智能技术和贴心的奢华以快速响应和静音精致呈现。\n", null, false, "EQB SUV", 54200m, null, 10000 },
                    { new Guid("a1b2c3d4-e5f6-7890-1234-567890abcdef"), new Guid("31d78bd0-0b4f-4e87-b02f-8f66d4ab2845"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>福特铸造的坚韧。</h3><p>美国最畅销的卡车，以其工作或娱乐的能力、创新和坚韧而闻名。</p>", "福特铸造的坚韧。\n美国最畅销的卡车，以其工作或娱乐的能力、创新和坚韧而闻名。\n", null, false, "Ford F-150", 45000m, null, 10020 },
                    { new Guid("a1b2c3d4-e5f6-a7b8-9012-3456c7d8e9f0"), new Guid("747f6d66-7524-40ca-8494-f65e85b5ee5d"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>激进的电动皮卡。</h3><p>一款具有外骨骼设计的未来派皮卡，提供多功能性、性能和耐用性。</p>", "激进的电动皮卡。\n一款具有外骨骼设计的未来派皮卡，提供多功能性、性能和耐用性。\n", null, false, "特斯拉 Cybertruck", 70000m, null, 10050 },
                    { new Guid("a3b4c5d6-e7f8-a9b0-1234-5678c9d0e1f2"), new Guid("582b8c19-0709-4dae-b7a6-fa0e704dad3c"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>合适尺寸的皮卡。</h3><p>一款坚固耐用的中型卡车，旨在满足工作需求和周末冒险。</p>", "合适尺寸的皮卡。\n一款坚固耐用的中型卡车，旨在满足工作需求和周末冒险。\n", null, false, "日产 Frontier", 35000m, null, 10032 },
                    { new Guid("a5b6c7d8-e9f0-a1b2-3456-7890c1d2e3f4"), new Guid("747f6d66-7524-40ca-8494-f65e85b5ee5d"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>大众市场电动轿车。</h3><p>特斯拉最实惠的车型，提供令人印象深刻的续航里程、性能和极简设计。</p>", "大众市场电动轿车。\n特斯拉最实惠的车型，提供令人印象深刻的续航里程、性能和极简设计。\n", null, false, "特斯拉 Model 3", 45000m, null, 10044 },
                    { new Guid("a7b8c9d0-e1f2-a3b4-5678-9012c3d4e5f6"), new Guid("31d78bd0-0b4f-4e87-b02f-8f66d4ab2845"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>紧凑型卡车，大构想。</h3><p>一款经济实惠且多功能的紧凑型皮卡，标配混合动力系统。</p>", "紧凑型卡车，大构想。\n一款经济实惠且多功能的紧凑型皮卡，标配混合动力系统。\n", null, false, "Ford Maverick", 28000m, null, 10026 },
                    { new Guid("a9b0c1d2-e3f4-a5b6-7890-1234c5d6e7f8"), new Guid("ecf0496f-f1e3-4d92-8fe4-0d7fa2b4ffa4"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>老板。</h3><p>原始的运动型活动车，在其细分市场中设定了奢华、性能和能力的基准。</p>", "老板。\n原始的运动型活动车，在其细分市场中设定了奢华、性能和能力的基准。\n", null, false, "宝马 X5 SAV", 70000m, null, 10038 },
                    { new Guid("a9f8e7b6-a5d4-c3b2-a1d0-e9f8e7b6a5d4"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>运动风格，SUV 实力。</h3><p>将轿跑的动感与 SUV 的多功能性相结合，GLC Coupe 在任何道路上都能展现强大的气场。</p>", "运动风格，SUV 实力。\n将轿跑的动感与 SUV 的多功能性相结合，GLC Coupe 在任何道路上都能展现强大的气场。\n", null, false, "GLC Coupe", 63500m, null, 10009 },
                    { new Guid("b0a9f8e7-b6a5-d4c3-b2a1-d0e9f8e7b6a5"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>智能驱动，令人印象深刻的设计。</h3><p>GLC SUV 在中型豪华车细分市场中为舒适性、技术和性能设定了基准，能够轻松适应您的驾驶需求。</p>", "智能驱动，令人印象深刻的设计。\nGLC SUV 在中型豪华车细分市场中为舒适性、技术和性能设定了基准，能够轻松适应您的驾驶需求。\n", null, false, "GLC SUV", 58900m, null, 10008 },
                    { new Guid("b0c1d2e3-f4a5-b6c7-8901-2345d6e7f8a9"), new Guid("ecf0496f-f1e3-4d92-8fe4-0d7fa2b4ffa4"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>行政级运动感。</h3><p>将动态性能、尖端科技和奢华舒适完美融合，为行政级人士提供精致体验。</p>", "行政级运动感。\n将动态性能、尖端科技和奢华舒适完美融合，为行政级人士提供精致体验。\n", null, false, "宝马 5 系列轿车", 65000m, null, 10039 },
                    { new Guid("b2c3d4e5-f6a7-89b0-12c3-45d678e9f0a1"), new Guid("31d78bd0-0b4f-4e87-b02f-8f66d4ab2845"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>标志性的美式肌肉车。</h3><p>激动人心的性能和无可否认的风格定义了传奇的福特野马轿跑。</p>", "标志性的美式肌肉车。\n激动人心的性能和无可否认的风格定义了传奇的福特野马轿跑。\n", null, false, "Ford Mustang", 40000m, null, 10021 },
                    { new Guid("b4c5d6e7-f8a9-b0c1-2345-6789d0e1f2a3"), new Guid("582b8c19-0709-4dae-b7a6-fa0e704dad3c"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>富有表现力的小型跨界车。</h3><p>通过可定制的风格脱颖而出，享受适合城市的灵活性和智能科技。</p>", "富有表现力的小型跨界车。\n通过可定制的风格脱颖而出，享受适合城市的灵活性和智能科技。\n", null, false, "日产 Kicks", 24000m, null, 10033 },
                    { new Guid("b6c7d8e9-f0a1-b2c3-4567-8901d2e3f4a5"), new Guid("747f6d66-7524-40ca-8494-f65e85b5ee5d"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>紧凑型电动 SUV。</h3><p>一款多功能电动 SUV，提供充足的空间、性能和访问特斯拉超级充电网络的能力。</p>", "紧凑型电动 SUV。\n一款多功能电动 SUV，提供充足的空间、性能和访问特斯拉超级充电网络的能力。\n", null, false, "特斯拉 Model Y", 55000m, null, 10045 },
                    { new Guid("b8c9d0e1-f2a3-b4c5-6789-0123d4e5f6a7"), new Guid("31d78bd0-0b4f-4e87-b02f-8f66d4ab2845"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>时尚的中型跨界车。</h3><p>将精致的设计、智能科技和动感性能结合在一起，形成一款两厢跨界车。</p>", "时尚的中型跨界车。\n将精致的设计、智能科技和动感性能结合在一起，形成一款两厢跨界车。\n", null, false, "Ford Edge", 40000m, null, 10027 },
                    { new Guid("c1b0a9f8-e7b6-a5d4-c3b2-a1d0e9f8e7b6"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>多功能性与空间的完美结合。</h3><p>GLB 尺寸虽小，但提供了意想不到的宽敞空间，选配第三排座椅，使其成为家庭出游的灵活且友好的紧凑型 SUV。</p>", "多功能性与空间的完美结合。\nGLB 尺寸虽小，但提供了意想不到的宽敞空间，选配第三排座椅，使其成为家庭出游的灵活且友好的紧凑型 SUV。\n", null, false, "GLB SUV", 52500m, null, 10007 },
                    { new Guid("c1d2e3f4-a5b6-c7d8-9012-3456e7f8a9b0"), new Guid("ecf0496f-f1e3-4d92-8fe4-0d7fa2b4ffa4"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>电动性能轿跑。</h3><p>宝马首款全电动 Gran Coupe，提供令人印象深刻的续航里程和标志性的宝马驾驶动态。</p>", "电动性能轿跑。\n宝马首款全电动 Gran Coupe，提供令人印象深刻的续航里程和标志性的宝马驾驶动态。\n", null, false, "宝马 i4 Gran Coupe", 60000m, null, 10040 },
                    { new Guid("c3d4e5f6-a7b8-9c01-23d4-56e789f0a1b2"), new Guid("31d78bd0-0b4f-4e87-b02f-8f66d4ab2845"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>冒险准备好的 SUV。</h3><p>一款宽敞且功能强大的 SUV，专为家庭冒险而设计，提供三排座椅和现代科技。</p>", "冒险准备好的 SUV。\n一款宽敞且功能强大的 SUV，专为家庭冒险而设计，提供三排座椅和现代科技。\n", null, false, "Ford Explorer", 48000m, null, 10022 },
                    { new Guid("c5d6e7f8-a9b0-c1d2-3456-7890e1f2a3b4"), new Guid("582b8c19-0709-4dae-b7a6-fa0e704dad3c"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>日产的电动跨界车。</h3><p>体验未来驾驶，尽在全电动 Ariya 中，融合流线型设计和先进的电动汽车技术。</p>", "日产的电动跨界车。\n体验未来驾驶，尽在全电动 Ariya 中，融合流线型设计和先进的电动汽车技术。\n", null, false, "日产 Ariya", 50000m, null, 10034 },
                    { new Guid("c7d8e9f0-a1b2-c3d4-5678-9012e3f4a5b6"), new Guid("747f6d66-7524-40ca-8494-f65e85b5ee5d"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>奢华电动基准。</h3><p>重新定义电动性能的轿车，提供令人难以置信的加速、续航和科技。</p>", "奢华电动基准。\n重新定义电动性能的轿车，提供令人难以置信的加速、续航和科技。\n", null, false, "特斯拉 Model S", 90000m, null, 10046 },
                    { new Guid("c9d0e1f2-a3b4-c5d6-7890-1234e5f6a7b8"), new Guid("582b8c19-0709-4dae-b7a6-fa0e704dad3c"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>适合家庭的跨界车。</h3><p>日产畅销的紧凑型 SUV，提供先进的安全功能和舒适多变的内饰。</p>", "适合家庭的跨界车。\n日产畅销的紧凑型 SUV，提供先进的安全功能和舒适多变的内饰。\n", null, false, "日产 Rogue", 32000m, null, 10028 },
                    { new Guid("d0e1f2a3-b4c5-d6e7-8901-2345f6a7b8c9"), new Guid("582b8c19-0709-4dae-b7a6-fa0e704dad3c"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>智能中型轿车。</h3><p>一款时尚的轿车，配备可选的全轮驱动和驾驶辅助技术。</p>", "智能中型轿车。\n一款时尚的轿车，配备可选的全轮驱动和驾驶辅助技术。\n", null, false, "日产 Altima", 30000m, null, 10029 },
                    { new Guid("d2a1b0c9-f8e7-b6a5-d4c3-b2a1d0e9f8e7"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>紧凑的车身，宏大的抱负。</h3><p>灵活而富有冒险精神，GLA 将 SUV 的多功能性与紧凑型的高效完美结合，非常适合在城市街道或风景优美的路线中穿行。</p>", "紧凑的车身，宏大的抱负。\n灵活而富有冒险精神，GLA 将 SUV 的多功能性与紧凑型的高效完美结合，非常适合在城市街道或风景优美的路线中穿行。\n", null, false, "GLA SUV", 49900m, null, 10006 },
                    { new Guid("d2e3f4a5-b6c7-d8e9-0123-4567f8a9b0c1"), new Guid("ecf0496f-f1e3-4d92-8fe4-0d7fa2b4ffa4"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>电动科技旗舰。</h3><p>未来 SAV 的大胆愿景，具有可持续奢华、突破性科技和令人振奋的电动动力。</p>", "电动科技旗舰。\n未来 SAV 的大胆愿景，具有可持续奢华、突破性科技和令人振奋的电动动力。\n", null, false, "宝马 iX SAV", 90000m, null, 10041 },
                    { new Guid("d4e5f6a7-b8c9-d0e1-f234-567890a1b2c3"), new Guid("31d78bd0-0b4f-4e87-b02f-8f66d4ab2845"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>紧凑型多功能性。</h3><p>一款时尚且高效的紧凑型 SUV，提供城市驾驶和周末度假的灵活性。</p>", "紧凑型多功能性。\n一款时尚且高效的紧凑型 SUV，提供城市驾驶和周末度假的灵活性。\n", null, false, "Ford Escape", 35000m, null, 10023 },
                    { new Guid("d6e7f8a9-b0c1-d2e3-4567-8901f2a3b4c5"), new Guid("582b8c19-0709-4dae-b7a6-fa0e704dad3c"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>传奇性能。</h3><p>标志性的跑车回归，结合了永恒的设计和令人兴奋的双涡轮增压 V6 动力。</p>", "传奇性能。\n标志性的跑车回归，结合了永恒的设计和令人兴奋的双涡轮增压 V6 动力。\n", null, false, "日产 Z", 45000m, null, 10035 },
                    { new Guid("d8e9f0a1-b2c3-d4e5-6789-0123f4a5b6c7"), new Guid("747f6d66-7524-40ca-8494-f65e85b5ee5d"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>带有鹰翼门的电动 SUV。</h3><p>一款独特的家庭 SUV，具有独特的鹰翼门、令人印象深刻的续航和性能。</p>", "带有鹰翼门的电动 SUV。\n一款独特的家庭 SUV，具有独特的鹰翼门、令人印象深刻的续航和性能。\n", null, false, "特斯拉 Model X", 100000m, null, 10047 },
                    { new Guid("e1f2a3b4-c5d6-e7f8-9012-3456a7b8c9d0"), new Guid("582b8c19-0709-4dae-b7a6-fa0e704dad3c"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>锋利的紧凑型轿车。</h3><p>在紧凑轿车级别中提供意想不到的风格、标准安全功能和优质感受。</p>", "锋利的紧凑型轿车。\n在紧凑轿车级别中提供意想不到的风格、标准安全功能和优质感受。\n", null, false, "日产 Sentra", 25000m, null, 10030 },
                    { new Guid("e3b2a1d0-c9f8-e7b6-a5d4-c3b2a1d0e9f8"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>奢华的未来，电动化。</h3><p>旗舰电动轿车将前卫设计与开创性技术和惊人性能相融合，为电动出行设定了新的标准。</p>", "奢华的未来，电动化。\n旗舰电动轿车将前卫设计与开创性技术和惊人性能相融合，为电动出行设定了新的标准。\n", null, false, "EQS Sedan", 136000m, null, 10005 },
                    { new Guid("e3f4a5b6-c7d8-e9f0-1234-5678a9b0c1d2"), new Guid("ecf0496f-f1e3-4d92-8fe4-0d7fa2b4ffa4"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>高性能图标。</h3><p>传奇的 M3 提供无与伦比的赛道准备性能，同时兼顾日常实用性。</p>", "高性能图标。\n传奇的 M3 提供无与伦比的赛道准备性能，同时兼顾日常实用性。\n", null, false, "宝马 M3 Sedan", 80000m, null, 10042 },
                    { new Guid("e5f6a7b8-c9d0-e1f2-3456-7890a1b2c3d4"), new Guid("31d78bd0-0b4f-4e87-b02f-8f66d4ab2845"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>野性重生。</h3><p>图标的回归。福特 Bronco 专为强悍的越野能力和冒险而生。</p>", "野性重生。\n图标的回归。福特 Bronco 专为强悍的越野能力和冒险而生。\n", null, false, "Ford Bronco", 42000m, null, 10024 },
                    { new Guid("e7f8a9b0-c1d2-e3f4-5678-9012a3b4c5d6"), new Guid("ecf0496f-f1e3-4d92-8fe4-0d7fa2b4ffa4"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>终极驾驶机器。</h3><p>典型的运动轿车，平衡了动态性能、日常实用性和奢华感。</p>", "终极驾驶机器。\n典型的运动轿车，平衡了动态性能、日常实用性和奢华感。\n", null, false, "宝马 3 系列轿车", 50000m, null, 10036 },
                    { new Guid("e9f0a1b2-c3d4-e5f6-7890-1234a5b6c7d8"), new Guid("747f6d66-7524-40ca-8494-f65e85b5ee5d"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>增强版电动运动轿车。</h3><p>在 Model 3 的基础上，增加了更快的加速、赛道模式和更运动的调校。</p>", "增强版电动运动轿车。\n在 Model 3 的基础上，增加了更快的加速、赛道模式和更运动的调校。\n", null, false, "特斯拉 Model 3 Performance", 55000m, null, 10048 },
                    { new Guid("f0a1b2c3-d4e5-f6a7-8901-2345b6c7d8e9"), new Guid("747f6d66-7524-40ca-8494-f65e85b5ee5d"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>超越疯狂的速度。</h3><p>提供惊人的加速数据，使其成为有史以来最快的量产车之一。</p>", "超越疯狂的速度。\n提供惊人的加速数据，使其成为有史以来最快的量产车之一。\n", null, false, "特斯拉 Model S Plaid", 110000m, null, 10049 },
                    { new Guid("f2a3b4c5-d6e7-f8a9-0123-4567b8c9d0e1"), new Guid("582b8c19-0709-4dae-b7a6-fa0e704dad3c"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>坚固的 3 排 SUV。</h3><p>重返坚固。Pathfinder 提供多达八个座位和强大的性能，适合家庭冒险。</p>", "坚固的 3 排 SUV。\n重返坚固。Pathfinder 提供多达八个座位和强大的性能，适合家庭冒险。\n", null, false, "日产 Pathfinder", 40000m, null, 10031 },
                    { new Guid("f4a3b2c1-d0e9-f8a7-b6c5-d4e3f2a1b0c9"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>电动奢华 SUV 的巅峰。</h3><p>体验旗舰级舒适性和突破性技术，尽在全电动 SUV 形态中，重新定义可持续奢华，最多可容纳七人。</p>", "电动奢华 SUV 的巅峰。\n体验旗舰级舒适性和突破性技术，尽在全电动 SUV 形态中，重新定义可持续奢华，最多可容纳七人。\n", null, false, "EQS SUV", 136000m, null, 10004 },
                    { new Guid("f4a5b6c7-d8e9-f0a1-2345-6789b0c1d2e3"), new Guid("ecf0496f-f1e3-4d92-8fe4-0d7fa2b4ffa4"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>敞篷自由。</h3><p>经典跑车体验，结合现代宝马性能、灵活性和风格。</p>", "敞篷自由。\n经典跑车体验，结合现代宝马性能、灵活性和风格。\n", null, false, "宝马 Z4 跑车", 60000m, null, 10043 },
                    { new Guid("f6a7b8c9-d0e1-f2a3-4567-8901b2c3d4e5"), new Guid("31d78bd0-0b4f-4e87-b02f-8f66d4ab2845"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>电动惊喜。</h3><p>一款全电动 SUV，承载着野马的名字，带来令人振奋的性能和先进的科技。</p>", "电动惊喜。\n一款全电动 SUV，承载着野马的名字，带来令人振奋的性能和先进的科技。\n", null, false, "Ford Mustang Mach-E", 55000m, null, 10025 },
                    { new Guid("f8a9b0c1-d2e3-f4a5-6789-0123b4c5d6e7"), new Guid("ecf0496f-f1e3-4d92-8fe4-0d7fa2b4ffa4"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>多才多艺的运动型活动车。</h3><p>将动态的宝马驾驶特性与 SAV 的多功能性和实用性相结合。</p>", "多才多艺的运动型活动车。\n将动态的宝马驾驶特性与 SAV 的多功能性和实用性相结合。\n", null, false, "宝马 X3 SAV", 55000m, null, 10037 },
                    { new Guid("f9f8e7b6-a5d4-c3b2-a1d0-e9f8e7b6a5d4"), new Guid("6fae78f3-b067-40fb-a2d5-9c8dd5eb2e08"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "<h3>开放式驾驶的自由，优雅的设计。</h3><p>体验敞篷驾驶的乐趣，CLE Cabriolet 提供精致的风格、四季舒适和令人振奋的性能。</p>", "开放式驾驶的自由，优雅的设计。\n体验敞篷驾驶的乐趣，CLE Cabriolet 提供精致的风格、四季舒适和令人振奋的性能。\n", null, false, "CLE Cabriolet", 75000m, null, 10019 }
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
        }
    }
}
