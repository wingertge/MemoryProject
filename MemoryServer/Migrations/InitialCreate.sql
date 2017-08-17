CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" varchar(150) NOT NULL,
    "ProductVersion" varchar(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

CREATE TABLE "AspNetRoles" (
    "Id" uuid NOT NULL,
    "ConcurrencyStamp" text NULL,
    "Name" varchar(256) NULL,
    "NormalizedName" varchar(256) NULL,
    CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id")
);

CREATE TABLE "Languages" (
    "Id" serial NOT NULL,
    "EnglishCountryName" text NULL,
    "EnglishName" text NULL,
    "IETFTag" text NULL,
    "NativeCountryName" text NULL,
    "NativeName" text NULL,
    CONSTRAINT "PK_Languages" PRIMARY KEY ("Id")
);

CREATE TABLE "AspNetRoleClaims" (
    "Id" serial NOT NULL,
    "ClaimType" text NULL,
    "ClaimValue" text NULL,
    "RoleId" uuid NOT NULL,
    CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUsers" (
    "Id" uuid NOT NULL,
    "AccessFailedCount" int4 NOT NULL,
    "ConcurrencyStamp" text NULL,
    "Email" varchar(256) NULL,
    "EmailConfirmed" bool NOT NULL,
    "LastLanguageFromId" int4 NULL,
    "LastLanguageToId" int4 NULL,
    "LastLogin" timestamp NOT NULL,
    "LockoutEnabled" bool NOT NULL,
    "LockoutEnd" timestamptz NULL,
    "NormalizedEmail" varchar(256) NULL,
    "NormalizedUserName" varchar(256) NULL,
    "PasswordHash" text NULL,
    "PhoneNumber" text NULL,
    "PhoneNumberConfirmed" bool NOT NULL,
    "PremiumUntil" timestamp NOT NULL,
    "SecurityStamp" text NULL,
    "SignedUp" timestamp NOT NULL,
    "TwoFactorEnabled" bool NOT NULL,
    "UserName" varchar(256) NULL,
    CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetUsers_Languages_LastLanguageFromId" FOREIGN KEY ("LastLanguageFromId") REFERENCES "Languages" ("Id") ON DELETE NO ACTION,
    CONSTRAINT "FK_AspNetUsers_Languages_LastLanguageToId" FOREIGN KEY ("LastLanguageToId") REFERENCES "Languages" ("Id") ON DELETE NO ACTION
);

CREATE TABLE "Lessons" (
    "Id" uuid NOT NULL,
    "LanguageFromId" int4 NULL,
    "LanguageToId" int4 NULL,
    "Meaning" text NULL,
    "Pronunciation" text NULL,
    "Reading" text NULL,
    CONSTRAINT "PK_Lessons" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Lessons_Languages_LanguageFromId" FOREIGN KEY ("LanguageFromId") REFERENCES "Languages" ("Id") ON DELETE NO ACTION,
    CONSTRAINT "FK_Lessons_Languages_LanguageToId" FOREIGN KEY ("LanguageToId") REFERENCES "Languages" ("Id") ON DELETE NO ACTION
);

CREATE TABLE "AspNetUserClaims" (
    "Id" serial NOT NULL,
    "ClaimType" text NULL,
    "ClaimValue" text NULL,
    "UserId" uuid NOT NULL,
    CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserLogins" (
    "LoginProvider" text NOT NULL,
    "ProviderKey" text NOT NULL,
    "ProviderDisplayName" text NULL,
    "UserId" uuid NOT NULL,
    CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserRoles" (
    "UserId" uuid NOT NULL,
    "RoleId" uuid NOT NULL,
    CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserTokens" (
    "UserId" uuid NOT NULL,
    "LoginProvider" text NOT NULL,
    "Name" text NOT NULL,
    "Value" text NULL,
    CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Theme" (
    "OwnerId" uuid NOT NULL,
    "BackgroundPrimary" text NULL,
    "BackgroundSecondary" text NULL,
    "BackgroundTertiary" text NULL,
    "BorderPrimary" text NULL,
    "BorderSecondary" text NULL,
    "BorderTertiary" text NULL,
    "TextPrimary" text NULL,
    "TextSecondary" text NULL,
    "TextTertiary" text NULL,
    CONSTRAINT "PK_Theme" PRIMARY KEY ("OwnerId"),
    CONSTRAINT "FK_Theme_AspNetUsers_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserLists" (
    "Id" uuid NOT NULL,
    "Created" timestamp NOT NULL,
    "LanguageFromId" int4 NULL,
    "LanguageToId" int4 NULL,
    "LastModified" timestamp NOT NULL,
    "Likes" int4 NOT NULL,
    "Ordered" bool NOT NULL,
    "OwnerId" uuid NULL,
    CONSTRAINT "PK_UserLists" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_UserLists_Languages_LanguageFromId" FOREIGN KEY ("LanguageFromId") REFERENCES "Languages" ("Id") ON DELETE NO ACTION,
    CONSTRAINT "FK_UserLists_Languages_LanguageToId" FOREIGN KEY ("LanguageToId") REFERENCES "Languages" ("Id") ON DELETE NO ACTION,
    CONSTRAINT "FK_UserLists_AspNetUsers_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES "AspNetUsers" ("Id") ON DELETE NO ACTION
);

CREATE TABLE "Assignments" (
    "Id" uuid NOT NULL,
    "LessonId" uuid NULL,
    "NextReview" timestamp NOT NULL,
    "OwnerId" uuid NULL,
    "Stage" int4 NOT NULL,
    CONSTRAINT "PK_Assignments" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Assignments_Lessons_LessonId" FOREIGN KEY ("LessonId") REFERENCES "Lessons" ("Id") ON DELETE NO ACTION,
    CONSTRAINT "FK_Assignments_AspNetUsers_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES "AspNetUsers" ("Id") ON DELETE NO ACTION
);

CREATE TABLE "AudioLocation" (
    "Id" uuid NOT NULL,
    "LessonId" uuid NULL,
    "RelFileName" text NULL,
    "UploaderId" uuid NULL,
    CONSTRAINT "PK_AudioLocation" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AudioLocation_Lessons_LessonId" FOREIGN KEY ("LessonId") REFERENCES "Lessons" ("Id") ON DELETE NO ACTION,
    CONSTRAINT "FK_AudioLocation_AspNetUsers_UploaderId" FOREIGN KEY ("UploaderId") REFERENCES "AspNetUsers" ("Id") ON DELETE NO ACTION
);

CREATE TABLE "BurnedAssignments" (
    "OwnerId" uuid NOT NULL,
    "LessonId" uuid NOT NULL,
    CONSTRAINT "PK_BurnedAssignments" PRIMARY KEY ("OwnerId", "LessonId"),
    CONSTRAINT "FK_BurnedAssignments_Lessons_LessonId" FOREIGN KEY ("LessonId") REFERENCES "Lessons" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_BurnedAssignments_AspNetUsers_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserListEntry" (
    "OwnerId" uuid NOT NULL,
    "LessonId" uuid NOT NULL,
    "Order" int4 NOT NULL,
    CONSTRAINT "PK_UserListEntry" PRIMARY KEY ("OwnerId", "LessonId"),
    CONSTRAINT "FK_UserListEntry_Lessons_LessonId" FOREIGN KEY ("LessonId") REFERENCES "Lessons" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UserListEntry_UserLists_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES "UserLists" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserListSubscription" (
    "ListId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    CONSTRAINT "PK_UserListSubscription" PRIMARY KEY ("ListId", "UserId"),
    CONSTRAINT "FK_UserListSubscription_AspNetUsers_ListId" FOREIGN KEY ("ListId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UserListSubscription_UserLists_UserId" FOREIGN KEY ("UserId") REFERENCES "UserLists" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Reviews" (
    "Id" uuid NOT NULL,
    "EndStage" int4 NOT NULL,
    "LessonId" uuid NULL,
    "ReviewDone" timestamp NOT NULL,
    "StartStage" int4 NOT NULL,
    "WrongAnswers" int4 NOT NULL,
    CONSTRAINT "PK_Reviews" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Reviews_Assignments_LessonId" FOREIGN KEY ("LessonId") REFERENCES "Assignments" ("Id") ON DELETE NO ACTION
);

CREATE TABLE "ListUpdateTracker" (
    "ListId" uuid NOT NULL,
    "LessonId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    CONSTRAINT "PK_ListUpdateTracker" PRIMARY KEY ("ListId", "LessonId", "UserId"),
    CONSTRAINT "FK_ListUpdateTracker_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_ListUpdateTracker_UserListEntry_ListId_LessonId" FOREIGN KEY ("ListId", "LessonId") REFERENCES "UserListEntry" ("OwnerId", "LessonId") ON DELETE CASCADE
);

CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");

CREATE UNIQUE INDEX "RoleNameIndex" ON "AspNetRoles" ("NormalizedName");

CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");

CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");

CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");

CREATE INDEX "IX_AspNetUsers_LastLanguageFromId" ON "AspNetUsers" ("LastLanguageFromId");

CREATE INDEX "IX_AspNetUsers_LastLanguageToId" ON "AspNetUsers" ("LastLanguageToId");

CREATE INDEX "EmailIndex" ON "AspNetUsers" ("NormalizedEmail");

CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName");

CREATE INDEX "IX_Assignments_LessonId" ON "Assignments" ("LessonId");

CREATE INDEX "IX_Assignments_OwnerId" ON "Assignments" ("OwnerId");

CREATE INDEX "IX_AudioLocation_LessonId" ON "AudioLocation" ("LessonId");

CREATE INDEX "IX_AudioLocation_UploaderId" ON "AudioLocation" ("UploaderId");

CREATE INDEX "IX_BurnedAssignments_LessonId" ON "BurnedAssignments" ("LessonId");

CREATE INDEX "IX_Lessons_LanguageFromId" ON "Lessons" ("LanguageFromId");

CREATE INDEX "IX_Lessons_LanguageToId" ON "Lessons" ("LanguageToId");

CREATE INDEX "IX_ListUpdateTracker_UserId" ON "ListUpdateTracker" ("UserId");

CREATE INDEX "IX_Reviews_LessonId" ON "Reviews" ("LessonId");

CREATE INDEX "IX_UserListEntry_LessonId" ON "UserListEntry" ("LessonId");

CREATE INDEX "IX_UserLists_LanguageFromId" ON "UserLists" ("LanguageFromId");

CREATE INDEX "IX_UserLists_LanguageToId" ON "UserLists" ("LanguageToId");

CREATE INDEX "IX_UserLists_OwnerId" ON "UserLists" ("OwnerId");

CREATE INDEX "IX_UserListSubscription_UserId" ON "UserListSubscription" ("UserId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20170807011208_InitialCreate', '2.0.0-preview2-25794');

