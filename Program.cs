using MuslimAds;
using Microsoft.EntityFrameworkCore; // Cette directive est nécessaire pour UseSqlite
using Microsoft.AspNetCore.Identity;
using MuslimAds.Infra.Entities;
using MuslimAds.Infra;
using MuslimAds.Api.Interfaces; // Ajoutez cette ligne
using MuslimAds.Infra.Mail;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")) // Utilise "DefaultConnection" comme clé
);
builder.Services.AddScoped<IMailService, SmtpMailService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();


builder.Services.AddIdentity<UserEntity, IdentityRole>(options => {
    options.SignIn.RequireConfirmedEmail = true; // Ajoutez cette ligne pour exiger la confirmation d'e-mail
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
