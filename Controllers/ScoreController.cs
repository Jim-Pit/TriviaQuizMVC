using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TriviaQuizMVC.Services;

namespace TriviaQuizMVC.Controllers
{
    public class ScoreController : BaseController<ScoreController>
    {
        private readonly StashQuizService _stashService;

        public ScoreController(
           ILogger<ScoreController> logger,
           UserManager<IdentityUser> userManager,
           RoleManager<IdentityRole> roleManager,
           //TriviaDbContext dbContext,
           StashQuizService tempService)
           : base(logger, userManager, roleManager)
        {
            _stashService = tempService;
        }

        public IActionResult Index()
        {
            var quiz = _stashService.CurrentQuiz;

            quiz.Score = quiz.GetScore();
            ViewData[nameof(quiz.Score)] = quiz.Score;

            //return View();
            return View(quiz);
        }
    }
}
