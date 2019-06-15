using Microsoft.AspNetCore.Mvc;

namespace YouTubeToPodcast.Controllers
{
    public class HomeController : Controller
    {
        [Route("/Error/{statusCode?}")]
        public IActionResult Error(int? statusCode) => View(model: statusCode);
    }
}
