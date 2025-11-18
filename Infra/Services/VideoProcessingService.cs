namespace KitabStock.Infra.Services;

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
    /// Traite une vidéo uploadée : extrait métadonnées, génère HLS, compresse
    /// </summary>
    /// <param name="uploadedFile">Fichier vidéo uploadé</param>
    /// <param name="videoId">ID de la vidéo (sera utilisé comme nom de dossier)</param>
    /// <param name="title">Titre de la vidéo (sera utilisé pour nommer le ZIP)</param>
    /// <param name="baseDirectory">Dossier de base pour les vidéos (par défaut Infra/res/videos)</param>
    public async Task<VideoProcessingResult> ProcessVideoAsync(IFormFile uploadedFile, Guid videoId, string title, string? baseDirectory = null)
    {
        if (uploadedFile == null || uploadedFile.Length == 0)
        {
            throw new ArgumentException("Le fichier vidéo est vide ou invalide");
        }

        // Définir le dossier de base
        baseDirectory ??= Path.Combine(Directory.GetCurrentDirectory(), "Infra", "res", "videos");
        
        // Créer le dossier pour cette vidéo avec l'ID comme nom
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
            // 1. Extraire les métadonnées
            var metadata = await _metadataExtractor.ExtractMetadataAsync(originalFilePath);

            // 2. Vérifier si un filigrane par défaut existe
            var watermarkPath = Path.Combine(Directory.GetCurrentDirectory(), "Infra", "res", "watermark", "default.png");
            if (!File.Exists(watermarkPath))
            {
                Console.WriteLine("⚠️ Aucun filigrane trouvé. Le streaming HLS sera généré sans filigrane.");
                Console.WriteLine($"💡 Pour ajouter un filigrane, placez une image PNG à: {watermarkPath}");
                watermarkPath = null;
            }
            else
            {
                Console.WriteLine($"✅ Filigrane détecté: {watermarkPath}");
            }

            // 3. Générer les segments HLS (master.m3u8 et 0.ts, 1.ts, 2.ts, etc.) avec filigrane
            await _hlsGenerator.GenerateHlsSegmentsAsync(originalFilePath, videoDirectory, watermarkPath);

            // 3.1 Générer la miniature (thumbnail) à partir de la vidéo
            var thumbnailPath = Path.Combine(videoDirectory, "thumbnail.jpg");
            await _thumbnailGenerator.GenerateThumbnailAsync(originalFilePath, thumbnailPath, "00:00:02", 640);

            // 4. Calculer la taille du fichier original avant compression
            var originalSize = _videoCompressor.GetFileSizeInMb(originalFilePath);

            // 5. Créer un nom de fichier sécurisé pour le ZIP à partir du titre
            var safeTitle = GetSafeFileName(title);
            var zipFileName = $"{safeTitle}.zip";

            // 6. Compresser la vidéo originale dans un ZIP (vidéo SANS filigrane pour le téléchargement)
            var zipPath = await _videoCompressor.CompressVideoToZipAsync(
                originalFilePath, 
                videoDirectory, 
                zipFileName
            );

            // 7. Calculer la taille du ZIP
            var zipSize = _videoCompressor.GetFileSizeInMb(zipPath);

            // 8. Supprimer le fichier vidéo original après compression
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
            // En cas d'erreur, nettoyer le dossier créé
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

            throw new Exception($"Erreur lors du traitement de la vidéo: {ex.Message}", ex);
        }
    }


    /// <summary>
    /// Crée un nom de fichier sécurisé en supprimant les caractères invalides
    /// </summary>
    private string GetSafeFileName(string fileName)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var safeName = string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
        
        // Limiter la longueur à 100 caractères
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

