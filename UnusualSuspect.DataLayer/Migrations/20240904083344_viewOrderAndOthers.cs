using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnusualSuspect.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class viewOrderAndOthers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "SoftSetting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ViewOrder",
                table: "SmsSendingStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ViewOrder",
                table: "ScoreType",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ViewOrder",
                table: "RoleCard",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ViewOrder",
                table: "ReportUserType",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ViewOrder",
                table: "ReadyToGameStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ViewOrder",
                table: "PriceType",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ViewOrder",
                table: "PreGameGroupStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ViewOrder",
                table: "GameStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "SoftSetting");

            migrationBuilder.DropColumn(
                name: "ViewOrder",
                table: "SmsSendingStatus");

            migrationBuilder.DropColumn(
                name: "ViewOrder",
                table: "ScoreType");

            migrationBuilder.DropColumn(
                name: "ViewOrder",
                table: "RoleCard");

            migrationBuilder.DropColumn(
                name: "ViewOrder",
                table: "ReportUserType");

            migrationBuilder.DropColumn(
                name: "ViewOrder",
                table: "ReadyToGameStatus");

            migrationBuilder.DropColumn(
                name: "ViewOrder",
                table: "PriceType");

            migrationBuilder.DropColumn(
                name: "ViewOrder",
                table: "PreGameGroupStatus");

            migrationBuilder.DropColumn(
                name: "ViewOrder",
                table: "GameStatus");
        }
    }
}
