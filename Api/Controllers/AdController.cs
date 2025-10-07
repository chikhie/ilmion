using Microsoft.AspNetCore.Mvc;
using MuslimAds.Infra.Entities;
using MuslimAds.Infra;
using Microsoft.EntityFrameworkCore;

namespace MuslimAds.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AdController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAds()
    {
        var ads = await _context.Ads.ToListAsync();
        return Ok(ads);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAd(AdEntity ad)
    {
        await _context.Ads.AddAsync(ad);
        await _context.SaveChangesAsync();
        return Ok(ad);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAd(AdEntity ad)
    {
        _context.Ads.Update(ad);
        await _context.SaveChangesAsync();
        return Ok(ad);
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteAd(Guid id)
    {
        var ad = await _context.Ads.FindAsync(id);
        if (ad == null)
        {
            return NotFound();
        }
        _context.Ads.Remove(ad);
        await _context.SaveChangesAsync();
        return Ok(ad);
    }
}