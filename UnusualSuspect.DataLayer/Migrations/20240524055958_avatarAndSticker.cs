using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnusualSuspect.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class avatarAndSticker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "StickerPackageId",
                table: "Sticker",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AvatarPackage",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvatarPackage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StickerPackage",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StickerPackage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Avatar",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsFree = table.Column<bool>(type: "bit", nullable: false),
                    AvatarPackageId = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avatar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Avatar_AvatarPackage_AvatarPackageId",
                        column: x => x.AvatarPackageId,
                        principalTable: "AvatarPackage",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sticker_StickerPackageId",
                table: "Sticker",
                column: "StickerPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Avatar_AvatarPackageId",
                table: "Avatar",
                column: "AvatarPackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sticker_StickerPackage_StickerPackageId",
                table: "Sticker",
                column: "StickerPackageId",
                principalTable: "StickerPackage",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sticker_StickerPackage_StickerPackageId",
                table: "Sticker");

            migrationBuilder.DropTable(
                name: "Avatar");

            migrationBuilder.DropTable(
                name: "StickerPackage");

            migrationBuilder.DropTable(
                name: "AvatarPackage");

            migrationBuilder.DropIndex(
                name: "IX_Sticker_StickerPackageId",
                table: "Sticker");

            migrationBuilder.DropColumn(
                name: "StickerPackageId",
                table: "Sticker");
        }
    }
}
