using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechLife.CSDLMoiTruong.App.ApiClients;

namespace TechLife.CSDLMoiTruong.App.Controllers
{
    [AllowAnonymous]
    public class AccessDeniedController : BaseController
    {
        public AccessDeniedController(IUserApiClient userService, ILogger<BaseController> logger) : base(userService, logger)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}