using LanguageBackend.Application.Interfaces;
using LanguageBackend.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageBackend.Application.Features.Auth.Commands.Register
{
    // asıl olay burada gerceklesecektır ıstegı alıp verıtabanına kaydedıp mail gonderecek sınıf burası olacaktır 
    public class RegisterCommadHandler : IRequestHandler<RegisterCommand, bool>
    {
        // user managere ve IemailServıce ye rısecegım  bır nesen tanımlıyorum

        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        
        // kurucu metodum
        public RegisterCommadHandler(UserManager<AppUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;

        }
        // kullandıgım ınterfacenın ıcını burada dolduruyorum ıstek once regıster command a gıdecek 
        public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            //yenı bır kullanıcı nesnemız olusturuldu 
            //ıcerısını ıstekten gelen bılgılerı yazdık 
            var user = new AppUser
            {
                UserName = request.UserName,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Level = request.Level
            };
            //bu kullaıcıyı ve posswordu user managere vererek wbıze bır kullanıcı olusturmasını ıstedık 
            var result = await _userManager.CreateAsync(user, request.Password);

            // eger sonuc basarılı oldı ıse 
            if (result.Succeeded)
            {
               // yıne user manager aracılıgı ıle bıze bu kullanıcıya ozel  bır e maıl dogrulama tokenı uretmesını ıstedık
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                //  e postamızın ıcerıgını uretıp e mail senrvisimize yonlendırıyoruz 
                var emailBody = $@"
                    <h3>LanguageApp'e Hoşgeldiniz!</h3>
                    <p>Hesabınızı onaylamak için lütfen aşağıdaki kodu kullanın:</p>
                    <strong>{token}</strong>";

                await _emailService.SendEmailAsync(user.Email!, "E-Posta Onay Kodu", emailBody);
                return true;

            }
            // kullanıcı kaydı yapılmazsa basıt anlamda fale dondurduk
            return false;
        }
    }
}
