# PhantomChannel.Community

## 关于此解决方案

这是一个基于[领域驱动设计 (DDD)](https://abp.io/docs/latest/framework/architecture/domain-driven-design) 实践的分层启动解决方案。

### 先决条件

-   [.NET 9.0+ SDK](https://dotnet.microsoft.com/download/dotnet)
-   [Node v20.11+](https://nodejs.org/en)
-   [Redis](https://redis.io/)
-   [Minio or S3](https://min.io/)
-   [PostgreSql](https://www.postgresql.org/)

### 配置

在运行解决方案之前，您可能需要考虑更改以下配置：

-   检查 `PhantomChannel.Community.AuthServer`、`PhantomChannel.Community.HttpApi.Host` 和 `PhantomChannel.Community.DbMigrator` 项目下的 `appsettings.json` 文件中的 `ConnectionStrings`，并根据需要进行更改。
-   关于`OpenIddict`、`AuthServer`等配置请于[此处](https://abp.io)查阅

### 运行应用程序之前

#### 生成签名证书

在生产环境中，您需要使用生产签名证书。ABP 框架在您的应用程序中设置了签名和加密证书，并期望在您的应用程序中存在一个 `openiddict.pfx` 文件。

该证书已由 ABP CLI 生成，因此大多数情况下您不需要自己生成。然而，如果您需要生成证书，可以使用以下命令：

```bash
dotnet dev-certs https -v -ep openiddict.pfx -p 733359e3-c9b1-463d-9b68-8651148c1137
```

> `733359e3-c9b1-463d-9b68-8651148c1137` 是证书的密码，您可以将其更改为您想要的任何密码。

建议使用**两个** RSA 证书，与用于 HTTPS 的证书不同：一个用于加密，一个用于签名。

有关更多信息，请参阅：https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html#registering-a-certificate-recommended-for-production-ready-scenarios

> 此外，请参阅 [配置 OpenIddict](https://abp.io/docs/latest/deployment/configuring-openiddict#production-environment) 文档以获取更多信息。

#### 安装客户端库

在您的最终应用程序目录中运行以下命令：

```bash
abp install-libs
```

> 此命令为 MVC/Razor Pages 和 Blazor Server UI 安装所有 NPM 包，并且此命令已由 ABP CLI 运行，因此大多数情况下您不需要手动运行此命令。

#### 创建数据库

运行 `PhantomChannel.Community.DbMigrator` 以创建初始数据库。这应该在第一次运行时完成。如果稍后向解决方案中添加了新的数据库迁移，也需要执行此操作。

### 解决方案结构

这是一个分层单体应用程序，由以下应用程序组成：

-   `PhantomChannel.Community.DbMigrator`：一个控制台应用程序，用于应用迁移并播种初始数据。它在开发和生产环境中都很有用。
-   `PhantomChannel.Community.AuthServer`：集成了 OAuth 2.0（`OpenIddict`）和账户模块的 ASP.NET Core MVC / Razor Pages 应用程序。它用于对用户进行身份验证并颁发令牌。
-   `PhantomChannel.Community.HttpApi.Host`：ASP.NET Core API 应用程序，用于向客户端公开 API。

### 部署应用程序

各项服务的默认端口

-   `PhantomChannel.Community.AuthServer`：3250
-   `PhantomChannel.Community.HttpApi.Host`：3251
-   `PhantomChannel.Community.Management`：3252

### 其他资源

您可以查看以下资源以了解更多关于您的解决方案的信息：

-   [Web 应用程序开发教程](https://abp.io/docs/latest/tutorials/book-store/part-01?UI=Blazor&DB=EF)
-   [应用程序启动模板结构](https://abp.io/docs/latest/solution-templates/layered-web-application)
