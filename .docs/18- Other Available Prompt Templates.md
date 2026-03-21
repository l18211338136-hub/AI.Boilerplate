# 第十八阶段：其他可用的提示词模板

本项目包含几个专门的提示词模板，旨在帮助您完成特定的开发任务。每个提示词都经过精心编写，以遵循项目的约定和最佳实践。

## 可用的提示词

### 1. 脚手架提示词 (`scaffold.prompt.md`)

**文件位置**: `.github/prompts/scaffold.prompt.md`

**功能**: 为项目中的新实体生成完整的 CRUD（创建、读取、更新、删除）实现，包括从数据库到 UI 的所有必要层级。

**使用时机**: 当您需要向应用程序添加具有完整 CRUD 功能的新数据实体时。

**关键能力**:
- 为 EF Core 创建实体类型配置 (Entity Type Configuration)
- 生成带有验证属性的 DTO (数据传输对象)
- 使用 Mapperly 创建映射器以实现高性能对象映射
- 生成支持 OData 的 API 控制器
- 创建 `IAppController` 接口以用于强类型 HTTP 客户端
- 将资源字符串添加到 `AppStrings.resx` 以进行本地化
- 创建用于列出记录的数据网格页面 (Data Grid Page)
- 创建用于添加和编辑记录的“添加/编辑”页面
- 集成导航系统 (`PageUrls.cs`, `NavBar`, `NavPanel`)
- 更新 `AppJsonContext` 以进行 JSON 序列化
- 生成 EF Core 迁移

**使用示例**: “运行 `scaffold.prompt.md` 创建一个包含 Name, Description, Price 和 CategoryId 属性的 Product 实体”

---

### 2. Resx 提示词 (`resx.prompt.md`)

**文件位置**: `.github/prompts/resx.prompt.md`

**功能**: 自动识别代码中的硬编码字符串，并将它们移动到资源文件 (.resx) 中，以提供适当的本地化支持。

**使用时机**: 当您的 Blazor 组件、页面或控制器中包含需要本地化以支持多种语言的硬编码用户界面文本时。

**关键能力**:
- 识别所选代码中硬编码的用户界面字符串
- 使用适当的资源键将新条目添加到 `AppStrings.resx`
- 生成强类型资源类
- 更新代码以使用 `IStringLocalizer<AppStrings>` 模式
- 使用 `nameof(AppStrings.ResourceKey)` 实现类型安全的资源访问
- 保留带有占位符的字符串格式（例如 `{0}`, `{1}`）
- 遵循命名约定，使用描述性的资源键

**不会移动的内容**:
- CSS 类名或 ID
- 配置键
- API 端点或 URL
- 技术常量（文件扩展名、MIME 类型）
- 日志消息

**使用示例**: “在 `Dashboard.razor` 文件上运行 `resx.prompt.md`，将所有硬编码字符串移动到资源文件中”

---

### 3. Bitify 提示词 (`bitify.prompt.md`)

**文件位置**: `.github/prompts/bitify.prompt.md`

**功能**: 通过将标准 HTML 元素和自定义 CSS 替换为 Bit.BlazorUI 组件和感知主题的样式，使您的 Blazor 页面现代化。

**使用时机**: 当您有使用通用 HTML 元素（如 `<button>`, `<input>`, `<div>`）的页面，并希望将它们升级为使用 Bit.BlazorUI 组件库以确保一致性、更好的用户体验和主题支持时。

**关键能力**:
- 分析当前的 HTML 标记并识别可替换的元素
- 创建所有可替换 HTML 元素的现代化清单
- 使用 DeepWiki 研究合适的 Bit.BlazorUI 组件
- 用适当的 Bit.BlazorUI 组件替换 HTML 元素：
  - `<button>` → `BitButton`, `BitActionButton`
  - `<input type="text">` → `BitTextField`
  - `<select>` → `BitDropdown`
- 使用 `$bit-color-*` 变量将自定义 CSS 转换为感知主题的样式
- 使用 `::deep` 选择器进行正确的组件样式设置
- 更新事件处理器以使用 `WrapHandled` 模式
- 确保亮色/暗色主题兼容性

**使用示例**: “在 `UserProfile.razor` 页面上运行 `bitify.prompt.md`，将所有 HTML 元素替换为 Bit.BlazorUI 组件”