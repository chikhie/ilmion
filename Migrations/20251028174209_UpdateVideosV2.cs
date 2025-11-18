using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KitabStock.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVideosV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoCode",
                table: "Videos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoCode",
                table: "Videos");
        }
    }
}
