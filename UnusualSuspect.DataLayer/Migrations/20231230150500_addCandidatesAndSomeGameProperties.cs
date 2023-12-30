using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UnusualSuspect.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class addCandidatesAndSomeGameProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WonTheGame",
                table: "Game");

            migrationBuilder.AddColumn<DateTime>(
                name: "CurrentUserTurnStartedTime",
                table: "Game",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "GameStatusId",
                table: "Game",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1);

            migrationBuilder.AddColumn<short>(
                name: "OrderOfParticipationTalkBeginner",
                table: "Game",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "OrderOfParticipationTurnToTalk",
                table: "Game",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TalkingTurnStartedTime",
                table: "Game",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GameCandidate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CharacterCardId = table.Column<short>(type: "smallint", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    DateTimeAdded = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameCandidate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameCandidate_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameCandidate_CharacterCard_CharacterCardId",
                        column: x => x.CharacterCardId,
                        principalTable: "CharacterCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameCandidate_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GameStatus",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameStatus", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "GameStatus",
                columns: new[] { "Id", "Name", "Title" },
                values: new object[,]
                {
                    { (short)1, "WaitingForPlayers", "در انتظار بازیکنان جهت شروع بازی" },
                    { (short)2, "Talking", "صحبت های قبل از انتخاب" },
                    { (short)3, "WaitingForMainDetectiveToChoose", "در انتظار کارآگاه ستاره جهت انتخاب" },
                    { (short)4, "FinishedAndWonTheGame", "پایان با پیروزی" },
                    { (short)5, "FinishedAndLostTheGame", "پایان با شکست" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Game_GameStatusId",
                table: "Game",
                column: "GameStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_GameCandidate_CharacterCardId",
                table: "GameCandidate",
                column: "CharacterCardId");

            migrationBuilder.CreateIndex(
                name: "IX_GameCandidate_GameId",
                table: "GameCandidate",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameCandidate_UserId",
                table: "GameCandidate",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_GameStatus_GameStatusId",
                table: "Game",
                column: "GameStatusId",
                principalTable: "GameStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_GameStatus_GameStatusId",
                table: "Game");

            migrationBuilder.DropTable(
                name: "GameCandidate");

            migrationBuilder.DropTable(
                name: "GameStatus");

            migrationBuilder.DropIndex(
                name: "IX_Game_GameStatusId",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "CurrentUserTurnStartedTime",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "GameStatusId",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "OrderOfParticipationTalkBeginner",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "OrderOfParticipationTurnToTalk",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "TalkingTurnStartedTime",
                table: "Game");

            migrationBuilder.AddColumn<bool>(
                name: "WonTheGame",
                table: "Game",
                type: "bit",
                nullable: true);
        }
    }
}
