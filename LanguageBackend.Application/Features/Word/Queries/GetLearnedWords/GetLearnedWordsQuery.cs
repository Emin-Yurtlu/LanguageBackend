using MediatR;

namespace LanguageBackend.Application.Features.Word.Queries.GetLearnedWords
{
    public class GetLearnedWordsQuery : IRequest<List<GetLearnedWordsResult>>
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string UserId { get; set; } = string.Empty;
    }
}
