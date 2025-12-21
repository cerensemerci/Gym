using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basics.Migrations
{
    /// <inheritdoc />
    public partial class FinalSchemaFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "aa67b9c0-f7cb-4ff2-8372-e527d3f96ef4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                column: "ConcurrencyStamp",
                value: "a98f027f-e6c9-4e41-8d54-e561c2558baf");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048b0",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f1b9d891-f7c6-409e-87d8-252693b5c08b", "AQAAAAIAAYagAAAAEB8yb8cDASYEhiZ8Z21MdxwL8vJEHGCasq3Sq+CJ3oTsjC18V8zZ/N7NKbUHrbzflg==", "d881f491-f677-4774-8755-057fe607c3b9" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "bb09cb54-e174-4602-b0a0-00851da0a7f2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                column: "ConcurrencyStamp",
                value: "d79456e4-f634-4c95-ac9f-6892c5d20c0f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048b0",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e010adf3-0ff6-4873-8daa-a3f827878ac3", "AQAAAAIAAYagAAAAEBIn2rYxn8AFe2nQeQS2ZZGuSIGWD1PZBPat8vyd9D1+Cpb5tiO2lNQMV06J3TSBHg==", "a8ccc913-471a-4eae-9c3a-ca18ee7ef09f" });
        }
    }
}
