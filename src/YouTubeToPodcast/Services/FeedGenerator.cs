using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using YouTubeToPodcast.Controllers;
using YouTubeToPodcast.DTOs;
using YouTubeToPodcast.Extensions;
using YouTubeToPodcast.Models;

namespace YouTubeToPodcast.Services
{
    public class FeedGenerator : IFeedGenerator
    {
        private readonly IYouTubeApiClient _youTubeApiClient;
        private readonly LinkGenerator _linkGenerator;

        public FeedGenerator(IYouTubeApiClient youTubeApiClient, LinkGenerator linkGenerator)
        {
            _youTubeApiClient = youTubeApiClient;
            _linkGenerator = linkGenerator;
        }

        public async Task<FeedDTO> CreateFeedAsync(Podcast podcast, string scheme, HostString host)
        {
            var channel = await _youTubeApiClient.GetChannelAsync(podcast.YouTubeId, podcast.ChannelType);
            var videosIds = await _youTubeApiClient.GetVideosIds(channel.UploadsListId);
            var videos = await _youTubeApiClient.GetVideosAsync(videosIds);

            videos = videos.Where(v =>
                (string.IsNullOrWhiteSpace(podcast.Contains) || v.Title.Contains(podcast.Contains, StringComparison.OrdinalIgnoreCase))
                && v.Duration >= podcast.Duration);

            string fileUrlFactory(string id)
                => _linkGenerator.GetUriByAction(
                        nameof(PodcastController.File), nameof(PodcastController).RemoveSuffix(nameof(Controller)),
                        new { Id = id + Constants.YouTube.AudioFileExtension }, scheme, host);

            return new FeedDTO(channel, videos, podcast, fileUrlFactory);
        }
    }
}