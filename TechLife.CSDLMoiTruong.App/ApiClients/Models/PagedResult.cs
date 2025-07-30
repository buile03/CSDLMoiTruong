namespace TechLife.CSDLMoiTruong.App.ApiClients.Models
{
    public class PagedResult<T>
    {
        public string Keyword { get; set; }
        public List<T> Items { get; set; } = new();
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int PageCount { get; set; }
    }
}
