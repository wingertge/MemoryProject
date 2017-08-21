﻿// <auto-generated />
using MemoryServer.Core.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace MemoryServer.Migrations
{
    [DbContext(typeof(MemoryContext))]
    [Migration("20170820121640_NewsPosts")]
    partial class NewsPosts
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("MemoryCore.DbModels.AudioLocation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("LessonId");

                    b.Property<string>("RelFileName");

                    b.Property<Guid?>("UploaderId");

                    b.HasKey("Id");

                    b.HasIndex("LessonId");

                    b.HasIndex("UploaderId");

                    b.ToTable("AudioLocation");
                });

            modelBuilder.Entity("MemoryCore.DbModels.BurnedAssignment", b =>
                {
                    b.Property<Guid>("OwnerId");

                    b.Property<Guid>("LessonId");

                    b.HasKey("OwnerId", "LessonId");

                    b.HasIndex("LessonId");

                    b.ToTable("BurnedAssignments");
                });

            modelBuilder.Entity("MemoryCore.DbModels.Language", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EnglishCountryName");

                    b.Property<string>("EnglishName");

                    b.Property<string>("IETFTag");

                    b.Property<string>("NativeCountryName");

                    b.Property<string>("NativeName");

                    b.HasKey("Id");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("MemoryCore.DbModels.Lesson", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("LanguageFromId");

                    b.Property<int?>("LanguageToId");

                    b.Property<string>("Meaning");

                    b.Property<string>("Pronunciation");

                    b.Property<string>("Reading");

                    b.HasKey("Id");

                    b.HasIndex("LanguageFromId");

                    b.HasIndex("LanguageToId");

                    b.ToTable("Lessons");
                });

            modelBuilder.Entity("MemoryCore.DbModels.LessonAssignment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("LessonId");

                    b.Property<DateTime>("NextReview");

                    b.Property<Guid?>("OwnerId");

                    b.Property<int>("Stage");

                    b.HasKey("Id");

                    b.HasIndex("LessonId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("MemoryCore.DbModels.ListUpdateTracker", b =>
                {
                    b.Property<Guid>("ListId");

                    b.Property<Guid>("LessonId");

                    b.Property<Guid>("UserId");

                    b.HasKey("ListId", "LessonId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("ListUpdateTracker");
                });

            modelBuilder.Entity("MemoryCore.DbModels.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("AuthorId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime>("EditedAt");

                    b.Property<string>("Text");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("MemoryCore.DbModels.PostTag", b =>
                {
                    b.Property<Guid>("PostId");

                    b.Property<int>("TagId");

                    b.HasKey("PostId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("PostTag");
                });

            modelBuilder.Entity("MemoryCore.DbModels.Review", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EndStage");

                    b.Property<Guid?>("LessonId");

                    b.Property<DateTime>("ReviewDone");

                    b.Property<int>("StartStage");

                    b.Property<int>("WrongAnswers");

                    b.HasKey("Id");

                    b.HasIndex("LessonId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("MemoryCore.DbModels.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("MemoryCore.DbModels.Theme", b =>
                {
                    b.Property<Guid>("OwnerId");

                    b.Property<string>("BackgroundPrimary");

                    b.Property<string>("BackgroundSecondary");

                    b.Property<string>("BackgroundTertiary");

                    b.Property<string>("BorderPrimary");

                    b.Property<string>("BorderSecondary");

                    b.Property<string>("BorderTertiary");

                    b.Property<string>("TextPrimary");

                    b.Property<string>("TextSecondary");

                    b.Property<string>("TextTertiary");

                    b.HasKey("OwnerId");

                    b.ToTable("Theme");
                });

            modelBuilder.Entity("MemoryCore.DbModels.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<int?>("LastLanguageFromId");

                    b.Property<int?>("LastLanguageToId");

                    b.Property<DateTime>("LastLogin");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<DateTime>("PremiumUntil");

                    b.Property<string>("SecurityStamp");

                    b.Property<DateTime>("SignedUp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("LastLanguageFromId");

                    b.HasIndex("LastLanguageToId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("MemoryCore.DbModels.UserList", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<int?>("LanguageFromId");

                    b.Property<int?>("LanguageToId");

                    b.Property<DateTime>("LastModified");

                    b.Property<int>("Likes");

                    b.Property<bool>("Ordered");

                    b.Property<Guid?>("OwnerId");

                    b.HasKey("Id");

                    b.HasIndex("LanguageFromId");

                    b.HasIndex("LanguageToId");

                    b.HasIndex("OwnerId");

                    b.ToTable("UserLists");
                });

            modelBuilder.Entity("MemoryCore.DbModels.UserListEntry", b =>
                {
                    b.Property<Guid>("OwnerId");

                    b.Property<Guid>("LessonId");

                    b.Property<int>("Order");

                    b.HasKey("OwnerId", "LessonId");

                    b.HasIndex("LessonId");

                    b.ToTable("UserListEntry");
                });

            modelBuilder.Entity("MemoryCore.DbModels.UserListSubscription", b =>
                {
                    b.Property<Guid>("ListId");

                    b.Property<Guid>("UserId");

                    b.HasKey("ListId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserListSubscription");
                });

            modelBuilder.Entity("MemoryServer.Core.Database.DummyRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<Guid>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("MemoryCore.DbModels.AudioLocation", b =>
                {
                    b.HasOne("MemoryCore.DbModels.Lesson")
                        .WithMany("Audio")
                        .HasForeignKey("LessonId");

                    b.HasOne("MemoryCore.DbModels.User", "Uploader")
                        .WithMany()
                        .HasForeignKey("UploaderId");
                });

            modelBuilder.Entity("MemoryCore.DbModels.BurnedAssignment", b =>
                {
                    b.HasOne("MemoryCore.DbModels.Lesson", "Lesson")
                        .WithMany()
                        .HasForeignKey("LessonId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MemoryCore.DbModels.User", "Owner")
                        .WithMany("BurnedAssignments")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MemoryCore.DbModels.Lesson", b =>
                {
                    b.HasOne("MemoryCore.DbModels.Language", "LanguageFrom")
                        .WithMany()
                        .HasForeignKey("LanguageFromId");

                    b.HasOne("MemoryCore.DbModels.Language", "LanguageTo")
                        .WithMany()
                        .HasForeignKey("LanguageToId");
                });

            modelBuilder.Entity("MemoryCore.DbModels.LessonAssignment", b =>
                {
                    b.HasOne("MemoryCore.DbModels.Lesson", "Lesson")
                        .WithMany()
                        .HasForeignKey("LessonId");

                    b.HasOne("MemoryCore.DbModels.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("MemoryCore.DbModels.ListUpdateTracker", b =>
                {
                    b.HasOne("MemoryCore.DbModels.User", "TrackingUser")
                        .WithMany("PendingLessons")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MemoryCore.DbModels.UserListEntry", "Entry")
                        .WithMany("UpdateTrackers")
                        .HasForeignKey("ListId", "LessonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MemoryCore.DbModels.Post", b =>
                {
                    b.HasOne("MemoryCore.DbModels.User", "Author")
                        .WithMany("Posts")
                        .HasForeignKey("AuthorId");
                });

            modelBuilder.Entity("MemoryCore.DbModels.PostTag", b =>
                {
                    b.HasOne("MemoryCore.DbModels.Post", "Post")
                        .WithMany("Tags")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MemoryCore.DbModels.Tag", "Tag")
                        .WithMany("Posts")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MemoryCore.DbModels.Review", b =>
                {
                    b.HasOne("MemoryCore.DbModels.LessonAssignment", "Lesson")
                        .WithMany()
                        .HasForeignKey("LessonId");
                });

            modelBuilder.Entity("MemoryCore.DbModels.Theme", b =>
                {
                    b.HasOne("MemoryCore.DbModels.User", "Owner")
                        .WithOne("Theme")
                        .HasForeignKey("MemoryCore.DbModels.Theme", "OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MemoryCore.DbModels.User", b =>
                {
                    b.HasOne("MemoryCore.DbModels.Language", "LastLanguageFrom")
                        .WithMany()
                        .HasForeignKey("LastLanguageFromId");

                    b.HasOne("MemoryCore.DbModels.Language", "LastLanguageTo")
                        .WithMany()
                        .HasForeignKey("LastLanguageToId");
                });

            modelBuilder.Entity("MemoryCore.DbModels.UserList", b =>
                {
                    b.HasOne("MemoryCore.DbModels.Language", "LanguageFrom")
                        .WithMany()
                        .HasForeignKey("LanguageFromId");

                    b.HasOne("MemoryCore.DbModels.Language", "LanguageTo")
                        .WithMany()
                        .HasForeignKey("LanguageToId");

                    b.HasOne("MemoryCore.DbModels.User", "Owner")
                        .WithMany("CreatedLists")
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("MemoryCore.DbModels.UserListEntry", b =>
                {
                    b.HasOne("MemoryCore.DbModels.Lesson", "Lesson")
                        .WithMany("ListEntries")
                        .HasForeignKey("LessonId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MemoryCore.DbModels.UserList", "Owner")
                        .WithMany("Entries")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MemoryCore.DbModels.UserListSubscription", b =>
                {
                    b.HasOne("MemoryCore.DbModels.User", "User")
                        .WithMany("ListSubscriptions")
                        .HasForeignKey("ListId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MemoryCore.DbModels.UserList", "List")
                        .WithMany("Subscriptions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("MemoryServer.Core.Database.DummyRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("MemoryCore.DbModels.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("MemoryCore.DbModels.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("MemoryServer.Core.Database.DummyRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MemoryCore.DbModels.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("MemoryCore.DbModels.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
