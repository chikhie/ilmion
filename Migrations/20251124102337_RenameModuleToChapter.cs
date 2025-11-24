using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ilmanar.Migrations
{
    /// <inheritdoc />
    public partial class RenameModuleToChapter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Videos_VideoId",
                table: "Modules");

            migrationBuilder.DropTable(
                name: "ModulePurchases");

            migrationBuilder.DropIndex(
                name: "IX_Modules_SubjectId_DisplayOrder",
                table: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_Modules_VideoId",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "IsFree",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "VideoId",
                table: "Modules");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Modules",
                newName: "Label");

            migrationBuilder.CreateTable(
                name: "Chapters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    DurationMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsFree = table.Column<bool>(type: "INTEGER", nullable: false),
                    VideoId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ModuleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chapters_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Chapters_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ChapterPurchases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ChapterId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    StripeSessionId = table.Column<string>(type: "TEXT", nullable: true),
                    StripePaymentIntentId = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    GuestEmail = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChapterPurchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChapterPurchases_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChapterPurchases_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Modules_SubjectId",
                table: "Modules",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ChapterPurchases_ChapterId",
                table: "ChapterPurchases",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_ChapterPurchases_StripeSessionId",
                table: "ChapterPurchases",
                column: "StripeSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ChapterPurchases_UserId_ChapterId",
                table: "ChapterPurchases",
                columns: new[] { "UserId", "ChapterId" });

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_ModuleId_DisplayOrder",
                table: "Chapters",
                columns: new[] { "ModuleId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_VideoId",
                table: "Chapters",
                column: "VideoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChapterPurchases");

            migrationBuilder.DropTable(
                name: "Chapters");

            migrationBuilder.DropIndex(
                name: "IX_Modules_SubjectId",
                table: "Modules");

            migrationBuilder.RenameColumn(
                name: "Label",
                table: "Modules",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Modules",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Modules",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "Modules",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "Modules",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsFree",
                table: "Modules",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Modules",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "VideoId",
                table: "Modules",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ModulePurchases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ModuleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    GuestEmail = table.Column<string>(type: "TEXT", nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    StripePaymentIntentId = table.Column<string>(type: "TEXT", nullable: true),
                    StripeSessionId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModulePurchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModulePurchases_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModulePurchases_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Modules_SubjectId_DisplayOrder",
                table: "Modules",
                columns: new[] { "SubjectId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Modules_VideoId",
                table: "Modules",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_ModulePurchases_ModuleId",
                table: "ModulePurchases",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ModulePurchases_StripeSessionId",
                table: "ModulePurchases",
                column: "StripeSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ModulePurchases_UserId_ModuleId",
                table: "ModulePurchases",
                columns: new[] { "UserId", "ModuleId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Videos_VideoId",
                table: "Modules",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
