using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestGame.Repository.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lobbies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HostId = table.Column<int>(type: "int", nullable: false),
                    SecondClientId = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HostHealth = table.Column<int>(type: "int", nullable: false, defaultValueSql: "10"),
                    SecondClientHealth = table.Column<int>(type: "int", nullable: false, defaultValueSql: "10"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MovesCount = table.Column<int>(type: "int", nullable: false, defaultValueSql: "0"),
                    WinnerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lobbies_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lobbies_HostId",
                        column: x => x.HostId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lobbies_SecondClientId",
                        column: x => x.SecondClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lobbies_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lobbies_HostId",
                table: "Lobbies",
                column: "HostId");

            migrationBuilder.CreateIndex(
                name: "IX_Lobbies_SecondClientId",
                table: "Lobbies",
                column: "SecondClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Lobbies_WinnerId",
                table: "Lobbies",
                column: "WinnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lobbies");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
