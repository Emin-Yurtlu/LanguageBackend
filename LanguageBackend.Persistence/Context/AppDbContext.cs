using LanguageBackend.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageBackend.Persistence.Context
{
    // bu onıon mımarı katmaında verı tabanı ıslelerını  gercekleştırecegız context burada yer alacak 
    //context sınıfımı ıdentıty contexten ınterfacesınden  mıras aldı varsayılan ıdentıtyuser entıtıes yerıne ozelllestırdıgım appuser sınıfını kullan dedık 
    public class AppDbContext : IdentityDbContext<AppUser>
    {
       public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        
   
    protected override void OnModelCreating(ModelBuilder builder)
        {
            
        }
    }
}

