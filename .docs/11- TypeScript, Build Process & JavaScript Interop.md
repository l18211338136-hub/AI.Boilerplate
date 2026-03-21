# 第十一步：TypeScript、构建流程与 JavaScript 互操作

欢迎回到第十一步！在本阶段，您将了解本项目如何将 TypeScript 与 C# Blazor 集成，编译 TypeScript 和 SCSS 的构建流程，以及如何从 C# 代码调用 JavaScript 函数。

## 概述

本项目使用 **TypeScript** 进行类型安全的 JavaScript 开发，并配有自动化的构建流程，在构建管道中将 TypeScript 编译为 JavaScript，将 SCSS 编译为 CSS。您还将学习如何添加新的 npm 包以及如何在 C# Blazor 组件中调用 JavaScript 函数。

---

## 1. TypeScript 配置

### 位置
[`/src/Client/AI.Boilerplate.Client.Core/tsconfig.json`](/src/Client/AI.Boilerplate.Client.Core/tsconfig.json)

### 配置内容

```jsonc
{
    "compileOnSave": true,
    "compilerOptions": {
        "strict": true,
        "target": "ES2019",
        "module": "es2015",
        // ...
    }
}
```

### 关键设置说明

- **`strict: true`**: 启用所有严格类型检查选项，以提高代码质量
- **`target: "ES2019"`**: 将 TypeScript 编译为 ES2019 JavaScript
- **`module: "es2015"`**: 使用 ES2015 模块系统 (import/export)
- **`noImplicitAny: true`**: 要求显式类型注解，防止隐式的 `any` 类型
- **`lib: ["DOM", "ESNext"]`**: 包含 DOM 和现代 JavaScript API 的类型定义
- **`moduleResolution: "node"`**: 使用 Node.js 风格的模块解析

---

## 2. 使用 npm 进行包管理

### 位置
[`/src/Client/AI.Boilerplate.Client.Core/package.json`](/src/Client/AI.Boilerplate.Client.Core/package.json)

### 当前依赖项

```json
{
    "devDependencies": {
        "esbuild": "0.27.0",
        "sass": "1.94.0",
        "typescript": "5.9.3"
    }
}
```

### 每个包的作用

- **`typescript`**: TypeScript 编译器 (`tsc`)，将 `.ts` 文件转换为 `.js`
- **`esbuild`**: 超快的 JavaScript 打包工具，将所有 JavaScript 模块合并为一个压缩后的 `app.js` 文件
- **`sass`**: SCSS/Sass 编译器，将 `.scss` 文件转换为 `.css`

---

## 3. MSBuild 集成与构建流程

### 位置
[`/src/Client/AI.Boilerplate.Client.Core/AI.Boilerplate.Client.Core.csproj`](/src/Client/AI.Boilerplate.Client.Core/AI.Boilerplate.Client.Core.csproj)

### 构建管道

`.csproj` 文件定义了自定义的 MSBuild 目标，这些目标会在构建过程中自动运行：

```xml
<Target Name="BeforeBuildTasks" AfterTargets="CoreCompile">
    <CallTarget Targets="InstallNodejsDependencies" />
    <CallTarget Targets="BuildJavaScript" />
    <CallTarget Targets="BuildCssFiles" />
</Target>
```

### 构建流程

```
1. CoreCompile (C# 编译)
    ↓
2. BeforeBuildTasks
    ↓
3. InstallNodejsDependencies
    ↓
4. BuildJavaScript (TypeScript → JavaScript → 打包)
    ↓
5. BuildCssFiles (SCSS → CSS)
```

### 步骤 1: InstallNodejsDependencies

```xml
<Target Name="InstallNodejsDependencies" Inputs="package.json" Outputs="node_modules\.package-lock.json">
    <Exec Command="npm install" StandardOutputImportance="high" StandardErrorImportance="high" />
</Target>
```

**作用：**
- 运行 `npm install` 安装 `package.json` 中的所有包
- 仅在 `package.json` 更改时运行（增量构建优化）
- 创建包含所有依赖项的 `node_modules` 文件夹

### 步骤 2: BuildJavaScript

