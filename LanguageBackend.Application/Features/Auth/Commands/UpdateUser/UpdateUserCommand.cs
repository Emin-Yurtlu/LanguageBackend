using LanguageBackend.Domain.Enum; 
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LanguageBackend.Application.Features.Auth.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<bool>
    {
        // [JsonIgnore] ekleyerek Swagger'da bu alanın görünmesini engelliyoruz. 
        // Çünkü ID'yi kullanıcı değil, biz token'dan alacağız.
        [JsonIgnore]
        public string UserId { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public EnglishLevel Level { get; set; } 
    }
}
