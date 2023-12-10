using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Web;

using TriviaQuizMVC.Models;
using TriviaQuizMVC.Services;
using static TriviaQuizMVC.Models.Enums;

namespace TriviaQuizMVC.Controllers
{
    public class HomeController : BaseController<HomeController>
    {
        private readonly StashQuizService _stashService;

        public HomeController(
           ILogger<HomeController> logger,
           UserManager<IdentityUser> userManager,
           RoleManager<IdentityRole> roleManager,
           StashQuizService tempService)
           : base(logger, userManager, roleManager)
        {
            _stashService = tempService;
        }

        public async Task<IActionResult> Index()
        {
            return await ExecuteGet("?amount=5", async res =>
            {
                IList<QuizItem>? questions = null;
                if (res.IsSuccessStatusCode)
                {
                    //var readTask = res.Content.ReadAsStringAsync();
                    //readTask.Wait();
                    var jsonStr = (await res.Content.ReadAsStringAsync()).Trim('{', '}');
                    var resultsJsonAttribute = "\"results\":";
                    var resultsIndex = jsonStr.IndexOf(resultsJsonAttribute);
                    if (resultsIndex != -1)
                    {
                        jsonStr = jsonStr.Substring(resultsIndex + resultsJsonAttribute.Length);
                        Logger.LogInformation(jsonStr);
                        questions = Deserialize<List<QuizItem>>(jsonStr);
                        if (questions?.Any() == true)
                            questions.First().CurrentActive = true;
                    }
                }

                _stashService.SetCurentQuiz(new Quiz
                {
                    Questions = questions
                });

                return View(new Quiz
                {
                    Questions = questions
                });
            });
        }

        // PARAMS' NAMES MUST MATCH WITH ANONYMOUS OBJ SENT ON URL.ACTION CALL
        public IActionResult NavigateTo(
            string currentQuestionJSON,
            int goTo)
        {
            if (string.IsNullOrEmpty(currentQuestionJSON))
                throw new NullReferenceException();
            if (!Enum.TryParse<NavigateTo>(goTo.ToString(), out var navigateTo))
                throw new InvalidOperationException();

            try
            {
                var quiz = _stashService.CurrentQuiz;

                var current = Deserialize<QuizItem>(currentQuestionJSON);
                var idx = current?.Index ?? -1;

                if (current != null)
                {
                    var questions = quiz.Questions!;

                    // current's Guess comes null (binding problem???)
                    // ACTUALLY, current's Guess is the value of currently-active question's (CurrentActive == true) Guess when the view was rendered
                    //if (!string.IsNullOrEmpty(questions[idx].Guess) &&
                    //    current.Correct_Answer == questions[idx].Guess)
                    //    quiz.Score++; // SCORE WILL BE CALCULATED ONCE ON FINAL SUBMIT

                    questions[idx].CurrentActive = false;
                    if (navigateTo == Enums.NavigateTo.Next && idx < questions.Count - 1)
                        idx++;
                    else if (navigateTo == Enums.NavigateTo.Previous && idx > 0)
                        idx--;
                    questions[idx].CurrentActive = true;

                    //quiz.SubmitAvailable = idx == questions.Count;
                    quiz.SubmitAvailable = !questions.Any(x => string.IsNullOrEmpty(x.Guess));
                }

                // Partial-rendering using jQuery
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return PartialView("Index", quiz);

                return View("Index", quiz);
            }
            catch (Exception x)
            {
                throw;
            }
        }

        public IActionResult TrackSelected(string index, string input)
        {
            if (!int.TryParse(index, out var idx))
                return BadRequest();

            var quiz = _stashService.CurrentQuiz;
            var currentQuestion = quiz.Questions![idx];
            currentQuestion.Guess = input;
            return Ok();
        }


        #region Pre-existing project-template's actions

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion

    }
}