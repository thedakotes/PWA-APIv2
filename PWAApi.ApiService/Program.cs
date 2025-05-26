using System.Text;
using API.Services.PlantID;
using AutoMapper;
using EventApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PWAApi.ApiService.Authentication.Data;
using PWAApi.ApiService.Authentication.Models;
using PWAApi.ApiService.Authentication.Services;
using PWAApi.ApiService.Authentication.Utility;
using PWAApi.ApiService.Helpers.Seeders;
using PWAApi.ApiService.Middleware;
using PWAApi.ApiService.Repositories;
using PWAApi.ApiService.Repositories.Event;
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
builder.Services.AddHttpContextAccessor();

//Distributed Cache where we can add/remove items to/from the Redis cache
builder.AddRedisDistributedCache("cache");

// Add services to the container.
builder.Services.AddProblemDetails();

// Register your database context (change connection string as needed)
builder.Services.AddDbContext<AppDbContext>(options =>
    options
        .UseLazyLoadingProxies()
        .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<AuthDbContext>(options =>
    options
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
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ICalendarEventService, CalendarEventService>();
builder.Services.AddScoped<IAIService, OpenAIService>();
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddScoped<IReminderService, ReminderService>();
builder.Services.AddScoped<IReminderItemService, ReminderItemService>();
builder.Services.AddScoped<IReminderTaskService, ReminderTaskService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<WateringScheduleService>();
builder.Services.AddScoped<WikimediaService>();

// Seeders
builder.Services.AddScoped<AdminUserSeeder>();
builder.Services.AddScoped<ISeeder, TaxonomySeeder>();
builder.Services.AddScoped<ISeeder, VernacularNameSeeder>(); 
builder.Services.AddScoped<SeedManagerService>();
#endregion

#region Repositories
builder.Services.AddScoped<ICalendarEventRepository, EventRepository>();
builder.Services.AddScoped<IReminderRepository, ReminderRepository>();
builder.Services.AddScoped<IReminderItemRepository, ReminderItemRepository>();
builder.Services.AddScoped<IReminderTaskRepository, ReminderTaskRepository>();
builder.Services.AddScoped<TaxonomyRepository>();
#endregion

#region Config
builder.Services.AddScoped<ICurrentUser, HttpContextCurrentUser>();
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
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
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
    /*
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("JWT validation FAILED:");
            Console.WriteLine(context.Exception.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("JWT validated successfully.");
            return Task.CompletedTask;
        }
    };
    */
});

builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Run all migrations/ISeeders on startup to populate tables with current data
using (var scope = app.Services.CreateScope())
{
    // Run migrations
    var appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    appDb.Database.Migrate();

    // Run migrations
    var authDb = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    authDb.Database.Migrate();

    // Run seeders after migrations
    //var seedManager = scope.ServiceProvider.GetRequiredService<SeedManagerService>();
    //await seedManager.RunAllAsync(db);

    try
    {
        //We may want to turn this off after the initial run? Up to you.
        var adminSeeder = scope.ServiceProvider.GetRequiredService<AdminUserSeeder>();
        await adminSeeder.SeedAdminUserAsync();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the admin user.");
    }
}

// Configure the HTTP request pipeline.
//app.UseExceptionHandler();
app.UseMiddleware<ExceptionHandlingMiddleware>();

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

/*
app.Use(async (context, next) =>
{
    Console.WriteLine("➡️ Incoming request to: " + context.Request.Path);
    Console.WriteLine("🔐 Authorization header: " + context.Request.Headers["Authorization"]);
    Console.WriteLine("👤 IsAuthenticated: " + context.User.Identity?.IsAuthenticated);
    Console.WriteLine("👤 Auth type: " + context.User.Identity?.AuthenticationType);
    await next();
});
*/

app.UseAuthorization();

// Map controllers (API endpoints)
// In previous version of ASP.NET Core (3-5), you would need to call UseRouting and UseEndpoints explicitly
// But in ASP.NET Core 6+, the MapControllers method is sufficient to set up routing for attribute-routed controllers
app.MapControllers();

// Enable deployment debugging
//Disable this when we're doing custom exception handling
//app.UseDeveloperExceptionPage();

app.Run();
