namespace KitabStock.Infra.Payment;

using Stripe;
using Stripe.Checkout;
using Microsoft.Extensions.Configuration;
using KitabStock.Infra.Entities;

public class StripePaymentService
{
    private readonly string _secretKey;
    private readonly string _webhookSecret;
    private readonly IConfiguration _configuration;

    public StripePaymentService(IConfiguration configuration)
    {
        _configuration = configuration;
        _secretKey = configuration["Stripe:SecretKey"] ?? throw new ArgumentNullException("Stripe:SecretKey");
        _webhookSecret = configuration["Stripe:WebhookSecret"] ?? throw new ArgumentNullException("Stripe:WebhookSecret");
        
        StripeConfiguration.ApiKey = _secretKey;
    }

    /// <summary>
    /// Créer une session de paiement Stripe Checkout pour un utilisateur connecté
    /// </summary>
    public async Task<Session> CreateCheckoutSessionForUser(
        VideoEntity video, 
        string userId, 
        string userEmail,
        string successUrl,
        string cancelUrl)
    {
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "eur", // Ou configurable
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = video.Title,
                            Description = video.Description,
                        },
                        UnitAmount = (long)(video.Price * 100), // Stripe utilise les centimes
                    },
                    Quantity = 1,
                },
            },
            Mode = "payment",
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl,
            CustomerEmail = userEmail,
            Metadata = new Dictionary<string, string>
            {
                { "video_id", video.Id.ToString() },
                { "user_id", userId },
                { "purchase_type", "authenticated" }
            }
        };

        var service = new SessionService();
        return await service.CreateAsync(options);
    }

    /// <summary>
    /// Créer une session de paiement Stripe Checkout pour un invité (sans compte)
    /// </summary>
    public async Task<Session> CreateCheckoutSessionForGuest(
        VideoEntity video,
        string guestEmail,
        string successUrl,
        string cancelUrl)
    {
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "eur",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = video.Title,
                            Description = video.Description,
                        },
                        UnitAmount = (long)(video.Price * 100),
                    },
                    Quantity = 1,
                },
            },
            Mode = "payment",
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl,
            CustomerEmail = guestEmail,
            Metadata = new Dictionary<string, string>
            {
                { "video_id", video.Id.ToString() },
                { "guest_email", guestEmail },
                { "purchase_type", "guest" }
            }
        };

        var service = new SessionService();
        return await service.CreateAsync(options);
    }

    /// <summary>
    /// Récupérer une session Stripe
    /// </summary>
    public async Task<Session> GetSession(string sessionId)
    {
        var service = new SessionService();
        return await service.GetAsync(sessionId);
    }

    /// <summary>
    /// Vérifier et construire un événement webhook Stripe
    /// </summary>
    public Event ConstructWebhookEvent(string json, string stripeSignature)
    {
        try
        {
            return EventUtility.ConstructEvent(
                json,
                stripeSignature,
                _webhookSecret
            );
        }
        catch (StripeException ex)
        {
            throw new InvalidOperationException("Échec de la validation du webhook Stripe", ex);
        }
    }

    /// <summary>
    /// Créer un remboursement
    /// </summary>
    public async Task<Refund> CreateRefund(string paymentIntentId, string reason = "requested_by_customer")
    {
        var options = new RefundCreateOptions
        {
            PaymentIntent = paymentIntentId,
            Reason = reason
        };

        var service = new RefundService();
        return await service.CreateAsync(options);
    }

    /// <summary>
    /// Obtenir les détails d'un PaymentIntent
    /// </summary>
    public async Task<PaymentIntent> GetPaymentIntent(string paymentIntentId)
    {
        var service = new PaymentIntentService();
        return await service.GetAsync(paymentIntentId);
    }
}

