using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnusualSuspect.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class moveGameId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JoinedPreGame_Game_GameId",
                table: "JoinedPreGame");

            migrationBuilder.DropIndex(
                name: "IX_JoinedPreGame_GameId",
                table: "JoinedPreGame");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "JoinedPreGame");

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "PreGameGroup",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PreGameGroup_GameId",
                table: "PreGameGroup",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreGameGroup_Game_GameId",
                table: "PreGameGroup",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreGameGroup_Game_GameId",
                table: "PreGameGroup");

            migrationBuilder.DropIndex(
                name: "IX_PreGameGroup_GameId",
                table: "PreGameGroup");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "PreGameGroup");

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "JoinedPreGame",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JoinedPreGame_GameId",
                table: "JoinedPreGame",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_JoinedPreGame_Game_GameId",
                table: "JoinedPreGame",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id");
        }
    }
}
