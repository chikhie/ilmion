using System.IdentityModel.Tokens.Jwt; // AJOUTER CETTE LIGNE
using System.Security.Claims; // AJOUTER CETTE LIGNE
using System.Text;
using Ilmanar;
using Ilmanar.Api.Hubs; // Added
using Ilmanar.Api.Interfaces; // Ajoutez cette ligne
using Ilmanar.Api.Services; // Restored
using Ilmanar.Infra;
using Ilmanar.Infra.Entities;
using Ilmanar.Infra.Mail;
using Ilmanar.Infra.Services; // Added
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles; // Add this line
using Microsoft.EntityFrameworkCore; // Cette directive est nécessaire pour UseSqlite/UseNpgsql
using Microsoft.Extensions.FileProviders; // Add this line
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; // AJOUTER CETTE LIGNE
using Microsoft.OpenApi.Models; // AJOUTER CETTE LIGNE

// Désactiver le mapping par défaut des claims JWT
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var builder = WebApplication.CreateBuilder(args);

// Configuration de la base de données (PostgreSQL ou SQLite selon l'environnement)
var connectionString = builder.Configuration.GetConnectionString("DevDbConnection");

// Configuration pour accepter les DateTime sans Kind=UTC dans PostgreSQL
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (connectionString != null && connectionString.Contains("Host="))
    {
        // PostgreSQL pour la production/Docker
        options.UseNpgsql(connectionString);
    }
    else
    {
        // SQLite pour le développement local
        options.UseSqlite(connectionString ?? "Data Source=db.sqlite");
    }
});
builder.Services.AddScoped<IMailService, MailtrapMailService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUserProvider, UserProvider>();
builder.Services.AddScoped<UserRepo>();

// Services de paiement et abonnements
builder.Services.AddScoped<Ilmanar.Infra.Services.IStripePaymentService, Ilmanar.Infra.Services.StripePaymentService>();
builder.Services.AddScoped<Ilmanar.Infra.repository.ISubscriptionRepo, Ilmanar.Infra.repository.SubscriptionRepo>();

// Service de chiffrement des composants
builder.Services.AddScoped<ComponentEncryptionService>();

// Multiplayer Services
builder.Services.AddSignalR();
builder.Services.AddSingleton<ILobbyService, LobbyService>();
// MongoDB Configuration REMOVED
// BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard)); 
// builder.Services.Configure<Ilmanar.Infra.Settings.MongoDbSettings>(
//     builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<QuizService>();
builder.Services.AddSingleton<GameService>();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Ilmanar API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Veuillez entrer un token JWT valide",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.AddControllers();

// Configuration CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy
            .SetIsOriginAllowed(origin => true) // Allow any origin but reflect it for credentials support
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });

    options.AddPolicy("ProdCors", policy =>
    {
        policy
            .WithOrigins("https://ilmanar.site", "https://api.ilmanar.site", "https://ilmanar.chikhibra.workers.dev")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddIdentity<UserEntity, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = true; // Ajoutez cette ligne pour exiger la confirmation d'e-mail
    options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier; // Utiliser NameIdentifier pour l'ID utilisateur
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configurer Identity pour ne pas rediriger vers /Account/Login
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = 403;
        return Task.CompletedTask;
    };
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

            RoleClaimType = ClaimTypes.Role
        };

        // Empêcher la redirection automatique vers /Account/Login
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                var result = System.Text.Json.JsonSerializer.Serialize(new { message = "Non autorisé" });
                return context.Response.WriteAsync(result);
            }
        };
    });


builder.Services.AddAuthorization();
var app = builder.Build();

// app.UseExceptionHandler(errorApp =>
// {
//     errorApp.Run(async context =>
//     {
//         context.Response.StatusCode = 500;
//         context.Response.ContentType = "application/json";

//         // IMPORTANT : headers CORS même en cas d'erreur
//         context.Response.Headers["Access-Control-Allow-Origin"] = "https://ilmanar.site";
//         context.Response.Headers["Access-Control-Allow-Headers"] = "content-type, authorization";
//         context.Response.Headers["Access-Control-Allow-Methods"] = "POST, OPTIONS";

//         await context.Response.WriteAsync(
//             System.Text.Json.JsonSerializer.Serialize(new
//             {
//                 error = "Internal Server Error"
//             })
//         );
//     });
// });


// Configuration Swagger (uniquement en développement)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Ilmanar API V1");
        options.RoutePrefix = "swagger";
    });
}

// CORS doit être avant Authentication et Authorization
app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCors");
}
else
{
    app.UseCors("ProdCors");
}

app.UseAuthentication();
app.UseAuthorization();





// Configure static files for videos


// Servir le frontend Nuxt depuis wwwroot
var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
if (!Directory.Exists(wwwrootPath))
{
    Directory.CreateDirectory(wwwrootPath);
}

// Créer le dossier uploads pour les photos de profil
var uploadsPath = Path.Combine(wwwrootPath, "uploads", "profiles");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

// Créer le dossier protected pour les composants chiffrés
var protectedPath = Path.Combine(wwwrootPath, "protected", "components");
if (!Directory.Exists(protectedPath))
{
    Directory.CreateDirectory(protectedPath);
}

app.UseDefaultFiles(); // Sert index.html par défaut
app.UseStaticFiles(); // Sert tous les fichiers statiques de wwwroot

// API Controllers
app.MapControllers();
app.MapHub<GameHub>("/api/gameHub");


// Fallback pour le SPA - redirige toutes les routes non-API vers index.html
app.MapFallbackToFile("index.html");

// Initialiser la base de données uniquement si demandé via l'argument --seed
if (args.Contains("--seed"))
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            await Ilmanar.Infra.Data.DbInitializer.Initialize(services);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Une erreur est survenue lors du seeding de la DB.");
        }
    }
}

app.Run();

