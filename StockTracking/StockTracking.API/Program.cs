using Microsoft.EntityFrameworkCore;
using StockTracking.API.Data;
using StockTracking.API.Services.Jwt;
using StockTracking.API.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using StockTracking.API.Services.Interfaces;
using StockTracking.API.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// 1) Configure DbContext
builder.Services.AddDbContext<StockTrackingDbContext>(options =>
{
    // "StockDbConnection" --> connection string in appsettings.json
    options.UseNpgsql(builder.Configuration.GetConnectionString("StockDbConnection"));
});

// 2) Dependency injections
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISupplyService, SupplyService>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

// 3) JWT Bearer Settings 
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

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
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// 4) Authorization 
builder.Services.AddAuthorization();

// 5) Swagger Bearer Definition 
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer token in the format: 'Bearer YOUR_TOKEN'",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 6) Add Authentication and Authorization middleware
app.UseAuthentication(); // JWT authentication middleware
app.UseAuthorization();

app.MapControllers();

app.Run();
