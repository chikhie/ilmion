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
    public DbSet<ChapterEntity> Chapters { get; set; }
    public DbSet<SectionEntity> Sections { get; set; }
    public DbSet<SectionTypeEntity> SectionTypes { get; set; }
    public DbSet<SubscriptionEntity> Subscriptions { get; set; }
    
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
        
        // Configuration de la relation Module -> Chapters
        modelBuilder.Entity<ChapterEntity>()
            .HasOne(c => c.Module)
            .WithMany(m => m.Chapters)
            .HasForeignKey(c => c.ModuleId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Configuration de la relation Chapter -> Sections
        modelBuilder.Entity<SectionEntity>()
            .HasOne(s => s.Chapter)
            .WithMany(c => c.Sections)
            .HasForeignKey(s => s.ChapterId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Configuration de la relation Section -> SectionType
        modelBuilder.Entity<SectionEntity>()
            .HasOne(s => s.SectionType)
            .WithMany(st => st.Sections)
            .HasForeignKey(s => s.SectionTypeId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Index pour améliorer les performances
        modelBuilder.Entity<ChapterEntity>()
            .HasIndex(c => new { c.ModuleId, c.DisplayOrder });
        
        modelBuilder.Entity<SectionEntity>()
            .HasIndex(s => new { s.ChapterId, s.DisplayOrder });
        
        modelBuilder.Entity<SubjectEntity>()
            .HasIndex(s => s.Id);
        
        // Configuration de la relation Subscription -> User
        modelBuilder.Entity<SubscriptionEntity>()
            .HasOne(s => s.User)
            .WithMany(u => u.Subscriptions)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Index pour les abonnements
        modelBuilder.Entity<SubscriptionEntity>()
            .HasIndex(s => s.UserId);
        
        modelBuilder.Entity<SubscriptionEntity>()
            .HasIndex(s => s.StripeSubscriptionId);
        
        modelBuilder.Entity<SubscriptionEntity>()
            .HasIndex(s => new { s.UserId, s.Status });
        
        // Seed data
        SeedSectionTypes(modelBuilder);
        SeedSubjects(modelBuilder);
        SeedModules(modelBuilder);
        SeedChapters(modelBuilder);
        SeedSections(modelBuilder);
    }
    
    private void SeedSectionTypes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SectionTypeEntity>().HasData(
            new SectionTypeEntity { Id = 1, Label = "Texte" },
            new SectionTypeEntity { Id = 2, Label = "Image" },
            new SectionTypeEntity { Id = 3, Label = "Vidéo" },
            new SectionTypeEntity { Id = 4, Label = "Quiz" },
            new SectionTypeEntity { Id = 5, Label = "Code" },
            new SectionTypeEntity { Id = 6, Label = "Exercice" }
        );
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
    
    private void SeedModules(ModelBuilder modelBuilder)
    {
        var now = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        modelBuilder.Entity<ModuleEntity>().HasData(
            // Modules de Mathématiques
            new ModuleEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Label = "Algèbre", SubjectId = 1, DisplayOrder = 1, CreatedAt = now, UpdatedAt = now },
            new ModuleEntity { Id = Guid.Parse("11111111-1111-1111-1111-111111111112"), Label = "Géométrie", SubjectId = 1, DisplayOrder = 2, CreatedAt = now, UpdatedAt = now },
            
            // Modules de Physique
            new ModuleEntity { Id = Guid.Parse("22222222-2222-2222-2222-222222222221"), Label = "Mécanique", SubjectId = 2, DisplayOrder = 1, CreatedAt = now, UpdatedAt = now },
            new ModuleEntity { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Label = "Thermodynamique", SubjectId = 2, DisplayOrder = 2, CreatedAt = now, UpdatedAt = now },
            
            // Modules de Chimie
            new ModuleEntity { Id = Guid.Parse("33333333-3333-3333-3333-333333333331"), Label = "Chimie Organique", SubjectId = 3, DisplayOrder = 1, CreatedAt = now, UpdatedAt = now },
            
            // Modules de Biologie
            new ModuleEntity { Id = Guid.Parse("44444444-4444-4444-4444-444444444441"), Label = "Biologie Cellulaire", SubjectId = 4, DisplayOrder = 1, CreatedAt = now, UpdatedAt = now }
        );
    }
    
    private void SeedChapters(ModelBuilder modelBuilder)
    {
        var now = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        modelBuilder.Entity<ChapterEntity>().HasData(
            // Chapitres du module Algèbre
            new ChapterEntity 
            { 
                Id = Guid.Parse("aaaaaaaa-1111-1111-1111-111111111111"), 
                Title = "Introduction à l'algèbre",
                Description = "Découvrez les bases fondamentales de l'algèbre et ses applications",
                DisplayOrder = 1,
                DurationMinutes = 20,
                ModuleId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                VideoUrl = "https://test-streams.mux.dev/x36xhzz/x36xhzz.m3u8",
                CreatedAt = now,
                UpdatedAt = now
            },
            new ChapterEntity 
            { 
                Id = Guid.Parse("aaaaaaaa-1111-1111-1111-111111111112"), 
                Title = "Les équations du premier degré",
                Description = "Apprenez à résoudre les équations linéaires",
                DisplayOrder = 2,
                DurationMinutes = 35,
                ModuleId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                CreatedAt = now,
                UpdatedAt = now
            },
            new ChapterEntity 
            { 
                Id = Guid.Parse("aaaaaaaa-1111-1111-1111-111111111113"), 
                Title = "Les systèmes d'équations",
                Description = "Résolution de systèmes d'équations à plusieurs inconnues",
                DisplayOrder = 3,
                DurationMinutes = 45,
                ModuleId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                CreatedAt = now,
                UpdatedAt = now
            },
            
            // Chapitres du module Mécanique
            new ChapterEntity 
            { 
                Id = Guid.Parse("bbbbbbbb-2222-2222-2222-222222222221"), 
                Title = "Les lois de Newton",
                Description = "Comprendre les trois lois fondamentales du mouvement",
                DisplayOrder = 1,
                DurationMinutes = 30,
                ModuleId = Guid.Parse("22222222-2222-2222-2222-222222222221"),
                VideoUrl = "https://www.youtube.com/watch?v=kKKM8Y-u7ds",
                CreatedAt = now,
                UpdatedAt = now
            },
            new ChapterEntity 
            { 
                Id = Guid.Parse("bbbbbbbb-2222-2222-2222-222222222222"), 
                Title = "L'énergie cinétique",
                Description = "L'énergie liée au mouvement des corps",
                DisplayOrder = 2,
                DurationMinutes = 25,
                ModuleId = Guid.Parse("22222222-2222-2222-2222-222222222221"),
                CreatedAt = now,
                UpdatedAt = now
            },
            
            // Chapitres du module Chimie Organique
            new ChapterEntity 
            { 
                Id = Guid.Parse("cccccccc-3333-3333-3333-333333333331"), 
                Title = "Les hydrocarbures",
                Description = "Introduction aux composés organiques simples",
                DisplayOrder = 1,
                DurationMinutes = 40,
                ModuleId = Guid.Parse("33333333-3333-3333-3333-333333333331"),
                CreatedAt = now,
                UpdatedAt = now
            }
        );
    }
    
    private void SeedSections(ModelBuilder modelBuilder)
    {
        var now = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        modelBuilder.Entity<SectionEntity>().HasData(
            // Sections du chapitre "Introduction à l'algèbre"
            new SectionEntity
            {
                Id = Guid.Parse("eeeeeeee-1111-1111-1111-111111111111"),
                Title = "Qu'est-ce que l'algèbre ?",
                Content = "L'algèbre est une branche des mathématiques qui permet de résoudre des problèmes en utilisant des symboles et des lettres pour représenter des nombres et des quantités.\n\nElle est essentielle pour comprendre de nombreux concepts mathématiques avancés.",
                SectionTypeId = 1, // Texte
                DisplayOrder = 1,
                ChapterId = Guid.Parse("aaaaaaaa-1111-1111-1111-111111111111"),
                CreatedAt = now,
                UpdatedAt = now
            },
            new SectionEntity
            {
                Id = Guid.Parse("eeeeeeee-1111-1111-1111-111111111112"),
                Title = "Vidéo d'introduction",
                Content = "https://test-streams.mux.dev/x36xhzz/x36xhzz.m3u8",
                SectionTypeId = 3, // Vidéo
                DisplayOrder = 2,
                ChapterId = Guid.Parse("aaaaaaaa-1111-1111-1111-111111111111"),
                CreatedAt = now,
                UpdatedAt = now
            },
            new SectionEntity
            {
                Id = Guid.Parse("eeeeeeee-1111-1111-1111-111111111113"),
                Title = "Quiz de compréhension",
                Content = @"{""questions"":[{""id"":1,""question"":""Que représente le symbole 'x' en algèbre ?"",""options"":[""Un nombre fixe"",""Une variable"",""Une opération"",""Un résultat""],""correctAnswer"":1},{""id"":2,""question"":""Qu'est-ce qu'une équation ?"",""options"":[""Une addition"",""Une égalité entre deux expressions"",""Un nombre"",""Une division""],""correctAnswer"":1}]}",
                SectionTypeId = 4, // Quiz
                DisplayOrder = 3,
                ChapterId = Guid.Parse("aaaaaaaa-1111-1111-1111-111111111111"),
                CreatedAt = now,
                UpdatedAt = now
            },
            
            // Sections du chapitre "Les lois de Newton"
            new SectionEntity
            {
                Id = Guid.Parse("ffffffff-2222-2222-2222-222222222221"),
                Title = "Introduction aux lois de Newton",
                Content = "Isaac Newton a formulé trois lois fondamentales qui décrivent le mouvement des corps. Ces lois sont à la base de la mécanique classique.\n\n**Première loi** : Un corps au repos reste au repos, un corps en mouvement reste en mouvement à vitesse constante, sauf si une force extérieure agit sur lui.\n\n**Deuxième loi** : F = ma (Force = masse × accélération)\n\n**Troisième loi** : À toute action correspond une réaction égale et opposée.",
                SectionTypeId = 1, // Texte
                DisplayOrder = 1,
                ChapterId = Guid.Parse("bbbbbbbb-2222-2222-2222-222222222221"),
                CreatedAt = now,
                UpdatedAt = now
            },
            new SectionEntity
            {
                Id = Guid.Parse("ffffffff-2222-2222-2222-222222222222"),
                Title = "Illustration des forces",
                Content = "https://images.unsplash.com/photo-1635070041078-e363dbe005cb?q=80&w=800&auto=format&fit=crop",
                SectionTypeId = 2, // Image
                DisplayOrder = 2,
                ChapterId = Guid.Parse("bbbbbbbb-2222-2222-2222-222222222221"),
                CreatedAt = now,
                UpdatedAt = now
            },
            new SectionEntity
            {
                Id = Guid.Parse("ffffffff-2222-2222-2222-222222222223"),
                Title = "Exercice pratique",
                Content = "Calculez la force nécessaire pour déplacer un objet de 5 kg avec une accélération de 2 m/s².\n\nRappel : F = ma\n\nRéponse : ____ N",
                SectionTypeId = 6, // Exercice
                DisplayOrder = 3,
                ChapterId = Guid.Parse("bbbbbbbb-2222-2222-2222-222222222221"),
                CreatedAt = now,
                UpdatedAt = now
            }
        );
    }
}
