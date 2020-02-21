using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using YouTubeToPodcast.Database;
using YouTubeToPodcast.Models;

namespace YouTubeToPodcast.IntegrationTests
{
    public class AppShould : IClassFixture<Factory>
    {
        private readonly Factory _factory;

        public AppShould(Factory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task RenderMainPage()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task RedirectToAudioFile()
        {
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            var response = await client.GetAsync("/File/bRR4UtpFQMI.m4a");

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Contains("googlevideo.com", response.Headers.Location.AbsoluteUri);
        }

        [Fact]
        public async Task CreateNewPodcast()
        {
            var client = _factory.CreateClient();

            var response = await client.PostAsync("/", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Url", "https://www.youtube.com/channel/UC0-swBG9Ne0Vh4OuoJ2bjbA")
            }));

            var contnet = await response.Content.ReadAsStringAsync();

            Podcast podcastInDb = null;
            using (var scope = _factory.Server.Host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                podcastInDb = dbContext.Podcasts.FirstOrDefault(p => p.YouTubeId == "UC0-swBG9Ne0Vh4OuoJ2bjbA");
            }

            response.EnsureSuccessStatusCode();
            Assert.Contains("/Feed/", contnet);
            Assert.NotNull(podcastInDb);
        }

        [Fact]
        public async Task GenerateRssFeed()
        {
            string id = null;

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var serviceProvider = services.BuildServiceProvider();
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                        var podcast = new Podcast("https://www.youtube.com/channel/UCarEovlrD9QY-fy-Z6apIDQ", null, 0);

                        dbContext.Podcasts.Add(podcast);
                        dbContext.SaveChanges();

                        id = podcast.Id;
                    }
                });
            }).CreateClient();

            var response = await client.GetAsync($"/Feed/{id}");

            var content = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            Assert.Contains(MediaTypeNames.Application.Xml, response.Content.Headers.ContentType.ToString());
            Assert.Contains("<title>Patriot Act</title>", content);
        }
    }
}
