namespace Ilmanar.Infra.Services;

public class VideoProcessingService
{
    private readonly VideoMetadataExtractor _metadataExtractor;
    private readonly HlsGenerator _hlsGenerator;
    private readonly VideoCompressor _videoCompressor;
    private readonly ThumbnailGenerator _thumbnailGenerator;

    public VideoProcessingService()
    {
        _metadataExtractor = new VideoMetadataExtractor();
        _hlsGenerator = new HlsGenerator();
        _videoCompressor = new VideoCompressor();
        _thumbnailGenerator = new ThumbnailGenerator();
    }

    /// <summary>
    /// Traite une vidÃ©o uploadÃ©e : extrait mÃ©tadonnÃ©es, gÃ©nÃ¨re HLS, compresse
    /// </summary>
    /// <param name="uploadedFile">Fichier vidÃ©o uploadÃ©</param>
    /// <param name="videoId">ID de la vidÃ©o (sera utilisÃ© comme nom de dossier)</param>
    /// <param name="title">Titre de la vidÃ©o (sera utilisÃ© pour nommer le ZIP)</param>
    /// <param name="baseDirectory">Dossier de base pour les vidÃ©os (par dÃ©faut Infra/res/videos)</param>
    public async Task<VideoProcessingResult> ProcessVideoAsync(IFormFile uploadedFile, Guid videoId, string title, string? baseDirectory = null)
    {
        if (uploadedFile == null || uploadedFile.Length == 0)
        {
            throw new ArgumentException("Le fichier vidÃ©o est vide ou invalide");
        }

        // DÃ©finir le dossier de base
        baseDirectory ??= Path.Combine(Directory.GetCurrentDirectory(), "Infra", "res", "videos");
        
        // CrÃ©er le dossier pour cette vidÃ©o avec l'ID comme nom
        var videoDirectory = Path.Combine(baseDirectory, videoId.ToString());
        if (!Directory.Exists(videoDirectory))
        {
            Directory.CreateDirectory(videoDirectory);
        }

        // Sauvegarder le fichier original
        var originalFileName = $"filename{Path.GetExtension(uploadedFile.FileName)}";
        var originalFilePath = Path.Combine(videoDirectory, originalFileName);
        
        using (var stream = new FileStream(originalFilePath, FileMode.Create))
        {
            await uploadedFile.CopyToAsync(stream);
        }

        try
        {
            // 1. Extraire les mÃ©tadonnÃ©es
            var metadata = await _metadataExtractor.ExtractMetadataAsync(originalFilePath);

            // 2. VÃ©rifier si un filigrane par dÃ©faut existe
            var watermarkPath = Path.Combine(Directory.GetCurrentDirectory(), "Infra", "res", "watermark", "default.png");
            if (!File.Exists(watermarkPath))
            {
                Console.WriteLine("âš ï¸ Aucun filigrane trouvÃ©. Le streaming HLS sera gÃ©nÃ©rÃ© sans filigrane.");
                Console.WriteLine($"ðŸ’¡ Pour ajouter un filigrane, placez une image PNG Ã : {watermarkPath}");
                watermarkPath = null;
            }
            else
            {
                Console.WriteLine($"âœ… Filigrane dÃ©tectÃ©: {watermarkPath}");
            }

            // 3. GÃ©nÃ©rer les segments HLS (master.m3u8 et 0.ts, 1.ts, 2.ts, etc.) avec filigrane
            await _hlsGenerator.GenerateHlsSegmentsAsync(originalFilePath, videoDirectory, watermarkPath);

            // 3.1 GÃ©nÃ©rer la miniature (thumbnail) Ã  partir de la vidÃ©o
            var thumbnailPath = Path.Combine(videoDirectory, "thumbnail.jpg");
            await _thumbnailGenerator.GenerateThumbnailAsync(originalFilePath, thumbnailPath, "00:00:02", 640);

            // 4. Calculer la taille du fichier original avant compression
            var originalSize = _videoCompressor.GetFileSizeInMb(originalFilePath);

            // 5. CrÃ©er un nom de fichier sÃ©curisÃ© pour le ZIP Ã  partir du titre
            var safeTitle = GetSafeFileName(title);
            var zipFileName = $"{safeTitle}.zip";

            // 6. Compresser la vidÃ©o originale dans un ZIP (vidÃ©o SANS filigrane pour le tÃ©lÃ©chargement)
            var zipPath = await _videoCompressor.CompressVideoToZipAsync(
                originalFilePath, 
                videoDirectory, 
                zipFileName
            );

            // 7. Calculer la taille du ZIP
            var zipSize = _videoCompressor.GetFileSizeInMb(zipPath);

            // 8. Supprimer le fichier vidÃ©o original aprÃ¨s compression
            if (File.Exists(originalFilePath))
            {
                File.Delete(originalFilePath);
            }

            return new VideoProcessingResult
            {
                Success = true,
                Metadata = metadata,
                VideoDirectory = videoDirectory,
                OriginalFilePath = originalFilePath,
                ZipFilePath = zipPath,
                ZipFileName = zipFileName,
                M3u8FilePath = Path.Combine(videoDirectory, "master.m3u8"),
                ThumbnailPath = thumbnailPath,
                OriginalFileName = originalFileName,
                OriginalSizeMb = originalSize,
                ZipSizeMb = zipSize
            };
        }
        catch (Exception ex)
        {
            // En cas d'erreur, nettoyer le dossier crÃ©Ã©
            if (Directory.Exists(videoDirectory))
            {
                try
                {
                    Directory.Delete(videoDirectory, true);
                }
                catch
                {
                    // Ignorer les erreurs de suppression
                }
            }

            throw new Exception($"Erreur lors du traitement de la vidÃ©o: {ex.Message}", ex);
        }
    }


    /// <summary>
    /// CrÃ©e un nom de fichier sÃ©curisÃ© en supprimant les caractÃ¨res invalides
    /// </summary>
    private string GetSafeFileName(string fileName)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var safeName = string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
        
        // Limiter la longueur Ã  100 caractÃ¨res
        if (safeName.Length > 100)
        {
            safeName = safeName.Substring(0, 100);
        }
        
        return string.IsNullOrWhiteSpace(safeName) ? "video" : safeName;
    }
}

public class VideoProcessingResult
{
    public bool Success { get; set; }
    public VideoMetadata Metadata { get; set; } = new();
    public string VideoDirectory { get; set; } = string.Empty;
    public string OriginalFilePath { get; set; } = string.Empty;
    public string ZipFilePath { get; set; } = string.Empty;
    public string ZipFileName { get; set; } = string.Empty;
    public string M3u8FilePath { get; set; } = string.Empty;
    public string ThumbnailPath { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public double OriginalSizeMb { get; set; }
    public double ZipSizeMb { get; set; }
}


