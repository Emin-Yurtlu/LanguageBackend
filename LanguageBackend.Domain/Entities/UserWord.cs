using System;

namespace LanguageBackend.Domain.Entities
{
    // bu sınıf bızım kelımelerımızın ozellıklerını tutacak ıd user ıd ingilizcesi turkcesı ıslearned ve seenat
    // burada ıslearned kısmı kullanıcı quzıde dogru bılırse trure olacak ve karsısına bır aha cıkmayacak bunun ıcın kullanacagız 
    public class UserWord
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        // kelımeler ıle kullanıcıları eslestırmek ıcın Navigation property — AppUser ile ilişki
        public AppUser User { get; set; } = null!;

        public string EnglishWord { get; set; } = string.Empty;

        public string TurkishMeaning { get; set; } = string.Empty;

        public bool IsLearned { get; set; } = false;

        public DateTime SeenAt { get; set; } = DateTime.UtcNow;
    }
}