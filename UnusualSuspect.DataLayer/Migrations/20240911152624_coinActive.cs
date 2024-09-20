using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnusualSuspect.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class coinActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CafebazaarAccessToken",
                table: "SoftSetting",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CafebazaarProductId",
                table: "SoftSetting",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SmsProviderApiKey",
                table: "SoftSetting",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CoinPackageUser",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CafebazaarAccessToken",
                table: "SoftSetting");

            migrationBuilder.DropColumn(
                name: "CafebazaarProductId",
                table: "SoftSetting");

            migrationBuilder.DropColumn(
                name: "SmsProviderApiKey",
                table: "SoftSetting");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CoinPackageUser");
        }
    }
}
