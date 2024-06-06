using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnusualSuspect.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class packagesAndPayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "PriceTypeId",
                table: "StickerPackage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "ViewOrder",
                table: "StickerPackage",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "GemPackageUser",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "GemPackageUser",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<short>(
                name: "PriceTypeId",
                table: "GemPackage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "ViewOrder",
                table: "GemPackage",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "CoinPackageUser",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<short>(
                name: "PriceTypeId",
                table: "CoinPackage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "ViewOrder",
                table: "CoinPackage",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "PriceTypeId",
                table: "AvatarPackage",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "ViewOrder",
                table: "AvatarPackage",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateTable(
                name: "AvatarPackageUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AvatarPackageId = table.Column<short>(type: "smallint", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvatarPackageUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvatarPackageUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AvatarPackageUser_AvatarPackage_AvatarPackageId",
                        column: x => x.AvatarPackageId,
                        principalTable: "AvatarPackage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PriceType",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StickerPackageUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    StickerPackageId = table.Column<short>(type: "smallint", nullable: false),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StickerPackageUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StickerPackageUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StickerPackageUser_StickerPackage_StickerPackageId",
                        column: x => x.StickerPackageId,
                        principalTable: "StickerPackage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CoinUsedUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    TimeAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UsedForPriceTypeId = table.Column<short>(type: "smallint", nullable: false),
                    ReferenceGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinUsedUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoinUsedUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CoinUsedUser_PriceType_UsedForPriceTypeId",
                        column: x => x.UsedForPriceTypeId,
                        principalTable: "PriceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GemUsedUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    TimeAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UsedForPriceTypeId = table.Column<short>(type: "smallint", nullable: false),
                    ReferenceGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GemUsedUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GemUsedUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GemUsedUser_PriceType_UsedForPriceTypeId",
                        column: x => x.UsedForPriceTypeId,
                        principalTable: "PriceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    TimeAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PurchaseToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsValid = table.Column<bool>(type: "bit", nullable: true),
                    ValidationCheckDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsedForPriceTypeId = table.Column<short>(type: "smallint", nullable: false),
                    ReferenceGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentUser_PriceType_UsedForPriceTypeId",
                        column: x => x.UsedForPriceTypeId,
                        principalTable: "PriceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StickerPackage_PriceTypeId",
                table: "StickerPackage",
                column: "PriceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GemPackage_PriceTypeId",
                table: "GemPackage",
                column: "PriceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CoinPackage_PriceTypeId",
                table: "CoinPackage",
                column: "PriceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AvatarPackage_PriceTypeId",
                table: "AvatarPackage",
                column: "PriceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AvatarPackageUser_AvatarPackageId",
                table: "AvatarPackageUser",
                column: "AvatarPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_AvatarPackageUser_UserId",
                table: "AvatarPackageUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CoinUsedUser_UsedForPriceTypeId",
                table: "CoinUsedUser",
                column: "UsedForPriceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CoinUsedUser_UserId",
                table: "CoinUsedUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GemUsedUser_UsedForPriceTypeId",
                table: "GemUsedUser",
                column: "UsedForPriceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GemUsedUser_UserId",
                table: "GemUsedUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentUser_UsedForPriceTypeId",
                table: "PaymentUser",
                column: "UsedForPriceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentUser_UserId",
                table: "PaymentUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StickerPackageUser_StickerPackageId",
                table: "StickerPackageUser",
                column: "StickerPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_StickerPackageUser_UserId",
                table: "StickerPackageUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AvatarPackage_PriceType_PriceTypeId",
                table: "AvatarPackage",
                column: "PriceTypeId",
                principalTable: "PriceType",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CoinPackage_PriceType_PriceTypeId",
                table: "CoinPackage",
                column: "PriceTypeId",
                principalTable: "PriceType",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GemPackage_PriceType_PriceTypeId",
                table: "GemPackage",
                column: "PriceTypeId",
                principalTable: "PriceType",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StickerPackage_PriceType_PriceTypeId",
                table: "StickerPackage",
                column: "PriceTypeId",
                principalTable: "PriceType",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvatarPackage_PriceType_PriceTypeId",
                table: "AvatarPackage");

            migrationBuilder.DropForeignKey(
                name: "FK_CoinPackage_PriceType_PriceTypeId",
                table: "CoinPackage");

            migrationBuilder.DropForeignKey(
                name: "FK_GemPackage_PriceType_PriceTypeId",
                table: "GemPackage");

            migrationBuilder.DropForeignKey(
                name: "FK_StickerPackage_PriceType_PriceTypeId",
                table: "StickerPackage");

            migrationBuilder.DropTable(
                name: "AvatarPackageUser");

            migrationBuilder.DropTable(
                name: "CoinUsedUser");

            migrationBuilder.DropTable(
                name: "GemUsedUser");

            migrationBuilder.DropTable(
                name: "PaymentUser");

            migrationBuilder.DropTable(
                name: "StickerPackageUser");

            migrationBuilder.DropTable(
                name: "PriceType");

            migrationBuilder.DropIndex(
                name: "IX_StickerPackage_PriceTypeId",
                table: "StickerPackage");

            migrationBuilder.DropIndex(
                name: "IX_GemPackage_PriceTypeId",
                table: "GemPackage");

            migrationBuilder.DropIndex(
                name: "IX_CoinPackage_PriceTypeId",
                table: "CoinPackage");

            migrationBuilder.DropIndex(
                name: "IX_AvatarPackage_PriceTypeId",
                table: "AvatarPackage");

            migrationBuilder.DropColumn(
                name: "PriceTypeId",
                table: "StickerPackage");

            migrationBuilder.DropColumn(
                name: "ViewOrder",
                table: "StickerPackage");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "GemPackageUser");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "GemPackageUser");

            migrationBuilder.DropColumn(
                name: "PriceTypeId",
                table: "GemPackage");

            migrationBuilder.DropColumn(
                name: "ViewOrder",
                table: "GemPackage");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "CoinPackageUser");

            migrationBuilder.DropColumn(
                name: "PriceTypeId",
                table: "CoinPackage");

            migrationBuilder.DropColumn(
                name: "ViewOrder",
                table: "CoinPackage");

            migrationBuilder.DropColumn(
                name: "PriceTypeId",
                table: "AvatarPackage");

            migrationBuilder.DropColumn(
                name: "ViewOrder",
                table: "AvatarPackage");
        }
    }
}
