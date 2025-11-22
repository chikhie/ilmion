namespace Ilmanar.Infra;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

public class ApplicationDbContext : IdentityDbContext<UserEntity>
{
    public DbSet<VideoEntity> Videos { get; set; }
    public DbSet<SubjectEntity> Subjects { get; set; }
    public DbSet<ModuleEntity> Modules { get; set; }
    public DbSet<ModulePurchaseEntity> ModulePurchases { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configuration de la relation Subject -> Modules
        modelBuilder.Entity<ModuleEntity>()
            .HasOne(m => m.Subject)
            .WithMany(s => s.Modules)
            .HasForeignKey(m => m.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Configuration de la relation Module -> Video (optionnelle)
        modelBuilder.Entity<ModuleEntity>()
            .HasOne(m => m.Video)
            .WithMany()
            .HasForeignKey(m => m.VideoId)
            .OnDelete(DeleteBehavior.SetNull);
        
        // Index pour améliorer les performances
        modelBuilder.Entity<ModuleEntity>()
            .HasIndex(m => new { m.SubjectId, m.DisplayOrder });
        
        modelBuilder.Entity<SubjectEntity>()
            .HasIndex(s => s.Id);
        
        // Configuration de la relation ModulePurchase -> User
        modelBuilder.Entity<ModulePurchaseEntity>()
            .HasOne(mp => mp.User)
            .WithMany(u => u.PurchasedModules)
            .HasForeignKey(mp => mp.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Configuration de la relation ModulePurchase -> Module
        modelBuilder.Entity<ModulePurchaseEntity>()
            .HasOne(mp => mp.Module)
            .WithMany(m => m.Purchases)
            .HasForeignKey(mp => mp.ModuleId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Index pour les achats
        modelBuilder.Entity<ModulePurchaseEntity>()
            .HasIndex(mp => new { mp.UserId, mp.ModuleId });
        
        modelBuilder.Entity<ModulePurchaseEntity>()
            .HasIndex(mp => mp.StripeSessionId);
        
        // Seed data pour les matières
        SeedSubjects(modelBuilder);
    }
    
    private void SeedSubjects(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SubjectEntity>().HasData(
            new SubjectEntity { Id = 1, Label = "Mathématiques" },
            new SubjectEntity { Id = 2, Label = "Physique" },
            new SubjectEntity { Id = 3, Label = "Chimie" },
            new SubjectEntity { Id = 4, Label = "Biologie" },
            new SubjectEntity { Id = 5, Label = "Informatique" }
        );
    }
}
