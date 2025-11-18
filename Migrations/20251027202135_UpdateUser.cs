using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KitabStock.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_AspNetUsers_UserId",
                table: "Videos");

            migrationBuilder.DropTable(
                name: "Websites");

            migrationBuilder.DropIndex(
                name: "IX_Videos_UserId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Budget",
                table: "Videos",
                newName: "Price");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Videos",
                newName: "Budget");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Videos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Videos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "Videos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Videos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Websites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Logo = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Websites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Websites_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Videos_UserId",
                table: "Videos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Websites_UserId",
                table: "Websites",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_AspNetUsers_UserId",
                table: "Videos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
