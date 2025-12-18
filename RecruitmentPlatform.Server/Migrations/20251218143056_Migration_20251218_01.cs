using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RecruitmentPlatform.Server.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20251218_01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("1032c4e5-b9ac-496d-adeb-977c130ad5d3"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("35def467-38e9-4773-8c04-0474eac10975"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("9bf6133d-0f16-4652-b430-d62db2ef561e"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("acab7799-7b11-4b0a-82d4-42337d0a3e16"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("ba7c3b9a-62c2-437c-b412-0f8b915f7c7c"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("cd996a14-5ece-4516-92ea-de70382ecb6b"));

            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InterviewSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PassThresholdPercentage = table.Column<int>(type: "int", nullable: false),
                    QuestionTimeLimitSeconds = table.Column<int>(type: "int", nullable: false),
                    TotalQuestionsPerSection = table.Column<int>(type: "int", nullable: false),
                    RandomizeQuestions = table.Column<bool>(type: "bit", nullable: false),
                    EmailNotificationsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewSettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AdminUsers",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "IsActive", "PasswordHash" },
                values: new object[] { new Guid("9254e1ec-eb31-47e0-98d2-079ac1e73047"), new DateTime(2025, 12, 18, 14, 30, 51, 653, DateTimeKind.Utc).AddTicks(4576), "admin@recruit.com", "System Administrator", true, "Admin@123" });

            migrationBuilder.InsertData(
                table: "InterviewSettings",
                columns: new[] { "Id", "EmailNotificationsEnabled", "PassThresholdPercentage", "QuestionTimeLimitSeconds", "RandomizeQuestions", "TotalQuestionsPerSection", "UpdatedAt" },
                values: new object[] { new Guid("3c26c51b-5e60-4b47-95f9-9601ffaf96ef"), true, 70, 60, true, 5, new DateTime(2025, 12, 18, 14, 30, 51, 653, DateTimeKind.Utc).AddTicks(4639) });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CorrectAnswerIndex", "Explanation", "Options", "Points", "Section", "Text" },
                values: new object[,]
                {
                    { new Guid("2eda02b9-427b-4bd3-aaac-fae5e27c4df0"), 1, ".NET provides a common runtime for multiple languages.", "[\"Web development\",\"Multiple language interoperability\",\"Database management\",\"Mobile apps\"]", 10, 0, "What is the primary purpose of the .NET Framework?" },
                    { new Guid("54b02d99-b484-4257-bab3-ada5eee2373d"), 3, "Multiple strategies can be combined for query optimization.", "[\"Use AsNoTracking()\",\"Increase timeout\",\"Add more indexes\",\"All of the above\"]", 20, 2, "How to optimize a slow database query in Entity Framework?" },
                    { new Guid("ab5e4210-6794-4a33-8cb0-a8d810ca005a"), 1, "Agile values customer collaboration over contract negotiation.", "[\"Detailed documentation\",\"Customer collaboration\",\"Contract negotiation\",\"Rigid processes\"]", 10, 0, "Which of the following is a key principle of Agile development?" },
                    { new Guid("afd3c046-06c5-46ff-883d-4f604adc5f3d"), 1, "Logs provide diagnostic information for root cause analysis.", "[\"Restart server\",\"Check logs\",\"Scale up instances\",\"Notify all users\"]", 20, 2, "A production API is returning 500 errors sporadically. First step?" },
                    { new Guid("c0969af4-bc60-4858-9510-c4a621852d43"), 3, "Behavior differs between value types and reference types.", "[\"No difference\",\"== checks reference, Equals checks value\",\"== checks value, Equals checks reference\",\"Depends on the type\"]", 15, 1, "What is the difference between '==' and 'Equals()' in C#?" },
                    { new Guid("ca383a44-f5a6-44e3-adc0-052db1a17d88"), 2, "Where() returns IEnumerable and uses deferred execution.", "[\"ToList()\",\"First()\",\"Where()\",\"Count()\"]", 15, 1, "Which LINQ method is used for deferred execution?" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUsers");

            migrationBuilder.DropTable(
                name: "InterviewSettings");

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("2eda02b9-427b-4bd3-aaac-fae5e27c4df0"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("54b02d99-b484-4257-bab3-ada5eee2373d"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("ab5e4210-6794-4a33-8cb0-a8d810ca005a"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("afd3c046-06c5-46ff-883d-4f604adc5f3d"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("c0969af4-bc60-4858-9510-c4a621852d43"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("ca383a44-f5a6-44e3-adc0-052db1a17d88"));

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CorrectAnswerIndex", "Explanation", "Options", "Points", "Section", "Text" },
                values: new object[,]
                {
                    { new Guid("1032c4e5-b9ac-496d-adeb-977c130ad5d3"), 1, "Agile values customer collaboration over contract negotiation.", "[\"Detailed documentation\",\"Customer collaboration\",\"Contract negotiation\",\"Rigid processes\"]", 10, 0, "Which of the following is a key principle of Agile development?" },
                    { new Guid("35def467-38e9-4773-8c04-0474eac10975"), 3, "Behavior differs between value types and reference types.", "[\"No difference\",\"== checks reference, Equals checks value\",\"== checks value, Equals checks reference\",\"Depends on the type\"]", 15, 1, "What is the difference between '==' and 'Equals()' in C#?" },
                    { new Guid("9bf6133d-0f16-4652-b430-d62db2ef561e"), 2, "Where() returns IEnumerable and uses deferred execution.", "[\"ToList()\",\"First()\",\"Where()\",\"Count()\"]", 15, 1, "Which LINQ method is used for deferred execution?" },
                    { new Guid("acab7799-7b11-4b0a-82d4-42337d0a3e16"), 3, "Multiple strategies can be combined for query optimization.", "[\"Use AsNoTracking()\",\"Increase timeout\",\"Add more indexes\",\"All of the above\"]", 20, 2, "How to optimize a slow database query in Entity Framework?" },
                    { new Guid("ba7c3b9a-62c2-437c-b412-0f8b915f7c7c"), 1, ".NET provides a common runtime for multiple languages.", "[\"Web development\",\"Multiple language interoperability\",\"Database management\",\"Mobile apps\"]", 10, 0, "What is the primary purpose of the .NET Framework?" },
                    { new Guid("cd996a14-5ece-4516-92ea-de70382ecb6b"), 1, "Logs provide diagnostic information for root cause analysis.", "[\"Restart server\",\"Check logs\",\"Scale up instances\",\"Notify all users\"]", 20, 2, "A production API is returning 500 errors sporadically. First step?" }
                });
        }
    }
}
