using System.Collections.Generic;
using Google.Apis.YouTube.v3.Data;

namespace YouTubeToPodcast.Models
{
    public class ChannelInfo
    {
        public ChannelInfo(Channel channel)
        {
            Id = channel.Id;
            Title = channel.Snippet.Title;
            Description = channel.Snippet.Description;
            ImageUrl = TakeBestQualityThumbnail(channel.Snippet.Thumbnails);
            UploadsListId = channel.ContentDetails.RelatedPlaylists.Uploads;
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string UploadsListId { get; set; }

        public string Url => $"https://www.youtube.com/channel/{Id}";

        private string TakeBestQualityThumbnail(ThumbnailDetails thumbnail)
            => thumbnail.Maxres?.Url
            ?? thumbnail.Standard?.Url
            ?? thumbnail.High?.Url
            ?? thumbnail.Medium?.Url
            ?? thumbnail.Default__?.Url;
    }
}