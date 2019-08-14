using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Senparc.Core.WorkContext.Provider
{
    public class AdminWorkContextProvider : IAdminWorkContextProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminWorkContextProvider(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        public AdminWorkContext GetAdminWorkContext()
        {
            AdminWorkContext adminWorkContext = new AdminWorkContext();

            adminWorkContext.UserName = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Name)?.Value;
            bool isConvertSucess = int.TryParse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier)?.Value, out int convertId);
            if (isConvertSucess)
            {
                adminWorkContext.AdminUserId = convertId;
            }
            adminWorkContext.RoleCodes = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Role)?.Value?.Split(",").ToList() ?? new List<string>();
            return adminWorkContext;
        }
    }
}
