using LanguageBackend.Application.Interfaces;
using MediatR;

namespace LanguageBackend.Application.Features.Word.Queries.GetLearnedWords
{
    public class GetAllWordsQueryHandler : IRequestHandler<GetAllWordsQuery, List<GetAllWordsResult>>
    {
        private readonly IUserWordRepository _userWordRepository;

        public GetAllWordsQueryHandler(IUserWordRepository userWordRepository)
        {
            _userWordRepository = userWordRepository;
        }

        public async Task<List<GetAllWordsResult>> Handle(GetAllWordsQuery request, CancellationToken cancellationToken)
        {
            var words = await _userWordRepository.GetAllWordsByUserIdAsync(request.UserId);

            if (!words.Any())
                return new List<GetAllWordsResult>();

            return words.Select(w => new GetAllWordsResult
            {
                EnglishWord = w.EnglishWord,
                TurkishMeaning = w.TurkishMeaning
            }).ToList();
        }
    }
}