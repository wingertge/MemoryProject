using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MemoryApi.Core.Database.Repositories;
using MemoryCore.DataTypes;
using MemoryServer.Core.Business;

namespace MemoryApi.Core.Business.Impl
{
    public class LanguageService : ILanguageService
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly ILanguageRepository _languageRepository;
        
        public LanguageService(IAssignmentRepository assignmentRepository, ILanguageRepository languageRepository)
        {
            _assignmentRepository = assignmentRepository;
            _languageRepository = languageRepository;
        }

        [ItemNotNull]
        public async Task<LanguagesPair> GetLanguagesByPopularity(string userId, Guid assignmentId, int lastLanguageFromId, int lastLanguageToId)
        {
            var fromSet = new List<Language>();
            var toSet = new List<Language>();
            var languageCount = await _languageRepository.GetLanguageCount();
            var assignment = await _assignmentRepository.FindAssignmentByIdAsync(assignmentId);
            if (assignment != null)
            {
                var assignmentLangFrom = await _languageRepository.FindLanguageByIdAsync(assignment.Lesson.Languages.LanguageFromId);
                var assignmentLangTo = await _languageRepository.FindLanguageByIdAsync(assignment.Lesson.Languages.LanguageToId);
                fromSet.Add(assignmentLangFrom);
                toSet.Add(assignmentLangTo);
            }
            var lastLanguageFrom = await _languageRepository.FindLanguageByIdAsync(lastLanguageFromId);
            var lastLanguageTo = await _languageRepository.FindLanguageByIdAsync(lastLanguageToId);
            if (lastLanguageFrom != null && lastLanguageTo != null)
            {
                if(!fromSet.Contains(lastLanguageFrom)) fromSet.Add(lastLanguageFrom);
                if(!toSet.Contains(lastLanguageTo)) toSet.Add(lastLanguageTo);
            }
            var userFromList = await _languageRepository.GetLanguageListByUserPopularityAsync(userId, true);
            var userToList = await _languageRepository.GetLanguageListByUserPopularityAsync(userId, false);

            fromSet.AddRange(userFromList.Where(a => !fromSet.Contains(a)));
            toSet.AddRange(userToList.Where(a => !toSet.Contains(a)));

            if(fromSet.Count == languageCount && toSet.Count == languageCount) return new LanguagesPair(fromSet, toSet);

            var globalFromList = await _languageRepository.GetLanguageListByGlobalPopularityAsync(true);
            var globalToList = await _languageRepository.GetLanguageListByGlobalPopularityAsync(false);

            fromSet.AddRange(globalFromList.Where(a => !fromSet.Contains(a)));
            toSet.AddRange(globalToList.Where(a => !toSet.Contains(a)));

            if (fromSet.Count == languageCount && toSet.Count == languageCount) return new LanguagesPair(fromSet, toSet);

            var allLanguages = await _languageRepository.GetLanguageListFull();

            fromSet.AddRange(allLanguages.Where(a => !fromSet.Contains(a)));
            toSet.AddRange(allLanguages.Where(a => !toSet.Contains(a)));

            return new LanguagesPair(fromSet, toSet);
        }
    }
}