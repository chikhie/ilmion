using Ilmanar.Infra.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ilmanar.Infra.repository;

public interface IModulePurchaseRepo
{
    Task<ModulePurchaseEntity?> GetByIdAsync(Guid id);
    Task<ModulePurchaseEntity?> GetByStripeSessionIdAsync(string sessionId);
    Task<List<ModulePurchaseEntity>> GetUserPurchasesAsync(string userId);
    Task<bool> HasUserPurchasedModuleAsync(string userId, Guid moduleId);
    Task<ModulePurchaseEntity> CreateAsync(ModulePurchaseEntity purchase);
    Task UpdateAsync(ModulePurchaseEntity purchase);
    Task<List<ModulePurchaseEntity>> GetPendingPurchasesAsync();
}

public class ModulePurchaseRepo : IModulePurchaseRepo
{
    private readonly ApplicationDbContext _context;

    public ModulePurchaseRepo(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ModulePurchaseEntity?> GetByIdAsync(Guid id)
    {
        return await _context.ModulePurchases
            .Include(mp => mp.Module)
            .Include(mp => mp.User)
            .FirstOrDefaultAsync(mp => mp.Id == id);
    }

    public async Task<ModulePurchaseEntity?> GetByStripeSessionIdAsync(string sessionId)
    {
        return await _context.ModulePurchases
            .Include(mp => mp.Module)
            .Include(mp => mp.User)
            .FirstOrDefaultAsync(mp => mp.StripeSessionId == sessionId);
    }

    public async Task<List<ModulePurchaseEntity>> GetUserPurchasesAsync(string userId)
    {
        return await _context.ModulePurchases
            .Include(mp => mp.Module)
                .ThenInclude(m => m.Subject)
            .Where(mp => mp.UserId == userId && mp.Status == PurchaseStatus.Completed)
            .OrderByDescending(mp => mp.PurchaseDate)
            .ToListAsync();
    }

    public async Task<bool> HasUserPurchasedModuleAsync(string userId, Guid moduleId)
    {
        return await _context.ModulePurchases
            .AnyAsync(mp => mp.UserId == userId && 
                           mp.ModuleId == moduleId && 
                           mp.Status == PurchaseStatus.Completed);
    }

    public async Task<ModulePurchaseEntity> CreateAsync(ModulePurchaseEntity purchase)
    {
        _context.ModulePurchases.Add(purchase);
        await _context.SaveChangesAsync();
        return purchase;
    }

    public async Task UpdateAsync(ModulePurchaseEntity purchase)
    {
        _context.ModulePurchases.Update(purchase);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ModulePurchaseEntity>> GetPendingPurchasesAsync()
    {
        // Récupérer les achats en attente depuis plus de 24h
        var yesterday = DateTime.UtcNow.AddDays(-1);
        return await _context.ModulePurchases
            .Where(mp => mp.Status == PurchaseStatus.Pending && mp.PurchaseDate < yesterday)
            .ToListAsync();
    }
}

