
using MediatR;

namespace LanguageBackend.Application.Features.Word.Queries.StartQuiz
{
    public class StartQuizQuery : IRequest<List<QuizQuestionResult>>
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string UserId { get; set; } = string.Empty;
       
    }
}