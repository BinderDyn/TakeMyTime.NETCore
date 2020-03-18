using Microsoft.EntityFrameworkCore.Migrations;

namespace TakeMyTime.DAL.Migrations
{
    public partial class assignment_id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Assignment_Id",
                table: "Entries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Assignment_Id",
                table: "Entries");
        }
    }
}
