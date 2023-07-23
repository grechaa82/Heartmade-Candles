using HeartmadeCandles.BusinessLogic.Services;
using HeartmadeCandles.Core.Interfaces.Repositories;
using HeartmadeCandles.Core.Interfaces.Services;
using HeartmadeCandles.DataAccess.MongoDB;
using HeartmadeCandles.DataAccess.MongoDB.Repositories;
using HeartmadeCandles.Admin.BL.Services;
using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
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

    builder.Services.AddAutoMapper(typeof(DataAccessMappingProfile));

    builder.Services.AddScoped<ICandleConstructorService, CandleConstructorService>();
    builder.Services.AddScoped<IAdminService, AdminService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();

    builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
    builder.Services.AddSingleton(options =>
    {
        var databaseSettings = builder.Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();
        var mongoDbClient = new MongoClient(databaseSettings.ConnectionString);

        return mongoDbClient.GetDatabase(databaseSettings.DatabaseName);
    });

    builder.Services.AddScoped<ICandleConstructorRepository, CandleConstructorRepository>();
    builder.Services.AddScoped<IAdminRepository, AdminRepository>();
    builder.Services.AddScoped<IAuthRepository, AuthRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

    //builder.Services.AddScoped<IOrderCreateHandler, OrderCreateHandler>();

    //Admin module
    builder.Services.AddDbContext<AdminDbContext>(options =>
    {
        options.UseNpgsql(
            connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
            npgsqlOptionsAction: builder => builder.MigrationsAssembly("HeartmadeCandles.Migrations"));
    });

    builder.Services.AddScoped<ICandleService, CandleService>();
    builder.Services.AddScoped<ICandleRepository, CandleRepository>();
    builder.Services.AddScoped<IDecorService, DecorService>();
    builder.Services.AddScoped<IDecorRepository, DecorRepository>();
    builder.Services.AddScoped<ILayerColorService, LayerColorService>();
    builder.Services.AddScoped<ILayerColorRepository, LayerColorRepository>();
    builder.Services.AddScoped<ISmellService, SmellService>();
    builder.Services.AddScoped<ISmellRepository, SmellRepository>();
    builder.Services.AddScoped<IWickService, WickService>();
    builder.Services.AddScoped<IWickRepository, WickRepository>();
    builder.Services.AddScoped<INumberOfLayerService, NumberOfLayerService>();
    builder.Services.AddScoped<INumberOfLayerRepository, NumberOfLayerRepository>();
    builder.Services.AddScoped<ITypeCandleService, TypeCandleService>();
    builder.Services.AddScoped<ITypeCandleRepository, TypeCandleRepository>();

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

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("SecurityKey").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

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