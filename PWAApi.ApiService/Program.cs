using System.Text;
using API.Services.PlantID;
using AutoMapper;
using EventApi.Data;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PWAApi.ApiService.Authentication;
using PWAApi.ApiService.Helpers.Seeders;
using PWAApi.ApiService.Repositories;
using PWAApi.ApiService.Services;
using PWAApi.ApiService.Services.AI;
using PWAApi.ApiService.Services.Caching;
using PWAApi.ApiService.Services.DbContext;
using PWAApi.ApiService.Services.PlantInfo;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Configure the HTTP request pipeline.
builder.Services.AddHttpClient();

//Distributed Cache where we can add/remove items to/from the Redis cache
builder.AddRedisDistributedCache("cache");

// Add services to the container.
builder.Services.AddProblemDetails();

// Register your database context (change connection string as needed)
builder.Services.AddDbContext<AppDbContext>(options =>
    options
        .UseLazyLoadingProxies()
        .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var redisConnection = config.GetConnectionString("cache"); // dynamic port auto-injected

    if (string.IsNullOrEmpty(redisConnection))
    {
        throw new InvalidOperationException("Redis connection string is not configured.");
    }

    return ConnectionMultiplexer.Connect(redisConnection);
});

builder.Services.AddScoped<ICacheService, RedisCacheService>();


// Register AutoMapper (scanning all assemblies for profiles)
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

#region Services
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IAIService, OpenAIService>();
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddScoped<AuthService>();

// Seeders
builder.Services.AddScoped<ISeeder, TaxonomySeeder>();
builder.Services.AddScoped<ISeeder, VernacularNameSeeder>(); 
builder.Services.AddScoped<SeedManagerService>();

builder.Services.AddScoped<WateringScheduleService>();
builder.Services.AddScoped<WikimediaService>();
#endregion

#region Repositories
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<TaxonomyRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
#endregion

// Set API providers from configuration
var plantIDAPIProvider = builder.Configuration["PlantIDProvider"];
switch (plantIDAPIProvider)
{
    case "PlantNet":
    default:
        builder.Services.AddScoped<IPlantIDService, PlantNetService>();
        break;
}

var plantInfoAPIProvider = builder.Configuration["PlantInfoProvider"];
switch (plantInfoAPIProvider)
{
    case "AI":
    default:
        builder.Services.AddScoped<IPlantInfoService, AIPlantInfoService>();
        break;
}

// Load user secrets
builder.Configuration.AddUserSecrets<Program>();

// Add controllers
builder.Services.AddControllers();

// Configure Swagger (if you're using it for API documentation)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.MapType<IFormFile>(() => new Microsoft.OpenApi.Models.OpenApiSchema
    {
        Type = "string",
        Format = "binary"
    });
});
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 50_000_000; // 50 MB
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; // Default API scheme
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


var app = builder.Build();

// Run all migrations/ISeeders on startup to populate tables with current data
using (var scope = app.Services.CreateScope())
{
    // Run migrations
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // Run seeders after migrations
    var seedManager = scope.ServiceProvider.GetRequiredService<SeedManagerService>();
    await seedManager.RunAllAsync(db);
}

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    // Check AutoMapper configuration
    try
    {
        var mapper = app.Services.GetRequiredService<IMapper>();
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"AutoMapper configuration error: {ex.Message}");
    }
}

// Configure the HTTP request pipeline for production
app.UseCors(MyAllowSpecificOrigins);

// Enable authorization (if applicable)

app.UseAuthentication();
app.UseAuthorization();

// Map controllers (API endpoints)
// In previous version of ASP.NET Core (3-5), you would need to call UseRouting and UseEndpoints explicitly
// But in ASP.NET Core 6+, the MapControllers method is sufficient to set up routing for attribute-routed controllers
app.MapControllers();

// Enable deployment debugging
app.UseDeveloperExceptionPage();

app.Run();
