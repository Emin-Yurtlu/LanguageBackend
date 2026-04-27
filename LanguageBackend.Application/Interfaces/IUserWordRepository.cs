using LanguageBackend.Domain.Entities;

// burada kelime verıtabanı ıslemlerı ınterfacesını yaptık 
namespace LanguageBackend.Application.Interfaces
{
    public interface IUserWordRepository
    {
        Task<List<string>> GetSeenWordsByUserIdAsync(string userId);
        Task<List<UserWord>> GetUnlearnedWordsByUserIdAsync(string userId);
        Task<List<string>> GetLearnedTurkishMeaningsByUserIdAsync(string userId);
        Task<List<UserWord>> GetLearnedWordsByUserIdAsync(string userId);
        Task<List<UserWord>> GetNotLearnedWordsByUserIdAsync(string userId);
        Task<List<UserWord>> GetAllWordsByUserIdAsync(string userId);
        Task<bool> ExistsAsync(string userId, string englishWord);
        Task AddWordAsync(UserWord word);
        Task<UserWord?> GetByIdAsync(int id);
        Task SaveChangesAsync();
    }
}