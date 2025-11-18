using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KitabStock.Migrations
{
    /// <inheritdoc />
    public partial class AddVideoPurchaseRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_AspNetUsers_UserEntityId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Videos_UserEntityId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "UserEntityId",
                table: "Videos");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Videos",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.CreateTable(
                name: "VideoPurchases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    VideoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PricePaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    PaymentMethod = table.Column<string>(type: "TEXT", nullable: true),
                    TransactionId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoPurchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoPurchases_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VideoPurchases_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VideoPurchases_PurchaseDate",
                table: "VideoPurchases",
                column: "PurchaseDate");

            migrationBuilder.CreateIndex(
                name: "IX_VideoPurchases_UserId_VideoId",
                table: "VideoPurchases",
                columns: new[] { "UserId", "VideoId" });

            migrationBuilder.CreateIndex(
                name: "IX_VideoPurchases_VideoId",
                table: "VideoPurchases",
                column: "VideoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VideoPurchases");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Videos",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "UserEntityId",
                table: "Videos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Videos_UserEntityId",
                table: "Videos",
                column: "UserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_AspNetUsers_UserEntityId",
                table: "Videos",
                column: "UserEntityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
