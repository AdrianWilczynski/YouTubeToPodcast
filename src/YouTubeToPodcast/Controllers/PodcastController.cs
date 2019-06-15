using Microsoft.AspNetCore.Mvc;
using YouTubeToPodcast.Database;
using YouTubeToPodcast.Models;
using YouTubeToPodcast.DTOs;
using System.Net.Mime;
using YouTubeToPodcast.Services;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using System;
using YouTubeToPodcast.Extensions;

namespace YouTubeToPodcast.Controllers
{
    public class PodcastController : Controller
    {
        private readonly IPodcastRepository _podcastRepository;
        private readonly IYouTubeScrapper _youTubeScrapper;
        private readonly IFeedGenerator _feedGenerator;
        private readonly IMemoryCache _memoryCache;

        public PodcastController(IPodcastRepository podcastRepository, IYouTubeScrapper youTubeScrapper, IFeedGenerator feedGenerator, IMemoryCache memoryCache)
        {
            _podcastRepository = podcastRepository;
            _youTubeScrapper = youTubeScrapper;
            _feedGenerator = feedGenerator;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [ResponseCache(Duration = Constants.Cache.MinutesTillExpiration * 60)]
        public IActionResult New() => View();

        [HttpPost]
        public IActionResult New(NewPodcastDTO newPodcast)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var podcast = new Podcast(newPodcast.Url, newPodcast.Contains, newPodcast.Duration);

            _podcastRepository.Add(podcast);
            _podcastRepository.Save();

            return RedirectToAction(nameof(Created), new { podcast.Id });
        }

        [Route("/[action]/{id}")]
        public IActionResult Created(string id)
        {
            var podcast = _podcastRepository.GetById(id);

            if (podcast is null)
            {
                return NotFound();
            }

            var feedUrl = Url.Action(nameof(Feed), nameof(PodcastController).RemoveSuffix(nameof(Controller)),
                new { podcast.Id }, Request.Scheme);

            return View(model: feedUrl);
        }

        [Route("/[action]/{id}")]
        [Produces(MediaTypeNames.Application.Xml)]
        [ResponseCache(Duration = Constants.Cache.MinutesTillExpiration * 60)]
        public async Task<ActionResult<FeedDTO>> Feed(string id)
        {
            var podcast = _podcastRepository.GetById(id);

            if (podcast is null)
            {
                return NotFound();
            }

            return await _memoryCache.GetOrCreateAsync($"{nameof(Feed)}:{id}", async e =>
            {
                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Constants.Cache.MinutesTillExpiration);

                return await _feedGenerator.CreateFeedAsync(podcast, Request.Scheme, Request.Host);
            });
        }

        [Route("/[action]/{id}")]
        public async Task<IActionResult> File(string id)
        {
            id = id.RemoveSuffix(Constants.YouTube.AudioFileExtension);

            var url = await _memoryCache.GetOrCreateAsync($"{nameof(File)}:{id}", async e =>
            {
                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Constants.Cache.MinutesTillExpiration);

                return await _youTubeScrapper.GetAudioFileUrl(id);
            });

            return Redirect(url);
        }
    }
}