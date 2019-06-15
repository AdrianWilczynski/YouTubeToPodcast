using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YouTubeToPodcast.Database;
using YouTubeToPodcast.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;
using YoutubeExplode;
using WebEssentials.AspNetCore.Pwa;

namespace YouTubeToPodcast
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddXmlSerializerFormatters();

            services.AddMemoryCache();
            services.AddResponseCaching();

            services.AddHttpContextAccessor();

            services.AddProgressiveWebApp(new PwaOptions
            {
                Strategy = ServiceWorkerStrategy.CacheFingerprinted,
                RoutesToPreCache = "/",
                OfflineRoute = "/Offline"
            });

            var youTubeApiKey = Configuration["YouTubeApi:Key"];
            services.AddTransient(_ => new YouTubeService(new BaseClientService.Initializer { ApiKey = youTubeApiKey }));

            services.AddTransient<IYoutubeClient, YoutubeClient>();

            services.AddScoped<IPodcastRepository, PodcastRepository>();
            services.AddTransient<IYouTubeApiClient, YouTubeApiClient>();
            services.AddTransient<IYouTubeScrapper, YouTubeScrapper>();
            services.AddTransient<IFeedGenerator, FeedGenerator>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithRedirects("/Error/{0}");

            app.UseHttpsRedirection();

            app.UseResponseCaching();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Podcast}/{action=New}/{id?}");
            });
        }
    }
}
