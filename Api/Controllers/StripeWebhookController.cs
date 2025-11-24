using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using Ilmanar.Infra.repository;
using Ilmanar.Infra.Entities;

namespace Ilmanar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StripeWebhookController : ControllerBase
{
    private readonly ISubscriptionRepo _subscriptionRepo;
    private readonly IConfiguration _configuration;
    private readonly ILogger<StripeWebhookController> _logger;

    public StripeWebhookController(
        ISubscriptionRepo subscriptionRepo,
        IConfiguration configuration,
        ILogger<StripeWebhookController> logger)
    {
        _subscriptionRepo = subscriptionRepo;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> HandleWebhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var webhookSecret = _configuration["Stripe:WebhookSecret"];

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                webhookSecret
            );

            _logger.LogInformation($"Webhook reçu: {stripeEvent.Type}");

            switch (stripeEvent.Type)
            {
                case "checkout.session.completed":
                    await HandleCheckoutSessionCompleted(stripeEvent);
                    break;

                case "checkout.session.expired":
                    await HandleCheckoutSessionExpired(stripeEvent);
                    break;

                case "payment_intent.succeeded":
                    await HandlePaymentIntentSucceeded(stripeEvent);
                    break;

                case "payment_intent.payment_failed":
                    await HandlePaymentIntentFailed(stripeEvent);
                    break;

                case "charge.refunded":
                    await HandleChargeRefunded(stripeEvent);
                    break;

                default:
                    _logger.LogInformation($"Type d'événement non géré: {stripeEvent.Type}");
                    break;
            }

            return Ok();
        }
        catch (StripeException ex)
        {
            _logger.LogError($"Erreur Stripe: {ex.Message}");
            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erreur lors du traitement du webhook: {ex.Message}");
            return StatusCode(500);
        }
    }

    private async Task HandleCheckoutSessionCompleted(Event stripeEvent)
    {
        var session = stripeEvent.Data.Object as Session;
        if (session == null) return;

        _logger.LogInformation($"Session complétée: {session.Id}");

        var subscription = await _subscriptionRepo.GetByStripeSessionIdAsync(session.Id);
        if (subscription == null)
        {
            _logger.LogWarning($"Abonnement non trouvé pour la session: {session.Id}");
            return;
        }

        subscription.Status = SubscriptionStatus.Active;
        subscription.StartDate = DateTime.UtcNow;
        subscription.UpdatedAt = DateTime.UtcNow;

        await _subscriptionRepo.UpdateAsync(subscription);
        _logger.LogInformation($"Abonnement {subscription.Id} activé");
    }

    private async Task HandleCheckoutSessionExpired(Event stripeEvent)
    {
        var session = stripeEvent.Data.Object as Session;
        if (session == null) return;

        _logger.LogInformation($"Session expirée: {session.Id}");

        var subscription = await _subscriptionRepo.GetByStripeSessionIdAsync(session.Id);
        if (subscription == null) return;

        subscription.Status = SubscriptionStatus.Expired;
        subscription.UpdatedAt = DateTime.UtcNow;
        await _subscriptionRepo.UpdateAsync(subscription);
        _logger.LogInformation($"Abonnement {subscription.Id} marqué comme expiré");
    }

    private async Task HandlePaymentIntentSucceeded(Event stripeEvent)
    {
        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
        if (paymentIntent == null) return;

        _logger.LogInformation($"PaymentIntent réussi: {paymentIntent.Id}");
        
        // Le CheckoutSessionCompleted devrait déjà gérer cela,
        // mais on peut ajouter une logique supplémentaire ici si nécessaire
    }

    private async Task HandlePaymentIntentFailed(Event stripeEvent)
    {
        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
        if (paymentIntent == null) return;

        _logger.LogWarning($"PaymentIntent échoué: {paymentIntent.Id}");
        
        // Trouver l'achat par PaymentIntentId (si disponible)
        // Vous devrez peut-être ajuster cette logique selon votre implémentation
    }

    private async Task HandleChargeRefunded(Event stripeEvent)
    {
        var charge = stripeEvent.Data.Object as Charge;
        if (charge == null) return;

        _logger.LogInformation($"Remboursement effectué: {charge.Id}");

        // Trouver l'achat correspondant et le marquer comme remboursé
        // Vous devrez peut-être rechercher par PaymentIntentId
        var paymentIntentId = charge.PaymentIntentId;
        if (string.IsNullOrEmpty(paymentIntentId)) return;

        // Note: Vous devrez peut-être ajouter une méthode dans le repo pour rechercher par PaymentIntentId
        _logger.LogInformation($"Traitement du remboursement pour PaymentIntent: {paymentIntentId}");
    }
}

