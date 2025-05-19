using CoreFinal.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("con")));
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(name:"default",pattern:"{controller=Home}/{action=Index}/{id?}");

app.Run();
