using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TechLife.CSDLMoiTruong.App.ApiClients;
using TechLife.CSDLMoiTruong.App.ApiClients.Models;
using TechLife.CSDLMoiTruong.App.Models;

namespace TechLife.CSDLMoiTruong.App.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(IUserApiClient userService, ILogger<HomeController> logger)
            : base(userService, logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xãy ra");
                return View();
            }
        }
    }
}