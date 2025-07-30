namespace TechLife.CSDLMoiTruong.App.Models
{
    public class MenuViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Module { get; set; }
        public bool IsActive { get; set; } = false;
    }
}