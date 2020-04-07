using Microsoft.EntityFrameworkCore.Migrations;

namespace KAndJCore.Data.Migrations
{
    public partial class ClientNewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CellPhone",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fax",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomePhone",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherPhone",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousAddress",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkEmail",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkPhone",
                table: "Client",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CellPhone",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Fax",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "HomePhone",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "OtherPhone",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "PreviousAddress",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "WorkEmail",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "WorkPhone",
                table: "Client");
        }
    }
}
