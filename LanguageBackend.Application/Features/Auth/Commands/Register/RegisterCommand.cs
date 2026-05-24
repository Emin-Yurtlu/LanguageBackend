using LanguageBackend.Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageBackend.Application.Features.Auth.Commands.Register
{
    // bu sınıf commnads register sınıfıdır kullanıcı kayıt olurken ondan hangı bılgılerı alacagımızı buraya yazarız 
    // bu sınıfın <bool> olma sebebı  bu ıslem sonunda bıze true veya false dönmesını saglamaktır 
    // register handler sınıfı bır ıstek snıfıdır 
    public class RegisterCommand:IRequest<string>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public EnglishLevel Level { get; set; }
    }
}
