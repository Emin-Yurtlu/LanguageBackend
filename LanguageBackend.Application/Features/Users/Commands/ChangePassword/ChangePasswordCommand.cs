using MediatR;
using System.Text.Json.Serialization;

namespace LanguageBackend.Application.Features.Users.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest<bool>
    {
        [JsonIgnore]
        public string UserId { get; set; } = string.Empty;

        public string CurrentPassword { get; set; } = string.Empty;

        public string NewPassword { get; set; } = string.Empty;
    }
}