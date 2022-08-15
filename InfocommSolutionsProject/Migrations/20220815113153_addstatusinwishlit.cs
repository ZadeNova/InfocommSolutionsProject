using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfocommSolutionsProject.Migrations
{
    public partial class addstatusinwishlit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "wishLists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "wishLists");
        }
    }
}
