using Microsoft.EntityFrameworkCore.Migrations;

namespace my_books.Migrations
{
    public partial class SecondDatabaseMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Dateadded",
                table: "Books",
                newName: "DateAdded");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateAdded",
                table: "Books",
                newName: "Dateadded");
        }
    }
}
