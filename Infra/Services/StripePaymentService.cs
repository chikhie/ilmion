using Stripe;
using Stripe.Checkout;
using Ilmanar.Infra.Entities;
using Microsoft.Extensions.Configuration;

namespace Ilmanar.Infra.Services;

public interface IStripePaymentService
{
    Task<Session> CreateCheckoutSessionAsync(ModuleEntity module, string userId, string? userEmail = null);
    Task<PaymentIntent> GetPaymentIntentAsync(string paymentIntentId);
    Task<Refund> CreateRefundAsync(string paymentIntentId);
}

public class StripePaymentService : IStripePaymentService
{
    private readonly IConfiguration _configuration;
    private readonly string _apiKey;
    private readonly string _successUrl;
    private readonly string _cancelUrl;

    public StripePaymentService(IConfiguration configuration)
    {
        _configuration = configuration;
        _apiKey = _configuration["Stripe:SecretKey"] ?? throw new InvalidOperationException("Stripe SecretKey not configured");
        _successUrl = _configuration["Stripe:SuccessUrl"] ?? "http://localhost:5000/payment/success";
        _cancelUrl = _configuration["Stripe:CancelUrl"] ?? "http://localhost:5000/payment/cancel";
        
        StripeConfiguration.ApiKey = _apiKey;
    }

    public async Task<Session> CreateCheckoutSessionAsync(ModuleEntity module, string userId, string? userEmail = null)
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
                        UnitAmount = (long)(module.Price * 100), // Stripe utilise des centimes
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = module.Title,
                            Description = module.Description,
                            Images = new List<string>()
                        }
                    },
                    Quantity = 1
                }
            },
            Mode = "payment",
            SuccessUrl = $"{_successUrl}?session_id={{CHECKOUT_SESSION_ID}}",
            CancelUrl = _cancelUrl,
            ClientReferenceId = userId,
            Metadata = new Dictionary<string, string>
            {
                { "module_id", module.Id.ToString() },
                { "user_id", userId }
            }
        };

        if (!string.IsNullOrEmpty(userEmail))
        {
            options.CustomerEmail = userEmail;
        }

        var service = new SessionService();
        var session = await service.CreateAsync(options);
        return session;
    }

    public async Task<PaymentIntent> GetPaymentIntentAsync(string paymentIntentId)
    {
        var service = new PaymentIntentService();
        return await service.GetAsync(paymentIntentId);
    }

    public async Task<Refund> CreateRefundAsync(string paymentIntentId)
    {
        var options = new RefundCreateOptions
        {
            PaymentIntent = paymentIntentId
        };

        var service = new RefundService();
        return await service.CreateAsync(options);
    }
}

