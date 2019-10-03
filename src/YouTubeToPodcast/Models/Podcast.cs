using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.WebUtilities;

namespace YouTubeToPodcast.Models
{
    public class Podcast
    {
        public Podcast() { }

        public Podcast(string url, string contains, int duration)
        {
            Id = NewShortId();
            YouTubeId = ExtractYouTubeId(url);
            ChannelType = DetermineChannelType(url);
            Contains = contains;
            Duration = TimeSpan.FromMinutes(duration);
        }

        public string Id { get; set; }

        public string YouTubeId { get; set; }
        public ChannelType ChannelType { get; set; }
        public string Contains { get; set; }
        public TimeSpan Duration { get; set; }

        private string NewShortId()
            => WebEncoders.Base64UrlEncode(Guid.NewGuid().ToByteArray());

        private string ExtractYouTubeId(string url)
            => Regex.Match(url, @"(https?:\/\/)?(www\.)?(m\.)?youtube\.com\/(channel|user)\/(?<identifier>[^\/?\s]+).*")
                .Groups["identifier"]
                .Value;

        private ChannelType DetermineChannelType(string url)
            => IsUrlOfType(url, "user") ? ChannelType.User
            : IsUrlOfType(url, "channel") ? ChannelType.Channel
            : throw new ArgumentException();

        private bool IsUrlOfType(string url, string type)
            => Regex.IsMatch(url, $@"(https?:\/\/)?(www\.)?(m\.)?youtube\.com\/{type}\/[^\/?\s]+.*");
    }
}