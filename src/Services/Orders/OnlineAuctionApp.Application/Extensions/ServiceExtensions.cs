using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OnlineAuctionApp.Application.Mappers;
using OnlineAuctionApp.Application.PipelineBehaviours;
using System.Reflection;

namespace OnlineAuctionApp.Application.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionBehavior<,>));

            #region Mapper Dependencyies Configure

            var config = new MapperConfiguration(c =>
            {
                c.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                c.AddProfile<OrderProfile>();
            });

            var mapper = config.CreateMapper();

            #endregion

            return services;
        }
    }
}
