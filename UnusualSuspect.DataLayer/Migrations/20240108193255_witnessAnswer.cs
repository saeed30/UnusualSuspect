using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnusualSuspect.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class witnessAnswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WitnessLastAnswer",
                table: "Game",
                type: "bit",
                nullable: true);

            migrationBuilder.InsertData(
                table: "GameStatus",
                columns: new[] { "Id", "Name", "Title" },
                values: new object[] { (short)6, "WaitingForWitnessToAnswer", "در انتظار شاهد جهت پاسخ به سوال" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GameStatus",
                keyColumn: "Id",
                keyValue: (short)6);

            migrationBuilder.DropColumn(
                name: "WitnessLastAnswer",
                table: "Game");
        }
    }
}
