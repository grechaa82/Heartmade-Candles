using HeartmadeCandles.Admin.BL.Services;
using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.DAL;
using HeartmadeCandles.Admin.DAL.Repositories;
using HeartmadeCandles.API;
using HeartmadeCandles.Auth.BL;
using HeartmadeCandles.Auth.Core;
using HeartmadeCandles.Constructor.BL.Services;
using HeartmadeCandles.Constructor.Core.Interfaces;
using HeartmadeCandles.Constructor.DAL;
using HeartmadeCandles.Constructor.DAL.Repositories;
using HeartmadeCandles.Order.BL.Services;
using HeartmadeCandles.Order.Bot;
using HeartmadeCandles.Order.Core.Interfaces;
using HeartmadeCandles.Order.DAL;
using HeartmadeCandles.Order.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

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

    if (!Directory.Exists("StaticFiles/"))
    {
        Directory.CreateDirectory("StaticFiles/Images");
    }

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowCors", policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                .WithHeaders().AllowAnyHeader()
                .WithMethods().AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowedToAllowWildcardSubdomains();
        });
    });

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtOptions:Issuer"],
            ValidAudience = builder.Configuration["JwtOptions:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:SecretKey"]))
        };
    });

    builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

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

    //Order module
    builder.Services.AddDbContext<OrderDbContext>(options =>
    {
        options.UseNpgsql(
            connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
            npgsqlOptionsAction: builder => builder.MigrationsAssembly("HeartmadeCandles.Migrations"));
    });

    builder.Services
        .AddScoped<IOrderService, OrderService>()
        .AddScoped<IOrderRepository, OrderRepository>();
    builder.Services.AddScoped<IOrderNotificationHandler, OrderNotificationHandler>();

    //Order module
    builder.Services.AddScoped<IAuthService, AuthService>();

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

    app.UseCors("AllowCors"); ;

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "StaticFiles")),
        RequestPath = "/StaticFiles"
    });

    app.UseAuthentication();

    app.UseAuthorization();

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