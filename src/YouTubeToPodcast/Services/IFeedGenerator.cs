using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using YouTubeToPodcast.DTOs;
using YouTubeToPodcast.Models;

namespace YouTubeToPodcast.Services
{
    public interface IFeedGenerator
    {
        Task<FeedDTO> CreateFeedAsync(Podcast podcast, string scheme, HostString host);
    }
}