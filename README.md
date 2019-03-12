<img src="https://weixin.senparc.com/images/SCF/logo.png" width="500" />

# SCF - SenparcCoreFramework

> 当前快速更新分支：[Developer-RazorPage-DDD-bs](https://github.com/SenparcCoreFramework/SCF/tree/Developer-RazorPage-DDD-bs)

> 我们欢迎第三方开源组件提供自己的解决方案，我们将会测试并集成到 SCF 中。

SenparcCoreFramework(SCF) 是一整套可用于构建基础项目的框架，包含了基础的缓存、数据库、模型、验证及配套管理后台，模块化，具有高度的可扩展性。

> 说明：SCF 由盛派（Senparc）团队经过多年优化迭代的自用系统底层框架 SenparcCore 整理而来，经历了 .NET 3.5/4.5 众多系统的实战检验，并最终移植到 .NET Core，目前已在多个 .NET Core 系统中稳定运行，在将其转型为开源项目的过程中，需要进行一系列的重构、注释完善和兼容性升级，目前尚处于雏形阶段，希望大家多提意见，我们会争取在最短的时间内优化并发布第一个试用版（Preview1）。感谢大家一直以来的支持！<br>
> <br>
> Preview1 版本中，我们将提供更加完善的模块化架构和辅助工具，当前源码已经可用于学习和测试使用。

SCF 除了会为大家提供完善的框架代码，还会：

1. 提供完善的项目自动生成服务（参考 [WeChatSampleBuilder](http://sdk.weixin.senparc.com/Home/WeChatSampleBuilder)），为开发者提供项目定制生成服务。

2. 提供丰富的应用示例，例如[微信](https://github.com/JeffreySu/WeiXinMPSDK)、[跨平台应用平台](https://www.neuchar.com/)对接，等等。

3. 提供完善的示例代码和文档。

4. 提供博客和视频教程（也欢迎开发者参与或发起）。

5. 提供交流社区，包括但不仅限于[问答网站](https://weixin.senparc.com/QA)、[QQ群](#qq-技术交流群)、微信群、直播群。

## QQ 技术交流群

<img src="https://sdk.weixin.senparc.com/images/QQ_Group_Avatar/SCF/QQ-Group.jpg" width="380" />

## 环境要求

- Visual Studio 2017 15.7 版本以上或 VS Code 最新版本

- .NET Core 2.2+ （未来将支持更多版本），SDK下载地址：https://dotnet.microsoft.com/download/dotnet-core/2.2

## 如何安装

SCF 将提供全自动的安装程序，目前正在整理阶段。发布之前，您可以通过以下手动方法开始使用：

### 第一步：准备数据库
确保已经安装 SQL Server 2008 及以上版本，系统登录用户具有数据库创建权限（可以不需要使用sa等账号登录），如果必须要使用账号登录，[请看这里](https://github.com/SenparcCoreFramework/SCF/wiki/%E5%A6%82%E4%BD%95%E4%BF%AE%E6%94%B9%E9%BB%98%E8%AE%A4%E6%95%B0%E6%8D%AE%E5%BA%93%E8%BF%9E%E6%8E%A5%E5%AD%97%E7%AC%A6%E4%B8%B2%EF%BC%9F)

### 第二步：准备命令行工具

#### 方法一（推荐）：
1. 使用命令行工具或 PowerShell 进入 `src/Senparc.Web` 路径，例如：`E:\SenparcCoreFramework\SCF\src\Senparc.Web`

2. 输入命令：`dotnet ef database update` 回车

#### 方法二（要看运气）：
1. 同步源代码到本地后，使用 Visual Studio 打开 `/src/SCF.sln`

2. 在 VS 菜单中选择【工具】>【Nuget包管理器】>【程序包管理器控制台】，打开命令窗口

3. 在【程序包管理器控制台】中的【默认项目】列表中选中 `Senparc.Web`（默认就是），在 `PM>` 符号后输入命令：`update-database` 回车


#### 等待结果

稍等片刻（会自动编译一次项目，因此请勿修改项目代码），完成后输出如下结果，表示数据库安装成功：

```
Applying migration '20181130085128_init'.
Done.
```

<img src="https://weixin.senparc.com/images/SCF/Install/02.png" />

### 第三步：初始化数据

 1. 将 `Senparc.Web` 项目设为启动项目，并运行，地址如：http://localhost:11946/

 <img src="https://weixin.senparc.com/images/SCF/Install/01.png" />
 
 2. 打开 http://localhost:11946/Install ，数据库将会自动初始化。
 
<img src="https://weixin.senparc.com/images/SCF/Install/03.png" />



 4. 完成后，保存页面上显示的账号和密码，根据提示进入管理员后台。


<img src="https://weixin.senparc.com/images/SCF/Install/04.png" />


### （备用）第四步：还原样式包

如果登录及管理员后台页面样式缺失，则需要进行这一步，否则可以忽略。

1. 检查当前系统中node是否安装：`node -v`
 
2. 检查当前系统中npm是否安装：`npm -v`

 <img src="http://image.mlkj.ymstudio.xyz/node&npm.png" />  

3. 检查当前系统中bower是否安装：`bower help` 

 <img src="http://image.mlkj.ymstudio.xyz/bower%E5%AE%89%E8%A3%85%E5%AE%8C%E6%88%90.png" />  

4. 如未安装 bower,则在命令提示符中运行指令，安装 bower:
  
 > `npm install -g bower`

5. 安装完成后，使用命令行进入 `Senparc.Web/wwwroot` 目录中，执行以下命令，初始化 bower 依赖，一直回车到完成：  

> `bower init` 

<img src="http://image.mlkj.ymstudio.xyz/bower-init.png" />  

6. 紧接着执行命令，安装项目样式、js 等依赖，这个执行过程会比较长，请耐心等待... 直到安装完成：  

> `bower install gentelella`  

<img src="http://image.mlkj.ymstudio.xyz/gentelella%E5%BA%93%E5%AE%89%E8%A3%85%E5%AE%8C%E6%88%90.png" />  

7. 完成。

## 待办事项：
- [ ] 前端包管理器要从Bower切换为LibMan
- [ ] 完善 DDD 实践
- [ ] 定制命名空间
- [ ] 发布官网及在线 Demo
- [ ] 发布定制模板生成器（在线）
 - [ ] 可定制命名空间
 - [ ] 可定制默认加载模块

