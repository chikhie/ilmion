using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ilmanar.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChapterVideoFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Videos_VideoId",
                table: "Chapters");

            migrationBuilder.DropIndex(
                name: "IX_Chapters_VideoId",
                table: "Chapters");

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Chapters",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Chapters");

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_VideoId",
                table: "Chapters",
                column: "VideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_Videos_VideoId",
                table: "Chapters",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
