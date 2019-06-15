using System.Collections.Generic;
using System.Threading.Tasks;
using YouTubeToPodcast.Models;

namespace YouTubeToPodcast.Services
{
    public interface IYouTubeApiClient
    {
        Task<ChannelInfo> GetChannelAsync(string id, ChannelType channelType);
        Task<IEnumerable<string>> GetVideosIds(string uploadsId);
        Task<IEnumerable<VideoInfo>> GetVideosAsync(IEnumerable<string> ids);
    }
}