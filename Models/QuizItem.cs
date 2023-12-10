using System.Text.Json.Serialization;

namespace TriviaQuizMVC.Models
{
    public class QuizItem
    {
        public string Category { get; set; }

        public string Question { get; set; }

        public string Correct_Answer { get; set; }
        public string[] Incorrect_Answers { get; set; }
        //public IEnumerable<string> Answers => Incorrect_Answers.Concat(new string[] { Correct_Answer });

        public string Difficulty { get; set; }
        public string Type { get; set; }

        #region Extra Data

        public int Index { get; set; }

        [JsonIgnore]
        private IList<string>? _shuffledAnswers; // { get; private set; }
        //private IEnumerable<string>? ShuffledAnswers; // { get; private set; }

        public string Guess { get; set; }

        public bool CurrentActive { get; set; }

        private IEnumerable<string> ShuffleAnswersGhost()
        {
            if (_shuffledAnswers != null) // ?.Any() == true)
                return _shuffledAnswers;
            _shuffledAnswers = new List<string>();
            var answers = Incorrect_Answers.Concat(new string[] { Correct_Answer }).ToList();
            foreach (var ans in answers.ToList())
            {
                var i = new Random().Next(answers.Count); // Next's max-value is exclusive
                _shuffledAnswers.Add(answers[i]);
                answers.RemoveAt(i);// OR answers.Remove(ans);
            }
            return _shuffledAnswers;
        }

        public IEnumerable<string> ShuffleAnswers()
        {
            if (_shuffledAnswers != null) // (ShuffledAnswers?.Any() == true)
                return _shuffledAnswers;

            var answersSet = Incorrect_Answers.Concat(new string[] { Correct_Answer });
            var remainingAnswersCount = answersSet.Count();
            _shuffledAnswers = new string[remainingAnswersCount];
            foreach (var ans in answersSet)
            {
                var i = new Random().Next(remainingAnswersCount--); // Next's max-value is exclusive
                var firstEmptyPos = _shuffledAnswers.IndexOf(_shuffledAnswers.Where(x => string.IsNullOrEmpty(x)).First());
                _shuffledAnswers[firstEmptyPos] = ans;
            }
            return _shuffledAnswers;
        }
        #endregion
    }
}
