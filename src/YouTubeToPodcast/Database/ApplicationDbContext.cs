using Microsoft.EntityFrameworkCore;
using YouTubeToPodcast.Models;

namespace YouTubeToPodcast.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Podcast> Podcasts { get; set; }
    }
}