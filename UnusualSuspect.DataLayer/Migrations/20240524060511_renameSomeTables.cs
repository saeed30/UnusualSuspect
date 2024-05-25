using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnusualSuspect.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class renameSomeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coin");

            migrationBuilder.DropTable(
                name: "Gem");

            migrationBuilder.CreateTable(
                name: "CoinPackageUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    TimeAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CoinPackageId = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinPackageUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoinPackageUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CoinPackageUser_CoinPackage_CoinPackageId",
                        column: x => x.CoinPackageId,
                        principalTable: "CoinPackage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GemPackageUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    TimeAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GemPackageId = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GemPackageUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GemPackageUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GemPackageUser_GemPackage_GemPackageId",
                        column: x => x.GemPackageId,
                        principalTable: "GemPackage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoinPackageUser_CoinPackageId",
                table: "CoinPackageUser",
                column: "CoinPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_CoinPackageUser_UserId",
                table: "CoinPackageUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GemPackageUser_GemPackageId",
                table: "GemPackageUser",
                column: "GemPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_GemPackageUser_UserId",
                table: "GemPackageUser",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoinPackageUser");

            migrationBuilder.DropTable(
                name: "GemPackageUser");

            migrationBuilder.CreateTable(
                name: "Coin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoinPackageId = table.Column<short>(type: "smallint", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    TimeAdded = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coin_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Coin_CoinPackage_CoinPackageId",
                        column: x => x.CoinPackageId,
                        principalTable: "CoinPackage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Gem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GemPackageId = table.Column<short>(type: "smallint", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    TimeAdded = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gem_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Gem_GemPackage_GemPackageId",
                        column: x => x.GemPackageId,
                        principalTable: "GemPackage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coin_CoinPackageId",
                table: "Coin",
                column: "CoinPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Coin_UserId",
                table: "Coin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Gem_GemPackageId",
                table: "Gem",
                column: "GemPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Gem_UserId",
                table: "Gem",
                column: "UserId");
        }
    }
}
