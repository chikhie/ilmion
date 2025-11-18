using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Globalization;

namespace KitabStock.Infra.Services;

public class VideoMetadata
{
    public TimeSpan Duration { get; set; }
    public string VideoResolution { get; set; } = string.Empty;
    public string VideoFormat { get; set; } = string.Empty;
    public double FrameRate { get; set; }
    public string ColorSpace { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }
}

public class VideoMetadataExtractor
{
    public async Task<VideoMetadata> ExtractMetadataAsync(string videoPath)
    {
        var metadata = new VideoMetadata();

        try
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "ffprobe",
                Arguments = $"-v quiet -print_format json -show_format -show_streams \"{videoPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(processStartInfo);
            if (process == null)
            {
                throw new Exception("Impossible de démarrer ffprobe");
            }

            var output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                var error = await process.StandardError.ReadToEndAsync();
                throw new Exception($"Erreur ffprobe: {error}");
            }

            // Parse JSON output
            using var jsonDoc = JsonDocument.Parse(output);
            var root = jsonDoc.RootElement;

            // Extract duration from format
            if (root.TryGetProperty("format", out var format))
            {
                if (format.TryGetProperty("duration", out var duration))
                {
                    metadata.Duration = TimeSpan.FromSeconds(double.Parse(duration.GetString() ?? "0", CultureInfo.InvariantCulture));
                }
            }

            // Extract video stream info
            if (root.TryGetProperty("streams", out var streams))
            {
                foreach (var stream in streams.EnumerateArray())
                {
                    if (stream.TryGetProperty("codec_type", out var codecType) && 
                        codecType.GetString() == "video")
                    {
                        // Resolution
                        if (stream.TryGetProperty("width", out var width) && 
                            stream.TryGetProperty("height", out var height))
                        {
                            metadata.Width = width.GetInt32();
                            metadata.Height = height.GetInt32();
                            metadata.VideoResolution = $"{metadata.Width}x{metadata.Height}";
                        }

                        // Format/Codec
                        if (stream.TryGetProperty("codec_name", out var codecName))
                        {
                            metadata.VideoFormat = codecName.GetString() ?? string.Empty;
                        }

                        // Frame rate
                        if (stream.TryGetProperty("r_frame_rate", out var frameRate))
                        {
                            var frameRateStr = frameRate.GetString() ?? "0/1";
                            var parts = frameRateStr.Split('/');
                            if (parts.Length == 2 && 
                                double.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var numerator) && 
                                double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var denominator) && 
                                denominator != 0)
                            {
                                metadata.FrameRate = Math.Round(numerator / denominator, 2);
                            }
                        }

                        // Color space
                        if (stream.TryGetProperty("color_space", out var colorSpace))
                        {
                            metadata.ColorSpace = colorSpace.GetString() ?? string.Empty;
                        }

                        break; // On prend seulement le premier stream vidéo
                    }
                }
            }

            return metadata;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erreur lors de l'extraction des métadonnées: {ex.Message}", ex);
        }
    }
}

