# Xscf 模块开发

SCF 底层支持库官方 Nuget 包源码

## Xscf 单功能执行模块开发

> 1.新建一个Dotnet Core的Class Library项目

![Image text](/start/images/create_dotnet_core_class_library.png)

> 2.输入Class Libray的名称,点击创建

![Image text](/start/images/input_dotnet_core_class_library_name.png)

> 3.建立Register.cs

![Image text](/start/images/create_register_and_functions_folder.png)

> 4.配置Register中的必要内容

|    名称     |    说明         |
|--------------|-----------------|
|  Name       |  模块名称
|  Uid  |  全球唯一码,最好使用工具生成
|  Version  |  模块版本号(更新版本靠它识别)
|  MenuName  |  安装到SCF中以后菜单中显示的名称
|  Icon  |  显示在菜单旁边的[字体图标](https://colorlib.com/polygon/gentelella/icons.html)
|  Description  |  模块的说明文字,安装前可以根据此说明了解模块的具体功能

![Image text](/start/images/register_content.png)

> 5.创建自定义方法类

![Image text](/start/images/create_functions_class_library.png)

> 6.完成自定义方法

    public class LaunchApp_Parameters : IFunctionParameter
    {
        [Required]
        [MaxLength(300)]
        [Description("自定义路径||文件名")]
        public string FilePath { get; set; }
    }

    //注意：Name 必须在单个 Xscf 模块中唯一！
    public override string Name => "应用程序";

    public override string Description => "启动所有的程序";

    public override Type FunctionParameterType => typeof(LaunchApp_Parameters);

    public LaunchApp(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    /// <summary>
    /// 运行
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public override FunctionResult Run(IFunctionParameter param)
    {
        var typeParam = param as LaunchApp_Parameters;

        FunctionResult result = new FunctionResult()
        {
            Success = true
        };

        StringBuilder sb = new StringBuilder();
        base.RecordLog(sb, "开始运行 LaunchApp");

        StartApp(typeParam.FilePath);

        //sb.AppendLine($"LaunchPath{typeParam.LaunchPath}");
        sb.AppendLine($"FilePath{typeParam.FilePath}");

        result.Log = sb.ToString();
        result.Message = "操作成功！";
        return result;
    }

> 7.在Register中注册自定义的方法类

![Image text](/start/images/register_add_functions.png)

> 8.发布Nuget,详细步骤到发布Nuget

## Xscf 自定义带页面功能模块开发

> 1.新建一个DotnetCore Class Library项目，输入项目名称

![Image text](/start/images/page_create_dotnet_core_class_library.png)

![Image text](/start/images/page_create_dotnet_core_class_library_input_name.png)

>> 1.1 目录名称如下

![Image text](/start/images/page_folder_struct.png)

>> 1.2 设置项目支持RazorPage功能

![Image text](/start/images/page_project_support_razor.png)

> 2.Senparc.Core.Models.DataBaseModel新建模型Color类

    using Senparc.Xscf.ExtensionAreaTemplate.Models.DatabaseModel.Dto;
    using Senparc.Scf.Core.Models;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;

    namespace Senparc.Xscf.ExtensionAreaTemplate
    {
        /// <summary>
        /// Color 实体类
        /// </summary>
        [Table(Register.DATABASE_PREFIX + nameof(Color))]//必须添加前缀，防止全系统中发生冲突
        [Serializable]
        public class Color : EntityBase<int>
        {
            /// <summary>
            /// 颜色码，0-255
            /// </summary>
            public int Red { get; private set; }
            /// <summary>
            /// 颜色码，0-255
            /// </summary>
            public int Green { get; private set; }

            /// <summary>
            /// 颜色码，0-255
            /// </summary>
            public int Blue { get; private set; }

            /// <summary>
            /// 附加列，测试多次数据库 Migrate
            /// </summary>
            public string AdditionNote { get; private set; }

            private Color() { }

            public Color(int red, int green, int blue)
            {
                if (red < 0 || green < 0 || blue < 0)
                {
                    Random();//随机
                }
                else
                {
                    Red = red;
                    Green = green;
                    Blue = blue;
                }
            }

            public Color(ColorDto colorDto)
            {
                Red = colorDto.Red;
                Green = colorDto.Green;
                Blue = colorDto.Blue;
            }

            public void Random()
            {
                //随机产生颜色代码
                var radom = new Random(SystemTime.Now.Second);
                Func<int> getRadomColorCode = () => radom.Next(0, 255);
                Red = getRadomColorCode();
                Green = getRadomColorCode();
                Blue = getRadomColorCode();
            }

            public void Brighten()
            {
                Red = Math.Min(255, Red + 10);
                Green = Math.Min(255, Green + 10);
                Blue = Math.Min(255, Blue + 10);
            }

            public void Darken()
            {
                Red = Math.Max(0, Red - 10);
                Green = Math.Max(0, Green - 10);
                Blue = Math.Max(0, Blue - 10);
            }
        }
    }

> 3.Senparc.Core.Models.DataBaseModel新建ColorDto类

![Image text](/start/images/page_create_dto_file_path.png)

    using Senparc.Scf.Core.Models;
    using Senparc.Scf.Core.Models.DataBaseModel;
    using System;
    using System.Collections.Generic;
    using System.Text;

    namespace Senparc.Xscf.ExtensionAreaTemplate.Models.DatabaseModel.Dto
    {
        public class ColorDto : DtoBase
        {
            /// <summary>
            /// 颜色码，0-255
            /// </summary>
            public int Red { get; private set; }
            /// <summary>
            /// 颜色码，0-255
            /// </summary>
            public int Green { get; private set; }

            /// <summary>
            /// 颜色码，0-255
            /// </summary>
            public int Blue { get; private set; }

            private ColorDto() { }
        }
    }

> 4.AutoMapperConfigs增加以下代码

![Image text](/start/images/page_create_mapping_file_path.png)

    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Senparc.Scf.Core.Models.DataBaseModel;
    using Senparc.Scf.XscfBase.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Text;

    namespace Senparc.Xscf.ExtensionAreaTemplate.Models
    {
        [XscfAutoConfigurationMapping]
        public class AreaTemplate_ColorConfigurationMapping : ConfigurationMappingWithIdBase<Color, int>
        {
            public override void Configure(EntityTypeBuilder<Color> builder)
            {
                builder.Property(e => e.Red).IsRequired();
                builder.Property(e => e.Green).IsRequired();
                builder.Property(e => e.Blue).IsRequired();
            }
        }
    }

> 5.Senparc.Areas.Admin.Areas.Admin.Pages下建立页面，更改IndexModel继承为BaseAdminPageModel

![Image text](/start/images/page_create_pages_file_path.png)

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using Senparc.Xscf.ExtensionAreaTemplate.Services;
    using Senparc.Scf.Core.Models.DataBaseModel;
    using Senparc.Scf.Service;
    using Senparc.Scf.XscfBase;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Senparc.Xscf.ExtensionAreaTemplate.Models.DatabaseModel.Dto;
    using Senparc.Scf.Core.Enums;
    using Senparc.Xscf.ExtensionAreaTemplate.Models;

    namespace Senparc.Xscf.ExtensionAreaTemplate.Areas.MyApp.Pages
    {
        public class MyHomePage : Senparc.Scf.AreaBase.Admin.AdminXscfModulePageModelBase
        {
            public ColorDto ColorDto { get; set; }

            private readonly ColorService _colorService;
            private readonly IServiceProvider _serviceProvider;
            public MyHomePage(IServiceProvider serviceProvider, ColorService colorService, Lazy<XscfModuleService> xscfModuleService)
                : base(xscfModuleService)
            {
                _colorService = colorService;
                _serviceProvider = serviceProvider;
            }

            public Task OnGetAsync()
            {
                var color = _colorService.GetObject(z => true, z => z.Id, OrderingType.Descending);
                ColorDto = _colorService.Mapper.Map<ColorDto>(color);
                return Task.CompletedTask;
            }

            public async Task OnGetBrightenAsync()
            {
                ColorDto = await _colorService.Brighten().ConfigureAwait(false);
            }

            public async Task OnGetDarkenAsync()
            {
                ColorDto = await _colorService.Darken().ConfigureAwait(false);
            }
            public async Task OnGetRandomAsync()
            {
                ColorDto = await _colorService.Random().ConfigureAwait(false);
            }
        }
    }


> 6.增加Service类

![Image text](/start/images/page_create_service_file_path.png)

    using Senparc.Xscf.ExtensionAreaTemplate.Models.DatabaseModel.Dto;
    using Senparc.Scf.Core.Enums;
    using Senparc.Scf.Repository;
    using Senparc.Scf.Service;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    namespace Senparc.Xscf.ExtensionAreaTemplate.Services
    {
        public class ColorService : ServiceBase<Color>
        {
            public ColorService(IRepositoryBase<Color> repo, IServiceProvider serviceProvider)
                : base(repo, serviceProvider)
            {
            }

            public async Task<ColorDto> CreateNewColor()
            {
                Color color = new Color(-1, -1, -1);
                await base.SaveObjectAsync(color).ConfigureAwait(false);
                ColorDto colorDto = base.Mapper.Map<ColorDto>(color);
                return colorDto;
            }

            public async Task<ColorDto> Brighten()
            {
                //TODO:异步方法需要添加排序功能
                var obj = this.GetObject(z => true, z => z.Id, OrderingType.Descending);
                obj.Brighten();
                await base.SaveObjectAsync(obj).ConfigureAwait(false);
                return base.Mapper.Map<ColorDto>(obj);
            }

            public async Task<ColorDto> Darken()
            {
                //TODO:异步方法需要添加排序功能
                var obj = this.GetObject(z => true, z => z.Id, OrderingType.Descending);
                obj.Darken();
                await base.SaveObjectAsync(obj).ConfigureAwait(false);
                return base.Mapper.Map<ColorDto>(obj);
            }

            public async Task<ColorDto> Random()
            {
                //TODO:异步方法需要添加排序功能
                var obj = this.GetObject(z => true, z => z.Id, OrderingType.Descending);
                obj.Random();
                await base.SaveObjectAsync(obj).ConfigureAwait(false);
                return base.Mapper.Map<ColorDto>(obj);
            }

            //TODO: 更多业务方法可以写到这里
        }
    }

> 7.Senparc.Core.Models中SenparcEntities里面增加

![Image text](/start/images/page_create_entity_file_path.png)

    using Microsoft.EntityFrameworkCore;
    using Senparc.Scf.Core.Models;
    using Senparc.Scf.XscfBase;
    using Senparc.Scf.XscfBase.Database;
    using System;
    using System.Collections.Generic;
    using System.Text;

    namespace Senparc.Xscf.ExtensionAreaTemplate.Models.DatabaseModel
    {
        public class MySenparcEntities : XscfDatabaseDbContext
        {
            public override IXscfDatabase XscfDatabaseRegister => new Register();
            public MySenparcEntities(DbContextOptions<MySenparcEntities> dbContextOptions) : base(dbContextOptions)
            {
            }

            public DbSet<Color> Colors { get; set; }


            //如无特殊需需要，OnModelCreating 方法可以不用写，已经在 Register 中要求注册
            //protected override void OnModelCreating(ModelBuilder modelBuilder)
            //{
            //}
        }
    }

> 8.在Senparc.Web下执行

![Image text](/start/images/page_dbcontext.png)

    using Senparc.Xscf.ExtensionAreaTemplate.Models.DatabaseModel;
    using Senparc.Scf.XscfBase.Database;
    using System;
    using System.IO;

    namespace Senparc.Xscf.ExtensionAreaTemplate
    {
        /// <summary>
        /// 设计时 DbContext 创建（仅在开发时创建 Code-First 的数据库 Migration 使用，在生产环境不会执行）
        /// </summary>
        public class SenparcDbContextFactory : SenparcDesignTimeDbContextFactoryBase<MySenparcEntities, Register>
        {
            /// <summary>
            /// 用于寻找 App_Data 文件夹，从而找到数据库连接字符串配置信息
            /// </summary>
            public override string RootDictionaryPath => Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"/*项目根目录*/, "..\\Senparc.Web"/*找到 Web目录，以获取统一的数据库连接字符串配置*/);
        }
    }


> 9.根据实际的Entities的名称，添加数据库更新命令

![Image text](/start/images/page_entity_name.png)

    add-migration Xscf_AreaTemplate_Init2 -Context MySenparcEntities


## 欢迎贡献代码！