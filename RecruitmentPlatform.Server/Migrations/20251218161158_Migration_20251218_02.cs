using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RecruitmentPlatform.Server.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20251218_02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AdminUsers",
                keyColumn: "Id",
                keyValue: new Guid("9254e1ec-eb31-47e0-98d2-079ac1e73047"));

            migrationBuilder.DeleteData(
                table: "InterviewSettings",
                keyColumn: "Id",
                keyValue: new Guid("3c26c51b-5e60-4b47-95f9-9601ffaf96ef"));

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

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "InterviewSessions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AdminUsers",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "IsActive", "PasswordHash" },
                values: new object[] { new Guid("7896425c-7a44-4474-b796-2deb6b3133cc"), new DateTime(2025, 12, 18, 16, 11, 54, 204, DateTimeKind.Utc).AddTicks(9282), "admin@recruit.com", "System Administrator", true, "Admin@123" });

            migrationBuilder.InsertData(
                table: "InterviewSettings",
                columns: new[] { "Id", "EmailNotificationsEnabled", "PassThresholdPercentage", "QuestionTimeLimitSeconds", "RandomizeQuestions", "TotalQuestionsPerSection", "UpdatedAt" },
                values: new object[] { new Guid("82ad9c18-3cdc-4eaa-b217-924f75ed2ba6"), true, 70, 60, true, 5, new DateTime(2025, 12, 18, 16, 11, 54, 204, DateTimeKind.Utc).AddTicks(9392) });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CorrectAnswerIndex", "Explanation", "Options", "Points", "Section", "Text" },
                values: new object[,]
                {
                    { new Guid("27738197-d0ea-4e42-8f12-a5e0810bf5b0"), 1, "Logs provide diagnostic information for root cause analysis.", "[\"Restart server\",\"Check logs\",\"Scale up instances\",\"Notify all users\"]", 20, 2, "A production API is returning 500 errors sporadically. First step?" },
                    { new Guid("7871d04c-907c-45da-934d-669f2ab7952a"), 1, "Agile values customer collaboration over contract negotiation.", "[\"Detailed documentation\",\"Customer collaboration\",\"Contract negotiation\",\"Rigid processes\"]", 10, 0, "Which of the following is a key principle of Agile development?" },
                    { new Guid("860a9bab-67cf-4831-883d-4dbd229eccf4"), 3, "Multiple strategies can be combined for query optimization.", "[\"Use AsNoTracking()\",\"Increase timeout\",\"Add more indexes\",\"All of the above\"]", 20, 2, "How to optimize a slow database query in Entity Framework?" },
                    { new Guid("b2611f74-7643-48d3-bd1b-50c1c5d43288"), 1, ".NET provides a common runtime for multiple languages.", "[\"Web development\",\"Multiple language interoperability\",\"Database management\",\"Mobile apps\"]", 10, 0, "What is the primary purpose of the .NET Framework?" },
                    { new Guid("d12ffd94-fede-4239-8a00-2723dfeef4a2"), 3, "Behavior differs between value types and reference types.", "[\"No difference\",\"== checks reference, Equals checks value\",\"== checks value, Equals checks reference\",\"Depends on the type\"]", 15, 1, "What is the difference between '==' and 'Equals()' in C#?" },
                    { new Guid("d8914805-fc74-4f71-a4fa-53e62a4b1c92"), 2, "Where() returns IEnumerable and uses deferred execution.", "[\"ToList()\",\"First()\",\"Where()\",\"Count()\"]", 15, 1, "Which LINQ method is used for deferred execution?" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AdminUsers",
                keyColumn: "Id",
                keyValue: new Guid("7896425c-7a44-4474-b796-2deb6b3133cc"));

            migrationBuilder.DeleteData(
                table: "InterviewSettings",
                keyColumn: "Id",
                keyValue: new Guid("82ad9c18-3cdc-4eaa-b217-924f75ed2ba6"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("27738197-d0ea-4e42-8f12-a5e0810bf5b0"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("7871d04c-907c-45da-934d-669f2ab7952a"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("860a9bab-67cf-4831-883d-4dbd229eccf4"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("b2611f74-7643-48d3-bd1b-50c1c5d43288"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("d12ffd94-fede-4239-8a00-2723dfeef4a2"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("d8914805-fc74-4f71-a4fa-53e62a4b1c92"));

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "InterviewSessions");

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
    }
}
