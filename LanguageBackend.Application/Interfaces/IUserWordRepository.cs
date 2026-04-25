using LanguageBackend.Domain.Entities;

// burada kelime verıtabanı ıslemlerı ınterfacesını yaptık 
namespace LanguageBackend.Application.Interfaces
{
    public interface IUserWordRepository
    {
        Task<List<string>> GetSeenWordsByUserIdAsync(string userId);
        Task<List<UserWord>> GetUnlearnedWordsByUserIdAsync(string userId);
        Task<List<string>> GetLearnedTurkishMeaningsByUserIdAsync(string userId);
        Task AddWordAsync(UserWord word);
        Task<UserWord?> GetByIdAsync(int id);
        Task SaveChangesAsync();
    }
}