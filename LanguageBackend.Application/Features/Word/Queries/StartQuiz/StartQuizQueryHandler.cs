using LanguageBackend.Application.Interfaces;
using MediatR;

namespace LanguageBackend.Application.Features.Word.Queries.StartQuiz
{
    public class StartQuizQueryHandler : IRequestHandler<StartQuizQuery, List<QuizQuestionResult>>
    {
        private readonly IUserWordRepository _userWordRepository;

        public StartQuizQueryHandler(IUserWordRepository userWordRepository)
        {
            _userWordRepository = userWordRepository;
        }

        public async Task<List<QuizQuestionResult>> Handle(StartQuizQuery request, CancellationToken cancellationToken)
        {
            // Kullanıcının henüz öğrenmediği kelimeler → bunlar sorular olacak
            var unlearned = await _userWordRepository.GetUnlearnedWordsByUserIdAsync(request.UserId);

            var selectedWords = unlearned.OrderBy(_ => Guid.NewGuid()).Take(10).ToList();

            if (!selectedWords.Any())
                return new List<QuizQuestionResult>();

            // Yanlış şıklar için → öğrenilmiş kelimelerden çek
            var wrongPool = await _userWordRepository.GetLearnedTurkishMeaningsByUserIdAsync(request.UserId);

            var questions = new List<QuizQuestionResult>();
            var random = new Random();

            foreach (var word in selectedWords)
            {
                // Yanlış şık adayları
                var wrongOptions = wrongPool
                    .Where(x => x != word.TurkishMeaning)
                    .OrderBy(_ => random.Next())
                    .Take(3)
                    .ToList();

                // Yanlış şık yetmezse öğrenilmemiş kelimelerden tamamla
                if (wrongOptions.Count < 3)
                {
                    var extra = selectedWords
                        .Where(x => x.Id != word.Id && !wrongOptions.Contains(x.TurkishMeaning))
                        .OrderBy(_ => random.Next())
                        .Take(3 - wrongOptions.Count)
                        .Select(x => x.TurkishMeaning)
                        .ToList();

                    wrongOptions.AddRange(extra);
                }

                // 4 şık: 1 doğru + 3 yanlış → karıştır
                var options = wrongOptions.ToList();
                options.Add(word.TurkishMeaning);
                options = options.OrderBy(_ => random.Next()).ToList();

                questions.Add(new QuizQuestionResult
                {
                    UserWordId = word.Id,
                    EnglishWord = word.EnglishWord,
                    Options = options,
                    CorrectAnswer = word.TurkishMeaning
                });
            }

            return questions;
        }
    }
}