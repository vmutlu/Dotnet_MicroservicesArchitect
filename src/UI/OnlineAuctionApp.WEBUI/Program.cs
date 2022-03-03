using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OnlineAuctionApp.Infrastructure.DataAccess;
using OnlineAuctionApp.Infrastructure.DataAccess.Seeds;
using OnlineAuctionApp.WEBUI.Extensions;
using System;

namespace OnlineAuctionApp.WEBUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            CreateSeedData(host);

            host.Migrate();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void CreateSeedData(IHost host)
        {
            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                var aspnetRunContext = services.GetRequiredService<WebApplicationContext>();
                SeedDatas.SeedAsync(aspnetRunContext, loggerFactory).Wait();
            }

            catch (Exception exception)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(exception, " An error occurred seeding the DB.");
            }
        }
    }
}
