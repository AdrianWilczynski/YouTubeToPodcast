using System.Linq;
using YouTubeToPodcast.Models;

namespace YouTubeToPodcast.Database
{
    public class PodcastRepository : IPodcastRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PodcastRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Podcast Add(Podcast podcast)
            => _dbContext.Podcasts.Add(podcast).Entity;

        public Podcast GetById(string id)
            => _dbContext.Podcasts.FirstOrDefault(p => p.Id == id);

        public void Save()
            => _dbContext.SaveChanges();
    }
}