```xml
<Target Name="BuildJavaScript" Inputs="@(TypeScriptFiles);tsconfig.json;package.json" Outputs="wwwroot\scripts\app.js">
    <Exec Command="node_modules/.bin/tsc" StandardOutputImportance="high" StandardErrorImportance="high" />
    <Exec Condition=" '$(Environment)' == 'Development' " 
          Command="node_modules/.bin/esbuild Scripts/index.js --bundle --outfile=wwwroot/scripts/app.js" 
          StandardOutputImportance="high" StandardErrorImportance="high" />
    <Exec Condition=" '$(Environment)' != 'Development' " 
          Command="node_modules/.bin/esbuild Scripts/index.js --bundle --outfile=wwwroot/scripts/app.js --minify" 
          StandardOutputImportance="high" StandardErrorImportance="high" />
</Target>
```

**作用：**
1. **TypeScript 编译**: 运行 `tsc` (TypeScript 编译器) 将所有 `.ts` 文件转换为 `.js` 文件
2. **打包 (开发环境)**: 使用 `esbuild` 将所有 JavaScript 模块打包成单个 `wwwroot/scripts/app.js` 文件
3. **打包 + 压缩 (生产/预发布环境)**: 同上，但添加 `--minify` 标志以减小文件大小

**增量构建优化：**
- 仅当 TypeScript 文件、`tsconfig.json` 或 `package.json` 更改时才重新构建
- 如果 `app.js` 是最新的，则跳过编译

### 步骤 3: BuildCssFiles

```xml
<Target Name="BuildCssFiles">
    <Exec Command="node_modules/.bin/sass Components:Components Styles/app.scss:wwwroot/styles/app.css --style compressed --silence-deprecation=import --update --color" 
          StandardOutputImportance="high" StandardErrorImportance="high" LogStandardErrorAsError="true" />
</Target>
```

**作用：**
- 将 `Styles/app.scss` 编译为 `wwwroot/styles/app.css`
- 处理 `Components` 文件夹中特定于组件的 `.razor.scss` 文件
- 使用 `--style compressed` 输出压缩后的 CSS
- 使用 `--update` 标志仅覆盖已更改的文件

---

## 4. JavaScript 互操作：从 C# 调用 JS

### JavaScript 端：App.ts

**位置:** [`/src/Client/AI.Boilerplate.Client.Core/Scripts/App.ts`](/src/Client/AI.Boilerplate.Client.Core/Scripts/App.ts)

这是主要的 TypeScript 文件，用于向 C# 代码暴露 JavaScript 函数：

```typescript
export class App {
    public static getTimeZone(): string {
        return Intl.DateTimeFormat().resolvedOptions().timeZone;
    }
}
```

### C# 端：IJSRuntimeExtensions.cs

**位置:** [`/src/Client/AI.Boilerplate.Client.Core/Infrastructure/Extensions/IJSRuntimeExtensions.cs`](/src/Client/AI.Boilerplate.Client.Core/Infrastructure/Extensions/IJSRuntimeExtensions.cs)

此文件定义了调用 `App.ts` 中 JavaScript 函数的 C# 扩展方法：

```csharp
using System.Reflection;
using AI.Boilerplate.Shared.Dtos.PushNotification;

namespace Microsoft.JSInterop;

public static partial class IJSRuntimeExtensions
{
    public static ValueTask<string> GetTimeZone(this IJSRuntime jsRuntime)
    {
        return jsRuntime.InvokeAsync<string>("App.getTimeZone");
    }
}
```

### 示例：GetTimeZone 方法

让我们以 `getTimeZone` 方法为例进行完整演示：

#### JavaScript (App.ts)
```typescript
public static getTimeZone(): string {
    return Intl.DateTimeFormat().resolvedOptions().timeZone;
}
```

#### C# 扩展方法 (IJSRuntimeExtensions.cs)
```csharp
public static ValueTask<string> GetTimeZone(this IJSRuntime jsRuntime)
{
    return jsRuntime.InvokeAsync<string>("App.getTimeZone");
}
```

#### 在 Blazor 组件中使用
```csharp
@inject IJSRuntime JSRuntime

@code {
    private string? userTimeZone;

    protected override async Task OnAfterFirstRenderAsync()
    {
        userTimeZone = await JSRuntime.GetTimeZone();
        StateHasChanged();
    }
}
```

