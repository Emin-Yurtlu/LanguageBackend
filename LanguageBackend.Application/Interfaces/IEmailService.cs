using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// e posta işlemleri  ıcın stmp servisine   ihtiyacımız var bunu bagımlılıkları tesrıne cevırerek yapacagız(Dependency Inversion)
// bunu neden ınterface  Dependency Inversion yapıyoruz cunku   bız sadec smtp ıle maıl gondermek ıstemeyebılırız yarın baska servıs ıle gondermek ıstedıgımızde 
// sadece servis kısmını degıstırırız yanı Dependency Inversion ıle nasıl gonderıldıgı degıl  gonderılme ısını cozmus oluruz 
//"E-posta gönder" sözleşmesidir, nasıl gönderileceğini bilmez
namespace LanguageBackend.Application.Interfaces
{
    public  interface IEmailService
    {

       //  bır ıntercace ıle e maıl gonderme degıskenlerı tanımladık hangı servısı kullanırsan kullan bunu ımplamet et buna gore neyle gonderecegını ayarla
       // toEmail   kime gönderilecek
       //  subject  mail başlığı
       // body      mail içeriği
       // task cunku asenkorın yap zaman alabılır beklemeden calıstır dedık 
        Task SendEmailAsync(string toEmail, string subject,string body);
    }
}
