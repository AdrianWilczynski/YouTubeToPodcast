using System.Linq;
using System.Threading.Tasks;
using MoreLinq;
using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;

namespace YouTubeToPodcast.Services
{
    public class YouTubeExplodeScrapper : IYouTubeScrapper
    {
        private readonly IYoutubeClient _youtubeClient;

        public YouTubeExplodeScrapper(IYoutubeClient youtubeClient)
        {
            _youtubeClient = youtubeClient;
        }

        public async Task<string> GetAudioFileUrl(string id)
        {
            var mediaStreams = await _youtubeClient.GetVideoMediaStreamInfosAsync(id);

            var audioStream = mediaStreams.Audio
                .Where(s => s.Container == Container.Mp4)
                .MaxBy(s => s.Bitrate)
                .First();

            return audioStream.Url;
        }
    }
}