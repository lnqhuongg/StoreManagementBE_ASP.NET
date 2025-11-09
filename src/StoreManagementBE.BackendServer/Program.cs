using Microsoft.EntityFrameworkCore;
using StoreManagementBE.BackendServer.Infrastructure.DI;
using StoreManagementBE.BackendServer.Mappings;
using StoreManagementBE.BackendServer.Models;

var builder = WebApplication.CreateBuilder(args);

// ?? Ket noi Database (MySQL)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 29))
    )
);

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Dang ky tat ca cac service tu folder Infrastructure 
builder.Services.AddApplicationServices();

builder.Services.Configure<StaticFileOptions>(options =>
{
    options.OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=31536000"); // Cache 1 năm cho ảnh
    };
});


// cai nay de tranh tinh trang bi loi khi backend va frontend chay khac port
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// dang ky controllers tu folder Controllers
builder.Services.AddControllers();


var app = builder.Build();

// Configure pipeline
app.UseCors("AllowAll");



// Serve static files từ wwwroot (quan trọng để truy cập ảnh)
app.UseStaticFiles();

// ?? Middleware
app.UseAuthorization();
app.UseHttpsRedirection();


// ?? Map route controllers
app.MapControllers();
var webRootPath = app.Environment.WebRootPath;
if (string.IsNullOrEmpty(webRootPath))
{
    webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
}

var imagePath = Path.Combine(webRootPath, "images");
if (!Directory.Exists(imagePath))
{
    Directory.CreateDirectory(imagePath);
    Console.WriteLine($"✅ Đã tạo thư mục ảnh: {imagePath}");
}
else
{
    Console.WriteLine($"📁 Thư mục ảnh đã tồn tại: {imagePath}");
}
app.Run();