using LanguageBackend.Application.Interfaces;
using LanguageBackend.Domain.Entities;
using LanguageBackend.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LanguageBackend.Persistence.Repositories
{
    public class UserWordRepository : IUserWordRepository
    {
        private readonly AppDbContext _context;

        public UserWordRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetSeenWordsByUserIdAsync(string userId)
        {
            return await _context.UserWords
                .Where(x => x.UserId == userId)
                .Select(x => x.EnglishWord)
                .ToListAsync();
        }

        public async Task<List<UserWord>> GetUnlearnedWordsByUserIdAsync(string userId)
        {
            return await _context.UserWords
                .Where(x => x.UserId == userId && !x.IsLearned)
                .ToListAsync();
        }

        public async Task<List<string>> GetLearnedTurkishMeaningsByUserIdAsync(string userId)
        {
            return await _context.UserWords
                .Where(x => x.UserId == userId && x.IsLearned)
                .Select(x => x.TurkishMeaning)
                .ToListAsync();
        }

        public async Task AddWordAsync(UserWord word)
        {
            await _context.UserWords.AddAsync(word);
        }

        public async Task<UserWord?> GetByIdAsync(int id)
        {
            return await _context.UserWords.FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(string userId, string englishWord)
        {
            return await _context.UserWords
                .AnyAsync(x => x.UserId == userId && x.EnglishWord.ToLower() == englishWord.ToLower());
        }

        public async Task<List<UserWord>> GetLearnedWordsByUserIdAsync(string userId)
        {
            return await _context.UserWords
                .Where(x => x.UserId == userId && x.IsLearned)
                .ToListAsync();
        }
        public async Task<List<UserWord>> GetNotLearnedWordsByUserIdAsync(string userId)
        {
            return await _context.UserWords
                .Where(x => x.UserId == userId && x.IsLearned==false)
                .ToListAsync();
        }

        public async Task<List<UserWord>> GetAllWordsByUserIdAsync(string userId)
        {
            return await _context.UserWords
                .Where(x => x.UserId == userId )
                .ToListAsync();
        }


    }
}