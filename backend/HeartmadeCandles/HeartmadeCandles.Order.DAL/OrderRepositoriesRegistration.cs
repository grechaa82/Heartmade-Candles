using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HeartmadeCandles.Order.DAL;

public static class OrderRepositoriesRegistration
{
    public static IServiceCollection AddOrderRepositories(this IServiceCollection services)
    {
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}