using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KitabStock.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyVideoPurchaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoPurchases_AspNetUsers_UserId",
                table: "VideoPurchases");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoPurchases_Videos_VideoId",
                table: "VideoPurchases");

            migrationBuilder.DropIndex(
                name: "IX_VideoPurchases_GuestEmail",
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
                name: "PaymentMethod",
                table: "VideoPurchases");

            migrationBuilder.DropColumn(
                name: "TransactionId",
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

            migrationBuilder.AlterColumn<string>(
                name: "PurchaseCode",
                table: "VideoPurchases",
                type: "TEXT",
                maxLength: 12,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 12,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripePaymentIntentId",
                table: "VideoPurchases",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoPurchases_AspNetUsers_UserId",
                table: "VideoPurchases",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoPurchases_Videos_VideoId",
                table: "VideoPurchases",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoPurchases_AspNetUsers_UserId",
                table: "VideoPurchases");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoPurchases_Videos_VideoId",
                table: "VideoPurchases");

            migrationBuilder.DropColumn(
                name: "StripePaymentIntentId",
                table: "VideoPurchases");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "VideoPurchases",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "PurchaseCode",
                table: "VideoPurchases",
                type: "TEXT",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 12);

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
                name: "PaymentMethod",
                table: "VideoPurchases",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "VideoPurchases",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VideoPurchases_GuestEmail",
                table: "VideoPurchases",
                column: "GuestEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoPurchases_AspNetUsers_UserId",
                table: "VideoPurchases",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoPurchases_Videos_VideoId",
                table: "VideoPurchases",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
