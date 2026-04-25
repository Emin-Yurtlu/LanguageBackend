namespace LanguageBackend.Application.Features.Word.Queries.StartQuiz
{
    public class QuizQuestionResult
    {
        public int UserWordId { get; set; }
        public string EnglishWord { get; set; } = string.Empty;
        // 1 doğru + 3 yanlış karışık sırada
        public List<string> Options { get; set; } = new();
        public string CorrectAnswer { get; set; } = string.Empty;
    }
}
