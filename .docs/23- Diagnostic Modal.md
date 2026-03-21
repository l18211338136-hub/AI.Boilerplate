# 第二十三阶段：诊断模态框

**诊断模态框 (Diagnostic Modal)** 是一个极其强大的内置故障排除工具，为开发者和支持团队提供有关应用程序运行时的综合信息。此功能在**所有环境**（开发、预发布和生产）以及**所有平台**（Web、Android、iOS、Windows、macOS）上均可用。

## 什么是诊断模态框？

诊断模态框位于 [`src/Client/AI.Boilerplate.Client.Core/Components/Layout/Diagnostic/AppDiagnosticModal.razor`](/src/Client/AI.Boilerplate.Client.Core/Components/Layout/Diagnostic/AppDiagnosticModal.razor)，是一个特殊的 UI 组件，可实时显示正在运行的应用程序的详细诊断信息。它充当一个**应用内故障排除控制台**，即使在无法使用传统开发者工具的移动设备上也能工作。

## 🎯 动手探索（强烈推荐）

**在深入阅读文档之前**，我们**强烈建议**您亲自体验诊断模态框：

1. 访问 **https://bitplatform.dev/demos**
2. 打开任何已发布的演示应用（管理面板、销售仪表板等）
3. 在页眉上点击 **7 次**，或者按下 **Ctrl+Shift+X**
4. 探索所有功能：
   - 查看遥测上下文
   - 过滤和搜索日志
   - 测试诊断 API
   - 尝试实用按钮
   - 查看详细日志信息

这种亲身体验将比单纯阅读文档让您更好地理解该工具的功能和价值。

---

## 主要功能

诊断模态框提供三个主要功能区域：

### 1. **遥测上下文查看器**

显示全面的应用程序上下文信息，包括：
- **用户信息**：用户 ID、电子邮件、角色、会话详情
- **设备信息**：平台、操作系统版本、浏览器/应用版本
- **应用程序状态**：环境（开发/预发布/生产）、文化/语言环境设置
- **网络信息**：服务器地址、连接状态、IP 地址
- **会话详情**：身份验证状态、会话 ID、最后活动时间

**可用操作：**
- **复制到剪贴板**：将所有遥测上下文格式化为文本并复制，便于与支持团队共享

---

### 2. **应用内日志查看器**

一个强大的日志查看和过滤系统，显示由 `DiagnosticLogger` 捕获的日志：

#### 过滤与搜索功能
- **文本搜索**：按消息内容、类别或状态值搜索日志
- **正则表达式支持**：启用正则表达式模式以进行高级模式匹配
- **按日志级别过滤**：按 Trace、Debug、Information、Warning、Error、Critical 过滤
- **按类别过滤**：多选下拉菜单，显示所有日志类别（例如 `Microsoft.EntityFrameworkCore`、`AI.Boilerplate.Client.Core.Services`）
- **排序顺序**：在升序/降序时间顺序之间切换

#### 日志详情视图
- **复制单个日志**：将任何日志条目复制到剪贴板
- **详细视图**：点击“详情”查看完整日志信息：
  - 类别
  - 消息
  - 异常详情（如果有）
  - 状态键值对
- **日志间导航**：使用“上一个/下一个”按钮浏览过滤后的日志
- **时间戳显示**：每条日志以 HH:mm:ss 格式显示时间
- **颜色编码严重性**：使用 BitColor 主题在视觉上区分日志级别

---

### 3. **实用按钮与操作**

模态框提供几个强大的诊断和维护操作：

#### 🔄 **重新加载日志 (Reload Logs)**
- 使用最新的内存中日志刷新日志查看器
- 当模态框打开时添加了新日志非常有用

#### 🗑️ **清除日志 (Clear Logs)**
- 清除内存存储中的所有日志
- 适用于开始新的诊断会话

#### ⚠️ **抛出测试异常 (Throw Test Exception)**
- 生成测试异常以验证错误处理
- 在 `InvalidOperationException`（未知异常）和 `DomainLogicException`（已知异常）之间交替
- 两者都包含测试数据以演示异常数据捕获
- **用途**：测试错误边界、异常处理程序和日志基础设施

#### 🔬 **调用诊断 API (Call Diagnostics API)**
- 向 [`DiagnosticsController.PerformDiagnostics`](/src/Server/AI.Boilerplate.Server.Api/Features/Diagnostics/DiagnosticsController.cs) 发送请求
- 返回全面的服务器端诊断信息，包括：
  - 客户端 IP 地址
  - HTTP 跟踪标识符
  - 身份验证状态
  - 所有 HTTP 请求头
  - 服务器环境名称
  - 基础 URL
  - 运行时信息（AOT 检测、GC 配置等）
- **同时测试**：
  - 推送通知功能（如果已订阅）
  - SignalR 连接（如果已连接）
- 在 BitMessageBox 中显示结果

#### 🛠️ **打开开发者工具 (Open Dev Tools)**
- 打开**应用内浏览器开发者工具**界面
- **关键功能**：在**移动设备**（Android/iOS）上工作，而传统开发者工具在这些设备上不可用
- 提供控制台、网络检查器和其他调试工具
- 实现方式：使用 `App.openDevTools()` JavaScript 互操作

#### ♻️ **调用 GC (Call GC)**（仅限非浏览器平台）
- 强制 .NET MAUI 和 Windows 平台进行垃圾回收
- 使用 SnackBar 通知显示 GC **前后**的内存使用情况
- 执行带有压缩的激进 GC：
  ```csharp
  GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: true);
  GC.WaitForPendingFinalizers();
  ```
- **用途**：测试内存管理和调查内存泄漏

#### 🧹 **清除应用文件 (Clear App Files)**
- 全面的应用存储清除操作：
  - 删除所有 WebAuthn 凭据
  - 注销当前用户
  - 清除所有本地/会话存储
  - **Web**：使用 `BitBswup.forceRefresh()` 卸载 service worker 并清除缓存存储
  - **混合应用**：强制完全重新加载应用程序
- **用途**：重置应用状态以进行故障排除或测试全新安装

#### 🔄 **更新应用 (Update App)**
- 强制立即检查并安装应用更新
- 使用 `IAppUpdateService.ForceUpdate()`
- 绕过正常的更新计划
- **用途**：测试强制更新系统或应用紧急修复