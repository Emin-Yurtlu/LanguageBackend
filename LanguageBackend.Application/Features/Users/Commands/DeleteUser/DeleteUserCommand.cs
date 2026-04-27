using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LanguageBackend.Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<bool>
    {
        [JsonIgnore]
        public string UserId { get; set; } = string.Empty;

    }
}
