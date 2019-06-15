using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using MoreLinq;
using YouTubeToPodcast.Models;

namespace YouTubeToPodcast.Services
{
    public class YouTubeApiClient : IYouTubeApiClient
    {
        private static class Parts
        {
            public const string ContentDetails = "contentDetails";
            public const string Snippet = "snippet";
        }

        private readonly YouTubeService _youTubeService;

        public YouTubeApiClient(YouTubeService youTubeService)
        {
            _youTubeService = youTubeService;
        }

        public async Task<ChannelInfo> GetChannelAsync(string id, ChannelType channelType)
        {
            var parts = string.Join(",", Parts.Snippet, Parts.ContentDetails);
            var request = _youTubeService.Channels.List(parts);

            if (channelType == ChannelType.Channel)
            {
                request.Id = id;
            }
            else
            {
                request.ForUsername = id;
            }

            var response = await request.ExecuteAsync();
            var channel = response.Items.Single();

            return new ChannelInfo(channel);
        }

        public async Task<IEnumerable<string>> GetVideosIds(string uploadsId)
        {
            var ids = new List<string>();
            var pageToken = string.Empty;

            for (int i = 0; i < Constants.YouTube.MaxRequests; i++)
            {
                var request = _youTubeService.PlaylistItems.List(Parts.ContentDetails);
                request.PlaylistId = uploadsId;
                request.MaxResults = Constants.YouTube.MaxResults;
                request.PageToken = pageToken;

                var response = await request.ExecuteAsync();

                ids.AddRange(response.Items.Select(v => v.ContentDetails.VideoId));

                if (response.NextPageToken is null)
                {
                    break;
                }
                else
                {
                    pageToken = response.NextPageToken;
                }
            }

            return ids;
        }

        public async Task<IEnumerable<VideoInfo>> GetVideosAsync(IEnumerable<string> ids)
        {
            var videos = new List<Video>();
            var parts = string.Join(",", Parts.Snippet, Parts.ContentDetails);

            foreach (var batch in ids.Batch(Constants.YouTube.MaxResults))
            {
                var request = _youTubeService.Videos.List(parts);
                request.Id = string.Join(",", batch);

                var response = await request.ExecuteAsync();

                videos.AddRange(response.Items);
            }

            return videos.Select(v => new VideoInfo(v));
        }
    }
}