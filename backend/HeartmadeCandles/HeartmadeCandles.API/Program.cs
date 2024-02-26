using HeartmadeCandles.Admin.BL;
using HeartmadeCandles.Admin.DAL;
using HeartmadeCandles.API;
using HeartmadeCandles.API.Extensions;
using HeartmadeCandles.Auth.BL;
using HeartmadeCandles.Bot.BL;
using HeartmadeCandles.Bot.DAL;
using HeartmadeCandles.Constructor.BL;
using HeartmadeCandles.Constructor.DAL;
using HeartmadeCandles.Order.BL;
using HeartmadeCandles.Order.DAL;
using HeartmadeCandles.Order.DAL.Mongo;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.FileProviders;
using MongoDB.Driver;
using Serilog;
using Serilog.Enrichers.Span;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .Enrich.WithSpan()
    .Enrich.WithCorrelationId()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

try
{
    builder.Services.AddLogging(
        loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(logger, true);
        });

    builder.Services.AddHttpLogging(logging => logging.LoggingFields = HttpLoggingFields.All);

    builder.Services.AddOpenTelemetryMetrics();

    if (!Directory.Exists("StaticFiles/"))
    {
        Directory.CreateDirectory("StaticFiles/Images");
    }

    builder.Services.AddCustomCors();

    builder.Services.AddApiAuthentication(builder.Configuration);

    builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

    builder.Services
        .AddAdminServices()
        .AddAdminRepositories()
        .AddAdminDbContext(builder.Configuration);

    builder.Services
        .AddConstructorServices()
        .AddConstructorRepositories()
        .AddConstructorDbContext(builder.Configuration);

    builder.Services
        .AddOrderServices()
        .AddOrderRepositories();

    builder.Services
        .AddBotServices()
        .AddBotRepositories();

    builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
    builder.Services.AddSingleton(options =>
    {
        var databaseSettings = builder.Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();
        var mongoDbClient = new MongoClient(databaseSettings.ConnectionString);

        return mongoDbClient.GetDatabase(databaseSettings.DatabaseName);
    });

    builder.Services.AddAuthServices();

    builder.Services.AddControllers().AddNewtonsoftJson();
     
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddHttpContextAccessor();

    builder.Services.AddSwaggerGen();

    builder.Services.AddHttpLogging(options => { });

    builder.Services.AddRateLimiterService();

    var app = builder.Build();

    app.UseHttpLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseRateLimiter();
    
    app.UseHttpsRedirection();
    
    app.UseCors("AllowCors");

    app.UseStaticFiles(
        new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(
                Path.Combine(builder.Environment.ContentRootPath, "StaticFiles")),
            RequestPath = "/api/StaticFiles"
        });

    app.UseAuthentication();

    app.UseAuthorization();
    
    app.MapControllers();
    
    app.Run();
}
catch (Exception ex)
{
    logger.Fatal(ex, "Unhandled exception");
    throw;
}
finally
{
    logger.Information("Shut down complete");
    logger.Dispose();
}