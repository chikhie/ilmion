namespace Ilmanar.Api.Dtos;

/// <summary>
/// DTO pour initier un paiement
/// </summary>
public class CreatePaymentDto
{
    public Guid ModuleId { get; set; }
}

/// <summary>
/// DTO de réponse pour une session de paiement
/// </summary>
public class PaymentSessionDto
{
    public string SessionId { get; set; } = string.Empty;
    public string SessionUrl { get; set; } = string.Empty;
    public Guid PurchaseId { get; set; }
}

/// <summary>
/// DTO pour afficher un achat
/// </summary>
public class ModulePurchaseDto
{
    public Guid Id { get; set; }
    public Guid ModuleId { get; set; }
    public string ModuleTitle { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public decimal AmountPaid { get; set; }
    public string Currency { get; set; } = "EUR";
    public string Status { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public DateTime? CompletedDate { get; set; }
}

