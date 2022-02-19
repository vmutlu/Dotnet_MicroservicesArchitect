using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OnlineAuctionApp.Application.Extensions;
using OnlineAuctionApp.Core.Abstract;
using OnlineAuctionApp.Core.Concrete;
using OnlineAuctionApp.Core.Producer;
using OnlineAuctionApp.Infrastructure;
using OnlineAuctionApp.OrderAPI.Consumers;
using OnlineAuctionApp.OrderAPI.Extensions;
using RabbitMQ.Client;
using System;

namespace OnlineAuctionApp.OrderAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => (Configuration) = (configuration);

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();

            services.AddInfrastructure(Configuration);

            services.AddControllers();


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

            services.AddSingleton<OrderConsumer>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            //Docker ile ayaga kaldýrýldýgýnda mapping hatasý için eklendi.
            services.AddAutoMapper(typeof(Startup));

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OnlineAuctionApp.OrderAPI", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnlineAuctionApp.OrderAPI v1"));
            }

            app.UseCors("AllowOrigin");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseEventListener();
        }
    }
}
