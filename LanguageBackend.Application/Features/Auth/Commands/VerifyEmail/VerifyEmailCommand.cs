using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageBackend.Application.Features.Auth.Commands.VerifyEmail
{
    public  class VerifyEmailCommand:IRequest<bool>
    {
        //  bu sınıf bır ıstek sınıfıdır bız command ıclerıne ıstekten alacagımız bılgılerı yazarız 
        // e maıl dogrulama kısmı ıcın bana kullanıcın e maıl ı ve e posta adresıne gonderılen tokanı olması yeterlı 
        public string Email { get; set; } = string.Empty;

        public string VerificationCode { get; set; } = string.Empty;

    }
}
