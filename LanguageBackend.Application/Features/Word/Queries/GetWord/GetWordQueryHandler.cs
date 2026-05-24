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
            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            var level = user.Level.ToString();

            int maxAttempts = 5; // Sonsuz döngüyü önlemek için limit
            var excludedWords = new List<string>();

            for (int i = 0; i < maxAttempts; i++)
            {
                // Gemini'den kelime iste (excludedWords listesini de gönderiyoruz)
                var (englishWord, turkishMeaning, exampleSentence, exampleSentenceTr) = await _geminiService.GetNewWordAsync(level, excludedWords);

                // Veritabanında var mı kontrol et
                var exists = await _userWordRepository.ExistsAsync(request.UserId, englishWord);

                if (!exists)
                {
                    // Veritabanında YOKSA: Kaydet ve işlemi bitir
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

                    return new GetWordResult
                    {
                        EnglishWord = englishWord,
                        TurkishMeaning = turkishMeaning,
                        ExampleSentence = exampleSentence,
                        ExampleSentenceTr = exampleSentenceTr
                    };
                }

                // Veritabanında VARSA: Kelimeyi hariç tutulacaklar listesine ekle ve döngüye devam et
                excludedWords.Add(englishWord);
            }

            // 5 denemede de sürekli veritabanında olan bir kelime geldiyse
            throw new Exception("Şu an için yeni bir kelime bulunamadı, lütfen tekrar deneyin.");
        }
    }
}