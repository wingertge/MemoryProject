using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MemoryApi.Core.Database.Repositories;
using MemoryCore.DataTypes;
using MemoryCore.DbModels;
using Microsoft.EntityFrameworkCore;

namespace MemoryServer.Core.Database.Repositories.Impl
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly ITransactionFactory<MemoryContext> _db;
        public LanguageRepository(ITransactionFactory<MemoryContext> db) => _db = db;

        public Task<Language> FindLanguageByIdAsync(int id)
        {
            return _db.CreateAsyncTransaction(context => context.Languages.FindAsync(id)).Run();
        }

        public Task<List<Language>> GetLanguageListByUserPopularityAsync(string userId, bool from)
        {
            return _db.CreateAsyncTransaction(async context =>
            {
                var assignments = context.Assignments.Where(a => a.OwnerId == userId);
                var lookup = from
                    ? assignments.GroupBy(a => a.Lesson.LanguageFrom)
                    : assignments.GroupBy(a => a.Lesson.LanguageTo);
                return await lookup.OrderByDescending(a => a.Count()).Select(a => a.Key).ToListAsync();
            }).Run();
        }

        public Task<List<Language>> GetLanguageListByGlobalPopularityAsync(bool from)
        {
            return _db.CreateAsyncTransaction(context =>
            {
                var lessons = context.Lessons;
                var lookup = from
                    ? lessons.GroupBy(a => a.LanguageFrom)
                    : lessons.GroupBy(a => a.LanguageTo);
                return lookup.OrderByDescending(a => a.Count()).Select(a => a.Key).ToListAsync();
            }).Run();
        }

        public Task<List<Language>> GetLanguageListFull() => _db.CreateAsyncTransaction(context => context.Languages.ToListAsync()).Run();
        public Task<int> GetLanguageCount() => _db.CreateAsyncTransaction(context => context.Languages.CountAsync()).Run();
    }
}