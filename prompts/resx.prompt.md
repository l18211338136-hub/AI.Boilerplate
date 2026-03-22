# 将硬编码字符串移至资源文件

你是一位专家，擅长使用资源文件 (.resx) 和 `IStringLocalizer<AppStrings>` 对 .NET 应用程序进行本地化。

## 指令

1.  **识别硬编码字符串**：在选定的代码或文件中，找出那些为了本地化而需要移至资源文件的字符串。
2.  **添加新资源条目**：如果 `src/Shared/Resources/AppStrings.resx` 中尚不存在相应条目，请添加新的资源项。
3.  **生成强类型资源类**：在 `src/Shared` 目录下运行 `dotnet build -t:PrepareResources` 命令。
4.  **更新代码**：修改代码以使用 `IStringLocalizer<AppStrings>` 和 `nameof(AppStrings.ResourceKey)` 模式。

## 上下文

-   **资源文件位置**：`src/Shared/Resources/AppStrings.resx`
-   **组件继承自**：`AppComponentBase` 或 `AppPageBase`（这些基类已提供可用的 `IStringLocalizer<AppStrings> Localizer`）
-   **控制器继承自**：`AppControllerBase`（此类已提供可用的 `IStringLocalizer<AppStrings> Localizer`）
-   **其他文件**：直接自动注入 (`AutoInject`) `IStringLocalizer<AppStrings>`
-   **使用模式**：
    -   Razor 文件中：`@Localizer[nameof(AppStrings.ResourceKey)]`
    -   C# 代码中：`Localizer[nameof(AppStrings.ResourceKey)]`

## 规则

1.  **使用描述性但简洁的资源键名**，能够反映内容或上下文。
2.  **适当时将相关字符串分组**，使用共同的前缀（例如：`SignIn*`, `Email*`, `Password*`）。
3.  **始终使用 `nameof(AppStrings.ResourceKey)`**，严禁对资源键使用字符串字面量。
4.  **保留字符串格式**：如果原始字符串包含占位符（如 `{0}`），请在资源值中保留它们。
5.  **不要移动以下内容**：
    -   CSS 类名或 ID
    -   配置键 (Configuration keys)
    -   API 端点或 URL
    -   技术常量（文件扩展名、MIME 类型等）
    -   日志消息

## 工作流

1.  **分析提供的代码**，识别面向用户的硬编码字符串。
2.  **检查现有的 `AppStrings.resx`**，确认是否已存在合适的资源条目。
3.  **向 `AppStrings.resx` 添加新条目**，为任何缺失的资源使用以下 XML 格式：
    ```xml
    <data name="ResourceKeyName" xml:space="preserve">
      <value>Resource Value Here</value>
    </data>
    ```
4.  **运行资源生成命令**：在 `src/Shared` 目录中执行 `dotnet build -t:PrepareResources`。
5.  **更新代码文件**，应用本地器 (localizer) 模式。
6.  **验证构建成功**，确保所有更改后项目能正常编译。

## 示例

### 修改前：
```razor
<BitButton>Save Changes</BitButton>
<BitText>Welcome to our application!</BitText>
```

### 修改后 (AppStrings.resx)：
```xml
<data name="Save" xml:space="preserve">
  <value>Save</value>
</data>
<data name="WelcomeMessage" xml:space="preserve">
  <value>Welcome to our application!</value>
</data>
```

### 修改后 (Razor 文件)：
```razor
<BitButton>@Localizer[nameof(AppStrings.Save)]</BitButton>
<BitText>@Localizer[nameof(AppStrings.WelcomeMessage)]</BitText>
```

现在，请按照这些指南，识别选定代码中的硬编码字符串并将其移至资源文件中。