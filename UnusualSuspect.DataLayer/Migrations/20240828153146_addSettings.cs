using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnusualSuspect.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class addSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccompliceLooseCoin",
                table: "SoftSetting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccompliceLooseScore",
                table: "SoftSetting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccompliceWinCoin",
                table: "SoftSetting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccompliceWinScore",
                table: "SoftSetting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CoinCostToEnterPreGameForHost",
                table: "SoftSetting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DetectiveLooseCoin",
                table: "SoftSetting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DetectiveLooseScore",
                table: "SoftSetting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DetectiveWinCoin",
                table: "SoftSetting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DetectiveWinScore",
                table: "SoftSetting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WitnessLooseCoin",
                table: "SoftSetting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WitnessLooseScore",
                table: "SoftSetting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WitnessWinCoin",
                table: "SoftSetting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WitnessWinScore",
                table: "SoftSetting",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccompliceLooseCoin",
                table: "SoftSetting");

            migrationBuilder.DropColumn(
                name: "AccompliceLooseScore",
                table: "SoftSetting");

            migrationBuilder.DropColumn(
                name: "AccompliceWinCoin",
                table: "SoftSetting");

            migrationBuilder.DropColumn(
                name: "AccompliceWinScore",
                table: "SoftSetting");

            migrationBuilder.DropColumn(
                name: "CoinCostToEnterPreGameForHost",
                table: "SoftSetting");

            migrationBuilder.DropColumn(
                name: "DetectiveLooseCoin",
                table: "SoftSetting");

            migrationBuilder.DropColumn(
                name: "DetectiveLooseScore",
                table: "SoftSetting");

            migrationBuilder.DropColumn(
                name: "DetectiveWinCoin",
                table: "SoftSetting");

            migrationBuilder.DropColumn(
                name: "DetectiveWinScore",
                table: "SoftSetting");

            migrationBuilder.DropColumn(
                name: "WitnessLooseCoin",
                table: "SoftSetting");

            migrationBuilder.DropColumn(
                name: "WitnessLooseScore",
                table: "SoftSetting");

            migrationBuilder.DropColumn(
                name: "WitnessWinCoin",
                table: "SoftSetting");

            migrationBuilder.DropColumn(
                name: "WitnessWinScore",
                table: "SoftSetting");
        }
    }
}
