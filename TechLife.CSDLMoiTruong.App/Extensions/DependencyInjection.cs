using TechLife.CSDLMoiTruong.Service;

namespace TechLife.CSDLMoiTruong.App.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<ILoaiCayTrongService, LoaiCayTrongService>();

            return services;
        }
    }
}
