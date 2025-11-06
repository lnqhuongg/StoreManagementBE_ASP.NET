// Infrastructure/ServiceCollectionExtensions.cs
using Microsoft.Extensions.DependencyInjection;
using StoreManagementBE.BackendServer.Services;
using StoreManagementBE.BackendServer.Services.Interfaces;

namespace StoreManagementBE.BackendServer.Infrastructure.DI
{
    public static class ServiceCollectionExtensions
    {
        // IServiceCollection la giao dien co san trong ASP.NET Core de dang ky cac dich vu (services) cho ung dung
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Đăng ký tất cả service ở đây
            services.AddScoped<INhanVienService, NhanVienService>();
            services.AddScoped<ILoaiSanPhamService, LoaiSanPhamService>();
            services.AddScoped<ISanPhamService, SanPhamService>();
            services.AddScoped<IPhieuNhapService, PhieuNhapService>();
            services.AddScoped<IChiTietPNService, ChiTietPNService>();
            services.AddScoped<INhaCungCapService, NhaCungCapService>();

            return services;
        }
    }
}