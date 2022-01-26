using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OnlineAuctionApp.ProductAPI.DataAccess.Abstract;
using OnlineAuctionApp.ProductAPI.DataAccess.Concrete;
using OnlineAuctionApp.ProductAPI.Settings.Abstract;
using OnlineAuctionApp.ProductAPI.Settings.Concrete;

namespace OnlineAuctionApp.ProductAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => (Configuration) = (configuration);

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ProductDatabaseSettings>(Configuration.GetSection("ProductDatabase"));

            services.AddSingleton<IProductDatabaseSettings>(s => s.GetRequiredService<IOptions<ProductDatabaseSettings>>().Value);

            services.AddScoped<IProductContext, ProductContext>();

            services.AddScoped<IRepository, Repository>();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OnlineAuctionApp.ProductAPI", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnlineAuctionApp.ProductAPI v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
