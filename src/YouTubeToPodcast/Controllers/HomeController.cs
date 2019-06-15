using Microsoft.AspNetCore.Mvc;

namespace YouTubeToPodcast.Controllers
{
    public class HomeController : Controller
    {
        [Route("/[action]/{statusCode?}")]
        public IActionResult Error(int? statusCode) => View(model: statusCode);
    }
}
