using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MemoryApi.Core.DbModels;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace MemoryApi.Core.Business.Impl
{
    public class ReviewStore : IReviewStore
    {
        private readonly IDistributedCache _cache;
        public ReviewStore(IDistributedCache cache)
        {
            _cache = cache;
        }

        [NotNull]
        protected virtual string GetKey(string userId) => "Reviews/" + userId;

        public async Task<bool> DoesUserExist(string userId)
        {
            return (await _cache.GetAsync(GetKey(userId))) != null;
        }

        public Task<ReviewCardContext> GetUserHand(string userId)
        {
            return GetCards(userId);
        }

        public async Task InitaliseUserHand(string userId, ReviewCardContext hand)
        {
            await SetCards(userId, hand);
        }

        public async Task AddUserCard(string userId, CardEntry card)
        {
            var currentHand = await GetCards(userId);
            currentHand.Add(card);
            await SetCards(userId, currentHand);
        }

        public async Task DiscardUserCard(string userId, CardEntry card)
        {
            var currentHand = await GetCards(userId);
            currentHand.Discard(card);
            await SetCards(userId, currentHand);
        }

        public async Task MarkCardIncorrect(string userId, CardEntry card)
        {
            var currentHand = await GetCards(userId);
            currentHand.Hand.Remove(card);
            card.IncorrectCount++;
            currentHand.Hand.Add(card);
            await SetCards(userId, currentHand);
        }

        public async Task<List<CardEntry>> CleanAssignmentSet(string userId, Assignment assignment)
        {
            var currentHand = await GetCards(userId);
            var cards = currentHand.DiscardPile.Where(a => a.Assignment.Id == assignment.Id).ToList();
            currentHand.DiscardPile.RemoveWhere(a => a.Assignment.Id == assignment.Id);
            await SetCards(userId, currentHand);
            return cards;
        }

        private async Task<ReviewCardContext> GetCards(string userId)
        {
            var currentHandJson = await _cache.GetStringAsync(GetKey(userId));
            return JsonConvert.DeserializeObject<ReviewCardContext>(currentHandJson);
        }

        private async Task SetCards(string userId, ReviewCardContext reviewCards)
        {
            await _cache.SetStringAsync(GetKey(userId), JsonConvert.SerializeObject(reviewCards));
        }
    }
}