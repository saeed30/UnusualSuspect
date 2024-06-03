using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnusualSuspect.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class reportUserType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "ReportUserTypeId",
                table: "ReportUser",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateTable(
                name: "ReportUserType",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportUserType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportUser_ReportUserTypeId",
                table: "ReportUser",
                column: "ReportUserTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportUser_ReportUserType_ReportUserTypeId",
                table: "ReportUser",
                column: "ReportUserTypeId",
                principalTable: "ReportUserType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportUser_ReportUserType_ReportUserTypeId",
                table: "ReportUser");

            migrationBuilder.DropTable(
                name: "ReportUserType");

            migrationBuilder.DropIndex(
                name: "IX_ReportUser_ReportUserTypeId",
                table: "ReportUser");

            migrationBuilder.DropColumn(
                name: "ReportUserTypeId",
                table: "ReportUser");
        }
    }
}
