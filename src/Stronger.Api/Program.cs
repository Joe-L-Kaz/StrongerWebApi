using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stronger.Api.Extensions;
using Stronger.Application.Abstractions.HttpClients;
using Stronger.Application.Extensions;
using Stronger.Infrastructure;
using Stronger.Infrastructure.HttpClients;
using Stronger.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // case-insensitive by default
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddHttpClient<IStrongerNotificationsApiClient, StrongerNotificationsApiClient>((sp, client) =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var section = config.GetSection("NotificationsApiConfiguration");

    var baseAddress = section["NotificationsApiBaseAddress"];
    if (string.IsNullOrWhiteSpace(baseAddress))
        throw new InvalidOperationException("NotificationsApiConfiguration:NotificationsApiBaseAddress is required.");

    var apiKey = section["NotificationsApiKey"];
    if (string.IsNullOrWhiteSpace(apiKey))
        throw new InvalidOperationException("NotificationsApiConfiguration:NotificationsApiKey is required.");

    var apiKeyHeaderName = section["ApiKeyHeaderName"];
    if (string.IsNullOrWhiteSpace(apiKeyHeaderName))
        apiKeyHeaderName = "X-API-Key";

    client.BaseAddress = new Uri(baseAddress);

    // Optional default headers
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

    // API key for every request
    client.DefaultRequestHeaders.Remove(apiKeyHeaderName);
    client.DefaultRequestHeaders.Add(apiKeyHeaderName, apiKey);

    // Optional timeout
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
        ValidAudience = builder.Configuration["JwtConfig:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services
    .AddAuthorization()
    .AddHttpContextAccessor();

builder.Services
    .AddApiLayer()
    .AddApplicationLayer()
    .AddInfrastructureLayer(builder.Configuration);


var app = builder.Build();

//await ApplyMigrationsWithRetryAsync(app.Services, app.Logger);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
await app.RunAsync();



static async Task ApplyMigrationsWithRetryAsync(IServiceProvider services, ILogger logger)
{
    const int maxAttempts = 10;
    var delay = TimeSpan.FromSeconds(2);

    for (var attempt = 1; attempt <= maxAttempts; attempt++)
    {
        try
        {
            using var scope = services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<StrongerDbContext>();

            logger.LogInformation("Applying database migrations (attempt {Attempt}/{Max})...", attempt, maxAttempts);
            
            await db.Database.MigrateAsync();

            logger.LogInformation("Database migrations applied successfully.");
            return;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Migration attempt {Attempt} failed. Retrying in {Delay}...", attempt, delay);
            await Task.Delay(delay);
        }
    }

    throw new InvalidOperationException("Failed to apply database migrations after multiple attempts.");
}