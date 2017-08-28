using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MemoryCore.DataTypes;

namespace MemoryApi.Core.Business
{
    public interface ILanguageService
    {
        Task<LanguagesPair> GetLanguagesByPopularity(string userId, Guid assignmentId, int lastLanguageFromId, int lastLanguageToId);
    }

    public class LanguagesPair
    {
        public List<Language> From { get; set; }
        public List<Language> To { get; set; }

        public LanguagesPair(List<Language> @from, List<Language> to)
        {
            From = @from;
            To = to;
        }
    }
}