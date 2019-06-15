using System.Threading.Tasks;

namespace YouTubeToPodcast.Services
{
    public interface IYouTubeScrapper
    {
        Task<string> GetAudioFileUrl(string id);
    }
}