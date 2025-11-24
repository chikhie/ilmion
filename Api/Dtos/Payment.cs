namespace Ilmanar.Api.Dtos;

/// <summary>
/// DTO pour créer un abonnement
/// </summary>
public class CreateSubscriptionDto
{
    public int SubscriptionType { get; set; } // 0 = Monthly, 1 = Annual
}

/// <summary>
/// DTO de réponse pour une session de paiement
/// </summary>
public class PaymentSessionDto
{
    public string SessionId { get; set; } = string.Empty;
    public string SessionUrl { get; set; } = string.Empty;
    public Guid SubscriptionId { get; set; }
}

/// <summary>
/// DTO pour afficher un abonnement
/// </summary>
public class SubscriptionDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal AmountPaid { get; set; }
    public string Currency { get; set; } = "EUR";
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? CancelledDate { get; set; }
    public bool IsActive { get; set; }
    public int DaysRemaining { get; set; }
}

