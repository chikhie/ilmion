using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KitabStock.Migrations
{
    /// <inheritdoc />
    public partial class AddGuestPurchaseSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "VideoPurchases",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "AccessCount",
                table: "VideoPurchases",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CodeExpiryDate",
                table: "VideoPurchases",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuestEmail",
                table: "VideoPurchases",
                type: "TEXT",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxAccessCount",
                table: "VideoPurchases",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PurchaseCode",
                table: "VideoPurchases",
                type: "TEXT",
                maxLength: 12,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VideoPurchases_GuestEmail",
                table: "VideoPurchases",
                column: "GuestEmail");

            migrationBuilder.CreateIndex(
                name: "IX_VideoPurchases_PurchaseCode",
                table: "VideoPurchases",
                column: "PurchaseCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VideoPurchases_GuestEmail",
                table: "VideoPurchases");

            migrationBuilder.DropIndex(
                name: "IX_VideoPurchases_PurchaseCode",
                table: "VideoPurchases");

            migrationBuilder.DropColumn(
                name: "AccessCount",
                table: "VideoPurchases");

            migrationBuilder.DropColumn(
                name: "CodeExpiryDate",
                table: "VideoPurchases");

            migrationBuilder.DropColumn(
                name: "GuestEmail",
                table: "VideoPurchases");

            migrationBuilder.DropColumn(
                name: "MaxAccessCount",
                table: "VideoPurchases");

            migrationBuilder.DropColumn(
                name: "PurchaseCode",
                table: "VideoPurchases");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "VideoPurchases",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
