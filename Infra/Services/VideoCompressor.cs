using System.IO.Compression;

namespace Ilmanar.Infra.Services;

public class VideoCompressor
{
    /// <summary>
    /// Compresse un fichier vidÃ©o dans une archive ZIP
    /// </summary>
    /// <param name="videoPath">Chemin complet vers la vidÃ©o Ã  compresser</param>
    /// <param name="outputDirectory">Dossier oÃ¹ sera crÃ©Ã© le fichier ZIP</param>
    /// <param name="zipFileName">Nom du fichier ZIP (optionnel, par dÃ©faut "original.zip")</param>
    /// <returns>Chemin complet vers le fichier ZIP crÃ©Ã©</returns>
    public async Task<string> CompressVideoToZipAsync(string videoPath, string outputDirectory, string zipFileName = "original.zip")
    {
        if (!File.Exists(videoPath))
        {
            throw new FileNotFoundException($"Le fichier vidÃ©o n'existe pas: {videoPath}");
        }

        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        var zipPath = Path.Combine(outputDirectory, zipFileName);

        try
        {
            // Supprimer le fichier ZIP s'il existe dÃ©jÃ 
            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }

            // CrÃ©er l'archive ZIP
            using (var archive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                var videoFileName = Path.GetFileName(videoPath);
                archive.CreateEntryFromFile(videoPath, videoFileName, CompressionLevel.Optimal);
            }

            Console.WriteLine($"VidÃ©o compressÃ©e avec succÃ¨s: {zipPath}");
            return zipPath;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erreur lors de la compression de la vidÃ©o: {ex.Message}", ex);
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


