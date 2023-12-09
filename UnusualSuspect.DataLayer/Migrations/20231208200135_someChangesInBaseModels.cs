using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnusualSuspect.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class someChangesInBaseModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemovedTurn",
                table: "CharacterCardGame");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "GameType",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "WonTheGame",
                table: "Game",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CharacterCardGame",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "GameType");

            migrationBuilder.DropColumn(
                name: "WonTheGame",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CharacterCardGame");

            migrationBuilder.AddColumn<short>(
                name: "RemovedTurn",
                table: "CharacterCardGame",
                type: "smallint",
                nullable: true);
        }
    }
}
