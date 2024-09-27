using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnusualSuspect.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class renamePayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CafebazaarProductId",
                table: "SoftSetting",
                newName: "CafebazaarPackageName");

            migrationBuilder.RenameColumn(
                name: "PackageName",
                table: "PaymentCafeBazaar",
                newName: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CafebazaarPackageName",
                table: "SoftSetting",
                newName: "CafebazaarProductId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "PaymentCafeBazaar",
                newName: "PackageName");
        }
    }
}
