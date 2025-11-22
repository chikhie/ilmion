using System.Diagnostics;

namespace Ilmanar.Infra.Services;

public class ThumbnailGenerator
{
    /// <summary>
    /// GÃ©nÃ¨re une miniature (thumbnail) Ã  partir d'une vidÃ©o
    /// </summary>
    /// <param name="videoPath">Chemin complet vers la vidÃ©o source</param>
    /// <param name="outputPath">Chemin complet oÃ¹ sauvegarder la miniature</param>
    /// <param name="timePosition">Position dans la vidÃ©o pour capturer l'image (format: "00:00:05" pour 5 secondes)</param>
    /// <param name="width">Largeur de la miniature (la hauteur sera calculÃ©e automatiquement)</param>
    public async Task GenerateThumbnailAsync(string videoPath, string outputPath, string timePosition = "00:00:02", int width = 640)
    {
        if (!File.Exists(videoPath))
        {
            throw new FileNotFoundException($"Le fichier vidÃ©o n'existe pas: {videoPath}");
        }

        // CrÃ©er le dossier de sortie si nÃ©cessaire
        var outputDirectory = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        try
        {
            // Commande FFmpeg pour extraire une frame Ã  la position spÃ©cifiÃ©e
            // -ss: position temporelle
            // -i: fichier d'entrÃ©e
            // -vframes 1: extraire une seule frame
            // -vf scale: redimensionner l'image
            var arguments = $"-ss {timePosition} " +
                          $"-i \"{videoPath}\" " +
                          $"-vframes 1 " +
                          $"-vf scale={width}:-1 " + // -1 = garde le ratio d'aspect
                          $"-q:v 2 " + // QualitÃ© JPEG (2-5 est trÃ¨s bonne)
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
                throw new Exception("Impossible de dÃ©marrer ffmpeg");
            }

            var error = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                throw new Exception($"Erreur lors de la gÃ©nÃ©ration de la miniature: {error}");
            }

            Console.WriteLine($"âœ… Miniature gÃ©nÃ©rÃ©e avec succÃ¨s: {outputPath}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Erreur lors de la gÃ©nÃ©ration de la miniature: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// VÃ©rifie si FFmpeg est disponible
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


