using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OnlineAuctionApp.AuctionAPI.DataAccess.Abstract;
using OnlineAuctionApp.AuctionAPI.DataAccess.Concrete;
using OnlineAuctionApp.AuctionAPI.Settings.Abstract;
using OnlineAuctionApp.AuctionAPI.Settings.Concrete;

namespace OnlineAuctionApp.AuctionAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SourcingDatabaseSettings>(Configuration.GetSection("SourcingDatabaseSetting"));

            services.AddSingleton<ISourcingDatabaseSettings>(s => s.GetRequiredService<IOptions<SourcingDatabaseSettings>>().Value);

            services.AddScoped<IAuctionContext, AuctionContext>();

            services.AddScoped<IAuctionRepository, AuctionRepository>();

            services.AddScoped<IBidRepository, BidRepository>();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OnlineAuctionApp.AuctionAPI", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnlineAuctionApp.AuctionAPI v1"));
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
