using HeartmadeCandles.Admin.BL;
using HeartmadeCandles.Admin.DAL;
using HeartmadeCandles.API.Extensions;
using HeartmadeCandles.Bot.BL;
using HeartmadeCandles.Bot.DAL;
using HeartmadeCandles.Constructor.BL;
using HeartmadeCandles.Constructor.DAL;
using HeartmadeCandles.Order.BL;
using HeartmadeCandles.Order.DAL;
using HeartmadeCandles.Order.DAL.Mongo;
using HeartmadeCandles.UserAndAuth.BL;
using HeartmadeCandles.UserAndAuth.DAL;
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

    const string staticFilesDirectory = "StaticFiles";
    const string imagesSubdirectory = "Images";
    const string defaultImagesSubdirectory = "DefaultImages";
    
    var staticFilesImagesPath = Path.Combine(staticFilesDirectory, imagesSubdirectory);
    var staticFilesDefaultImagesPath = Path.Combine(staticFilesImagesPath, defaultImagesSubdirectory);

    if (!Directory.Exists(staticFilesDirectory))
    {
        Directory.CreateDirectory(staticFilesDirectory);
    }

    if (!Directory.Exists(staticFilesImagesPath))
    {
        Directory.CreateDirectory(staticFilesImagesPath);
    }

    if (!Directory.Exists(staticFilesDefaultImagesPath))
    {
        Directory.CreateDirectory(staticFilesDefaultImagesPath);
    }

    builder.Services.AddCustomCors();

    builder.Services.AddApiAuthentication(builder.Configuration);

    builder.Services
        .AddAdminServices(Path.Combine(Directory.GetCurrentDirectory(), staticFilesImagesPath))
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

    builder.Services
        .AddUserAndAuthServices(builder.Configuration)
        .AddUserAndAuthRepositories()
        .AddUserAndAuthDbContext(builder.Configuration);

    builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
    builder.Services.AddSingleton(options =>
    {
        var databaseSettings = builder.Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();
        var mongoDbClient = new MongoClient(databaseSettings.ConnectionString);

        return mongoDbClient.GetDatabase(databaseSettings.DatabaseName);
    });

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
catch (Exception ex) when (ex.GetType().Name is not "StopTheHostException"
    && ex.GetType().Name is not "HostAbortedException")
{
    logger.Fatal(ex, "Unhandled exception");
}
finally
{
    logger.Information("Shut down complete");
    logger.Dispose();
}