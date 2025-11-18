using System.Diagnostics;

namespace KitabStock.Infra.Services;

public class ThumbnailGenerator
{
    /// <summary>
    /// Génère une miniature (thumbnail) à partir d'une vidéo
    /// </summary>
    /// <param name="videoPath">Chemin complet vers la vidéo source</param>
    /// <param name="outputPath">Chemin complet où sauvegarder la miniature</param>
    /// <param name="timePosition">Position dans la vidéo pour capturer l'image (format: "00:00:05" pour 5 secondes)</param>
    /// <param name="width">Largeur de la miniature (la hauteur sera calculée automatiquement)</param>
    public async Task GenerateThumbnailAsync(string videoPath, string outputPath, string timePosition = "00:00:02", int width = 640)
    {
        if (!File.Exists(videoPath))
        {
            throw new FileNotFoundException($"Le fichier vidéo n'existe pas: {videoPath}");
        }

        // Créer le dossier de sortie si nécessaire
        var outputDirectory = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        try
        {
            // Commande FFmpeg pour extraire une frame à la position spécifiée
            // -ss: position temporelle
            // -i: fichier d'entrée
            // -vframes 1: extraire une seule frame
            // -vf scale: redimensionner l'image
            var arguments = $"-ss {timePosition} " +
                          $"-i \"{videoPath}\" " +
                          $"-vframes 1 " +
                          $"-vf scale={width}:-1 " + // -1 = garde le ratio d'aspect
                          $"-q:v 2 " + // Qualité JPEG (2-5 est très bonne)
                          $"\"{outputPath}\"";

            var processStartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(processStartInfo);
            if (process == null)
            {
                throw new Exception("Impossible de démarrer ffmpeg");
            }

            var error = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                throw new Exception($"Erreur lors de la génération de la miniature: {error}");
            }

            Console.WriteLine($"✅ Miniature générée avec succès: {outputPath}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Erreur lors de la génération de la miniature: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Vérifie si FFmpeg est disponible
    /// </summary>
    public async Task<bool> IsFfmpegAvailableAsync()
    {
        try
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = "-version",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(processStartInfo);
            if (process == null) return false;

            await process.WaitForExitAsync();
            return process.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }
}

