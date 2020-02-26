using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.Extensions;
using Senparc.CO2NET.Helpers;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Service;
using Senparc.Scf.XscfBase;
using Senparc.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Senparc.Areas.Admin.Areas.Admin.Pages
{
    public class XscfModuleStartModel : BaseAdminPageModel
    {
        public IXscfRegister XscfRegister { get; set; }
        private readonly SysMenuService _sysMenuService;
        public Senparc.Scf.Core.Models.DataBaseModel.XscfModule XscfModule { get; set; }
        public Dictionary<IXscfFunction, List<FunctionParammeterInfo>> FunctionParammeterInfoCollection { get; set; } = new Dictionary<IXscfFunction, List<FunctionParammeterInfo>>();

        XscfModuleService _xscfModuleService;
        IServiceProvider _serviceProvider;

        public List<string> XscfModuleUpdateLog { get; set; }

        public string Msg { get; set; }
        public object Obj { get; set; }

        public XscfModuleStartModel(IServiceProvider serviceProvider, XscfModuleService xscfModuleService, SysMenuService sysMenuService)
        {
            _serviceProvider = serviceProvider;
            _xscfModuleService = xscfModuleService;
            _sysMenuService = sysMenuService;
        }

        public async Task OnGetAsync(string uid)
        {
            if (uid.IsNullOrEmpty())
            {
                throw new Exception("ģ����δ�ṩ��");
            }


            XscfModule = await _xscfModuleService.GetObjectAsync(z => z.Uid == uid).ConfigureAwait(false);

            if (XscfModule == null)
            {
                throw new Exception("ģ��δ��ӣ�");
            }

            if (!XscfModule.UpdateLog.IsNullOrEmpty())
            {
                XscfModuleUpdateLog = XscfModule.UpdateLog
                    .Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
            }
            else
            {
                XscfModuleUpdateLog = new List<string>();
            }

            XscfRegister = Senparc.Scf.XscfBase.Register.RegisterList.FirstOrDefault(z => z.Uid == uid);
            if (XscfRegister == null)
            {
                throw new Exception($"ģ�鶪ʧ��δ���أ�{Senparc.Scf.XscfBase.Register.RegisterList.Count}����");
            }

            foreach (var functionType in XscfRegister.Functions)
            {
                var function = _serviceProvider.GetService(functionType) as FunctionBase;//�磺Senparc.Xscf.ChangeNamespace.Functions.ChangeNamespace
                FunctionParammeterInfoCollection[function] = function.GetFunctionParammeterInfo().ToList();
            }
        }

        /// <summary>
        /// ����״̬
        /// </summary>
        /// <param name="id"></param>
        /// <param name="toState"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetChangeStateAsync(int id, XscfModules_State toState)
        {
            var module = await _xscfModuleService.GetObjectAsync(z => z.Id == id).ConfigureAwait(false);

            if (module == null)
            {
                throw new Exception("ģ��δ��ӣ�");
            }

            module.UpdateState(toState);
            await _xscfModuleService.SaveObjectAsync(module).ConfigureAwait(false);
            base.SetMessager(MessageType.success, "״̬����ɹ���");
            return RedirectToPage("Start", new { uid = module.Uid });
        }

        public async Task<IActionResult> OnPostRunFunctionAsync(string xscfUid, string xscfFunctionName, string xscfFunctionParams)
        {
            var xscfRegister = Senparc.Scf.XscfBase.Register.RegisterList.FirstOrDefault(z => z.Uid == xscfUid);

            if (xscfRegister == null)
            {
                return new JsonResult(new { success = false, msg = "ģ��δע�ᣡ" });
            }

            var xscfModule = await _xscfModuleService.GetObjectAsync(z => z.Uid == xscfRegister.Uid).ConfigureAwait(false);
            if (xscfModule == null)
            {
                return new JsonResult(new { success = false, msg = "��ǰģ��δ��װ��" });
            }

            if (xscfModule.State != XscfModules_State.����)
            {
                return new JsonResult(new { success = false, msg = $"��ǰģ��״̬Ϊ��{xscfModule.State}��,����Ϊ�����š�״̬��ģ��ſ�ִ�У�\r\n���⣬�����ǿ��ִ�д˷�����Ҳ������δͨ����֤�ĳ���ִ�У���Ϊ��֮ǰ��װ�İ汾�����Ѿ����µĳ��������ǡ�" });
            }

            FunctionBase function = null;

            foreach (var functionType in xscfRegister.Functions)
            {
                var fun = _serviceProvider.GetService(functionType) as FunctionBase;//�磺Senparc.Xscf.ChangeNamespace.Functions.ChangeNamespace
                var functionParameters = fun.GetFunctionParammeterInfo().ToList();
                if (fun.Name == xscfFunctionName)
                {
                    function = fun;
                    break;
                }
            }

            if (function == null)
            {
                return new JsonResult(new { success = false, msg = "����δƥ���ϣ�" });
            }

            var paras = SerializerHelper.GetObject(xscfFunctionParams, function.FunctionParameterType) as IFunctionParameter;
            //var paras = function.GenerateParameterInstance();

            try
            {
                var result = function.Run(paras);
                var data = new { success = true, msg = result };
                return new JsonResult(data);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, msg = $"�����쳣����ϸ��{ex.Message}" });
            }
        }

        /// <summary>
        /// ɾ��ģ��
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var module = await _xscfModuleService.GetObjectAsync(z => z.Id == id).ConfigureAwait(false);

            if (module == null)
            {
                throw new Exception("ģ��δ��ӣ�");
            }

            //ɾ���˵�
            Func<Task> uninstall = async () =>
            {
                //ɾ���˵�
                var menu = await _sysMenuService.GetObjectAsync(z => z.MenuName == module.MenuName).ConfigureAwait(false);
                if (menu != null)
                {
                    await _sysMenuService.DeleteObjectAsync(menu).ConfigureAwait(false);
                }
                await _xscfModuleService.DeleteObjectAsync(module).ConfigureAwait(false);
            };


            //���Դ��Ѽ��ص�ģ����ִ��ɾ������
            var register = Senparc.Scf.XscfBase.Register.RegisterList.FirstOrDefault(z => z.Uid == module.Uid);
            if (register == null)
            {
                //ֱ��ɾ������dll�Ѿ������ڣ��������������⣬ֻ���ڵ�ǰϵͳ��ֱ��ִ��ɾ��
                await uninstall().ConfigureAwait(false);
            }
            else
            {
                await register.UninstallAsync(uninstall).ConfigureAwait(false);
            }

            return RedirectToPage("Index");
        }
    }
}
