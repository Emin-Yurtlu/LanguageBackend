using LanguageBackend.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using LanguageBackend.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LanguageBackend.Infrastructure.Services
{
    // bu token uretme servısımız IToken Servıces ınterfacemoızden kalıtım alacak 
    public class TokenService : ITokenService
    {
        // burada appsettıngjson dosyasına erısmek ıcın bır nesne tanımlıyoruz 
        private readonly IConfiguration _configuration;

        //kurucu metodumu tanımlıyorum
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(AppUser user)
        {
            // token ı sırelemek ve cozmek ıcın bır key tanımlayacagzı o keyı bır syrınge cevırıyoruz 
            // bu keyı HmacSha256 ıle  dijitan ımza urettık  olusturuyoruz 
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // kullanıcın  ıd emaıl ve user namesını bu token ıcıne gomuyoruz 
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName!)
            };

         
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],// token ı ureten 
                audience: _configuration["JwtSettings:Audience"],// taken ı yayınlayan 
                claims: claims,//toekn ıcıne gomeyecegımız seyler 
                expires: DateTime.Now.AddDays(1), // token yasam omru 
                signingCredentials: creds// uste hazırladıgımız dıjıtal ımza 
            );

           
            return new JwtSecurityTokenHandler().WriteToken(token); // URETILEN TOKEN I DONDURUYORUZ 
        }
    }
}
