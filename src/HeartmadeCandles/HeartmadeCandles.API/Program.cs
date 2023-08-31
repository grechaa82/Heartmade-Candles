using HeartmadeCandles.Admin.BL.Services;
using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HeartmadeCandles.Admin.DAL;
using Serilog;
using HeartmadeCandles.Constructor.DAL;
using HeartmadeCandles.Constructor.DAL.Repositories;
using HeartmadeCandles.Constructor.Core.Interfaces;
using HeartmadeCandles.Constructor.BL.Services;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

try
{
    builder.Services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.ClearProviders();

        loggingBuilder.AddSerilog(logger, true);
    });

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(
            policy =>
            {
                policy.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
    });

    //Admin module
    builder.Services.AddDbContext<AdminDbContext>(options =>
    {
        options.UseNpgsql(
            connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
            npgsqlOptionsAction: builder => builder.MigrationsAssembly("HeartmadeCandles.Migrations"));
    });

    builder.Services
        .AddScoped<ICandleService, CandleService>()
        .AddScoped<ICandleRepository, CandleRepository>();
    builder.Services
        .AddScoped<IDecorService, DecorService>()
        .AddScoped<IDecorRepository, DecorRepository>();
    builder.Services
        .AddScoped<ILayerColorService, LayerColorService>()
        .AddScoped<ILayerColorRepository, LayerColorRepository>();
    builder.Services
        .AddScoped<ISmellService, SmellService>()
        .AddScoped<ISmellRepository, SmellRepository>();
    builder.Services
        .AddScoped<IWickService, WickService>()
        .AddScoped<IWickRepository, WickRepository>();
    builder.Services
        .AddScoped<INumberOfLayerService, NumberOfLayerService>()
        .AddScoped<INumberOfLayerRepository, NumberOfLayerRepository>();
    builder.Services
        .AddScoped<ITypeCandleService, TypeCandleService>()
        .AddScoped<ITypeCandleRepository, TypeCandleRepository>();

    //Constructor module
    builder.Services.AddDbContext<ConstructorDbContext>(options =>
    {
        options.UseNpgsql(
            connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
            npgsqlOptionsAction: builder => builder.MigrationsAssembly("HeartmadeCandles.Migrations"));
    });

    builder.Services
        .AddScoped<IConstructorService, ConstructorService>()
        .AddScoped<IConstructorRepository, ConstructorRepository>();

    builder.Services.AddControllers().AddNewtonsoftJson();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddHttpLogging(options => { });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseCors(x =>
    {
        x.WithHeaders().AllowAnyHeader();
        x.WithOrigins().AllowAnyOrigin();
        x.AllowAnyMethod();
    });

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "StaticFiles")),
        RequestPath = "/StaticFiles"
    });

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Fatal(ex.ToString());
    throw;
}
finally
{
    logger.Dispose();
}