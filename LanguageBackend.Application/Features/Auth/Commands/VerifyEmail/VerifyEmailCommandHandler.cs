using LanguageBackend.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageBackend.Application.Features.Auth.Commands.VerifyEmail
{
    // bu sınıfta verıfyemaılcommand ıle  aldıgımız e maıl ve token ıle dogrulama saglayacagız 
    public  class VerifyEmailCommandHandler :IRequestHandler<VerifyEmailCommand , bool>
    {
        private readonly UserManager<AppUser> _userManager;
        public VerifyEmailCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager= userManager;
            
        }

        public  async Task<bool> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            // ıstekten gelen e mail ile kullanıcıyı buluyoruz buraakı amacımız kayıt olmayan bır e maıle onay token ı gondermmemk
            // eger bu e maıl ıle kayıt olunmamıs ıse false dondur 
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return false;
            }
            //e maıl ıle kullanıcıyı buluyoruz  kulşlanıcıya gonderdıgımız token a da verıyoruz ve 
            // ıdentıty nın kendı ConfirmEmailAsync ıle dogrulama saglıyoruz ve verıytabı e maıl dogruma kısmı 1 oluyor 
            var result = await _userManager.ConfirmEmailAsync(user, request.Token);

            return result.Succeeded;
        }
    }
}
