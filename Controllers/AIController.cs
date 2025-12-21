using Basics.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basics.Controllers
{
    [Authorize]
    public class AIController : Controller
    {
        private readonly AIService _aiService;

        public AIController(AIService aiService)
        {
            _aiService = aiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetRecommendation(int age, double weight, double height, string gender, string goal)
        {
            var recommendation = await _aiService.GetExerciseRecommendations(age, weight, height, gender, goal);
            return Json(new { success = true, recommendation });
        }
    }
}
