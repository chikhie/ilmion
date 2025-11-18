using Microsoft.AspNetCore.Mvc;
using KitabStock.Infra.Entities;
using KitabStock.Infra;
using Microsoft.EntityFrameworkCore;
using KitabStock.Api.Dtos;
using KitabStock.Infra.Services;

namespace KitabStock.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VideoController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly VideoProcessingService _videoProcessingService;

    public VideoController(ApplicationDbContext context)
    {
        _context = context;
        _videoProcessingService = new VideoProcessingService();
    }

    [HttpGet]
    [HttpGet("all")]
    public async Task<IActionResult> GetVideos()
    {
        var videos = await _context.Videos.OrderByDescending(v => v.CreatedAt).Take(10).ToListAsync();
        return Ok(videos);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(5368709120)] // 5 GB
    [RequestFormLimits(MultipartBodyLengthLimit = 5368709120)]
    public async Task<IActionResult> CreateVideo([FromForm] CreateVideoDto videoDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (videoDto.VideoFile == null || videoDto.VideoFile.Length == 0)
        {
            return BadRequest(new { message = "Aucun fichier vidéo fourni" });
        }

        // Vérifier que c'est bien un fichier vidéo
        var allowedExtensions = new[] { ".mp4", ".avi", ".mov", ".mkv", ".wmv", ".flv", ".webm" };
        var fileExtension = Path.GetExtension(videoDto.VideoFile.FileName).ToLower();
        if (!allowedExtensions.Contains(fileExtension))
        {
            return BadRequest(new { message = "Format de fichier non supporté. Formats acceptés: " + string.Join(", ", allowedExtensions) });
        }

        try
        {
            // Générer un nouvel ID pour la vidéo
            var videoId = Guid.NewGuid();

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Début du traitement de la vidéo '{videoDto.Title}' (ID: {videoId})");

            // Traiter la vidéo : extraction métadonnées, génération HLS, compression
            var processingResult = await _videoProcessingService.ProcessVideoAsync(
                videoDto.VideoFile, 
                videoId, 
                videoDto.Title
            );

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Traitement terminé avec succès pour '{videoDto.Title}'");

            if (!processingResult.Success)
            {
                return StatusCode(500, new { message = "Erreur lors du traitement de la vidéo" });
            }

            // Créer l'entité vidéo avec les métadonnées extraites
            var video = new VideoEntity
            {
                Id = videoId,
                Title = videoDto.Title,
                Description = videoDto.Description,
                Duration = processingResult.Metadata.Duration,
                VideoResolution = processingResult.Metadata.VideoResolution,
                VideoFormat = processingResult.Metadata.VideoFormat,
                FrameRate = processingResult.Metadata.FrameRate,
                ColorSpace = processingResult.Metadata.ColorSpace,
                FilenameDownload = processingResult.ZipFileName, // Nom du fichier ZIP (basé sur le titre)
                Price = videoDto.Price,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Videos.Add(video);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVideos), new { id = video.Id }, new
            {
                video = video,
                processing = new
                {
                    originalSizeMb = processingResult.OriginalSizeMb,
                    zipSizeMb = processingResult.ZipSizeMb,
                    videoDirectory = processingResult.VideoDirectory,
                    zipFileName = processingResult.ZipFileName,
                    hlsGenerated = true
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Erreur lors de la création de la vidéo: {ex.Message}" });
        }
    }

    /// <summary>
    /// Récupère le fichier master.m3u8 pour le streaming HLS
    /// Route: /api/video/{videoId}/stream
    /// Les segments .ts seront résolus relativement : /api/video/{videoId}/{filename}
    /// </summary>
    [HttpGet("{videoId}/stream")]
    public IActionResult GetVideoStream(string videoId)
    {
        var m3u8Path = Path.Combine(Directory.GetCurrentDirectory(), "Infra", "res", "videos", videoId, "master.m3u8");
        
        if (!System.IO.File.Exists(m3u8Path))
        {
            return NotFound(new { message = "Fichier de streaming non trouvé" });
        }

        Response.Headers.Append("Access-Control-Allow-Origin", "*");
        return PhysicalFile(m3u8Path, "application/x-mpegURL");
    }

    /// <summary>
    /// Récupère les fichiers HLS (segments .ts)
    /// Route: /api/video/{videoId}/{filename} pour résoudre les chemins relatifs du m3u8
    /// Exemples: /api/video/{videoId}/0.ts, /api/video/{videoId}/1.ts
    /// </summary>
    [HttpGet("{videoId}/{filename}")]
    public IActionResult GetVideoFile(string videoId, string filename)
    {
        // Sécurité: vérifier que le filename ne contient pas de caractères dangereux
        if (filename.Contains("..") || filename.Contains("/") || filename.Contains("\\"))
        {
            return BadRequest(new { message = "Nom de fichier invalide" });
        }

        // Autoriser seulement les fichiers .ts
        if (!filename.EndsWith(".ts"))
        {
            return BadRequest(new { message = "Type de fichier non autorisé" });
        }

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Infra", "res", "videos", videoId, filename);
        
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound(new { message = $"Fichier non trouvé: {filename}" });
        }
        
        Response.Headers.Append("Access-Control-Allow-Origin", "*");
        return PhysicalFile(filePath, "video/MP2T");
    }

    /// <summary>
    /// Récupère la miniature (thumbnail) de la vidéo
    /// Route: /api/video/{videoId}/thumbnail
    /// </summary>
    [HttpGet("{videoId}/thumbnail")]
    public IActionResult GetThumbnail(string videoId)
    {
        var thumbnailPath = Path.Combine(Directory.GetCurrentDirectory(), "Infra", "res", "videos", videoId, "thumbnail.jpg");
        
        if (!System.IO.File.Exists(thumbnailPath))
        {
            return NotFound(new { message = "Miniature non trouvée" });
        }

        Response.Headers.Append("Access-Control-Allow-Origin", "*");
        Response.Headers.Append("Cache-Control", "public, max-age=31536000"); // Cache 1 an
        return PhysicalFile(thumbnailPath, "image/jpeg");
    }

    [HttpGet("{videoId}/download")]
    public IActionResult DownloadVideo(string videoId)
    {
        // Récupérer la vidéo pour obtenir le nom du fichier ZIP
        var video = _context.Videos.FirstOrDefault(v => v.Id.ToString() == videoId);
        if (video == null)
        {
            return NotFound(new { message = "Vidéo non trouvée" });
        }

        var zipPath = Path.Combine(Directory.GetCurrentDirectory(), "Infra", "res", "videos", videoId, video.FilenameDownload);
        if (!System.IO.File.Exists(zipPath))
        {
            return NotFound(new { message = "Fichier ZIP non trouvé" });
        }

        return PhysicalFile(zipPath, "application/zip", video.FilenameDownload);
    }

}