using MediatR;

namespace LanguageBackend.Application.Features.Word.Queries.GetLearnedWords
{
    public class GetNotLearnedWordsQuery : IRequest<List<GetNotLearnedWordsResult>>
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string UserId { get; set; } = string.Empty;
    }
}
