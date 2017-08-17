using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace MemoryCore.DbModels
{
    public class User : IdentityUser<Guid>
    { 
        public override Guid Id { get; set; }
        public override string UserName { get; set; }
        public override string Email { get; set; }
        public DateTime SignedUp { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime PremiumUntil { get; set; }

        public override string NormalizedUserName { get; set; }
        public override string NormalizedEmail { get; set; }
        public override bool EmailConfirmed { get; set; }
        [JsonIgnore]
        public override string PasswordHash { get; set; }
        [JsonIgnore]
        public override string SecurityStamp { get; set; }
        [JsonIgnore]
        public override string ConcurrencyStamp { get; set; }
        public override string PhoneNumber { get; set; }
        public override bool PhoneNumberConfirmed { get; set; }
        public override bool TwoFactorEnabled { get; set; }
        public override DateTimeOffset? LockoutEnd { get; set; }
        public override bool LockoutEnabled { get; set; }
        public override int AccessFailedCount { get; set; }
        [JsonIgnore]
        public List<UserList> CreatedLists { get; set; } = new List<UserList>();
        [JsonIgnore]
        public List<UserListSubscription> ListSubscriptions { get; set; } = new List<UserListSubscription>();
        [JsonIgnore]
        public List<ListUpdateTracker> PendingLessons { get; set; } = new List<ListUpdateTracker>();
        [JsonIgnore]
        public List<BurnedAssignment> BurnedAssignments { get; set; }
        public Language LastLanguageFrom { get; set; }
        public Language LastLanguageTo { get; set; }
        [JsonIgnore]
        public Theme Theme { get; set; }
        [JsonIgnore]
        public List<Post> Posts { get; set; }
    }
}