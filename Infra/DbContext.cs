namespace KitabStock.Infra;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

public class ApplicationDbContext : IdentityDbContext<UserEntity>
{
    public DbSet<VideoEntity> Videos { get; set; }
    public DbSet<VideoPurchaseEntity> VideoPurchases { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configuration des relations
        modelBuilder.Entity<VideoPurchaseEntity>()
            .HasOne(vp => vp.User)
            .WithMany(u => u.PurchasedVideos)
            .HasForeignKey(vp => vp.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Ne pas supprimer les achats si user supprimé
        
        modelBuilder.Entity<VideoPurchaseEntity>()
            .HasOne(vp => vp.Video)
            .WithMany()
            .HasForeignKey(vp => vp.VideoId)
            .OnDelete(DeleteBehavior.Restrict); // Ne pas supprimer les achats si vidéo supprimée
        
        // Index pour améliorer les performances des requêtes
        modelBuilder.Entity<VideoPurchaseEntity>()
            .HasIndex(vp => new { vp.UserId, vp.VideoId });
        
        modelBuilder.Entity<VideoPurchaseEntity>()
            .HasIndex(vp => vp.PurchaseDate);
        
        // Index unique sur le code d'achat
        modelBuilder.Entity<VideoPurchaseEntity>()
            .HasIndex(vp => vp.PurchaseCode)
            .IsUnique();
    }
}


