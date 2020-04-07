using Microsoft.EntityFrameworkCore.Migrations;

namespace KAndJCore.Data.Migrations
{
    public partial class DocumentExtension : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "Document",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extension",
                table: "Document");


        }
    }
}
