using System;
using MemoryCore.DbModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MemoryServer.Core.Database
{
    public class MemoryContext : IdentityDbContext<User, DummyRole, Guid>
    {
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<LessonAssignment> Assignments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<UserList> UserLists { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<BurnedAssignment> BurnedAssignments { get; set; }
        public DbSet<Post> Posts { get; set; }

        public MemoryContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserListEntry>().HasKey(a => new {a.OwnerId, a.LessonId});
            builder.Entity<BurnedAssignment>().HasKey(a => new { UserId = a.OwnerId, a.LessonId });
            builder.Entity<Theme>().HasKey(a => a.OwnerId);

            builder.Entity<User>().HasMany(a => a.CreatedLists).WithOne(a => a.Owner);

            builder.Entity<UserListSubscription>().HasKey(a => new {a.ListId, a.UserId});
            builder.Entity<UserListSubscription>()
                .HasOne(a => a.List)
                .WithMany(a => a.Subscriptions)
                .HasForeignKey(a => a.UserId);
            builder.Entity<UserListSubscription>()
                .HasOne(a => a.User)
                .WithMany(a => a.ListSubscriptions)
                .HasForeignKey(a => a.ListId);

            builder.Entity<ListUpdateTracker>().HasKey(a => new {a.ListId, a.LessonId, a.UserId});
            builder.Entity<ListUpdateTracker>()
                .HasOne(a => a.Entry)
                .WithMany(a => a.UpdateTrackers)
                .HasForeignKey(a => new {a.ListId, a.LessonId});
            builder.Entity<ListUpdateTracker>()
                .HasOne(a => a.TrackingUser)
                .WithMany(a => a.PendingLessons)
                .HasForeignKey(a => a.UserId);

            builder.Entity<PostTag>().HasKey(a => new { a.PostId, a.TagId });
            builder.Entity<PostTag>()
                .HasOne(a => a.Post)
                .WithMany(a => a.Tags)
                .HasForeignKey(a => a.PostId);
            builder.Entity<PostTag>()
                .HasOne(a => a.Tag)
                .WithMany(a => a.Posts)
                .HasForeignKey(a => a.TagId);
        }

        [DbFunction(Schema = "dbo")]
        public static int Levenshtein(string s1, string s2) { throw new NotImplementedException(); }
    }
}