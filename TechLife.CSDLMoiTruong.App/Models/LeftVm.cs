using Microsoft.AspNetCore.Mvc.Rendering;

namespace TechLife.CSDLMoiTruong.App.Models
{
    public class LeftVm
    {
        public List<MenuViewModel> menus { get; set; }
        public string moduleId { get; set; }
        public List<SelectListItem> modules { get; set; }
    }
}