using System.Diagnostics;

namespace KitabStock.Infra.Services;

/// <summary>
/// Vérifie que FFmpeg et FFprobe sont installés et accessibles
/// </summary>
public static class FfmpegChecker
{
    public static async Task<(bool isInstalled, string message)> CheckFfmpegInstallationAsync()
    {
        var ffmpegInstalled = await CheckCommandAsync("ffmpeg");
        var ffprobeInstalled = await CheckCommandAsync("ffprobe");

        if (ffmpegInstalled && ffprobeInstalled)
        {
            var version = await GetFfmpegVersionAsync();
            return (true, $"FFmpeg installé - {version}");
        }

        var missingTools = new List<string>();
        if (!ffmpegInstalled) missingTools.Add("ffmpeg");
        if (!ffprobeInstalled) missingTools.Add("ffprobe");

        var message = $"⚠️ Outils manquants: {string.Join(", ", missingTools)}\n\n" +
                     "Pour installer FFmpeg:\n" +
                     "Windows: Télécharger depuis https://ffmpeg.org/download.html et ajouter au PATH\n" +
                     "Linux: sudo apt install ffmpeg\n" +
                     "Mac: brew install ffmpeg";

        return (false, message);
    }

    private static async Task<bool> CheckCommandAsync(string command)
    {
        try
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = command,
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

    private static async Task<string> GetFfmpegVersionAsync()
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
            if (process == null) return "Version inconnue";

            var output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();

            // Extraire la première ligne qui contient la version
            var lines = output.Split('\n');
            return lines.Length > 0 ? lines[0].Trim() : "Version inconnue";
        }
        catch
        {
            return "Version inconnue";
        }
    }
}

