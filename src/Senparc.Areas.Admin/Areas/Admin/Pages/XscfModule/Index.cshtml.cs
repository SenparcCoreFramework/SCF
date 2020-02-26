using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.Extensions;
using Senparc.Core.Models.DataBaseModel;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Core.Models.DataBaseModel;
using Senparc.Scf.Service;
using Senparc.Scf.XscfBase;
using Senparc.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Senparc.Areas.Admin.Areas.Admin.Pages
{
    public class XscfModuleIndexModel : BaseAdminPageModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly XscfModuleService _xscfModuleService;
        private readonly SysMenuService _sysMenuService;

        public XscfModuleIndexModel(IServiceProvider serviceProvider, XscfModuleService xscfModuleService, SysMenuService sysMenuService)
        {
            CurrentMenu = "XscfModule";

            this._serviceProvider = serviceProvider;
            this._xscfModuleService = xscfModuleService;
            this._sysMenuService = sysMenuService;
        }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// ���ݿ��Ѵ��XscfModules
        /// </summary>
        public PagedList<XscfModule> XscfModules { get; set; }
        public List<IXscfRegister> NewXscfRegisters { get; set; }

        private void LoadNewXscfRegisters(PagedList<XscfModule> xscfModules)
        {
            NewXscfRegisters = Senparc.Scf.XscfBase.Register.RegisterList.Where(z => !xscfModules.Exists(m => m.Uid == z.Uid && m.Version == z.Version)).ToList() ?? new List<IXscfRegister>();
        }

        public async Task OnGetAsync()
        {
            XscfModules = await _xscfModuleService.GetObjectListAsync(PageIndex, 10, _ => true, _ => _.AddTime, Scf.Core.Enums.OrderingType.Descending);
            LoadNewXscfRegisters(XscfModules);
        }

        /// <summary>
        /// ɨ����ģ��
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetScanAsync(string uid)
        {
            if (uid.IsNullOrEmpty())
            {
                throw new Exception("ģ�鲻���ڣ�");
            }

            var xscfRegister = Senparc.Scf.XscfBase.Register.RegisterList.FirstOrDefault(z => z.Uid == uid);
            if (xscfRegister == null)
            {
                throw new Exception("ģ�鲻���ڣ�");
            }

            var xscfModule = await _xscfModuleService.GetObjectAsync(z => z.Uid == xscfRegister.Uid && z.Version == xscfRegister.Version);
            if (xscfModule != null)
            {
                throw new Exception("��ͬ�汾ģ���Ѱ�װ�������ظ���װ��");
            }

            XscfModules = await _xscfModuleService.GetObjectListAsync(PageIndex, 10, _ => true, _ => _.AddTime, Scf.Core.Enums.OrderingType.Descending).ConfigureAwait(false);

            var xscfModuleDto = XscfModules.Select(z => _xscfModuleService.Mapper.Map<CreateOrUpdate_XscfModuleDto>(z)).ToList();

            //����ģ��ɨ��
            var result = await Senparc.Scf.XscfBase.Register.ScanAndInstall(xscfModuleDto, _serviceProvider, async (register, installOrUpdate) =>
              {
                  var sysMenuService = _serviceProvider.GetService<SysMenuService>();

                  var topMenu = await sysMenuService.GetObjectAsync(z => z.MenuName == "��չģ��").ConfigureAwait(false);
                  var currentMenu = await sysMenuService.GetObjectAsync(z => z.ParentId == topMenu.Id && z.MenuName == register.MenuName).ConfigureAwait(false);//TODO: menu ����Ҫ��һ������Uid����չ����
                  SysMenuDto menuDto;

                  if (installOrUpdate == InstallOrUpdate.Update && currentMenu != null)
                  {
                      //���²˵�
                      menuDto = sysMenuService.Mapper.Map<SysMenuDto>(currentMenu);
                      menuDto.MenuName = register.MenuName;//���²˵�����
                  }
                  else
                  {
                      //�½��˵�
                      menuDto = new SysMenuDto(true, null, register.MenuName, topMenu.Id, $"/Admin/XscfModule/Start/?uid={register.Uid}", "fa fa-bars", 5, true, null);
                  }

                  await sysMenuService.CreateOrUpdateAsync(menuDto).ConfigureAwait(false);

                  if (installOrUpdate == InstallOrUpdate.Install)
                  {
                      //���²˵���Ϣ
                      var updateMenuDto = new UpdateMenuId_XscfModuleDto(register.Uid, menuDto.Id);
                      await _xscfModuleService.UpdateMenuId(updateMenuDto).ConfigureAwait(false);
                  }
              }).ConfigureAwait(false);

            base.SetMessager(Scf.Core.Enums.MessageType.info, result, true);

            //if (backpage=="Start")
            return RedirectToPage("Start", new { uid = uid });//ʼ�յ�����ҳ
            //return RedirectToPage("Index");
        }
    }
}