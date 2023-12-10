using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace TriviaQuizMVC.Controllers
{
    public abstract class BaseController<T> : Controller
    {
        protected readonly string BaseUri = "https://opentdb.com/api.php";

        protected ILogger<T> Logger { get; }

        protected UserManager<IdentityUser> UserManager { get; } // NOT IN USE
        protected RoleManager<IdentityRole> RoleManager { get; } // NOT IN USE

        //protected TriviaDbContext DbContext { get; }

        protected BaseController(
            ILogger<T> logger,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
            //TriviaDbContext dbContext)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            //DbContext = dbContext;
            Logger = logger;
        }


        public async Task<IActionResult> ExecuteGet(string queryString, Func<HttpResponseMessage, Task<IActionResult>> func)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://opentdb.com/api.php");
                //client.BaseAddress = new Uri("https://opentdb.com/api_config.php");
                var res = await client.GetAsync(queryString);
                return await func(res);
            }
        }

        #region Serialization

        public static string Serialize<R>(R obj)
        {
            return JsonSerializer.Serialize(obj);
            //new JsonSerializerOptions
            //{
            //    PropertyNameCaseInsensitive = true
            //});
        }

        protected R? Deserialize<R>(string jsonStr)
        {
            return JsonSerializer.Deserialize<R>(
                jsonStr,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        #endregion

    }
}