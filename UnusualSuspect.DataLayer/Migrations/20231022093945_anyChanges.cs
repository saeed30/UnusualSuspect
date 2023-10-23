using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnusualSuspect.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class anyChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "GameType",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "PreGameGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReadyToGameTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CalulatedJoinedUsers = table.Column<short>(type: "smallint", nullable: false),
                    GameTypeId = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreGameGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreGameGroup_GameType_GameTypeId",
                        column: x => x.GameTypeId,
                        principalTable: "GameType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReadyToGameStatus",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadyToGameStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JoinedPreGame",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JoinTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsOwnerOfPreGroup = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PreGameGroupId = table.Column<int>(type: "int", nullable: false),
                    ReadyToGameStatusId = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoinedPreGame", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JoinedPreGame_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JoinedPreGame_PreGameGroup_PreGameGroupId",
                        column: x => x.PreGameGroupId,
                        principalTable: "PreGameGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JoinedPreGame_ReadyToGameStatus_ReadyToGameStatusId",
                        column: x => x.ReadyToGameStatusId,
                        principalTable: "ReadyToGameStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JoinedPreGame_PreGameGroupId",
                table: "JoinedPreGame",
                column: "PreGameGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinedPreGame_ReadyToGameStatusId",
                table: "JoinedPreGame",
                column: "ReadyToGameStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinedPreGame_UserId",
                table: "JoinedPreGame",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PreGameGroup_GameTypeId",
                table: "PreGameGroup",
                column: "GameTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JoinedPreGame");

            migrationBuilder.DropTable(
                name: "PreGameGroup");

            migrationBuilder.DropTable(
                name: "ReadyToGameStatus");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "GameType");
        }
    }
}
