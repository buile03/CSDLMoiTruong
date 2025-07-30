using System.Text.Json;
using TechLife.CSDLMoiTruong.Common.Result;

namespace TechLife.CSDLMoiTruong.App.Extensions
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xử lý dữ liệu!");
                if (context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                    var result = JsonSerializer.Serialize(Result.Error("Lỗi xử lý dữ liệu!"), new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                    await context.Response.WriteAsync(result);
                }
                else
                {
                    context.Response.Redirect("/Error");
                }
            }
        }
    }
}