namespace Ilmanar.Infra;

using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<UserEntity>
{


    // New Entities

    public DbSet<UserGameScoreEntity> UserGameScores { get; set; }
    public DbSet<SubscriptionEntity> Subscriptions { get; set; }

    // Removed: Modules, Chapters, Sections, SectionTypes, UserProgress, UserLearningLogs

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuration de la relation Subscription -> User
        modelBuilder.Entity<SubscriptionEntity>()
            .HasOne(s => s.User)
            .WithMany(u => u.Subscriptions)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Index pour les abonnements
        modelBuilder.Entity<SubscriptionEntity>()
            .HasIndex(s => s.UserId);

        // Config UserGameScore
        modelBuilder.Entity<UserGameScoreEntity>()
            .HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);



        // Seed data


    }








}
