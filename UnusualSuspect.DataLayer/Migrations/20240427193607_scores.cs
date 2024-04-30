using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UnusualSuspect.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class scores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CalculatedScore",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RankingDaily",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RankingMonthly",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RankingWeekly",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ScoreType",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Score",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    TimeAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: true),
                    ScoreTypeId = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Score", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Score_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Score_ScoreType_ScoreTypeId",
                        column: x => x.ScoreTypeId,
                        principalTable: "ScoreType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ScoreType",
                columns: new[] { "Id", "Name", "Title" },
                values: new object[,]
                {
                    { (short)1, "GameWonAsDetective", "برد با نقش کارآگاه" },
                    { (short)2, "GameWonAsMainDetective", "برد با نقش کارآگاه ستاره دار" },
                    { (short)3, "GameWonAsWitness", "برد با نقش شاهد" },
                    { (short)4, "GameWonAsAccomplice", "برد با نقش شریک جرم" },
                    { (short)5, "GameLostAsDetective", "باخت با نقش کارآگاه" },
                    { (short)6, "GameLostAsMainDetective", "باخت با نقش کارآگاه ستاره دار" },
                    { (short)7, "GameLostAsWitness", "باخت با نقش شاهد" },
                    { (short)8, "GameLostAsAccomplice", "باخت با نقش شریک جرم" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Score_GameId",
                table: "Score",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Score_ScoreTypeId",
                table: "Score",
                column: "ScoreTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Score");

            migrationBuilder.DropTable(
                name: "ScoreType");

            migrationBuilder.DropColumn(
                name: "CalculatedScore",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RankingDaily",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RankingMonthly",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RankingWeekly",
                table: "AspNetUsers");
        }
    }
}
