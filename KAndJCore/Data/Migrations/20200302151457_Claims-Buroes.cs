using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KAndJCore.Data.Migrations
{
    public partial class ClaimsBuroes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Document");

            migrationBuilder.AddColumn<string>(
                name: "FileFullName",
                table: "Document",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Buro",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    AddressLine3 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buro", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Template",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Template", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Claim",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: false),
                    BuroId = table.Column<Guid>(nullable: false),
                    TemplateId = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    CurrentIteration = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Claim_Buro_BuroId",
                        column: x => x.BuroId,
                        principalTable: "Buro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Claim_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Claim_Template_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Template",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dispute",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ClaimId = table.Column<string>(nullable: true),
                    AccountId = table.Column<string>(nullable: true),
                    CompleteReason = table.Column<string>(nullable: true),
                    ClaimId1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dispute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dispute_Claim_ClaimId1",
                        column: x => x.ClaimId1,
                        principalTable: "Claim",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Claim_BuroId",
                table: "Claim",
                column: "BuroId");

            migrationBuilder.CreateIndex(
                name: "IX_Claim_ClientId",
                table: "Claim",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Claim_TemplateId",
                table: "Claim",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Dispute_ClaimId1",
                table: "Dispute",
                column: "ClaimId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dispute");

            migrationBuilder.DropTable(
                name: "Claim");

            migrationBuilder.DropTable(
                name: "Buro");

            migrationBuilder.DropTable(
                name: "Template");

            migrationBuilder.DropColumn(
                name: "FileFullName",
                table: "Document");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Document",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
