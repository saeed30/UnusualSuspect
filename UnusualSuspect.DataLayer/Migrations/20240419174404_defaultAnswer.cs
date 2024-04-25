using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnusualSuspect.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class defaultAnswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestionCharacterCardDefaultAnswer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<short>(type: "smallint", nullable: false),
                    CharacterCardId = table.Column<short>(type: "smallint", nullable: false),
                    DefaultAnswer = table.Column<bool>(type: "bit", nullable: false),
                    DateTimeAdded = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionCharacterCardDefaultAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionCharacterCardDefaultAnswer_CharacterCard_CharacterCardId",
                        column: x => x.CharacterCardId,
                        principalTable: "CharacterCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionCharacterCardDefaultAnswer_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionCharacterCardDefaultAnswer_CharacterCardId",
                table: "QuestionCharacterCardDefaultAnswer",
                column: "CharacterCardId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionCharacterCardDefaultAnswer_QuestionId",
                table: "QuestionCharacterCardDefaultAnswer",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionCharacterCardDefaultAnswer");
        }
    }
}
