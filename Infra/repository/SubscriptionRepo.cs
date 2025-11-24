using Ilmanar.Infra.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ilmanar.Infra.repository;

public interface ISubscriptionRepo
{
    Task<SubscriptionEntity?> GetByIdAsync(Guid id);
    Task<SubscriptionEntity?> GetByStripeSessionIdAsync(string sessionId);
    Task<SubscriptionEntity?> GetByStripeSubscriptionIdAsync(string subscriptionId);
    Task<SubscriptionEntity?> GetActiveSubscriptionAsync(string userId);
    Task<List<SubscriptionEntity>> GetUserSubscriptionsAsync(string userId);
    Task<bool> HasActiveSubscriptionAsync(string userId);
    Task<SubscriptionEntity> CreateAsync(SubscriptionEntity subscription);
    Task UpdateAsync(SubscriptionEntity subscription);
    Task<List<SubscriptionEntity>> GetExpiredSubscriptionsAsync();
}

public class SubscriptionRepo : ISubscriptionRepo
{
    private readonly ApplicationDbContext _context;

    public SubscriptionRepo(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SubscriptionEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Subscriptions
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<SubscriptionEntity?> GetByStripeSessionIdAsync(string sessionId)
    {
        return await _context.Subscriptions
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.StripeSessionId == sessionId);
    }

    public async Task<SubscriptionEntity?> GetByStripeSubscriptionIdAsync(string subscriptionId)
    {
        return await _context.Subscriptions
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.StripeSubscriptionId == subscriptionId);
    }

    public async Task<SubscriptionEntity?> GetActiveSubscriptionAsync(string userId)
    {
        return await _context.Subscriptions
            .Where(s => s.UserId == userId && 
                       s.Status == SubscriptionStatus.Active &&
                       s.EndDate > DateTime.UtcNow)
            .OrderByDescending(s => s.EndDate)
            .FirstOrDefaultAsync();
    }

    public async Task<List<SubscriptionEntity>> GetUserSubscriptionsAsync(string userId)
    {
        return await _context.Subscriptions
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> HasActiveSubscriptionAsync(string userId)
    {
        return await _context.Subscriptions
            .AnyAsync(s => s.UserId == userId && 
                          s.Status == SubscriptionStatus.Active &&
                          s.EndDate > DateTime.UtcNow);
    }

    public async Task<SubscriptionEntity> CreateAsync(SubscriptionEntity subscription)
    {
        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync();
        return subscription;
    }

    public async Task UpdateAsync(SubscriptionEntity subscription)
    {
        _context.Subscriptions.Update(subscription);
        await _context.SaveChangesAsync();
    }

    public async Task<List<SubscriptionEntity>> GetExpiredSubscriptionsAsync()
    {
        // Récupérer les abonnements actifs mais expirés
        var now = DateTime.UtcNow;
        return await _context.Subscriptions
            .Where(s => s.Status == SubscriptionStatus.Active && s.EndDate <= now)
            .ToListAsync();
    }
}

