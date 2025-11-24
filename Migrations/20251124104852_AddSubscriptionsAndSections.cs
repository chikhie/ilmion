using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ilmanar.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionsAndSections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChapterPurchases");

            migrationBuilder.DropColumn(
                name: "IsFree",
                table: "Chapters");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Chapters");

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    ChapterId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sections_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    StripeSubscriptionId = table.Column<string>(type: "TEXT", nullable: true),
                    StripeCustomerId = table.Column<string>(type: "TEXT", nullable: true),
                    StripeSessionId = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CancelledDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GuestEmail = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sections_ChapterId_DisplayOrder",
                table: "Sections",
                columns: new[] { "ChapterId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_StripeSubscriptionId",
                table: "Subscriptions",
                column: "StripeSubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UserId",
                table: "Subscriptions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UserId_Status",
                table: "Subscriptions",
                columns: new[] { "UserId", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.AddColumn<bool>(
                name: "IsFree",
                table: "Chapters",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Chapters",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "ChapterPurchases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ChapterId = table.Column<Guid>(type: "TEXT", nullable: false),
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
        }
    }
}
