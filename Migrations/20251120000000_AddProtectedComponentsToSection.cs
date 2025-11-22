using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ilmanar.Migrations
{
    /// <inheritdoc />
    public partial class AddProtectedComponentsToSection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ComponentPath",
                table: "Sections",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ComponentHash",
                table: "Sections",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComponentPath",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "ComponentHash",
                table: "Sections");
        }
    }
}

