using LanguageBackend.Application.Interfaces;
using MediatR;

namespace LanguageBackend.Application.Features.Word.Queries.GetLearnedWords
{
    public class GetLearnedWordsQueryHandler : IRequestHandler<GetLearnedWordsQuery, List<GetLearnedWordsResult>>
    {
        private readonly IUserWordRepository _userWordRepository;

        public GetLearnedWordsQueryHandler(IUserWordRepository userWordRepository)
        {
            _userWordRepository = userWordRepository;
        }

        public async Task<List<GetLearnedWordsResult>> Handle(GetLearnedWordsQuery request, CancellationToken cancellationToken)
        {
            var words = await _userWordRepository.GetLearnedWordsByUserIdAsync(request.UserId);

            if (!words.Any())
                return new List<GetLearnedWordsResult>();

            return words.Select(w => new GetLearnedWordsResult
            {
                EnglishWord = w.EnglishWord,
                TurkishMeaning = w.TurkishMeaning
            }).ToList();
        }
    }
}