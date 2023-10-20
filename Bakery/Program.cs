using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Bakery.Models;

namespace Bakery
{
  class Program
  {
    static void Main(string[] args)
    {
      WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

      builder.Services.AddControllersWithViews();

      builder.Services.AddDbContext<BakeryContext>(
                      DbContextOptions => DbContextOptions
                      .UseMySql(
                        builder.Configuration
                        ["ConnectionStrings:DefaultConnection"],
                        ServerVersion.AutoDetect(builder.Configuration
                        ["ConnectionStrings:DefaultConnection"]
                        )
                      )
                    );
      builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<BakeryContext>()
                .AddDefaultTokenProviders();

      //DELETE BEFORE DEPLOYMENT
      builder.Services.Configure<IdentityOptions>(options =>
      {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 0;
        options.Password.RequiredUniqueChars = 0;
      });

      WebApplication app = builder.Build();

      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();
      app.MapControllerRoute(
          name: "default",
          pattern: "{controller=Account}/{action=Index}/{id?}");

      app.Run();
    }
  }
}