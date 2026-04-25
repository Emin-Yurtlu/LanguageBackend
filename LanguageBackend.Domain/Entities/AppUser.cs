using LanguageBackend.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageBackend.Domain.Entities
{

    // burada ıdentıty user tablosuna ek olarak 3 tane kolan ekledık  bu sınıfı ıdentıtyuserdan kalıtım aldı 
    // artık bızım en genıs sınıfımız appuser sınıfıdır 
    // tablomuzda domaın ıcınde tanımladıgımız enum sabıtlerını tutacagımız bır alanda yaptık 
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public EnglishLevel Level { get; set; }
    }
}
