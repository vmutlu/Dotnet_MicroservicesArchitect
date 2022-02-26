using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using OnlineAuctionApp.WEBUI.Extensions;

namespace OnlineAuctionApp.WEBUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Migrate().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
