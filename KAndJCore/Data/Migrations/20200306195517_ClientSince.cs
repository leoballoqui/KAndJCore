using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KAndJCore.Data.Migrations
{
    public partial class ClientSince : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "Client");

            migrationBuilder.AddColumn<DateTime>(
                name: "ClientSince",
                table: "Client",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientSince",
                table: "Client");

            migrationBuilder.AddColumn<DateTime>(
                name: "Completed",
                table: "Client",
                type: "datetime2",
                nullable: true);
        }
    }
}
