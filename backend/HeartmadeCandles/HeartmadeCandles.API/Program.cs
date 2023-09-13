using HeartmadeCandles.Admin.BL;
using HeartmadeCandles.Admin.DAL;
using HeartmadeCandles.API;
using HeartmadeCandles.Auth.BL;
using HeartmadeCandles.Constructor.BL;
using HeartmadeCandles.Constructor.DAL;
using HeartmadeCandles.Order.BL;
using HeartmadeCandles.Order.Bot;
using HeartmadeCandles.Order.DAL;
using OpenTelemetry.Trace;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Enrichers.Span;
using System.Text;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .Enrich.WithSpan()
    .Enrich.WithCorrelationId()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

try
{
    builder.Services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.ClearProviders();

        loggingBuilder.AddSerilog(logger, true);
    });

    builder.Services.AddHttpLogging(logging => logging.LoggingFields = HttpLoggingFields.All);

    builder.Services.ConfigureOpenTelemetryTracerProvider(builder =>
    {
        builder
            .AddJaegerExporter(options =>
            {
                options.AgentHost = "jaeger";
            })
            .AddSource("HeartmadeCandles.API")
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                .AddTelemetrySdk()
                .AddService(serviceName: "HeartmadeCandles.API", serviceVersion: "1.0.0"))
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddSqlClientInstrumentation(options => options.SetDbStatementForText = true)
            .AddJaegerExporter(exporter =>
            {
                exporter.AgentHost = "localhost";
                exporter.AgentPort = 6831;
            })
            .AddEntityFrameworkCoreInstrumentation(options =>
            {
                options.SetDbStatementForText = true;
            });
    });

    if (!Directory.Exists("StaticFiles/"))
    {
        Directory.CreateDirectory("StaticFiles/Images");
    }

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowCors", policy =>
        {
            policy.WithOrigins("http://95.140.152.201", "http://localhost:5173", "http://localhost", "http://localhost:5000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
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

    var app = builder.Build();

    app.UseHttpLogging();

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
    logger.Fatal(ex, "Unhandled exception");
    throw;
}
finally
{
    logger.Information("Shut down complete");
    logger.Dispose();
}