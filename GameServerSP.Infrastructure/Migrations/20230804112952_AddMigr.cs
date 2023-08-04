using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameServerSP.Infrastructure.Migrations
{
    public partial class AddMigr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Coins = table.Column<int>(type: "INTEGER", nullable: false),
                    Rolls = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "Coins", "Name", "Rolls" },
                values: new object[] { 1, 10, "Pavel", 10 });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "Coins", "Name", "Rolls" },
                values: new object[] { 2, 15, "Ivan", 15 });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "Coins", "Name", "Rolls" },
                values: new object[] { 3, 20, "Maria", 20 });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "Coins", "Name", "Rolls" },
                values: new object[] { 4, 25, "Nadina", 25 });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "PlayerId" },
                values: new object[] { new Guid("3a82c65a-c527-414f-8be4-54aaac85e7bf"), 2 });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "PlayerId" },
                values: new object[] { new Guid("5dbf2f74-7dba-4555-a321-ab87e9f1db3e"), 3 });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "PlayerId" },
                values: new object[] { new Guid("92847dd1-e313-4ee6-9e24-05b3e61e22af"), 4 });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "PlayerId" },
                values: new object[] { new Guid("95b76c18-3e66-44b0-b692-95e8c0dd3de8"), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_PlayerId",
                table: "Devices",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
