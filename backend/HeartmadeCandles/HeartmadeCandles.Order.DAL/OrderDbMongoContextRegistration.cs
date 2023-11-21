using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace HeartmadeCandles.Order.DAL.Mongo;

public static class OrderDbMongoContextRegistration
{
    public static IServiceCollection AddOrderDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(options =>
        {
            var mongoDbClient = new MongoClient(configuration.GetConnectionString("MongoDbConnection"));

            return mongoDbClient.GetDatabase("lcf");
        });

        return services;
    }
}