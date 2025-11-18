using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KitabStock.Migrations
{
    /// <inheritdoc />
    public partial class RemoveVideoCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoCode",
                table: "Videos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoCode",
                table: "Videos",
                type: "TEXT",
                maxLength: 12,
                nullable: false,
                defaultValue: "");
        }
    }
}
