using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MemoryApi.Core.DbModels;
using MemoryCore.Models;

namespace MemoryApi.Core.Business
{
    public interface IReviewStore
    {
        Task<bool> DoesUserExist(string userId);
        Task<ReviewCardContext> GetUserHand(string userId);
        Task InitaliseUserHand(string userId, ReviewCardContext hand);
        Task AddUserCard(string userId, CardEntry card);
        Task DiscardUserCard(string userId, CardEntry card);
        Task<List<CardEntry>> CleanAssignmentSet(string userId, Assignment assignment);
        Task MarkCardIncorrect(string userId, CardEntry card);
    }

    public class ReviewCardContext
    {
        public HashSet<CardEntry> Hand { get; set; } = new HashSet<CardEntry>();
        public HashSet<CardEntry> DiscardPile { get; set; } = new HashSet<CardEntry>();
        public Queue<CardEntry> UpcomingQueue { get; set; } = new Queue<CardEntry>();

        public void Add(CardEntry card) => UpcomingQueue.Enqueue(card);

        public void Discard(CardEntry card)
        {
            Hand.Remove(card);
            DiscardPile.Add(card);
            if (UpcomingQueue.Count > 0) Hand.Add(UpcomingQueue.Dequeue());
        }

        public void FillHand(int count)
        {
            count = Math.Min(count - Hand.Count, UpcomingQueue.Count);
            for (var i = 0; i < count; i++)
                Hand.Add(UpcomingQueue.Dequeue());
        }
    }

    public class CardEntry
    {
        public Assignment Assignment { get; set; }
        public ReviewField FromField { get; set; }
        public ReviewField ToField { get; set; }
        public int IncorrectCount { get; set; }
    }
}