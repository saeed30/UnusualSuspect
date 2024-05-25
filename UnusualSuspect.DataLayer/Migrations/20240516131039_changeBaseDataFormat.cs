using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UnusualSuspect.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class changeBaseDataFormat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DeleteData(
            //    table: "GameStatus",
            //    keyColumn: "Id",
            //    keyValue: (short)1);

            //migrationBuilder.DeleteData(
            //    table: "GameStatus",
            //    keyColumn: "Id",
            //    keyValue: (short)2);

            //migrationBuilder.DeleteData(
            //    table: "GameStatus",
            //    keyColumn: "Id",
            //    keyValue: (short)3);

            //migrationBuilder.DeleteData(
            //    table: "GameStatus",
            //    keyColumn: "Id",
            //    keyValue: (short)4);

            //migrationBuilder.DeleteData(
            //    table: "GameStatus",
            //    keyColumn: "Id",
            //    keyValue: (short)5);

            //migrationBuilder.DeleteData(
            //    table: "GameStatus",
            //    keyColumn: "Id",
            //    keyValue: (short)6);

            //migrationBuilder.DeleteData(
            //    table: "PreGameGroupStatus",
            //    keyColumn: "Id",
            //    keyValue: (short)1);

            //migrationBuilder.DeleteData(
            //    table: "PreGameGroupStatus",
            //    keyColumn: "Id",
            //    keyValue: (short)2);

            //migrationBuilder.DeleteData(
            //    table: "PreGameGroupStatus",
            //    keyColumn: "Id",
            //    keyValue: (short)3);

            //migrationBuilder.DeleteData(
            //    table: "ReadyToGameStatus",
            //    keyColumn: "Id",
            //    keyValue: (short)1);

            //migrationBuilder.DeleteData(
            //    table: "ReadyToGameStatus",
            //    keyColumn: "Id",
            //    keyValue: (short)2);

            //migrationBuilder.DeleteData(
            //    table: "ReadyToGameStatus",
            //    keyColumn: "Id",
            //    keyValue: (short)3);

            //migrationBuilder.DeleteData(
            //    table: "RoleCard",
            //    keyColumn: "Id",
            //    keyValue: (short)1);

            //migrationBuilder.DeleteData(
            //    table: "RoleCard",
            //    keyColumn: "Id",
            //    keyValue: (short)2);

            //migrationBuilder.DeleteData(
            //    table: "RoleCard",
            //    keyColumn: "Id",
            //    keyValue: (short)3);

            //migrationBuilder.DeleteData(
            //    table: "RoleCard",
            //    keyColumn: "Id",
            //    keyValue: (short)4);

            //migrationBuilder.DeleteData(
            //    table: "ScoreType",
            //    keyColumn: "Id",
            //    keyValue: (short)1);

            //migrationBuilder.DeleteData(
            //    table: "ScoreType",
            //    keyColumn: "Id",
            //    keyValue: (short)2);

            //migrationBuilder.DeleteData(
            //    table: "ScoreType",
            //    keyColumn: "Id",
            //    keyValue: (short)3);

            //migrationBuilder.DeleteData(
            //    table: "ScoreType",
            //    keyColumn: "Id",
            //    keyValue: (short)4);

            //migrationBuilder.DeleteData(
            //    table: "ScoreType",
            //    keyColumn: "Id",
            //    keyValue: (short)5);

            //migrationBuilder.DeleteData(
            //    table: "ScoreType",
            //    keyColumn: "Id",
            //    keyValue: (short)6);

            //migrationBuilder.DeleteData(
            //    table: "ScoreType",
            //    keyColumn: "Id",
            //    keyValue: (short)7);

            //migrationBuilder.DeleteData(
            //    table: "ScoreType",
            //    keyColumn: "Id",
            //    keyValue: (short)8);

            //migrationBuilder.DeleteData(
            //    table: "SmsSendingStatus",
            //    keyColumn: "Id",
            //    keyValue: (short)1);

            //migrationBuilder.DeleteData(
            //    table: "SmsSendingStatus",
            //    keyColumn: "Id",
            //    keyValue: (short)2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.InsertData(
            //    table: "GameStatus",
            //    columns: new[] { "Id", "Name", "Title" },
            //    values: new object[,]
            //    {
            //        { (short)1, "WaitingForPlayers", "در انتظار بازیکنان جهت شروع بازی" },
            //        { (short)2, "Talking", "صحبت های قبل از انتخاب" },
            //        { (short)3, "WaitingForMainDetectiveToChoose", "در انتظار کارآگاه ستاره جهت انتخاب" },
            //        { (short)4, "FinishedAndWonTheGame", "پایان با پیروزی" },
            //        { (short)5, "FinishedAndLostTheGame", "پایان با شکست" },
            //        { (short)6, "WaitingForWitnessToAnswer", "در انتظار شاهد جهت پاسخ به سوال" }
            //    });

            //migrationBuilder.InsertData(
            //    table: "PreGameGroupStatus",
            //    columns: new[] { "Id", "Name", "Title" },
            //    values: new object[,]
            //    {
            //        { (short)1, "NotReady", "قبل از آمادگی جهت بازی" },
            //        { (short)2, "Ready", "آماده جهت بازی" },
            //        { (short)3, "InGame", "در حال بازی" }
            //    });

            //migrationBuilder.InsertData(
            //    table: "ReadyToGameStatus",
            //    columns: new[] { "Id", "Name", "Title" },
            //    values: new object[,]
            //    {
            //        { (short)1, "NotReady", "عدم آمادگی" },
            //        { (short)2, "Notified", "اطلاع رسانی شده جهت تایید آمادگی" },
            //        { (short)3, "Ready", "آماده جهت بازی" }
            //    });

            //migrationBuilder.InsertData(
            //    table: "RoleCard",
            //    columns: new[] { "Id", "ImageUrl", "IsActive", "Name", "Title" },
            //    values: new object[,]
            //    {
            //        { (short)1, "", true, "Detective", "کارآگاه" },
            //        { (short)2, "", true, "MainDetective", "کارآگاه ستاره دار" },
            //        { (short)3, "", true, "Witness", "شاهد" },
            //        { (short)4, "", true, "Accomplice", "شریک جرم" }
            //    });

            //migrationBuilder.InsertData(
            //    table: "ScoreType",
            //    columns: new[] { "Id", "Name", "Title" },
            //    values: new object[,]
            //    {
            //        { (short)1, "GameWonAsDetective", "برد با نقش کارآگاه" },
            //        { (short)2, "GameWonAsMainDetective", "برد با نقش کارآگاه ستاره دار" },
            //        { (short)3, "GameWonAsWitness", "برد با نقش شاهد" },
            //        { (short)4, "GameWonAsAccomplice", "برد با نقش شریک جرم" },
            //        { (short)5, "GameLostAsDetective", "باخت با نقش کارآگاه" },
            //        { (short)6, "GameLostAsMainDetective", "باخت با نقش کارآگاه ستاره دار" },
            //        { (short)7, "GameLostAsWitness", "باخت با نقش شاهد" },
            //        { (short)8, "GameLostAsAccomplice", "باخت با نقش شریک جرم" }
            //    });

            //migrationBuilder.InsertData(
            //    table: "SmsSendingStatus",
            //    columns: new[] { "Id", "Name", "Title" },
            //    values: new object[,]
            //    {
            //        { (short)1, "Success", "ارسال موفق" },
            //        { (short)2, "Failed", "ارسال نا موفق" }
            //    });
        }
    }
}
