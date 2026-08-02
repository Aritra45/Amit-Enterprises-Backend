using System.Security.Claims;
using System.Text;
using API.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Modules.Booking.Extensions;
using Modules.Booking.Infrastructure.Persistence;
using Modules.Identity.Extensions;
using Modules.Identity.Infrastructure.Persistence;
using Modules.Master.Extensions;
using Modules.Master.Infrastructure.Persistence;
using Serilog;
using Shared.Core.Settings;
using Shared.Infrastructure.Extensions;
using Shared.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);

const string FrontendCorsPolicy = "Frontend";

// ── LOGGING ──────────────────────────────────────────────────────────────
builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Console()
        .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day);
});

// ── MODULES ──────────────────────────────────────────────────────────────
builder.Services.AddSharedInfrastructure();
builder.Services.AddIdentityModule(builder.Configuration);
builder.Services.AddMasterModule(builder.Configuration);
builder.Services.AddBookingModule(builder.Configuration);

// ── CORS ─────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy(FrontendCorsPolicy, policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy.SetIsOriginAllowed(origin =>
            {
                if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri))
                {
                    return false;
                }

                return uri.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase)
                    || uri.Host.Equals("127.0.0.1", StringComparison.OrdinalIgnoreCase);
            });
        }
        else
        {
            var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
            policy.WithOrigins(allowedOrigins);
        }

        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// ── JWT SETTINGS (STRONGLY TYPED) ─────────────────────────────────────────
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

if (jwtSettings is null || string.IsNullOrWhiteSpace(jwtSettings.Key))
{
    throw new InvalidOperationException("JwtSettings are not configured. Check appsettings.json.");
}

// ── CLOUDINARY SETTINGS (STRONGLY TYPED) ──────────────────────────────────
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));

// ── REGISTER JWT BEARER EVENTS HANDLER (checks the logout blacklist) ─────
builder.Services.AddScoped<JwtBearerEventsHandler>();

// ── JWT AUTHENTICATION ────────────────────────────────────────────────────
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),

            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,

            RoleClaimType = ClaimTypes.Role
        };

        options.EventsType = typeof(JwtBearerEventsHandler);
    });

// ── AUTHORIZATION ────────────────────────────────────────────────────────
builder.Services.AddAuthorization();

// ── CONTROLLERS (one MVC application part per module) ────────────────────
builder.Services.AddControllers()
    .AddApplicationPart(typeof(Modules.Identity.IdentityAssemblyReference).Assembly)
    .AddApplicationPart(typeof(Modules.Master.MasterAssemblyReference).Assembly)
    .AddApplicationPart(typeof(Modules.Booking.BookingAssemblyReference).Assembly);

// ── SWAGGER ────────────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Amit Enterprises POS API",
        Version = "v1",
        Description = "Grocery POS and Inventory Management System"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter: Bearer {your-token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ── SWAGGER UI (development only) ─────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Amit Enterprises POS API v1");
    });
}

// ── MIDDLEWARE ───────────────────────────────────────────────────────────
app.UseSharedExceptionMiddleware();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors(FrontendCorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ── AUTO MIGRATION + SEEDING ───────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var identityDb = services.GetRequiredService<IdentityDbContext>();
        await identityDb.Database.MigrateAsync();
        await IdentityDbSeeder.SeedAsync(identityDb, services.GetRequiredService<Shared.Core.Abstractions.IPasswordHasher>());

        var masterDb = services.GetRequiredService<MasterDbContext>();
        await masterDb.Database.MigrateAsync();

        var bookingDb = services.GetRequiredService<BookingDbContext>();
        await bookingDb.Database.MigrateAsync();

        logger.LogInformation("Database migrations applied successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error applying database migrations.");
    }
}

app.Run();
