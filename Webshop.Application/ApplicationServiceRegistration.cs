using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Webshop.Application.Contracts;

namespace Webshop.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddScoped<IDispatcher>(sp => new Dispatcher(sp.GetService<IMediator>()));

            return services;
        }
    }
}
