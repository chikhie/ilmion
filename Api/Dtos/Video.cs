using Microsoft.AspNetCore.Http;

public class VideoRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public double Budget { get; set; } = 0;
}

public class CreateVideoDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; } = 0;
    public IFormFile? VideoFile { get; set; }
    // Les autres champs (Duration, VideoResolution, VideoCode, etc.) seront extraits/gÃ©nÃ©rÃ©s automatiquement
}


