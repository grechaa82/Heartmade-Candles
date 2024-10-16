using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Exporter;
using OpenTelemetry.ResourceDetectors.Container;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace HeartmadeCandles.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddOpenTelemetryMetrics(this IServiceCollection services)
    {
        void AppResourceBuilder(ResourceBuilder resource)
        {
            resource.AddDetector(new ContainerResourceDetector());
        }

        services.AddOpenTelemetry()
            .ConfigureResource(AppResourceBuilder)
            .WithTracing(
                tracerBuilder =>
                    tracerBuilder
                        .AddSource("HeartmadeCandles.API")
                        .ConfigureResource(
                            resource =>
                                resource.AddService(
                                    "HeartmadeCandles.API",
                                    serviceVersion: "1.0.0"))
                        .AddAspNetCoreInstrumentation()
                        .AddGrpcClientInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddOtlpExporter(
                            options =>
                            {
                                options.Endpoint = new Uri("http://jaeger:4318/v1/traces");
                                options.Protocol = OtlpExportProtocol.HttpProtobuf;
                            })
                        .AddSqlClientInstrumentation(options => options.SetDbStatementForText = true)
                        .AddEntityFrameworkCoreInstrumentation(options => options.SetDbStatementForText = true));
    }

    public static void AddCustomCors(this IServiceCollection services, IConfiguration configuration)
    {
        var domain = configuration["HostDomain"];
        var frontendPort = configuration["FrontendPort"];

        var originParams = new string[]{
            $"http://{domain}",
            $"https://{domain}",
            $"http://{domain}:{frontendPort}",
            $"https://{domain}:{frontendPort}",
            "http://localhost:5174"
        };
        
        services.AddCors(
            options =>
            {
                options.AddPolicy(
                    "AllowCors", policy =>
                    {
                        policy.WithOrigins(originParams)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()
                            .WithExposedHeaders("X-Total-Count");
                    });
            });
    }

    public static AuthenticationBuilder AddApiAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
            .AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["JwtOptions:Issuer"],
                        ValidAudience = configuration["JwtOptions:Audience"],
                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(configuration["JwtOptions:SecretKey"] ?? string.Empty))
                    };
                });
    }

    public static void AddRateLimiterService(this IServiceCollection services)
    {
        services.AddRateLimiter(
            options =>
            {
                options.AddConcurrencyLimiter(
                    RateLimiterPolicyNames.ConcurrencyPolicy, opt =>
                    {
                        opt.PermitLimit = 10;
                        opt.QueueLimit = 20;
                        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    }).RejectionStatusCode = 429;

                options.AddFixedWindowLimiter(
                    RateLimiterPolicyNames.FixedWindowPolicy, opt =>
                    {
                        opt.Window = TimeSpan.FromSeconds(5);
                        opt.PermitLimit = 10;
                        opt.QueueLimit = 20;
                        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    }).RejectionStatusCode = 429;
            });
    }
}