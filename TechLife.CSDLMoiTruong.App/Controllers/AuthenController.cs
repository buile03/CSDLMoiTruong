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
using TechLife.CSDLMoiTruong.Common;

namespace TechLife.CSDLMoiTruong.App.Controllers
{
    [AllowAnonymous]
    public class AuthenController : BaseController
    {
        private readonly IUserApiClient _userService;
        private readonly IConfiguration _configuration;

        public AuthenController(IUserApiClient userService,
            IConfiguration configuration,
            ILogger<AuthenController> logger) : base(userService, logger)
        {
            _userService = userService;
            _configuration = configuration;
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
        public async Task<IActionResult> Index(string ReturnUrl = "/")
        {
            try
            {
                var result = await _userService.GetToken("techlife.syt", "Abcd@1234");

                if (!result.IsSuccessed)
                {
                    return Redirect("/Login/?ReturnUrl=" + ReturnUrl);
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

                return Redirect(ReturnUrl);
            }
            catch
            {
                return Redirect("/Login/?ReturnUrl=" + ReturnUrl);
            }
        }
    }
}
