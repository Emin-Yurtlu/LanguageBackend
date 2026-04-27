using MediatR;

namespace LanguageBackend.Application.Features.Word.Queries.GetLearnedWords
{
    public class GetAllWordsQuery : IRequest<List<GetAllWordsResult>>
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string UserId { get; set; } = string.Empty;
    }
}

