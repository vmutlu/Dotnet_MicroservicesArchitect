using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineAuctionApp.Core.Entities;
using OnlineAuctionApp.Infrastructure.DataAccess;

namespace OnlineAuctionApp.WEBUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => (Configuration) = (configuration);

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WebApplicationContext>(option => option.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"), m => m.MigrationsAssembly(typeof(WebApplicationContext).Assembly.FullName)), ServiceLifetime.Singleton);

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<WebApplicationContext>();

            services.AddControllersWithViews();

            //services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            //{
            //    options.Cookie.Name = "LoginCookie";
            //    options.LoginPath = "Home/Login";
            //    options.LogoutPath = "Home/Logout";
            //    options.ExpireTimeSpan = System.TimeSpan.FromDays(180);
            //    options.SlidingExpiration = false;
            //});

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Home/Login";
                options.LogoutPath = $"/Home/Logout";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
