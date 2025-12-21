using Microsoft.AspNetCore.Mvc;

namespace Basics.Controllers
{
    public class SetupController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
