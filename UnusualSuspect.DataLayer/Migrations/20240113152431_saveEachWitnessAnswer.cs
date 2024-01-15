using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnusualSuspect.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class saveEachWitnessAnswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnswerUserId",
                table: "QuestionGame",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UserAnswer",
                table: "QuestionGame",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionGame_AnswerUserId",
                table: "QuestionGame",
                column: "AnswerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionGame_AspNetUsers_AnswerUserId",
                table: "QuestionGame",
                column: "AnswerUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionGame_AspNetUsers_AnswerUserId",
                table: "QuestionGame");

            migrationBuilder.DropIndex(
                name: "IX_QuestionGame_AnswerUserId",
                table: "QuestionGame");

            migrationBuilder.DropColumn(
                name: "AnswerUserId",
                table: "QuestionGame");

            migrationBuilder.DropColumn(
                name: "UserAnswer",
                table: "QuestionGame");
        }
    }
}
