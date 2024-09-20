using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnusualSuspect.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class paymentAndPackage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchaseToken",
                table: "PaymentUser");

            migrationBuilder.DropColumn(
                name: "ValidationCheckDateTime",
                table: "PaymentUser");

            migrationBuilder.DropColumn(
                name: "ValidationError",
                table: "PaymentUser");

            migrationBuilder.AddColumn<bool>(
                name: "IsOneTimeUse",
                table: "StickerPackage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOneTimeUse",
                table: "GemPackage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOneTimeUse",
                table: "CoinPackage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOneTimeUse",
                table: "AvatarPackage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "PaymentCafeBazaar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValidationError = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PackageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValidationCheckDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsValid = table.Column<bool>(type: "bit", nullable: true),
                    PaymentUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentCafeBazaar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentCafeBazaar_PaymentUser_PaymentUserId",
                        column: x => x.PaymentUserId,
                        principalTable: "PaymentUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCafeBazaar_PaymentUserId",
                table: "PaymentCafeBazaar",
                column: "PaymentUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentCafeBazaar");

            migrationBuilder.DropColumn(
                name: "IsOneTimeUse",
                table: "StickerPackage");

            migrationBuilder.DropColumn(
                name: "IsOneTimeUse",
                table: "GemPackage");

            migrationBuilder.DropColumn(
                name: "IsOneTimeUse",
                table: "CoinPackage");

            migrationBuilder.DropColumn(
                name: "IsOneTimeUse",
                table: "AvatarPackage");

            migrationBuilder.AddColumn<string>(
                name: "PurchaseToken",
                table: "PaymentUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidationCheckDateTime",
                table: "PaymentUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValidationError",
                table: "PaymentUser",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
