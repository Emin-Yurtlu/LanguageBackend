using LanguageBackend.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageBackend.Infrastructure.Services
{
    // bu clas IEmailSercive ınterfacesınden  ımplament aldı yanı ordakı metodun ıcının bu clasta nasıl gonderecegımıze gore (smtp) dolduracagız 
    public  class EmailService : IEmailService
    {
        // smtp ayarları dısardan almak ıcın  IConfiguration hazır ınterfacesıne _configuration aracılıgı ıle erısecegzı 
        private readonly IConfiguration _configuration;

        // kurucu metodum IConfiguration hazır bır kod 
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        
        public  async Task SendEmailAsync(string toEmail, string subject, string body)// duzenleyecegım SendEmailAsync ınterfacem  bu ıslem asenkron olcak
        {
            //appsettıngjson dosyasına tanımlayacagımız smtp sunucu adresı,port,kullanıcı adı ve sıfre bılgılerını asagıda cekıyoruz 
            // bu bıze ılerde maıl ayarlarını tek bır yerden degıstırme ımkanı sunacka 
            var host = _configuration["SmtpSettings:Host"];
            var port = int.Parse(_configuration["SmtpSettings:Port"]!);
            var user = _configuration["SmtpSettings:Username"];
            var pass = _configuration["SmtpSettings:Password"];


            // smtp clint olusturm
            using var client = new SmtpClient(host, port)// hangı maıl sunucusuna baglanacaksın  host+port
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(user, pass),//  NetworkCredential ıle gırıs bılgılerım 
                EnableSsl = true// guvenlı baglantı kullanma 
            };

            // mail iceriği
            using var mailMessage = new MailMessage(from: user!, to: toEmail, subject, body)// kımden=userdan kıme=ToEmail konu=subject ıcerık=body
            {
                IsBodyHtml = true // HTML formatında 
            };

            await client.SendMailAsync(mailMessage); // yukarıda olusturdugumuz maili maılı Client.sendmail aracılıgı ıle asenkron gonnder
        }
    }
}
