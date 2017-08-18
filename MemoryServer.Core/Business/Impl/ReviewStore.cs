using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryCore.DbModels;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace MemoryServer.Core.Business.Impl
{
    public class ReviewStore : IReviewStore
    {
        private readonly IDistributedCache _cache;
        public ReviewStore(IDistributedCache cache)
        {
            _cache = cache;
        }

        protected virtual string GetKey(User user) => "Reviews/" + user.Id;

        public async Task<bool> DoesUserExist(User user)
        {
            return (await _cache.GetAsync(GetKey(user))) != null;
        }

        public Task<ReviewCardContext> GetUserHand(User user)
        {
            return GetCards(user);
        }

        public async Task InitaliseUserHand(User user, ReviewCardContext hand)
        {
            await SetCards(user, hand);
        }

        public async Task AddUserCard(User user, CardEntry card)
        {
            var currentHand = await GetCards(user);
            currentHand.Add(card);
            await SetCards(user, currentHand);
        }

        public async Task DiscardUserCard(User user, CardEntry card)
        {
            var currentHand = await GetCards(user);
            currentHand.Discard(card);
            await SetCards(user, currentHand);
        }

        public async Task MarkCardIncorrect(User user, CardEntry card)
        {
            var currentHand = await GetCards(user);
            currentHand.Hand.Remove(card);
            card.IncorrectCount++;
            currentHand.Hand.Add(card);
            await SetCards(user, currentHand);
        }

        public async Task<List<CardEntry>> CleanAssignmentSet(User user, LessonAssignment assignment)
        {
            var currentHand = await GetCards(user);
            var cards = currentHand.DiscardPile.Where(a => a.Assignment.Id == assignment.Id).ToList();
            currentHand.DiscardPile.RemoveWhere(a => a.Assignment.Id == assignment.Id);
            await SetCards(user, currentHand);
            return cards;
        }

        private async Task<ReviewCardContext> GetCards(User user)
        {
            var currentHandJson = await _cache.GetStringAsync(GetKey(user));
            return JsonConvert.DeserializeObject<ReviewCardContext>(currentHandJson);
        }

        private async Task SetCards(User user, ReviewCardContext reviewCards)
        {
            await _cache.SetStringAsync(GetKey(user), JsonConvert.SerializeObject(reviewCards));
        }
    }
}