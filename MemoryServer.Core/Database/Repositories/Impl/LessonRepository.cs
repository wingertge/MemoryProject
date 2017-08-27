using System;
using System.Threading.Tasks;
using MemoryApi.Core.Database.Repositories;
using MemoryCore.DbModels;
using Microsoft.EntityFrameworkCore;

namespace MemoryServer.Core.Database.Repositories.Impl
{
    public class LessonRepository : ILessonRepository
    {
        private readonly ITransactionFactory<MemoryContext> _db;
        public LessonRepository(ITransactionFactory<MemoryContext> db) => _db = db;

        public Task<Lesson> FindLessonByContentAsync(int langFrom, int langTo, string reading, string pronunciation, string meaning)
        {
            return _db.CreateAsyncTransaction(context =>
            {
                return context.Lessons.FirstOrDefaultAsync(a =>
                    a.LanguageFrom.Id == langFrom && a.LanguageTo.Id == langTo && a.Reading == reading &&
                    a.Pronunciation == pronunciation && a.Meaning == meaning);
            }).Run();
        }

        public Task<Lesson> CreateOrUpdateLessonAsync(Lesson lesson)
        {
            return _db.CreateAsyncTransaction(async context =>
            {
                var result = lesson.Id != Guid.Empty ? context.Lessons.Update(lesson).Entity : context.Lessons.Add(lesson).Entity;

                await context.SaveChangesAsync();
                return result;
            }).Run();
        }
    }
}