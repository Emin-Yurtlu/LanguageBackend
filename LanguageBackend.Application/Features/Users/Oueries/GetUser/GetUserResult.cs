using LanguageBackend.Domain.Enum;

namespace LanguageBackend.Application.Features.Users.Oueries.GetUser
{
    public class GetUserResult
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public EnglishLevel Level { get; set; }
    }
}
