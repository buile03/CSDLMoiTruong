using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechLife.CSDLMoiTruong.App.ApiClients;
using TechLife.CSDLMoiTruong.App.Extensions;

namespace TechLife.CSDLMoiTruong.App.Controllers
{
    public class LogoutController : BaseController
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<LogoutController> _logger;

        public LogoutController(IUserApiClient userApiClient, ILogger<LogoutController> logger) : base(userApiClient, logger)
        {
            _logger = logger;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index(string ReturnUrl = "/")
        {
            try
            {

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                return Redirect(ReturnUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi đăng nhập {0}", Request.GetFullUrl());
                return Redirect(ReturnUrl);
            }
        }
    }
}