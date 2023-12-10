using System.Web;
using TriviaQuizMVC.Models;

namespace TriviaQuizMVC.Services
{
    public class StashQuizService
    {
        public Quiz CurrentQuiz { get; private set; }

        public void SetCurentQuiz(Quiz quiz)
        {
            var idx = 0;
            if (quiz.Questions?.Any() == true)
            {
                foreach (var item in quiz.Questions!)
                {
                    //quiz.Questions.ElementAt(idx);
                    item.Question = HttpUtility.HtmlDecode(item.Question);
                    item.Correct_Answer = HttpUtility.HtmlDecode(item.Correct_Answer);
                    IList<string> beatyfiedIncorrectAnswers = new List<string>();
                    foreach (var incorrectAns in item.Incorrect_Answers.ToList())
                    {
                        beatyfiedIncorrectAnswers.Add(HttpUtility.HtmlDecode(incorrectAns));
                    }
                    item.Incorrect_Answers = beatyfiedIncorrectAnswers.ToArray();

                    item.Index = idx++;
                }
            }
            CurrentQuiz = quiz;
        }
    }
}
