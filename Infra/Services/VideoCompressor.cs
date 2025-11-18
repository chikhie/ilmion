using System.IO.Compression;

namespace KitabStock.Infra.Services;

public class VideoCompressor
{
    /// <summary>
    /// Compresse un fichier vidéo dans une archive ZIP
    /// </summary>
    /// <param name="videoPath">Chemin complet vers la vidéo à compresser</param>
    /// <param name="outputDirectory">Dossier où sera créé le fichier ZIP</param>
    /// <param name="zipFileName">Nom du fichier ZIP (optionnel, par défaut "original.zip")</param>
    /// <returns>Chemin complet vers le fichier ZIP créé</returns>
    public async Task<string> CompressVideoToZipAsync(string videoPath, string outputDirectory, string zipFileName = "original.zip")
    {
        if (!File.Exists(videoPath))
        {
            throw new FileNotFoundException($"Le fichier vidéo n'existe pas: {videoPath}");
        }

        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        var zipPath = Path.Combine(outputDirectory, zipFileName);

        try
        {
            // Supprimer le fichier ZIP s'il existe déjà
            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }

            // Créer l'archive ZIP
            using (var archive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                var videoFileName = Path.GetFileName(videoPath);
                archive.CreateEntryFromFile(videoPath, videoFileName, CompressionLevel.Optimal);
            }

            Console.WriteLine($"Vidéo compressée avec succès: {zipPath}");
            return zipPath;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erreur lors de la compression de la vidéo: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Obtient la taille d'un fichier en Mo
    /// </summary>
    public double GetFileSizeInMb(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return 0;
        }

        var fileInfo = new FileInfo(filePath);
        return Math.Round(fileInfo.Length / (1024.0 * 1024.0), 2);
    }
}

