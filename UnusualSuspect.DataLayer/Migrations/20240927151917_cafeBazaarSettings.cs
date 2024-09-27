using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnusualSuspect.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class cafeBazaarSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CafebazaarClientId",
                table: "SoftSetting",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CafebazaarClientSecret",
                table: "SoftSetting",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CafebazaarRefreshToken",
                table: "SoftSetting",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CafebazaarClientId",
                table: "SoftSetting");

            migrationBuilder.DropColumn(
                name: "CafebazaarClientSecret",
                table: "SoftSetting");

            migrationBuilder.DropColumn(
                name: "CafebazaarRefreshToken",
                table: "SoftSetting");
        }
    }
}
