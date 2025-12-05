using Microsoft.AspNetCore.Mvc;
using Basics.Models;


namespace Basics.Controllers

{
    public class GymController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Apply()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Apply([FromForm] Employee model)
        {   
            
            Repository repo = new Repository();
            repo.AddEmployee(model);
            return View("Feedback", model);
        }
    }
}