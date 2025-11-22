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
    "Description" TEXT NOT NULL,
    "Duration" TEXT NOT NULL,
    "VideoResolution" TEXT NOT NULL,
    "VideoFormat" TEXT NOT NULL,
    "FrameRate" REAL NOT NULL,
    "FilenameDownload" TEXT NOT NULL,
    "ColorSpace" TEXT NOT NULL,
    "Link" TEXT NOT NULL,
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
    "Title" TEXT NOT NULL,
    "Description" TEXT NOT NULL,
    "DisplayOrder" INTEGER NOT NULL,
    "DurationMinutes" INTEGER NOT NULL,
    "Price" decimal(18,2) NOT NULL,
    "IsFree" INTEGER NOT NULL,
    "VideoId" TEXT NULL,
    "SubjectId" INTEGER NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NOT NULL,
    CONSTRAINT "FK_Modules_Subjects_SubjectId" FOREIGN KEY ("SubjectId") REFERENCES "Subjects" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Modules_Videos_VideoId" FOREIGN KEY ("VideoId") REFERENCES "Videos" ("Id") ON DELETE SET NULL
);


CREATE TABLE "ModulePurchases" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_ModulePurchases" PRIMARY KEY,
    "UserId" TEXT NOT NULL,
    "ModuleId" TEXT NOT NULL,
    "AmountPaid" decimal(18,2) NOT NULL,
    "Currency" TEXT NOT NULL,
    "StripeSessionId" TEXT NULL,
    "StripePaymentIntentId" TEXT NULL,
    "Status" INTEGER NOT NULL,
    "PurchaseDate" TEXT NOT NULL,
    "CompletedDate" TEXT NULL,
    "GuestEmail" TEXT NULL,
    CONSTRAINT "FK_ModulePurchases_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_ModulePurchases_Modules_ModuleId" FOREIGN KEY ("ModuleId") REFERENCES "Modules" ("Id") ON DELETE RESTRICT
);


CREATE TABLE "Sections" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Sections" PRIMARY KEY,
    "Title" TEXT NOT NULL,
    "Content" TEXT NOT NULL,
    "Type" INTEGER NOT NULL,
    "DisplayOrder" INTEGER NOT NULL,
    "ComponentPath" TEXT NULL,
    "ComponentHash" TEXT NULL,
    "ModuleId" TEXT NOT NULL,
    "CreatedAt" TEXT NOT NULL,
    "UpdatedAt" TEXT NOT NULL,
    CONSTRAINT "FK_Sections_Modules_ModuleId" FOREIGN KEY ("ModuleId") REFERENCES "Modules" ("Id") ON DELETE CASCADE
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


CREATE INDEX "IX_ModulePurchases_ModuleId" ON "ModulePurchases" ("ModuleId");


CREATE INDEX "IX_ModulePurchases_StripeSessionId" ON "ModulePurchases" ("StripeSessionId");


CREATE INDEX "IX_ModulePurchases_UserId_ModuleId" ON "ModulePurchases" ("UserId", "ModuleId");


CREATE INDEX "IX_Modules_SubjectId_DisplayOrder" ON "Modules" ("SubjectId", "DisplayOrder");


CREATE INDEX "IX_Modules_VideoId" ON "Modules" ("VideoId");


CREATE INDEX "IX_Sections_ModuleId_DisplayOrder" ON "Sections" ("ModuleId", "DisplayOrder");


CREATE INDEX "IX_Subjects_Id" ON "Subjects" ("Id");


