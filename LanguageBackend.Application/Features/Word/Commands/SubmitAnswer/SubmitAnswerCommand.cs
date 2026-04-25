using MediatR;

namespace LanguageBackend.Application.Features.Word.Commands.SubmitAnswer
{
    public class SubmitAnswerCommand : IRequest<SubmitAnswerResult>
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string UserId { get; set; } = string.Empty;

        public int UserWordId { get; set; }
        public string SelectedAnswer { get; set; } = string.Empty;
    }
}