namespace TechLife.CSDLMoiTruong.App.ApiClients.Models
{
    public class ApiViewModel<T>
    {
        public bool IsSuccessed { get; set; }
        public string Message { get; set; }
        public T ResultObj { get; set; }
    }
}
