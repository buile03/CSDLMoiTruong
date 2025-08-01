using TechLife.CSDLMoiTruong.Service;

namespace TechLife.CSDLMoiTruong.App.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<ILoaiCayTrongService, LoaiCayTrongService>();
            services.AddTransient<ISinhVatGayHaiService, SinhVatGayHaiService>();
            services.AddTransient<IDiaBanAnhHuongService, DiaBanAnhHuongService>();
            services.AddTransient<IThoiTietService, ThoiTietService>();
            services.AddTransient<ISoLieuSinhTruongService, SoLieuSinhTruongService>();
            services.AddTransient<ITinhHinhGayHaiCayTrongService, TinhHinhGayHaiCayTrongService>();

            return services;
        }
    }
}
