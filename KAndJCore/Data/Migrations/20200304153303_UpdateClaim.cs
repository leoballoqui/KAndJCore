using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KAndJCore.Data.Migrations
{
    public partial class UpdateClaim : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "Dispute",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<DateTime>(
                name: "NextRevision",
                table: "Claim",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Claim",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dispute_AccountId",
                table: "Dispute",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dispute_Account_AccountId",
                table: "Dispute",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dispute_Account_AccountId",
                table: "Dispute");

            migrationBuilder.DropIndex(
                name: "IX_Dispute_AccountId",
                table: "Dispute");

            migrationBuilder.DropColumn(
                name: "NextRevision",
                table: "Claim");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Claim");

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "Dispute",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }
    }
}
