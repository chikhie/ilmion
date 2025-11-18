namespace KitabStock.Api.Dtos;

public class PurchaseVideoRequest
{
    public Guid VideoId { get; set; }
    public string StripePaymentIntentId { get; set; } = string.Empty;
}

public class GuestPurchaseVideoRequest
{
    public Guid VideoId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string StripePaymentIntentId { get; set; } = string.Empty;
}

public class VideoPurchaseResponse
{
    public Guid Id { get; set; }
    public Guid VideoId { get; set; }
    public string VideoTitle { get; set; } = string.Empty;
    public string? UserId { get; set; }
    public string? PurchaseCode { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal PricePaid { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class UserPurchasedVideosResponse
{
    public Guid VideoId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public decimal PricePaid { get; set; }
    public DateTime PurchaseDate { get; set; }
}

public class DownloadVideoRequest
{
    public string PurchaseCode { get; set; } = string.Empty;
}

public class AccessVideoWithCodeResponse
{
    public bool IsValid { get; set; }
    public string? Message { get; set; }
    public VideoAccessInfo? Video { get; set; }
}

public class VideoAccessInfo
{
    public Guid VideoId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public decimal PricePaid { get; set; }
    public int AccessCount { get; set; }
    public int MaxAccessCount { get; set; }
    public DateTime? ExpiryDate { get; set; }
}

