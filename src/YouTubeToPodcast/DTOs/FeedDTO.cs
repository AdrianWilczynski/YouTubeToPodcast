using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using YouTubeToPodcast.Models;

namespace YouTubeToPodcast.DTOs
{
    [XmlRoot("rss")]
    public class FeedDTO
    {
        private const string ITunesNamespace = "http://www.itunes.com/dtds/podcast-1.0.dtd";

        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Xmlns = new XmlSerializerNamespaces(new[]
        {
             new XmlQualifiedName("itunes", ITunesNamespace)
        });

        public FeedDTO() { }

        public FeedDTO(ChannelInfo channel, IEnumerable<VideoInfo> videos, Podcast podcast, Func<string, string> fileUrlFactory)
        {
            Version = 2.0M;
            Channel = new ChannelDTO
            {
                Title = string.IsNullOrWhiteSpace(podcast.Contains) ? channel.Title : $"{channel.Title} - {podcast.Contains}",
                Link = channel.Url,
                Author = channel.Title,
                Description = channel.Description,
                Summary = channel.Description,
                Subtitle = channel.Title,
                Image = new ImageDTO
                {
                    ImageUrl = channel.ImageUrl
                },
                Items = videos.Select(v => new ItemDto
                {
                    Title = v.Title,
                    Guid = v.Url,
                    Link = v.Url,
                    Description = v.Description,
                    Summary = v.Description,
                    Author = channel.Title,
                    Duration = v.Duration,
                    PublicationDate = v.PublicationDate?.ToString("r"),
                    Enclosure = new EnclosureDto
                    {
                        Url = fileUrlFactory(v.Id),
                        Type = Constants.YouTube.AudioMimeType
                    }
                }).ToArray()
            };
        }

        [XmlAttribute("version")]
        public decimal Version { get; set; }

        [XmlElement("channel")]
        public ChannelDTO Channel { get; set; }

        public class ChannelDTO
        {
            [XmlElement("title")]
            public string Title { get; set; }

            [XmlElement("link")]
            public string Link { get; set; }

            [XmlElement("author", Namespace = ITunesNamespace)]
            public string Author { get; set; }

            [XmlElement("description")]
            public string Description { get; set; }

            [XmlElement("summary", Namespace = ITunesNamespace)]
            public string Summary { get; set; }

            [XmlElement("subtitle", Namespace = ITunesNamespace)]
            public string Subtitle { get; set; }

            [XmlElement("image", Namespace = ITunesNamespace)]
            public ImageDTO Image { get; set; }

            [XmlElement("item")]
            public ItemDto[] Items { get; set; }
        }

        public class ImageDTO
        {
            [XmlAttribute("href")]
            public string ImageUrl { get; set; }
        }

        public class ItemDto
        {
            [XmlElement("title")]
            public string Title { get; set; }

            [XmlElement("guid")]
            public string Guid { get; set; }

            [XmlElement("link")]
            public string Link { get; set; }

            [XmlElement("description")]
            public string Description { get; set; }

            [XmlElement("summary", Namespace = ITunesNamespace)]
            public string Summary { get; set; }

            [XmlElement("author", Namespace = ITunesNamespace)]
            public string Author { get; set; }

            [XmlElement("duration", Namespace = ITunesNamespace)]
            public TimeSpan? Duration { get; set; }

            [XmlElement("pubDate")]
            public string PublicationDate { get; set; }

            [XmlElement("enclosure")]
            public EnclosureDto Enclosure { get; set; }
        }

        public class EnclosureDto
        {
            [XmlAttribute("url")]
            public string Url { get; set; }

            [XmlAttribute("type")]
            public string Type { get; set; }
        }
    }
}