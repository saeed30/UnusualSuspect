using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnusualSuspect.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class addAvatarId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "AvatarId",
                table: "AspNetUsers",
                type: "smallint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarId",
                table: "AspNetUsers");
        }
    }
}
