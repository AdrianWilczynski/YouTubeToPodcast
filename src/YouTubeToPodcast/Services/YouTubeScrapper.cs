using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;

namespace YouTubeToPodcast.Services
{
    public class YouTubeScrapper : IYouTubeScrapper
    {
        private readonly IYoutubeClient _youtubeClient;

        public YouTubeScrapper(IYoutubeClient youtubeClient)
        {
            _youtubeClient = youtubeClient;
        }

        public async Task<string> GetAudioFileUrl(string id)
        {
            var mediaStreams = await _youtubeClient.GetVideoMediaStreamInfosAsync(id);
            var audioStream = mediaStreams.Audio.Single(s => s.Container == Container.Mp4);
            return audioStream.Url;
        }
    }
}