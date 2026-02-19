using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;

namespace Ilmanar.Api.Controllers
{
    [ApiController]
    [Route("api/videos")]
    public class VideoController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public VideoController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet("{videoId}/master.m3u8")]
        public IActionResult GetMasterPlaylist(string videoId)
        {
            var path = Path.Combine(_env.ContentRootPath, "Infra", "res", "videos", videoId, "master.m3u8");
            
            if (!System.IO.File.Exists(path))
            {
                return NotFound($"Playlist not found for video {videoId}");
            }

            return PhysicalFile(path, "application/vnd.apple.mpegurl");
        }

        [HttpGet("{videoId}/{segment}")]
        public IActionResult GetSegment(string videoId, string segment)
        {

            if (string.IsNullOrEmpty(segment) || !segment.EndsWith(".ts"))
            {
                return BadRequest("Invalid segment file");
            }

            var path = Path.Combine(_env.ContentRootPath, "Infra", "res", "videos", videoId, segment);
            
            if (!System.IO.File.Exists(path))
            {
                return NotFound($"Segment {segment} not found for video {videoId}");
            }

            return PhysicalFile(path, "video/MP2T");
        }
        [HttpGet]
        public IActionResult GetVideos()
        {
            var videosPath = Path.Combine(_env.ContentRootPath, "Infra", "res", "videos");
            
            if (!Directory.Exists(videosPath))
            {
                return Ok(new List<string>());
            }

            // Récupère les noms des dossiers (ID ou noms des vidéos)
            var directories = Directory.GetDirectories(videosPath)
                                       .Select(d => Path.GetFileName(d))
                                       .ToList();

            return Ok(directories);
        }
    }
}
