﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HeartmadeCandles.Admin.DAL
{
    public static class AdminDbContextRegistration
    {
        public static IServiceCollection AddAdminDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AdminDbContext>(options =>
            {
                options.UseNpgsql(
                    connectionString: configuration.GetConnectionString("DefaultConnection"),
                    npgsqlOptionsAction: builder => builder.MigrationsAssembly("HeartmadeCandles.Migrations"));
            });

            return services;
        }
    }
}
