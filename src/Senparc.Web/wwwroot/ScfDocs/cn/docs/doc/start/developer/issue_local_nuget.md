## 发布本地Nuget包

> 1.点开项目属性

![Image text](/start/images/project_properties.png)

> 2.项目包设置发布选项,勾选生成时创建nuget文件

![Image text](/start/images/build_general_nuget_file.png)

> 3.编辑项目文件

![Image text](/start/images/edit_project_file.png)

> 4.编辑需要生成Nuget文件的条件及必要参数

    <PropertyGroup>
      <TargetFramework>netcoreapp3.1</TargetFramework>
      <Version>0.0.1.6-beta1</Version>
      <AssemblyName>Senparc.Xscf.Application</AssemblyName>
      <RootNamespace>Senparc.Xscf.Application</RootNamespace>
      <GeneratePackageOnBuild Condition=" '$(Configuration)' == 'Release' ">true</GeneratePackageOnBuild>
      <Description>Senparc.Xscf.Application 此模块提供给开发者一个可以运行某些程序检测及执行的模块</Description>
      <Copyright>SenparcCoreFramework</Copyright>
      <PackageTags>SenparcCoreFramework,SCF,Senparc.Xscf.Application</PackageTags>
      <Authors>SenparcCoreFramework</Authors>
      <Owners>SenparcCoreFramework</Owners>
      <PackageLicenseUrl>https://github.com/SenparcCoreFramework/ScfPackageSources/blob/master/LICENSE</PackageLicenseUrl>
      <Title>XSCF 应用程序模块</Title>
      <ProjectUrl> https://github.com/SenparcCoreFramework/SCF</ProjectUrl>
      <PackageProjectUrl>https://github.com/SenparcCoreFramework/ScfPackageSources</PackageProjectUrl>
      <PackageIconUrl>http://sdk.weixin.senparc.com/Images/logo-square-scf.jpg</PackageIconUrl>
      <PackageReleaseNotes>
        v0.1 创世
      </PackageReleaseNotes>
      <RepositoryUrl> https://github.com/SenparcCoreFramework/ScfPackageSources</RepositoryUrl>
      <Configurations>Debug;Release;Test</Configurations>
    </PropertyGroup>
    <PropertyGroup Condition=" '$(Configuration)' == 'Release' ">
      <OutputPath>..\..\BuildOutPut</OutputPath>
      <DocumentationFile>..\..\BuildOutPut\Senparc.Xscf.Application.XML</DocumentationFile>
      <DefineConstants>$(DefineConstants);RELEASE</DefineConstants>
      <Optimize>true</Optimize>
      <DebugType>pdbonly</DebugType>
      <ErrorReport>prompt</ErrorReport>
      <CodeAnalysisRuleSet>MinimumRecommendedRules.ruleset</CodeAnalysisRuleSet>
    </PropertyGroup>

> 5.重新生成项目

![Image text](/start/images/project_build.png)

> 6.找到生成的Nuget包(这里用的是Debug环境,所以生成的在Debug目录下)

![Image text](/start/images/general_nuget_file_success.png)

> 7.Copy本地Nuget包到指定的文件目录中(方便引用)

![Image text](/start/images/copy_to_local_nuget_source.png)

## 引用本地的Nuget源

> 1.进入源设置的方法有2种

>> 1-1.点击工具栏Tools->Options->Nuget Package Manager

![Image text](/start/images/enter_nuget_source_setting_1.png)

>> 1-2.点击项目右键

![Image text](/start/images/enter_nuget_source_setting_2_1.png)

![Image text](/start/images/enter_nuget_source_setting_2_2.png)

![Image text](/start/images/enter_nuget_source_setting_1.png)

> 2.添加nuget源

![Image text](/start/images/click_add_nuget_button.png)

![Image text](/start/images/prefect_nuget_infomation.png)

> 3.选择本地nuget源

![Image text](/start/images/select_nuget_source.png)

> 4.引入本地nuget源

![Image text](/start/images/install_nuget_source.png)

![Image text](/start/images/install_finished.png)

> 5.重新生成项目

> 6.效果展示

![Image text](/start/images/show_new_module_can_install.png)

![Image text](/start/images/enable_module.png)

![Image text](/start/images/exec_module_function.png)