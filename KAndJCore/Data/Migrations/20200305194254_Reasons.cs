using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KAndJCore.Data.Migrations
{
    public partial class Reasons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReasonId",
                table: "Account",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Reason",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reason", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_ReasonId",
                table: "Account",
                column: "ReasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Reason_ReasonId",
                table: "Account",
                column: "ReasonId",
                principalTable: "Reason",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Reason_ReasonId",
                table: "Account");

            migrationBuilder.DropTable(
                name: "Reason");

            migrationBuilder.DropIndex(
                name: "IX_Account_ReasonId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "ReasonId",
                table: "Account");
        }
    }
}
