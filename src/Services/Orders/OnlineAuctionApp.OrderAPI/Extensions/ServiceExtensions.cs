using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineAuctionApp.OrderAPI.Consumers;

namespace OnlineAuctionApp.OrderAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static OrderConsumer Listener { get; set; }

        public static IApplicationBuilder UseEventListener(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetService<OrderConsumer>();

            var life = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            life.ApplicationStarted.Register(OnStarted);
            life.ApplicationStopped.Register(OnStopped);

            return app;
        }

        private static void OnStarted() => Listener.Consume();

        private static void OnStopped() => Listener.Disconnected();
    }
}
