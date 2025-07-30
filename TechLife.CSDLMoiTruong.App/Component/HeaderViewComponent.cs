using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechLife.CSDLMoiTruong.App.ApiClients;
using TechLife.CSDLMoiTruong.App.Extensions;
using TechLife.CSDLMoiTruong.App.Extensions.Authorizations;
using TechLife.CSDLMoiTruong.App.Models;
using TechLife.CSDLMoiTruong.Common.Enums;

namespace TechLife.CSDLMoiTruong.App.Component
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly IUserApiClient _userService;

        public HeaderViewComponent(IUserApiClient userService)
        {
            _userService = userService;
        }

        private new CustomClaimsPrincipal User => new CustomClaimsPrincipal(_userService, (ClaimsPrincipal)base.User);

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var modules = new List<MenuViewModel>();
            var menus = new List<MenuViewModel>();

            modules.Add(new MenuViewModel() { Name = StringEnum.GetStringValue(HeThong.home), Module = HeThong.home.ToString(), Url = "/", Icon = "fa-home" });
            

            string path = Request.Path.Value.Replace('/', ' ').Trim();

            var module = modules.Where(v => !string.IsNullOrWhiteSpace(v.Url) && v.Url.Replace('/', ' ').Trim().Equals(path, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            var menu = menus.Where(v => !string.IsNullOrWhiteSpace(v.Url) && v.Url.Replace('/', ' ').Trim().Equals(path, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            modules = modules.Select(v => new MenuViewModel()
            {
                Id = v.Id,
                Icon = v.Icon,
                Name = v.Name,
                Url = v.Url,
                Module = v.Module,
                IsActive = !string.IsNullOrWhiteSpace(v.Url) && v.Url == module?.Url
            }).ToList();

            menus = menus.Select(v => new MenuViewModel()
            {
                Id = v.Id,
                Icon = v.Icon,
                Name = v.Name,
                Url = v.Url,
                Module = v.Module,
                IsActive = !string.IsNullOrWhiteSpace(v.Url) && v.Url == menu?.Url
            }).ToList();

            var navigationVm = new NavigationViewModel()
            {
                UserInfo = Request.GetUser(),
                ReturnUrl = Request.GetRawUrl(),
                Modules = modules,
                Menus = menus,
            };

            return await Task.Run(() =>
                 View("_Header", navigationVm)
            );
        }
    }
}