using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineAuctionApp.Core.Entities;

namespace OnlineAuctionApp.Infrastructure.DataAccess
{
    public class WebApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public WebApplicationContext(DbContextOptions<WebApplicationContext> dbContextOptions) : base(dbContextOptions){}

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
