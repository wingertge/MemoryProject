using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MemoryCore.DbModels;
using MemoryCore.Models;
using MemoryServer.Core.Business.Impl;

namespace MemoryServer.Core.Business
{
    public interface IReviewStore
    {
        Task<bool> DoesUserExist(User user);
        Task<ReviewCardContext> GetUserHand(User user);
        Task InitaliseUserHand(User user, ReviewCardContext hand);
        Task AddUserCard(User user, CardEntry card);
        Task DiscardUserCard(User user, CardEntry card);
        Task<List<CardEntry>> CleanAssignmentSet(User user, LessonAssignment assignment);
        Task MarkCardIncorrect(User user, CardEntry card);
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
        public LessonAssignment Assignment { get; set; }
        public ReviewField FromField { get; set; }
        public ReviewField ToField { get; set; }
        public int IncorrectCount { get; set; }
    }
}