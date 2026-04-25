using LanguageBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageBackend.Application.Interfaces
{
    // bu ınterfacemızde e maili dogrulanmıs  dogrulanmıs kullanıcı ıcın gırıs yapmaya yarayan bır token uretecegız 
    public  interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
