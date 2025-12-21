using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Basics.Migrations
{
    /// <inheritdoc />
    public partial class ProjectFullSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Trainers",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Trainers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "TrainerAvailabilities",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "TrainerAvailabilities",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "TrainerAvailabilities",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Gyms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    WorkingHours = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gyms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    TrainerID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_Trainers_TrainerID",
                        column: x => x.TrainerID,
                        principalTable: "Trainers",
                        principalColumn: "TrainerID");
                });

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

            migrationBuilder.InsertData(
                table: "Gyms",
                columns: new[] { "Id", "Address", "Name", "WorkingHours" },
                values: new object[,]
                {
                    { 1, "Serdivan, Sakarya", "Sakarya Merkez Spor Salonu", "07:00 - 23:00" },
                    { 2, "Erenler, Sakarya", "Erenler VIP Gym", "08:00 - 22:00" }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Duration", "Name", "Price", "TrainerID" },
                values: new object[,]
                {
                    { 1, 60, "Fitness", 100m, null },
                    { 2, 45, "Yoga", 150m, null },
                    { 3, 50, "Pilates", 200m, null },
                    { 4, 60, "Crossfit", 180m, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainerAvailabilities_ServiceId",
                table: "TrainerAvailabilities",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_TrainerID",
                table: "Services",
                column: "TrainerID");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainerAvailabilities_Services_ServiceId",
                table: "TrainerAvailabilities",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainerAvailabilities_Services_ServiceId",
                table: "TrainerAvailabilities");

            migrationBuilder.DropTable(
                name: "Gyms");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropIndex(
                name: "IX_TrainerAvailabilities_ServiceId",
                table: "TrainerAvailabilities");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "TrainerAvailabilities");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "TrainerAvailabilities");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "TrainerAvailabilities");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Trainers",
                newName: "FullName");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "d8bf3f37-2a28-479f-8915-48d7768ba795");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                column: "ConcurrencyStamp",
                value: "25bfdc19-574e-43fe-b0f4-974fa58af1a7");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048b0",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "00d9669a-45e3-4e0e-ad67-bde65b621080", "AQAAAAIAAYagAAAAEJM7aR8v0uhuZhePoB4zbn3RMz92RlWm09HZIFoW5vEfvIyPR1onAPYmNN7Oesruaw==", "a3c2cc13-32b3-4ef1-8d4b-f55897cf20bc" });
        }
    }
}
