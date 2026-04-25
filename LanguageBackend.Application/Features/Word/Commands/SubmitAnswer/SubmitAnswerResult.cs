namespace LanguageBackend.Application.Features.Word.Commands.SubmitAnswer
{
    public class SubmitAnswerResult
    {
        public bool IsCorrect { get; set; }
        public string CorrectAnswer { get; set; } = string.Empty;
    }
}
