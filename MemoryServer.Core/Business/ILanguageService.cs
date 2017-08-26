using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MemoryCore.DbModels;
using MemoryCore.Models;
using MemoryServer.Core.Business.Impl;

namespace MemoryServer.Core.Business
{
    public interface ILanguageService
    {
        Task<LanguagesPair> GetLanguagesByPopularity(string userId, Guid assignmentId);
    }

    public class LanguagesPair
    {
        public HashSet<Language> From { get; set; }
        public HashSet<Language> To { get; set; }

        public LanguagesPair(HashSet<Language> @from, HashSet<Language> to)
        {
            From = @from;
            To = to;
        }
    }
}