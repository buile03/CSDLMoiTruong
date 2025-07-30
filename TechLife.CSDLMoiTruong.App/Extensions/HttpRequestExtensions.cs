namespace TechLife.CSDLMoiTruong.App.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GetRawUrlSSOVNeID(this HttpRequest request)
        {
            var httpContext = request.HttpContext;
            return $"{request.Scheme}://{request.Host}/LoginWithSSOVNeID/";
        }

        public static string GetFullUrl(this HttpRequest request)
        {
            return $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
        }

        public static string GetHost(this HttpRequest request)
        {
            return $"{request.Scheme}://{request.Host}";
        }
    }
}