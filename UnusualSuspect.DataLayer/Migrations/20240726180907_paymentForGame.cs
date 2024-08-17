using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnusualSuspect.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class paymentForGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CoinCostToEnterPreGame",
                table: "SoftSetting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CoinAmountUsedToEnter",
                table: "Participate",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "Participate",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()");

            migrationBuilder.AddColumn<int>(
                name: "CoinAmountUsedToEnter",
                table: "JoinedPreGame",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "JoinedPreGame",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()");
        }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoinCostToEnterPreGame",
                table: "SoftSetting");

            migrationBuilder.DropColumn(
                name: "CoinAmountUsedToEnter",
                table: "Participate");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Participate");

            migrationBuilder.DropColumn(
                name: "CoinAmountUsedToEnter",
                table: "JoinedPreGame");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "JoinedPreGame");
        }
    }
}
