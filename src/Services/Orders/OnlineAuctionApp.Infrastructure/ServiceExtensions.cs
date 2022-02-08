using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineAuctionApp.Domain.DataAccess.Abstract;
using OnlineAuctionApp.Infrastructure.Concrete.Context;

namespace OnlineAuctionApp.Infrastructure
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderContext>(option => option.UseSqlServer(configuration.GetConnectionString("OrderConnection"), m => m.MigrationsAssembly(typeof(OrderContext).Assembly.FullName)), ServiceLifetime.Singleton);

         /*   services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IOrderRepository, OrderRepository>();*/

            return services;
        }
    }
}
