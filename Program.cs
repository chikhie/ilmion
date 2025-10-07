using MuslimAds;
using Microsoft.EntityFrameworkCore; // Cette directive est nécessaire pour UseSqlite
using Microsoft.AspNetCore.Identity;
using MuslimAds.Infra.Entities;
using MuslimAds.Infra;
using MuslimAds.Api.Interfaces; // Ajoutez cette ligne
using MuslimAds.Infra.Mail;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MuslimAds.Api.Services; // AJOUTER CETTE LIGNE
using Microsoft.OpenApi.Models; // AJOUTER CETTE LIGNE
using System.Security.Claims; // AJOUTER CETTE LIGNE

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddScoped<IMailService, SmtpMailService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // AJOUTER CETTE LIGNE
builder.Services.AddScoped<IUserProvider, UserProvider>(); // AJOUTER CETTE LIGNE
builder.Services.AddScoped<UserRepo>(); // AJOUTER CETTE LIGNE

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "MuslimAds API", Version = "v1" });
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
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "MuslimAds API V1");
    options.RoutePrefix = string.Empty; // Définit Swagger UI comme page d'accueil
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.Run();
