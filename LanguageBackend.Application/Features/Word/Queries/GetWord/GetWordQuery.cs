using MediatR;

namespace LanguageBackend.Application.Features.Word.Queries.GetWord
{
    public class GetWordQuery : IRequest<GetWordResult>
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string UserId { get; set; } = string.Empty;
    }
}