using System;
using System.Xml;
using Google.Apis.YouTube.v3.Data;

namespace YouTubeToPodcast.Models
{
    public class VideoInfo
    {
        public VideoInfo(Video video)
        {
            Id = video.Id;
            Title = video.Snippet.Title;
            Description = video.Snippet.Description;
            PublicationDate = video.Snippet.PublishedAt;
            Duration = ConvertISO8601DurationToTimeSpan(video.ContentDetails.Duration);
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? PublicationDate { get; set; }
        public TimeSpan? Duration { get; set; }

        public string Url => $"https://www.youtube.com/watch?v={Id}";

        private TimeSpan? ConvertISO8601DurationToTimeSpan(string duration)
            => duration is null ? null : (TimeSpan?)XmlConvert.ToTimeSpan(duration);
    }
}