using LanguageBackend.Application.Interfaces;
using LanguageBackend.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LanguageBackend.Application.Features.Auth.Commands.Register
{
    public class RegisterCommadHandler : IRequestHandler<RegisterCommand, bool>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;

        public RegisterCommadHandler(UserManager<AppUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new AppUser
            {
                UserName = request.UserName,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Level = request.Level
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                // ✅ 6 haneli rastgele kod üret
                var code = new Random().Next(100000, 999999).ToString();

                // ✅ Kodu ve son geçerlilik tarihini kullanıcıya kaydet (15 dakika geçerli)
                user.VerificationCode = code;
                user.VerificationCodeExpiry = DateTime.UtcNow.AddMinutes(15);
                await _userManager.UpdateAsync(user);

                // ✅ E-postaya kodu gönder
                var emailBody = $@"
                    <h3>WordFlow'a Hoşgeldiniz!</h3>
                    <p>E-posta doğrulama kodunuz:</p>
                    <h1 style='letter-spacing: 8px; color: #6C63FF;'>{code}</h1>
                    <p>Bu kod <strong>15 dakika</strong> geçerlidir.</p>";

                await _emailService.SendEmailAsync(user.Email!, "WordFlow - E-Posta Doğrulama Kodu", emailBody);
                return true;
            }

            return false;
        }
    }
}
