namespace TechLife.CSDLMoiTruong.App.Models
{
    public class NavigationViewModel
    {
        public string ReturnUrl { set; get; }
        public bool IsNhieuDonVi { set; get; }
        public UserLoginRequest UserInfo { set; get; }
        public List<MenuViewModel> Modules { get; set; }
        public List<MenuViewModel> Menus { get; set; }
    }
}