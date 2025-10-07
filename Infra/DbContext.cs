namespace MuslimAds.Infra;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

public class ApplicationDbContext : IdentityDbContext<UserEntity>
{

    public DbSet<AdEntity> Ads { get; set; }
    public DbSet<WebsiteEntity> Websites { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AdEntity>()
            .HasOne(e => e.User)
            .WithMany(e => e.Ads)
            .HasForeignKey(e => e.UserId);

        modelBuilder.Entity<WebsiteEntity>()
            .HasOne(e => e.User)
            .WithMany(e => e.Websites)
            .HasForeignKey(e => e.UserId);
    }
}


