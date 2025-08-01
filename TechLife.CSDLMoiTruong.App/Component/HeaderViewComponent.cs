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
            var modules = new List<MenuViewModel>
            {
                new MenuViewModel { Name = StringEnum.GetStringValue(HeThong.home), Module = HeThong.home.ToString(), Url = "/", Icon = "fa-home" },
                new MenuViewModel { Name = StringEnum.GetStringValue(HeThong.sinhvatgayhai), Module = HeThong.sinhvatgayhai.ToString(), Icon = "fa-bug" },
                new MenuViewModel { Name = StringEnum.GetStringValue(HeThong.thoitietvasinhtruong), Module = HeThong.thoitietvasinhtruong.ToString(), Icon = "fa-cloud" },
                new MenuViewModel { Name = StringEnum.GetStringValue(HeThong.chatluongsanpham), Module = HeThong.chatluongsanpham.ToString(), Icon = "fa-star" },
                new MenuViewModel { Name = StringEnum.GetStringValue(HeThong.cosogiongcaytrong), Module = HeThong.cosogiongcaytrong.ToString(), Icon = "fa-leaf" }
            };

            var menus = new List<MenuViewModel>();
            if (User.IsInRole("manage_system"))
            {
                menus.AddRange(new List<MenuViewModel>
                {
                    new MenuViewModel { Name = "Loại cây trồng", Url = "/LoaiCayTrong", Icon = "fa-leaf", Module = HeThong.sinhvatgayhai.ToString() },
                    new MenuViewModel { Name = "Sinh vật gây hại", Url = "/SinhVatGayHai", Icon = "fa-bug", Module = HeThong.sinhvatgayhai.ToString() },
                    new MenuViewModel { Name = "Địa bàn ảnh hưởng", Url = "/DiaBanAnhHuong", Icon = "fa-map", Module = HeThong.sinhvatgayhai.ToString() },
                    new MenuViewModel { Name = "Số liệu SVGH", Url = "/SoLieuSinhVatGayHai", Icon = "fa-line-chart", Module = HeThong.sinhvatgayhai.ToString() }
                });

                        menus.AddRange(new List<MenuViewModel>
                {
                    new MenuViewModel { Name = "Thời tiết", Url = "/ThoiTiet", Icon = "fa-cloud", Module = HeThong.thoitietvasinhtruong.ToString() },
                    new MenuViewModel { Name = "Số liệu sinh trưởng", Url = "/SoLieuSinhTruong", Icon = "fa-leaf", Module = HeThong.thoitietvasinhtruong.ToString() }
                });

                        menus.AddRange(new List<MenuViewModel>
                {
                    new MenuViewModel { Name = "Đơn vị công bố", Url = "/DonViCongBo", Icon = "fa-building", Module = HeThong.chatluongsanpham.ToString() },
                    new MenuViewModel { Name = "Sản phẩm công bố", Url = "/SanPhamCongBo", Icon = "fa-cube", Module = HeThong.chatluongsanpham.ToString() }
                });

                        menus.AddRange(new List<MenuViewModel>
                {
                    //new MenuViewModel { Name = "Giống cây trồng", Url = "/GiongCayTrong", Icon = "fa-leaf", Module = HeThong.cosogiongcaytrong.ToString() },
                    new MenuViewModel { Name = "Cơ sở sản xuất giống", Url = "/CoSoGiong", Icon = "fa-building", Module = HeThong.cosogiongcaytrong.ToString() }
                });

                        menus.AddRange(new List<MenuViewModel>
                {
                    new MenuViewModel { Name = "Tài khoản", Url = "/TaiKhoan", Icon = "fa-users", Module = HeThong.hethong.ToString() },
                    new MenuViewModel { Name = "Quyền", Url = "/Quyen", Icon = "fa-cogs", Module = HeThong.hethong.ToString() },
                    new MenuViewModel { Name = "Nhật ký hệ thống", Url = "/NhatKyHeThong", Icon = "fa-history", Module = HeThong.hethong.ToString() }
                });
            }

            string path = Request.Path.Value.Replace('/', ' ').Trim();

            var module = modules.FirstOrDefault(v => !string.IsNullOrWhiteSpace(v.Url) && v.Url.Replace('/', ' ').Trim().Equals(path, StringComparison.OrdinalIgnoreCase));
            var menu = menus.FirstOrDefault(v => !string.IsNullOrWhiteSpace(v.Url) && v.Url.Replace('/', ' ').Trim().Equals(path, StringComparison.OrdinalIgnoreCase));

            modules = modules.Select(v => new MenuViewModel
            {
                Id = v.Id,
                Icon = v.Icon,
                Name = v.Name,
                Url = v.Url,
                Module = v.Module,
                IsActive = !string.IsNullOrWhiteSpace(v.Url) && v.Url == module?.Url
            }).ToList();

            menus = menus.Select(v => new MenuViewModel
            {
                Id = v.Id,
                Icon = v.Icon,
                Name = v.Name,
                Url = v.Url,
                Module = v.Module,
                IsActive = !string.IsNullOrWhiteSpace(v.Url) && v.Url == menu?.Url
            }).ToList();

            var navigationVm = new NavigationViewModel
            {
                UserInfo = User.GetUser(),
                ReturnUrl = Request.GetRawUrl(),
                Modules = modules,
                Menus = menus
            };

            return await Task.Run(() => View("_Header", navigationVm));
        }
    }
}