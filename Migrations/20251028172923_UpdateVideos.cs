using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KitabStock.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVideos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColorSpace",
                table: "Videos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "Videos",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "FilenameDownload",
                table: "Videos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "FrameRate",
                table: "Videos",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "VideoFormat",
                table: "Videos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VideoResolution",
                table: "Videos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorSpace",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "FilenameDownload",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "FrameRate",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "VideoFormat",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "VideoResolution",
                table: "Videos");
        }
    }
}
