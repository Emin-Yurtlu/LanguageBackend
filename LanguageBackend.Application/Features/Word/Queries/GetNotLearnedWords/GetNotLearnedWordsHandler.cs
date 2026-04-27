using LanguageBackend.Application.Interfaces;
using MediatR;

namespace LanguageBackend.Application.Features.Word.Queries.GetLearnedWords
{
    public class GetNotLearnedWordsQueryHandler : IRequestHandler<GetNotLearnedWordsQuery, List<GetNotLearnedWordsResult>>
    {
        private readonly IUserWordRepository _userWordRepository;

        public GetNotLearnedWordsQueryHandler(IUserWordRepository userWordRepository)
        {
            _userWordRepository = userWordRepository;
        }

        public async Task<List<GetNotLearnedWordsResult>> Handle(GetNotLearnedWordsQuery request, CancellationToken cancellationToken)
        {
            var words = await _userWordRepository.GetNotLearnedWordsByUserIdAsync(request.UserId);

            if (!words.Any())
                return new List<GetNotLearnedWordsResult>();

            return words.Select(w => new GetNotLearnedWordsResult
            {
                EnglishWord = w.EnglishWord,
                TurkishMeaning = w.TurkishMeaning
            }).ToList();
        }
    }
}