using System.Diagnostics;

namespace KitabStock.Infra.Services;

public class HlsGenerator
{
    /// <summary>
    /// Génère les segments HLS à partir d'un fichier vidéo avec filigrane
    /// </summary>
    /// <param name="inputVideoPath">Chemin complet vers la vidéo source</param>
    /// <param name="outputDirectory">Dossier où seront créés les segments HLS</param>
    /// <param name="watermarkPath">Chemin vers l'image de filigrane (optionnel)</param>
    /// <param name="segmentDuration">Durée de chaque segment en secondes (par défaut 1s)</param>
    public async Task GenerateHlsSegmentsAsync(string inputVideoPath, string outputDirectory, string? watermarkPath = null, int segmentDuration = 1)
    {
        if (!File.Exists(inputVideoPath))
        {
            throw new FileNotFoundException($"Le fichier vidéo n'existe pas: {inputVideoPath}");
        }

        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        // Fichier master.m3u8 et segments numérotés 0.ts, 1.ts, 2.ts, etc.
        var m3u8Path = Path.Combine(outputDirectory, "master.m3u8");
        var segmentPattern = Path.Combine(outputDirectory, "%d.ts");

        try
        {
            // Construction de la commande FFmpeg
            // Si un filigrane est fourni, on l'applique avec le filtre overlay
            string videoFilter = "";
            string inputParams = $"-i \"{inputVideoPath}\" ";
            
            if (!string.IsNullOrEmpty(watermarkPath) && File.Exists(watermarkPath))
            {
                // Ajouter l'image du filigrane comme input
                inputParams = $"-i \"{inputVideoPath}\" -i \"{watermarkPath}\" ";
                
                // Filigrane en bas à droite avec transparence
                // overlay=W-w-10:H-h-10 = 10px de marge depuis le coin inférieur droit
                videoFilter = "-filter_complex \"[1:v]format=rgba,colorchannelmixer=aa=0.6[logo];[0:v][logo]overlay=W-w-10:H-h-10\" ";
                
                Console.WriteLine($"Application du filigrane: {watermarkPath}");
            }
            
            // Commande FFmpeg complète pour générer HLS
            // hls_time définit la durée maximale de chaque segment (1 seconde)
            var arguments = inputParams +
                          videoFilter +
                          $"-c:v libx264 -preset fast -c:a aac " +
                          $"-b:v 2M -b:a 128k " +
                          $"-movflags +faststart " +
                          $"-hls_time {segmentDuration} " +
                          $"-hls_playlist_type vod " +
                          $"-hls_segment_filename \"{segmentPattern}\" " +
                          $"\"{m3u8Path}\"";

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

            // FFmpeg écrit dans stderr même pour les infos normales
            var error = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                throw new Exception($"Erreur lors de la génération HLS: {error}");
            }

            Console.WriteLine($"Segments HLS générés avec succès dans: {outputDirectory}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Erreur lors de la génération des segments HLS: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Vérifie si FFmpeg est installé et accessible
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

