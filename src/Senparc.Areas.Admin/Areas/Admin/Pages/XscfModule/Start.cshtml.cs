using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.Cache;
using Senparc.CO2NET.Extensions;
using Senparc.CO2NET.Helpers;
using Senparc.CO2NET.Trace;
using Senparc.Scf.AreaBase.Admin.Filters;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Service;
using Senparc.Scf.XscfBase;
using Senparc.Scf.XscfBase.Threads;
using Senparc.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Senparc.Areas.Admin.Areas.Admin.Pages
{
    [IgnoreAuth]
    public class XscfModuleStartModel : BaseAdminPageModel
    {
        public IXscfRegister XscfRegister { get; set; }
        private readonly SysMenuService _sysMenuService;
        public Senparc.Scf.Core.Models.DataBaseModel.XscfModule XscfModule { get; set; }
        public Dictionary<IXscfFunction, List<FunctionParameterInfo>> FunctionParameterInfoCollection { get; set; } = new Dictionary<IXscfFunction, List<FunctionParameterInfo>>();

        XscfModuleService _xscfModuleService;
        IServiceProvider _serviceProvider;

        public List<string> XscfModuleUpdateLog { get; set; }

        /// <summary>
        /// 获取当前模块的已注册线程信息
        /// </summary>
        public IEnumerable<KeyValuePair<ThreadInfo, Thread>> RegisteredThreadInfo { get; set; }

        /// <summary>
        /// 是否必须更新（常规读取失败）
        /// </summary>
        public bool MustUpdate { get; set; }

        public string Msg { get; set; }
        public object Obj { get; set; }

        public XscfModuleStartModel(IServiceProvider serviceProvider, XscfModuleService xscfModuleService, SysMenuService sysMenuService)
        {
            _serviceProvider = serviceProvider;
            _xscfModuleService = xscfModuleService;
            _sysMenuService = sysMenuService;
        }

        public async Task OnGetAsync()
        {
            await Task.CompletedTask;
            //            if (uid.IsNullOrEmpty())
            //            {
            //                throw new Exception("模块编号未提供！");
            //            }


            //            XscfModule = await _xscfModuleService.GetObjectAsync(z => z.Uid == uid).ConfigureAwait(false);

            //            if (XscfModule == null)
            //            {
            //                throw new Exception("模块未添加！");
            //            }

            //            if (!XscfModule.UpdateLog.IsNullOrEmpty())
            //            {
            //                XscfModuleUpdateLog = XscfModule.UpdateLog
            //                    .Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
            //                    .ToList();
            //            }
            //            else
            //            {
            //                XscfModuleUpdateLog = new List<string>();
            //            }

            //            XscfRegister = Senparc.Scf.XscfBase.Register.RegisterList.FirstOrDefault(z => z.Uid == uid);
            //            if (XscfRegister == null)
            //            {
            //                throw new Exception($"模块丢失或未加载（{Senparc.Scf.XscfBase.Register.RegisterList.Count}）！");
            //            }

            //            try
            //            {
            //                foreach (var functionType in XscfRegister.Functions)
            //                {
            //                    var function = _serviceProvider.GetService(functionType) as FunctionBase;//如：Senparc.Xscf.ChangeNamespace.Functions.ChangeNamespace
            //                    FunctionParameterInfoCollection[function] = await function.GetFunctionParameterInfoAsync(_serviceProvider, true);
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                SenparcTrace.SendCustomLog("模块读取失败", @$"模块：{XscfModule.Name} / {XscfModule.MenuName} / {XscfModule.Uid}
            //请尝试更新此模块后刷新页面！");
            //                MustUpdate = true;
            //            }

            //            RegisteredThreadInfo = XscfRegister.RegisteredThreadInfo;
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="toState"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetChangeStateAsync(int id, XscfModules_State toState)
        {
            var module = await _xscfModuleService.GetObjectAsync(z => z.Id == id).ConfigureAwait(false);

            if (module == null)
            {
                throw new Exception("模块未添加！");
            }

            module.UpdateState(toState);
            await _xscfModuleService.SaveObjectAsync(module).ConfigureAwait(false);
            base.SetMessager(MessageType.success, "状态变更成功！");
            return RedirectToPage("Start", new { uid = module.Uid });
        }

        /// <summary>
        /// 提交信息，执行方法
        /// </summary>
        /// <param name="xscfUid"></param>
        /// <param name="xscfFunctionName"></param>
        /// <param name="xscfFunctionParams"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostRunFunctionAsync([FromBody]ExecuteFuncParamDto executeFuncParamDto)
        {
            var xscfRegister = Senparc.Scf.XscfBase.Register.RegisterList.FirstOrDefault(z => z.Uid == executeFuncParamDto.XscfUid);

            if (xscfRegister == null)
            {
                return new JsonResult(new { success = false, msg = "模块未注册！" });
            }

            var xscfModule = await _xscfModuleService.GetObjectAsync(z => z.Uid == xscfRegister.Uid).ConfigureAwait(false);
            if (xscfModule == null)
            {
                return new JsonResult(new { success = false, msg = "当前模块未安装！" });
            }

            if (xscfModule.State != XscfModules_State.开放)
            {
                return new JsonResult(new { success = false, msg = $"当前模块状态为【{xscfModule.State}】,必须为【开放】状态的模块才可执行！\r\n此外，如果您强制执行此方法，也将按照未通过验证的程序集执行，因为您之前安装的版本可能已经被新的程序所覆盖。" });
            }

            FunctionBase function = null;

            foreach (var functionType in xscfRegister.Functions)
            {
                var fun = _serviceProvider.GetService(functionType) as FunctionBase;//如：Senparc.Xscf.ChangeNamespace.Functions.ChangeNamespace
                //var functionParameters = await function.GetFunctionParameterInfoAsync(_serviceProvider, false);
                if (fun.Name == executeFuncParamDto.XscfFunctionName)
                {
                    function = fun;
                    break;
                }
            }

            if (function == null)
            {
                return new JsonResult(new { success = false, msg = "方法未匹配上！" });
            }

            var paras = SerializerHelper.GetObject(executeFuncParamDto.XscfFunctionParams, function.FunctionParameterType) as IFunctionParameter;
            //var paras = function.GenerateParameterInstance();

            var result = function.Run(paras);

            var tempId = "Xscf-FunctionRun-" + Guid.NewGuid().ToString("n");
            //记录日志缓存
            if (!result.Log.IsNullOrEmpty())
            {
                var cache = _serviceProvider.GetObjectCacheStrategyInstance();
                await cache.SetAsync(tempId, result.Log, TimeSpan.FromMinutes(5));//TODO：可设置
            }

            var data = new { success = result.Success, msg = result.Message.HtmlEncode(), log = result.Log, exception = result.Exception?.Message, tempId = tempId };
            return new JsonResult(data);
        }

        /// <summary>
        /// 获取日志
        /// </summary>
        /// <param name="tempId"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetLogAsync(string tempId)
        {
            var cache = _serviceProvider.GetObjectCacheStrategyInstance();
            var log = await cache.GetAsync<string>(tempId);
            if (log == null)
            {
                return Content("日志文件不存在或已下载！");
            }

            await cache.RemoveFromCacheAsync(tempId);

            return File(Encoding.UTF8.GetBytes(log), "text/plain", $"xscf-log-{tempId}.txt");
        }

        /// <summary>
        /// 删除模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var module = await _xscfModuleService.GetObjectAsync(z => z.Id == id).ConfigureAwait(false);

            if (module == null)
            {
                throw new Exception("模块未添加！");
            }

            //删除菜单
            Func<Task> uninstall = async () =>
            {
                //删除菜单
                var menu = await _sysMenuService.GetObjectAsync(z => z.Id == module.MenuId).ConfigureAwait(false);
                if (menu != null)
                {
                    //删除菜单
                    await _sysMenuService.DeleteObjectAsync(menu).ConfigureAwait(false);
                    //更新菜单缓存
                    await _sysMenuService.GetMenuDtoByCacheAsync(true).ConfigureAwait(false);
                }
                await _xscfModuleService.DeleteObjectAsync(module).ConfigureAwait(false);
            };


            //尝试从已加载的模块中执行删除过程
            var register = Senparc.Scf.XscfBase.Register.RegisterList.FirstOrDefault(z => z.Uid == module.Uid);
            if (register == null)
            {
                //直接删除，如dll已经不存在，可能引发此问题，只能在当前系统内直接执行删除
                await uninstall().ConfigureAwait(false);
            }
            else
            {
                await register.UninstallAsync(_serviceProvider, uninstall).ConfigureAwait(false);
            }

            return RedirectToPage("Index");
        }


        /// <summary>
        /// handler=Detail
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetDetailAsync(string uid)
        {
            bool mustUpdate = false;
            if (uid.IsNullOrEmpty())
            {
                throw new Exception("模块编号未提供！");
            }


            Scf.Core.Models.DataBaseModel.XscfModule xscfModule = await _xscfModuleService.GetObjectAsync(z => z.Uid == uid).ConfigureAwait(false);

            if (xscfModule == null)
            {
                throw new Exception("模块未添加！");
            }
            IEnumerable<string> xscfModuleUpdateLog = new List<string>();
            if (!xscfModule.UpdateLog.IsNullOrEmpty())
            {
                xscfModuleUpdateLog = xscfModule.UpdateLog
                    .Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
            }

            IXscfRegister xscfRegister = Senparc.Scf.XscfBase.Register.RegisterList.FirstOrDefault(z => z.Uid == uid);
            if (xscfRegister == null)
            {
                throw new Exception($"模块丢失或未加载（{Senparc.Scf.XscfBase.Register.RegisterList.Count}）！");
            }
            IDictionary<IXscfFunction, List<FunctionParameterInfo>> functionParameterInfoCollection = new Dictionary<IXscfFunction, List<FunctionParameterInfo>>();
            try
            {
                foreach (var functionType in xscfRegister.Functions)
                {
                    var function = _serviceProvider.GetService(functionType) as FunctionBase;//如：Senparc.Xscf.ChangeNamespace.Functions.ChangeNamespace
                    functionParameterInfoCollection[function] = await function.GetFunctionParameterInfoAsync(_serviceProvider, true);
                }
            }
            catch (Exception)
            {
                SenparcTrace.SendCustomLog("模块读取失败", @$"模块：{XscfModule.Name} / {XscfModule.MenuName} / {XscfModule.Uid}
请尝试更新此模块后刷新页面！");
                mustUpdate = true;
            }

            IEnumerable<KeyValuePair<ThreadInfo, Thread>> registeredThreadInfo = xscfRegister.RegisteredThreadInfo;
            return Ok(new
            {
                mustUpdate,
                xscfModule,
                xscfModuleUpdateLog,
                xscfRegister = new
                {
                    AreaHomeUrl = xscfRegister.GetAreaHomeUrl(),
                    xscfRegister.MenuName,
                    xscfRegister.Icon,
                    xscfRegister.Version,
                    xscfRegister.Uid,
                    areaPageMenuItems = (xscfRegister as Scf.Core.Areas.IAreaRegister)?.AareaPageMenuItems ?? new List<Scf.Core.Areas.AreaPageMenuItem>(),
                    Interfaces = xscfRegister.GetType().GetInterfaces().Select(_ => _.Name),
                    FunctionCount = xscfRegister.Functions.Count,
                    registeredThreadInfo = xscfRegister.RegisteredThreadInfo.Select(_ => new
                    {
                        kye = new
                        {
                            _.Key.Name,
                            _.Key.StoryHtml
                        },
                        Value = new
                        {
                            _.Value.IsAlive,
                            IsBackground = _.Value.IsAlive ? new bool?(_.Value.IsBackground) : null,
                            ThreadState = _.Value.IsAlive ? new ThreadState?(_.Value.ThreadState) : null,
                            ThreadStateStr = _.Value.IsAlive ? _.Value.ThreadState.ToString() : null
                        }
                    })
                },
                functionParameterInfoCollection = functionParameterInfoCollection
                .Select(_ => new
                {
                    Key = new
                    {
                        _.Key.Name,
                        _.Key.Description
                    },
                    _.Value
                }),
                registeredThreadInfo = registeredThreadInfo.Select(_ => new
                {
                    kye = new
                    {
                        _.Key.Name,
                        _.Key.StoryHtml
                    },
                    Value = new
                    {
                        _.Value.IsAlive,
                        IsBackground = _.Value.IsAlive ? new bool?(_.Value.IsBackground) : null,
                        ThreadState = _.Value.IsAlive ? new ThreadState?(_.Value.ThreadState) : null,
                        ThreadStateStr = _.Value.IsAlive ? _.Value.ThreadState.ToString() : null
                    }
                })
            });
        }

        /// <summary>
        /// handler=ChangeStateAjax
        /// </summary>
        /// <param name="id"></param>
        /// <param name="toState"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetChangeStateAjaxAsync(int id, XscfModules_State toState)
        {
            var module = await _xscfModuleService.GetObjectAsync(z => z.Id == id).ConfigureAwait(false);

            if (module == null)
            {
                throw new Exception("模块未添加！");
            }

            module.UpdateState(toState);
            await _xscfModuleService.SaveObjectAsync(module).ConfigureAwait(false);
            return Ok(true);
        }
    }

    public class ExecuteFuncParamDto
    {
        [Required]
        public string XscfUid { get; set; }
        [Required]
        public string XscfFunctionName { get; set; }
        [Required]
        public string XscfFunctionParams { get; set; }
    }
}
