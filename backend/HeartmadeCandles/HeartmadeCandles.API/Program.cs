using HeartmadeCandles.Admin.BL;
using HeartmadeCandles.Admin.DAL;
using HeartmadeCandles.API;
using HeartmadeCandles.API.Extensions;
using HeartmadeCandles.Auth.BL;
using HeartmadeCandles.Constructor.BL;
using HeartmadeCandles.Constructor.DAL;
using HeartmadeCandles.Order.BL;
using HeartmadeCandles.Order.Bot;
using HeartmadeCandles.Order.DAL;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.FileProviders;
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
        .AddOrderRepositories()
        .AddOrderDbContext(builder.Configuration)
        .AddOrderNotificationServices();

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
            RequestPath = "/StaticFiles"
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