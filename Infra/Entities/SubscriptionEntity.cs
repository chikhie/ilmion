using System.ComponentModel.DataAnnotations.Schema;

namespace Ilmanar.Infra.Entities;

/// <summary>
/// Représente un abonnement utilisateur (mensuel ou annuel)
/// </summary>
public class SubscriptionEntity
{
    public Guid Id { get; set; }
    
    // Utilisateur
    public string UserId { get; set; } = string.Empty;
    public UserEntity User { get; set; } = null!;
    
    // Type d'abonnement
    public SubscriptionType Type { get; set; }
    
    // Prix payé
    [Column(TypeName = "decimal(18,2)")]
    public decimal AmountPaid { get; set; }
    
    public string Currency { get; set; } = "EUR";
    
    // Stripe
    public string? StripeSubscriptionId { get; set; }
    public string? StripeCustomerId { get; set; }
    public string? StripeSessionId { get; set; }
    
    // Statut
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;
    
    // Dates
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? CancelledDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Email pour les achats invités (optionnel)
    public string? GuestEmail { get; set; }
}

public enum SubscriptionType
{
    Annual = 0    // Annuel (unique offre à 10€/an)
}

public enum SubscriptionStatus
{
    Active = 0,      // Actif
    Expired = 1,     // Expiré
    Cancelled = 2,   // Annulé
    Pending = 3      // En attente de paiement
}

