using LanguageBackend.Application.Interfaces;
using LanguageBackend.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace LanguageBackend.Application.Features.Auth.Commands.Register
{
    public class RegisterCommadHandler : IRequestHandler<RegisterCommand, string>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RegisterCommadHandler(UserManager<AppUser> userManager, IServiceScopeFactory serviceScopeFactory)
        {
            _userManager = userManager;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
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
                var code = new Random().Next(100000, 999999).ToString();
                user.VerificationCode = code;
                user.VerificationCodeExpiry = DateTime.UtcNow.AddMinutes(15);
                await _userManager.UpdateAsync(user);

                var emailBody = $@"...{code}...";
                var email = user.Email!;

                // Arka planda mail gönderimi, kullanıcının kaydını bekletmez.
                _ = Task.Run(async () =>
                {
                    try
                    {
                        using var scope = _serviceScopeFactory.CreateScope();
                        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                        await emailService.SendEmailAsync(email, "WordFlow - E-Posta Doğrulama Kodu", emailBody);
                    }
                    catch (Exception ex)
                    {
                        // Hatayı görebilmemiz için geçici olarak masaüstüne yazdırıyoruz
                        System.IO.File.WriteAllText(@"c:\Users\memin\Desktop\mail_error.txt", ex.ToString());
                        Console.WriteLine($"Mail gönderilemedi: {ex.Message}");
                    }
                });

                return ""; // Başarılıysa boş string döner
            }

            // Hata varsa hataları birleştirip string olarak dönüyoruz
            var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
            return errors;
        }
    }
}
