using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KAndJCore.Data.Migrations
{
    public partial class AddAccountStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dispute_Claim_ClaimId1",
                table: "Dispute");

            migrationBuilder.DropIndex(
                name: "IX_Dispute_ClaimId1",
                table: "Dispute");

            migrationBuilder.DropColumn(
                name: "ClaimId1",
                table: "Dispute");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClaimId",
                table: "Dispute",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "Dispute",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Account",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Dispute_ClaimId",
                table: "Dispute",
                column: "ClaimId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dispute_Claim_ClaimId",
                table: "Dispute",
                column: "ClaimId",
                principalTable: "Claim",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dispute_Claim_ClaimId",
                table: "Dispute");

            migrationBuilder.DropIndex(
                name: "IX_Dispute_ClaimId",
                table: "Dispute");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Account");

            migrationBuilder.AlterColumn<string>(
                name: "ClaimId",
                table: "Dispute",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "Dispute",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "ClaimId1",
                table: "Dispute",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dispute_ClaimId1",
                table: "Dispute",
                column: "ClaimId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Dispute_Claim_ClaimId1",
                table: "Dispute",
                column: "ClaimId1",
                principalTable: "Claim",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
