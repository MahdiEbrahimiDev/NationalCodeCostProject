using Microsoft.EntityFrameworkCore;
using NationalCodeCostProject.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
    });
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IExcelImportService, ExcelImportService>();
    builder.Services.AddScoped<ICostService, CostService>();
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();   
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");

app.Run();
