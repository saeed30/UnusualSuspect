using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnusualSuspect.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class changeRepetition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<short>(
                name: "RepetitionTypeId",
                table: "GemPackage",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1);

            migrationBuilder.AddColumn<short>(
                name: "RepetitionTypeId",
                table: "CoinPackage",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1);

            migrationBuilder.CreateTable(
                name: "RepetitionType",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ViewOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepetitionType", x => x.Id);
                });
            migrationBuilder.Sql(@"INSERT INTO [RepetitionType]
           ([Id]
           ,[Name]
           ,[Title]
           ,[ViewOrder])
     VALUES
           (1
           ,N'NoLimit'
           ,N'بدون محدودیت'
           ,1)");
            migrationBuilder.CreateIndex(
                name: "IX_GemPackage_RepetitionTypeId",
                table: "GemPackage",
                column: "RepetitionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CoinPackage_RepetitionTypeId",
                table: "CoinPackage",
                column: "RepetitionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoinPackage_RepetitionType_RepetitionTypeId",
                table: "CoinPackage",
                column: "RepetitionTypeId",
                principalTable: "RepetitionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GemPackage_RepetitionType_RepetitionTypeId",
                table: "GemPackage",
                column: "RepetitionTypeId",
                principalTable: "RepetitionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoinPackage_RepetitionType_RepetitionTypeId",
                table: "CoinPackage");

            migrationBuilder.DropForeignKey(
                name: "FK_GemPackage_RepetitionType_RepetitionTypeId",
                table: "GemPackage");

            migrationBuilder.DropTable(
                name: "RepetitionType");

            migrationBuilder.DropIndex(
                name: "IX_GemPackage_RepetitionTypeId",
                table: "GemPackage");

            migrationBuilder.DropIndex(
                name: "IX_CoinPackage_RepetitionTypeId",
                table: "CoinPackage");

            migrationBuilder.DropColumn(
                name: "RepetitionTypeId",
                table: "GemPackage");

            migrationBuilder.DropColumn(
                name: "RepetitionTypeId",
                table: "CoinPackage");

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
        }
    }
}
