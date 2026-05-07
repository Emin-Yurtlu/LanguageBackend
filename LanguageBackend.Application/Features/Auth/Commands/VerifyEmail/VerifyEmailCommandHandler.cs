using LanguageBackend.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LanguageBackend.Application.Features.Auth.Commands.VerifyEmail
{
    public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, bool>
    {
        private readonly UserManager<AppUser> _userManager;

        public VerifyEmailCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            // E-mail ile kullanıcıyı bul
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return false;

            // Kod eşleşiyor mu?
            if (user.VerificationCode != request.Token)
                return false;

            // Kodun süresi dolmuş mu?
            if (user.VerificationCodeExpiry == null || user.VerificationCodeExpiry < DateTime.UtcNow)
                return false;

            // ✅ E-postayı doğrulanmış olarak işaretle
            user.EmailConfirmed = true;

            // ✅ Kodu temizle (bir daha kullanılmasın)
            user.VerificationCode = null;
            user.VerificationCodeExpiry = null;

            await _userManager.UpdateAsync(user);
            return true;
        }
    }
}
