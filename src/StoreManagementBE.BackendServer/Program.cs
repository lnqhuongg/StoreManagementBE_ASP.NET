using Microsoft.EntityFrameworkCore;
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJS",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // Next.js dev server
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// ?? ??ng k� Service
builder.Services.AddScoped<ILoaiSanPhamService, LoaiSanPhamService>();
builder.Services.AddScoped<ISanPhamService, SanPhamService>();

// ?? Th�m Controllers
builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("AllowNextJS");

// ?? Middleware
app.UseHttpsRedirection();
app.UseAuthorization();

// ?? Map route controllers
app.MapControllers();


app.Run();
