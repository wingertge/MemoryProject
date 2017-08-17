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

        public async Task<LanguagesPair> GetLanguagesByPopularity(User user, Guid assignmentId)
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
            if (user.LastLanguageFrom != null && user.LastLanguageTo != null)
            {
                fromSet.Add(user.LastLanguageFrom);
                toSet.Add(user.LastLanguageTo);
            }
            var userFromList = await _languageRepository.GetLanguageListByUserPopularityAsync(user, true);
            var userToList = await _languageRepository.GetLanguageListByUserPopularityAsync(user, false);
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