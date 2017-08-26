using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MemoryCore.DbModels;
using MemoryServer.Core.Database.Repositories;

namespace MemoryServer.Core.Business.Impl
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

        public async Task<LanguagesPair> GetLanguagesByPopularity(string userId, Guid assignmentId, int lastLanguageFromId, int lastLanguageToId)
        {
            var fromSet = new HashSet<Language>();
            var toSet = new HashSet<Language>();
            var languageCount = await _languageRepository.GetLanguageCount();
            var assignment = await _assignmentRepository.FindAssignmentByIdAsync(assignmentId);
            if (assignment != null)
            {
                fromSet.Add(assignment.Lesson.LanguageFrom);
                toSet.Add(assignment.Lesson.LanguageTo);
            }
            var lastLanguageFrom = await _languageRepository.FindLanguageByIdAsync(lastLanguageFromId);
            var lastLanguageTo = await _languageRepository.FindLanguageByIdAsync(lastLanguageToId);
            if (lastLanguageFrom != null && lastLanguageTo != null)
            {
                fromSet.Add(lastLanguageFrom);
                toSet.Add(lastLanguageTo);
            }
            var userFromList = await _languageRepository.GetLanguageListByUserPopularityAsync(userId, true);
            var userToList = await _languageRepository.GetLanguageListByUserPopularityAsync(userId, false);
            foreach (var language in userFromList)
                fromSet.Add(language);
            foreach (var language in userToList)
                toSet.Add(language);
            if(fromSet.Count == languageCount && toSet.Count == languageCount) return new LanguagesPair(fromSet, toSet);

            var globalFromList = await _languageRepository.GetLanguageListByGlobalPopularityAsync(true);
            var globalToList = await _languageRepository.GetLanguageListByGlobalPopularityAsync(false);
            foreach (var language in globalFromList)
                fromSet.Add(language);
            foreach (var language in globalToList)
                toSet.Add(language);

            if (fromSet.Count == languageCount && toSet.Count == languageCount) return new LanguagesPair(fromSet, toSet);

            var allLanguages = await _languageRepository.GetLanguageListFull();
            foreach (var language in allLanguages)
            {
                fromSet.Add(language);
                toSet.Add(language);
            }
            return new LanguagesPair(fromSet, toSet);
        }
    }
}