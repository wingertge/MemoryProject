CREATE TABLE "Posts" (
    "Id" uuid NOT NULL,
    "AuthorId" uuid NULL,
    "CreatedAt" timestamp NOT NULL,
    "EditedAt" timestamp NOT NULL,
    "Text" text NULL,
    "Title" text NULL,
    CONSTRAINT "PK_Posts" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Posts_AspNetUsers_AuthorId" FOREIGN KEY ("AuthorId") REFERENCES "AspNetUsers" ("Id") ON DELETE NO ACTION
);

CREATE TABLE "Tag" (
    "Id" serial NOT NULL,
    "Value" text NULL,
    CONSTRAINT "PK_Tag" PRIMARY KEY ("Id")
);

CREATE TABLE "PostTag" (
    "PostId" uuid NOT NULL,
    "TagId" int4 NOT NULL,
    CONSTRAINT "PK_PostTag" PRIMARY KEY ("PostId", "TagId"),
    CONSTRAINT "FK_PostTag_Posts_PostId" FOREIGN KEY ("PostId") REFERENCES "Posts" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_PostTag_Tag_TagId" FOREIGN KEY ("TagId") REFERENCES "Tag" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Posts_AuthorId" ON "Posts" ("AuthorId");

CREATE INDEX "IX_PostTag_TagId" ON "PostTag" ("TagId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20170820121640_NewsPosts', '2.0.0-rtm-26452');

