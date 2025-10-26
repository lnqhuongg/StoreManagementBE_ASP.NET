using Microsoft.EntityFrameworkCore;
using StoreManagementBE.BackendServer.Mappings;
using StoreManagementBE.BackendServer.Models;
using StoreManagementBE.BackendServer.Services;
using StoreManagementBE.BackendServer.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ?? K?t n?i Database (MySQL)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 29))
    )
);

// ?? ??ng k� Service
builder.Services.AddScoped<ILoaiSanPhamService, LoaiSanPhamService>();
builder.Services.AddScoped<INhaCungCapService, NhaCungCapService>();


builder.Services.AddScoped<INhanVienService, NhanVienService>();
builder.Services.AddScoped<IPhieuNhapService, PhieuNhapService>();

// ?? Th�m Controllers
builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// ?? Middleware
app.UseHttpsRedirection();
app.UseAuthorization();

// ?? Map route controllers
app.MapControllers();

app.Run();
