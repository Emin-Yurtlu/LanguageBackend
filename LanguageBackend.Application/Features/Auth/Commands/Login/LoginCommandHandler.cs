using LanguageBackend.Application.Interfaces;
using LanguageBackend.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageBackend.Application.Features.Auth.Commands.Login
{
    public  class LoginCommandHandler :IRequestHandler<LoginCommand,string>
    {
        // uermanager ve Itokenservıceye ulasacagımız nesnelerı tanımlıyoruz
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        
        // kurucu metodumuzu yazıyoruz 
        public LoginCommandHandler(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        // logın commansdan e maıl ve pasword alarak 
        public  async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // e maıl ıle kullanıcı buluyoruz kullanıcı varmı oontrolunu yapuıyoruz 
             var user = await _userManager.FindByEmailAsync(request.Email);


            if(user == null) 
             {
                return "Hata: Kullanıcı bulunamadı.";

            }
            // kullanıcın e maıl adresı dogrulanmıs mı ona bakıyoruz 
            if (!user.EmailConfirmed)
            {
                return "Hata: Lütfen önce e-posta adresinizi onaylayın.";
            }

           // kullanıcının sıfresını  ve  ıstekden  gelen  sıfreyıo karsılastırıyoruz 
         var passwordCheck= await _userManager.CheckPasswordAsync(user,request.Password);
            if (!passwordCheck)
            {
                return "Hata: Şifre hatalı.";
            }

            // hersey uygunsa ITokenserviceye gıt kullanıcıya ozel tokan uret  
            return _tokenService.CreateToken(user);
        }
    }
}
