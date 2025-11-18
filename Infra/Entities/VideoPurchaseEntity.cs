namespace KitabStock.Infra.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class VideoPurchaseEntity
{
    [Key]
    public Guid Id { get; set; }
    
    // Clé étrangère vers l'utilisateur (TOUJOURS rempli - user temporaire si invité)
    [Required]
    public string UserId { get; set; } = string.Empty;
    
    [ForeignKey("UserId")]
    public UserEntity User { get; set; } = null!;
    
    // Clé étrangère vers la vidéo
    [Required]
    public Guid VideoId { get; set; }
    
    [ForeignKey("VideoId")]
    public VideoEntity Video { get; set; } = null!;
    
    // Code d'achat unique
    [Required]
    [MaxLength(12)]
    public string PurchaseCode { get; set; } = string.Empty;
    
    // Informations sur l'achat
    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal PricePaid { get; set; }
    
    // Statut de l'achat
    public PurchaseStatus Status { get; set; } = PurchaseStatus.Pending;
    
    // Stripe Payment Intent ID
    [Required]
    [MaxLength(255)]
    public string StripePaymentIntentId { get; set; } = string.Empty;
}

public enum PurchaseStatus
{
    Pending = 0,      // En attente
    Completed = 1,    // Complété
    Failed = 2,       // Échoué
    Refunded = 3      // Remboursé
}

