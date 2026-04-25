using LanguageBackend.Application.Interfaces;
using MediatR;

namespace LanguageBackend.Application.Features.Word.Commands.SubmitAnswer
{
    public class SubmitAnswerCommandHandler : IRequestHandler<SubmitAnswerCommand, SubmitAnswerResult>
    {
        private readonly IUserWordRepository _userWordRepository;

        public SubmitAnswerCommandHandler(IUserWordRepository userWordRepository)
        {
            _userWordRepository = userWordRepository;
        }

        public async Task<SubmitAnswerResult> Handle(SubmitAnswerCommand request, CancellationToken cancellationToken)
        {
            // Kelimeyi DB'den bul
            var word = await _userWordRepository.GetByIdAsync(request.UserWordId);

            if (word == null || word.UserId != request.UserId)
                return new SubmitAnswerResult
                {
                    IsCorrect = false,
                    CorrectAnswer = string.Empty
                };

            // Cevap doğru mu?
            var isCorrect = word.TurkishMeaning.Trim().ToLower() == request.SelectedAnswer.Trim().ToLower();

            // Doğruysa IsLearned = true yap, bir daha sorulmayacak
            if (isCorrect)
            {
                word.IsLearned = true;
                await _userWordRepository.SaveChangesAsync();
            }

            return new SubmitAnswerResult
            {
                IsCorrect = isCorrect,
                CorrectAnswer = word.TurkishMeaning
            };
        }
    }
}