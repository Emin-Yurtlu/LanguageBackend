using MediatR;

namespace LanguageBackend.Application.Features.Users.Oueries.GetUser
{
    public class GetUserByIdQuery : IRequest<GetUserResult>
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string UserId { get; set; } = string.Empty;
    }
}
