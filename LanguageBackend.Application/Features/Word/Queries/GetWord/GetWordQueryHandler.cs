using LanguageBackend.Application.Interfaces;
using LanguageBackend.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LanguageBackend.Application.Features.Word.Queries.GetWord
{
    public class GetWordQueryHandler : IRequestHandler<GetWordQuery, GetWordResult>
    {
        private readonly IUserWordRepository _userWordRepository;
        private readonly IGeminiService _geminiService;
        private readonly UserManager<AppUser> _userManager;

        public GetWordQueryHandler(
            IUserWordRepository userWordRepository,
            IGeminiService geminiService,
            UserManager<AppUser> userManager)
        {
            _userWordRepository = userWordRepository;
            _geminiService = geminiService;
            _userManager = userManager;
        }

        public async Task<GetWordResult> Handle(GetWordQuery request, CancellationToken cancellationToken)
        {
            // Kullanıcının seviyesini çek
            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            var level = user.Level.ToString();

            // Gemini'den kelime iste (sadece seviye gönder)
            var (englishWord, turkishMeaning) = await _geminiService.GetNewWordAsync(level, new List<string>());

            // Veritabanında var mı kontrol et
            var exists = await _userWordRepository.ExistsAsync(request.UserId, englishWord);

            if (!exists)
            {
                // Yoksa kaydet
                var userWord = new UserWord
                {
                    UserId = request.UserId,
                    EnglishWord = englishWord,
                    TurkishMeaning = turkishMeaning,
                    IsLearned = false,
                    SeenAt = DateTime.UtcNow
                };

                await _userWordRepository.AddWordAsync(userWord);
                await _userWordRepository.SaveChangesAsync();
            }

            // Kullanıcıya döndür
            return new GetWordResult
            {
                EnglishWord = englishWord,
                TurkishMeaning = turkishMeaning
            };
        }
    }
}