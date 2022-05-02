using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineAuctionApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAuctionApp.Infrastructure.DataAccess.Seeds
{
    public class SeedDatas
    {
        public static async Task SeedAsync(WebApplicationContext webAppContext, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            try
            {
                webAppContext.Database.Migrate();

                if (!webAppContext.ApplicationUsers.Any())
                {
                    webAppContext.ApplicationUsers.AddRange(GetPreconfiguredOrders());
                    await webAppContext.SaveChangesAsync().ConfigureAwait(false);
                }
            }
            catch (Exception exception)
            {
                if (retryForAvailability < 50)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<SeedDatas>();
                    log.LogError(exception.Message);
                    System.Threading.Thread.Sleep(2000);

                    await SeedAsync(webAppContext, loggerFactory, retryForAvailability).ConfigureAwait(false);
                }

                throw;
            }
        }

        private static IEnumerable<ApplicationUser> GetPreconfiguredOrders()
        {
            return new List<ApplicationUser>()
            {
               new ApplicationUser
               {
                   FirstName ="User_One",
                   LastName="UserLastName_One",
                   IsAdmin =false,
                   IsSeller = true
               }
            };
        }
    }
}
