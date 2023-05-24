using Microsoft.AspNetCore.Mvc;

namespace OskarLAspNet.Controllers
{
    public class NotFoundController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
