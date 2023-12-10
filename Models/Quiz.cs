namespace TriviaQuizMVC.Models
{
    public class Quiz
    {
        public IList<QuizItem>? Questions { get; set; }
        public int? Total => Questions?.Count;

        public int Score { get; set; } = -1;

        public QuizItem? CurrentlyActive => Questions?.FirstOrDefault(x => x.CurrentActive);

        public bool SubmitAvailable { get; set; }

        public int GetScore()
        {
            return Questions!.Where(q => q.Guess == q.Correct_Answer).Count();
        }
    }
}
