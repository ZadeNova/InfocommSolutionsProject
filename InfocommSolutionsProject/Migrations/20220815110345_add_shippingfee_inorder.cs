using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfocommSolutionsProject.Migrations
{
    public partial class add_shippingfee_inorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FreeShipping",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FreeShipping",
                table: "Orders");
        }
    }
}