### 工作原理

1. **TypeScript 方法**: `App.getTimeZone()` 使用浏览器的 `Intl.DateTimeFormat` API 获取用户的时区
2. **C# 扩展方法**: `GetTimeZone()` 调用 `jsRuntime.InvokeAsync<string>("App.getTimeZone")` 来执行 JavaScript 函数
3. **组件使用**: 任何 Blazor 组件都可以调用 `await JSRuntime.GetTimeZone()` 来获取用户的时区

---

## 5. 演示：添加新的 npm 包 (uuid)

让我们通过一个完整的示例来演示如何添加 `uuid` 包以生成唯一标识符。

### 步骤 1: 安装包

在 `AI.Boilerplate.Client.Core` 目录中运行以下命令：

```powershell
cd src/Client/AI.Boilerplate.Client.Core
npm install uuid
npm install --save-dev @types/uuid
```

**每个命令的作用：**
- `npm install uuid`: 安装 `uuid` 包（运行时依赖）
- `npm install --save-dev @types/uuid`: 安装 `uuid` 的 TypeScript 类型定义（开发依赖）

### 步骤 2: 更新 package.json

运行命令后，您的 `package.json` 应如下所示：

```json
{
    "dependencies": {
        "uuid": "^11.0.3"
    },
    "devDependencies": {
        "esbuild": "0.27.0",
        "sass": "1.94.0",
        "typescript": "5.9.3",
        "@types/uuid": "^10.0.0"
    }
}
```

### 步骤 3: 在 App.ts 中添加 TypeScript 方法

在 [`Scripts/App.ts`](/src/Client/AI.Boilerplate.Client.Core/Scripts/App.ts) 的 `App` 类顶部添加导入和新方法：

```typescript
import { v4 as uuidv4 } from 'uuid';

export class App {
    // ... 现有方法 ...

    public static generateUuid(): string {
        return uuidv4();
    }
}
```

### 步骤 4: 添加 C# 扩展方法

将此方法添加到 [`Extensions/IJSRuntimeExtensions.cs`](/src/Client/AI.Boilerplate.Client.Core/Extensions/IJSRuntimeExtensions.cs)：

```csharp
public static ValueTask<string> GenerateUuid(this IJSRuntime jsRuntime)
{
    return jsRuntime.InvokeAsync<string>("App.generateUuid");
}
```

### 步骤 5: 在 Blazor 组件中使用

现在您可以在任何 Blazor 组件中使用它：

```xml
@page "/uuid-demo"
@inject IJSRuntime JSRuntime

<BitText Typography="BitTypography.H4">UUID 生成器演示</BitText>

<BitButton OnClick="GenerateNewUuid">生成 UUID</BitButton>

@if (!string.IsNullOrEmpty(generatedUuid))
{
    <BitText>生成的 UUID: @generatedUuid</BitText>
}

@code {
    private string? generatedUuid;

    private async Task GenerateNewUuid()
    {
        generatedUuid = await JSRuntime.GenerateUuid();
    }
}
```

### 步骤 6: 构建项目

运行构建以编译 TypeScript 并打包新代码：

```powershell
cd src/Server/AI.Boilerplate.Server.Web
dotnet build
```

构建过程将：
1. 安装新的 `uuid` 包（通过 `npm install`）
2. 将 `App.ts` 编译为 JavaScript（通过 `tsc`）
3. 将所有 JavaScript（包括 `uuid`）打包到 `app.js` 中（通过 `esbuild`）

---

## 6. 常见场景

### 添加新的 TypeScript 文件

1. 在 `Scripts/` 文件夹中创建文件（例如 `Scripts/MyHelpers.ts`）
2. 导出您想要使用的函数：
   ```typescript
   export function myHelper(): string {
       return "Hello from TypeScript!";
   }
   ```
3. 在 `Scripts/index.ts` 中导入并在需要时暴露到 window 对象：
   ```typescript
   import { myHelper } from './MyHelpers';
   (window as any).myHelper = myHelper;
   ```
4. 构建项目 - TypeScript 编译器和 esbuild 会自动处理它

---