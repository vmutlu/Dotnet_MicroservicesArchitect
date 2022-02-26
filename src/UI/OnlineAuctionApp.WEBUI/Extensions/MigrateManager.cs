using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineAuctionApp.Infrastructure.DataAccess;
using System;
using System.Diagnostics;

namespace OnlineAuctionApp.WEBUI.Extensions
{
    public static class MigrateManager
    {
        public static IHost Migrate(this IHost host)
        {
            using var scope = host.Services.CreateScope();

            HandleException(async () =>
            {
                var orderContext = scope.ServiceProvider.GetRequiredService<WebApplicationContext>();

                if (orderContext.Database.ProviderName is not "Microsoft.EntityFrameworkCore.InMemory")
                    orderContext.Database.Migrate();
            });

            return host;
        }

        private static void HandleException(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);

                throw;
            }
        }
    }
}
