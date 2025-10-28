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
// dang ky controllers tu folder Controllers
builder.Services.AddControllers();

var app = builder.Build();

// ?? Middleware
app.UseHttpsRedirection();
app.UseAuthorization();

// ?? Map route controllers
app.MapControllers();

app.Run();
