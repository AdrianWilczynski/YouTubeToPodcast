using System;
using System.IO;
using System.Threading.Tasks;
using Medallion.Shell;
using Microsoft.AspNetCore.Hosting;

namespace YouTubeToPodcast.Services
{
    public class YouTubeDlScrapper : IYouTubeScrapper
    {
        private readonly IHostingEnvironment _env;

        public YouTubeDlScrapper(IHostingEnvironment env)
        {
            _env = env;
        }

        public async Task<string> GetAudioFileUrl(string id)
            => (await Command.Run("python",
                    new[]
                    {
                        Path.Combine(_env.ContentRootPath, "youtube-dl"),
                        "-f", "bestaudio[ext=m4a]", "-g", $"https://www.youtube.com/watch?v={id}"
                    },
                    options =>
                    {
                        options.ThrowOnError();
                        options.Timeout(TimeSpan.FromMinutes(3));
                    })
                .Task)
                .StandardOutput
                .Trim();
    }
}