using YouTubeToPodcast.Models;

namespace YouTubeToPodcast.Database
{
    public interface IPodcastRepository
    {
        Podcast Add(Podcast podcast);
        Podcast GetById(string id);
        void Save();
    }
}