using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechLife.CSDLMoiTruong.App.ApiClients;
using TechLife.CSDLMoiTruong.App.Extensions;
using TechLife.CSDLMoiTruong.App.Models;
using TechLife.CSDLMoiTruong.Common;

namespace TechLife.CSDLMoiTruong.App.Controllers
{
    [AllowAnonymous]
    public class LoginController : BaseController
    {
        private readonly IUserApiClient _userService;
        private readonly IConfiguration _configuration;

        public LoginController(IUserApiClient userService,
            IConfiguration configuration,
            ILogger<LoginController> logger) : base(userService, logger)
        {
            _userService = userService;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            string ip = HttpContext.Connection.RemoteIpAddress.ToString();

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return View();
        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration["Tokens:Audience"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("xt7oidxpIRs9uDVnZEu9kZKqmumiF1e1RINb3UMlwCGA3O3Xywc8OZkOs4dmwf"));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginRequest request, string ReturnUrl = "/")
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Vui lòng nhập thông tin bắt buộc");

                    return View(request);
                }
                string ip = HttpContext.Connection.RemoteIpAddress.ToString();

                var result = await _userService.GetToken(request.UserName, request.Password);

                if (!result.IsSuccessed)
                {
                    ModelState.AddModelError("", result.Message);
                    return View(request);
                }

                string accessToken = result.ResultObj;

                var userPrincipal = this.ValidateToken(accessToken);

                var currentClaims = userPrincipal.Claims.ToList();

                currentClaims.Add(new Claim("AccessToken", accessToken));

                var identity = new ClaimsIdentity(currentClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(SystemConstants.AppSettings.ExpireMinutes),
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            principal,
                            authProperties);

                HttpContext.Session.SetString("AccessToken", accessToken);

                return Redirect(ReturnUrl);
            }
            catch
            {
                ModelState.AddModelError("", "Đăng nhập không thành công");
                return View(request);
            }
        }
    }
}