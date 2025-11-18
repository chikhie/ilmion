using KitabStock;
using Microsoft.EntityFrameworkCore; // Cette directive est nécessaire pour UseSqlite
using Microsoft.AspNetCore.Identity;
using KitabStock.Infra.Entities;
using KitabStock.Infra;
using KitabStock.Api.Interfaces; // Ajoutez cette ligne
using KitabStock.Infra.Mail;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using KitabStock.Api.Services; // AJOUTER CETTE LIGNE
using Microsoft.OpenApi.Models; // AJOUTER CETTE LIGNE
using System.Security.Claims; // AJOUTER CETTE LIGNE
using Microsoft.Extensions.FileProviders; // Add this line
using Microsoft.AspNetCore.StaticFiles; // Add this line

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddScoped<IMailService, SmtpMailService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // AJOUTER CETTE LIGNE
builder.Services.AddScoped<IUserProvider, UserProvider>(); // AJOUTER CETTE LIGNE
builder.Services.AddScoped<UserRepo>(); // AJOUTER CETTE LIGNE
builder.Services.AddScoped<KitabStock.Infra.repository.VideoPurchaseRepo>(); // Repository pour les achats de vidéos
builder.Services.AddScoped<KitabStock.Infra.Payment.StripePaymentService>(); // Service de paiement Stripe

// Configurer les timeouts et limites pour l'upload de vidéos
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 5368709120; // 5 GB
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartHeadersLengthLimit = int.MaxValue;
});

// Configurer Kestrel pour accepter de gros fichiers
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 5368709120; // 5 GB
    serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(30);
    serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(30);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "KitabStock API", Version = "v1" });
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


builder.Services.AddIdentity<UserEntity, IdentityRole>(options => {
    options.SignIn.RequireConfirmedEmail = true; // Ajoutez cette ligne pour exiger la confirmation d'e-mail
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

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

            NameClaimType = ClaimTypes.NameIdentifier,
            RoleClaimType = ClaimTypes.Role
        };
    });


builder.Services.AddAuthorization();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "KitabStock API V1");
    options.RoutePrefix = string.Empty; // Définit Swagger UI comme page d'accueil
});

app.UseAuthentication();
app.UseAuthorization();

// Configure static files for the videoTest directory
var videoPath = Path.Combine(Directory.GetCurrentDirectory(), "Infra", "res", "videos", "videoTest");
if (!Directory.Exists(videoPath))
{
    Directory.CreateDirectory(videoPath);
}

var provider = new FileExtensionContentTypeProvider();
// Add .m3u8 and .ts to the MIME type mapping if not already present
provider.Mappings[".m3u8"] = "application/x-mpegURL";
provider.Mappings[".ts"] = "video/MP2T";

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(videoPath),
    RequestPath = "/videos", // The URL path to access the files
    ContentTypeProvider = provider
});

app.MapControllers();

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.Run();
