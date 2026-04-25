using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageBackend.Application.Features.Auth.Commands.Login
{
    // işlem sonunda uretılen tokene string olarak dondurecek bız bunu logıncommand hanler sınıfında kullanacagız 
    public  class LoginCommand:IRequest<string>
    {
        // Giriş ıcın kullanıcının emaıl ve sıfresı lazım olacak 
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
