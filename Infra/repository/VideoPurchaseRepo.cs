namespace KitabStock.Infra.repository;

using KitabStock.Infra.Entities;
using Microsoft.EntityFrameworkCore;

public class VideoPurchaseRepo
{
    private readonly ApplicationDbContext _context;

    public VideoPurchaseRepo(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Vérifier si un utilisateur a acheté une vidéo
    /// </summary>
    public async Task<bool> HasUserPurchasedVideo(string userId, Guid videoId)
    {
        return await _context.VideoPurchases
            .AnyAsync(vp => vp.UserId == userId 
                         && vp.VideoId == videoId 
                         && vp.Status == PurchaseStatus.Completed);
    }

    /// <summary>
    /// Obtenir toutes les vidéos achetées par un utilisateur
    /// </summary>
    public async Task<List<VideoPurchaseEntity>> GetUserPurchases(string userId)
    {
        return await _context.VideoPurchases
            .Include(vp => vp.Video)
            .Where(vp => vp.UserId == userId && vp.Status == PurchaseStatus.Completed)
            .OrderByDescending(vp => vp.PurchaseDate)
            .ToListAsync();
    }

    /// <summary>
    /// Obtenir un achat spécifique
    /// </summary>
    public async Task<VideoPurchaseEntity?> GetPurchaseById(Guid purchaseId)
    {
        return await _context.VideoPurchases
            .Include(vp => vp.Video)
            .Include(vp => vp.User)
            .FirstOrDefaultAsync(vp => vp.Id == purchaseId);
    }

    /// <summary>
    /// Créer un nouvel achat
    /// </summary>
    public async Task<VideoPurchaseEntity> CreatePurchase(VideoPurchaseEntity purchase)
    {
        _context.VideoPurchases.Add(purchase);
        await _context.SaveChangesAsync();
        return purchase;
    }

    /// <summary>
    /// Obtenir les statistiques d'achat d'une vidéo
    /// </summary>
    public async Task<(int totalSales, decimal totalRevenue)> GetVideoSalesStats(Guid videoId)
    {
        var purchases = await _context.VideoPurchases
            .Where(vp => vp.VideoId == videoId && vp.Status == PurchaseStatus.Completed)
            .ToListAsync();

        return (purchases.Count, purchases.Sum(p => p.PricePaid));
    }

    /// <summary>
    /// Obtenir tous les achats d'une vidéo
    /// </summary>
    public async Task<List<VideoPurchaseEntity>> GetVideoPurchases(Guid videoId)
    {
        return await _context.VideoPurchases
            .Include(vp => vp.User)
            .Where(vp => vp.VideoId == videoId)
            .OrderByDescending(vp => vp.PurchaseDate)
            .ToListAsync();
    }

    /// <summary>
    /// Mettre à jour le statut d'un achat
    /// </summary>
    public async Task<bool> UpdatePurchaseStatus(Guid purchaseId, PurchaseStatus newStatus)
    {
        var purchase = await _context.VideoPurchases.FindAsync(purchaseId);
        if (purchase == null)
            return false;

        purchase.Status = newStatus;
        await _context.SaveChangesAsync();
        return true;
    }
}

