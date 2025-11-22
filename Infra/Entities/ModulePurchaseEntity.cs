using System.ComponentModel.DataAnnotations.Schema;

namespace Ilmanar.Infra.Entities;

/// <summary>
/// Représente l'achat d'un module par un utilisateur
/// </summary>
public class ModulePurchaseEntity
{
    public Guid Id { get; set; }
    
    // Utilisateur
    public string UserId { get; set; } = string.Empty;
    public UserEntity User { get; set; } = null!;
    
    // Module acheté
    public Guid ModuleId { get; set; }
    public ModuleEntity Module { get; set; } = null!;
    
    // Informations de paiement
    [Column(TypeName = "decimal(18,2)")]
    public decimal AmountPaid { get; set; }
    
    public string Currency { get; set; } = "EUR";
    
    // Stripe
    public string? StripeSessionId { get; set; }
    public string? StripePaymentIntentId { get; set; }
    
    // Statut
    public PurchaseStatus Status { get; set; } = PurchaseStatus.Pending;
    
    // Dates
    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedDate { get; set; }
    
    // Email pour les achats invités (optionnel)
    public string? GuestEmail { get; set; }
}

public enum PurchaseStatus
{
    Pending = 0,      // En attente de paiement
    Completed = 1,    // Paiement complété
    Failed = 2,       // Paiement échoué
    Refunded = 3      // Remboursé
}

