using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OnlineAuctionApp.AuctionAPI.DataAccess.Abstract;
using OnlineAuctionApp.AuctionAPI.DataAccess.Concrete;
using OnlineAuctionApp.AuctionAPI.Settings.Abstract;
using OnlineAuctionApp.AuctionAPI.Settings.Concrete;
using OnlineAuctionApp.Core.Abstract;
using OnlineAuctionApp.Core.Concrete;
using OnlineAuctionApp.Core.Producer;
using RabbitMQ.Client;
using System;

namespace OnlineAuctionApp.AuctionAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => (Configuration) = (configuration);

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SourcingDatabaseSettings>(Configuration.GetSection("SourcingDatabaseSetting"));

            services.AddSingleton<ISourcingDatabaseSettings>(s => s.GetRequiredService<IOptions<SourcingDatabaseSettings>>().Value);

            services.AddScoped<IAuctionContext, AuctionContext>();

            services.AddScoped<IAuctionRepository, AuctionRepository>();

            services.AddScoped<IBidRepository, BidRepository>();

            services.AddAutoMapper(typeof(Startup));

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OnlineAuctionApp.AuctionAPI", Version = "v1" });
            });

            #region Event Bus Configuration

            services.AddSingleton<IRabbitMQConnection>(c =>
            {
                var logger = c.GetRequiredService<ILogger<RabbitMQConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["EventBus:HostName"]
                };

                if (!string.IsNullOrWhiteSpace(Configuration["EventBus:UserName"]))
                    factory.UserName = Configuration["EventBus:UserName"];

                if (!string.IsNullOrWhiteSpace(Configuration["EventBus:Password"]))
                    factory.Password = Configuration["EventBus:Password"];

                var retryCount = 5;
                if (!string.IsNullOrWhiteSpace(Configuration["EventBus:RetryCount"]))
                    retryCount = Convert.ToInt32(Configuration["EventBus:RetryCount"]);

                return new RabbitMQConnection(factory, retryCount, logger);
            });

            services.AddSingleton<RabbitMQProducer>();

            #endregion

            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
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

            app.UseCors("AllowOrigin");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
