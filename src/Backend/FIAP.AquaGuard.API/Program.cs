using DotNetEnv;
using FIAP.AquaGuard.API.Filters;
using FIAP.AquaGuard.Application;
using FIAP.AquaGuard.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Globalization;
using System.Security.Claims;
using System.Text;

static string FindEnvFile()
{
    var directory = new DirectoryInfo(AppContext.BaseDirectory);

    while (directory != null)
    {
        var envPath = Path.Combine(directory.FullName, ".env");

        if (File.Exists(envPath))
            return envPath;

        directory = directory.Parent;
    }

    throw new FileNotFoundException("Arquivo .env não encontrado.");
}

Env.Load(FindEnvFile());

var jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")!;
var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER")!;
var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")!;

var builder = WebApplication.CreateBuilder(args);

/// :: Configure JWT authentication.
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,

            ValidateAudience = true,
            ValidAudience = audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            ),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,

            RoleClaimType = ClaimTypes.Role,
            NameClaimType = ClaimTypes.NameIdentifier
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {

                if (context.Request.Cookies.TryGetValue("access_token", out var token))
                {
                    context.Token = token;
                }

                return Task.CompletedTask;
            }
        };
    });

/// :: AddAuthParams.
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"))
    .AddPolicy("ManagerOrAdmin", policy =>
        policy.RequireRole("Admin", "Manager"))
    .AddPolicy("EmployeeManagerOrAdmin", policy =>
        policy.RequireRole("Admin", "Manager", "Employee"))
    .AddPolicy("AuthenticatedUsers", policy =>
        policy.RequireAuthenticatedUser());

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

/// :: Configure localization for using bilingual messages.
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo> { new("en"), new("pt-BR"), new("es") };

    options.DefaultRequestCulture = new RequestCulture("en");

    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders = [new AcceptLanguageHeaderRequestCultureProvider()];
});

/// :: Add a global exception filter to handle exceptions and return appropriate responses.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
