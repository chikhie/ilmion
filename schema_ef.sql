CREATE TABLE "AspNetRoles" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_AspNetRoles" PRIMARY KEY,
    "Name" TEXT NULL,
    "NormalizedName" TEXT NULL,
    "ConcurrencyStamp" TEXT NULL
);


CREATE TABLE "AspNetUsers" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_AspNetUsers" PRIMARY KEY,
    "RefreshToken" TEXT NULL,
    "RefreshTokenExpiryTime" TEXT NOT NULL,
    "FirstName" TEXT NULL,
    "LastName" TEXT NULL,
    "ProfilePicture" TEXT NULL,
    "Bio" TEXT NULL,
    "DateOfBirth" TEXT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NOT NULL,
    "UserName" TEXT NULL,
    "NormalizedUserName" TEXT NULL,
    "Email" TEXT NULL,
    "NormalizedEmail" TEXT NULL,
    "EmailConfirmed" INTEGER NOT NULL,
    "PasswordHash" TEXT NULL,
    "SecurityStamp" TEXT NULL,
    "ConcurrencyStamp" TEXT NULL,
    "PhoneNumber" TEXT NULL,
    "PhoneNumberConfirmed" INTEGER NOT NULL,
    "TwoFactorEnabled" INTEGER NOT NULL,
    "LockoutEnd" TEXT NULL,
    "LockoutEnabled" INTEGER NOT NULL,
    "AccessFailedCount" INTEGER NOT NULL
);


CREATE TABLE "Subjects" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Subjects" PRIMARY KEY AUTOINCREMENT,
    "Label" TEXT NOT NULL
);


CREATE TABLE "Videos" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Videos" PRIMARY KEY,
    "Title" TEXT NOT NULL,
    "Duration" TEXT NOT NULL,
    "VideoResolution" TEXT NOT NULL,
    "VideoFormat" TEXT NOT NULL,
    "ThumbnailPath" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NOT NULL
);


CREATE TABLE "AspNetRoleClaims" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY AUTOINCREMENT,
    "RoleId" TEXT NOT NULL,
    "ClaimType" TEXT NULL,
    "ClaimValue" TEXT NULL,
    CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);


CREATE TABLE "AspNetUserClaims" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY AUTOINCREMENT,
    "UserId" TEXT NOT NULL,
    "ClaimType" TEXT NULL,
    "ClaimValue" TEXT NULL,
    CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);


CREATE TABLE "AspNetUserLogins" (
    "LoginProvider" TEXT NOT NULL,
    "ProviderKey" TEXT NOT NULL,
    "ProviderDisplayName" TEXT NULL,
    "UserId" TEXT NOT NULL,
    CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);


CREATE TABLE "AspNetUserRoles" (
    "UserId" TEXT NOT NULL,
    "RoleId" TEXT NOT NULL,
    CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);


CREATE TABLE "AspNetUserTokens" (
    "UserId" TEXT NOT NULL,
    "LoginProvider" TEXT NOT NULL,
    "Name" TEXT NOT NULL,
    "Value" TEXT NULL,
    CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);


CREATE TABLE "Modules" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Modules" PRIMARY KEY,
    "Label" TEXT NOT NULL,
    "SubjectId" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NOT NULL,
    CONSTRAINT "FK_Modules_Subjects_SubjectId" FOREIGN KEY ("SubjectId") REFERENCES "Subjects" ("Id") ON DELETE CASCADE
);


CREATE TABLE "Chapters" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Chapters" PRIMARY KEY,
    "Title" TEXT NOT NULL,
    "Description" TEXT NOT NULL,
    "DisplayOrder" INTEGER NOT NULL,
    "DurationMinutes" INTEGER NOT NULL,
    "Price" decimal(18,2) NOT NULL,
    "IsFree" INTEGER NOT NULL,
    "VideoId" TEXT NULL,
    "ModuleId" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NOT NULL,
    "Content" TEXT NULL,
    CONSTRAINT "FK_Chapters_Modules_ModuleId" FOREIGN KEY ("ModuleId") REFERENCES "Modules" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Chapters_Videos_VideoId" FOREIGN KEY ("VideoId") REFERENCES "Videos" ("Id") ON DELETE SET NULL
);


CREATE TABLE "ChapterPurchases" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_ChapterPurchases" PRIMARY KEY,
    "UserId" TEXT NOT NULL,
    "ChapterId" TEXT NOT NULL,
    "AmountPaid" decimal(18,2) NOT NULL,
    "Currency" TEXT NOT NULL,
    "StripeSessionId" TEXT NULL,
    "StripePaymentIntentId" TEXT NULL,
    "Status" INTEGER NOT NULL,
    "PurchaseDate" TEXT NOT NULL,
    "CompletedDate" TEXT NULL,
    "GuestEmail" TEXT NULL,
    CONSTRAINT "FK_ChapterPurchases_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_ChapterPurchases_Chapters_ChapterId" FOREIGN KEY ("ChapterId") REFERENCES "Chapters" ("Id") ON DELETE RESTRICT
);


INSERT INTO "Subjects" ("Id", "Label")
VALUES (1, 'Mathématiques');
SELECT changes();

INSERT INTO "Subjects" ("Id", "Label")
VALUES (2, 'Physique');
SELECT changes();

INSERT INTO "Subjects" ("Id", "Label")
VALUES (3, 'Chimie');
SELECT changes();

INSERT INTO "Subjects" ("Id", "Label")
VALUES (4, 'Biologie');
SELECT changes();

INSERT INTO "Subjects" ("Id", "Label")
VALUES (5, 'Informatique');
SELECT changes();



CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");


CREATE UNIQUE INDEX "RoleNameIndex" ON "AspNetRoles" ("NormalizedName");


CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");


CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");


CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");


CREATE INDEX "EmailIndex" ON "AspNetUsers" ("NormalizedEmail");


CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName");


CREATE INDEX "IX_ChapterPurchases_ChapterId" ON "ChapterPurchases" ("ChapterId");


CREATE INDEX "IX_ChapterPurchases_StripeSessionId" ON "ChapterPurchases" ("StripeSessionId");


CREATE INDEX "IX_ChapterPurchases_UserId_ChapterId" ON "ChapterPurchases" ("UserId", "ChapterId");


CREATE INDEX "IX_Chapters_ModuleId_DisplayOrder" ON "Chapters" ("ModuleId", "DisplayOrder");


CREATE INDEX "IX_Chapters_VideoId" ON "Chapters" ("VideoId");


CREATE INDEX "IX_Modules_SubjectId" ON "Modules" ("SubjectId");


CREATE INDEX "IX_Subjects_Id" ON "Subjects" ("Id");


