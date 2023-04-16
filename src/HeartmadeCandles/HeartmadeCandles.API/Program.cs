using HeartmadeCandles.Bot;
using HeartmadeCandles.BusinessLogic.Services;
using HeartmadeCandles.Core.Interfaces;
using HeartmadeCandles.Core.Interfaces.Repositories;
using HeartmadeCandles.Core.Interfaces.Services;
using HeartmadeCandles.DataAccess.MongoDB;
using HeartmadeCandles.DataAccess.MongoDB.Repositories;
using HeartmadeCandles.Admin.BL.Services;
using HeartmadeCandles.Admin.Core.Interfaces;
using HeartmadeCandles.Admin.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;
using HeartmadeCandles.Admin.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:3000");
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

builder.Services.AddDbContext<AdminDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<ICandleConstructorRepository, CandleConstructorRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

//builder.Services.AddScoped<IOrderCreateHandler, OrderCreateHandler>();

//Admin module
builder.Services.AddScoped<ICandleService, CandleService>();
builder.Services.AddScoped<ICandleRepository, CandleRepository>();

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
