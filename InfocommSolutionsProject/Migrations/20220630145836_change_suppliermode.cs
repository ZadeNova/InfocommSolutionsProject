using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfocommSolutionsProject.Migrations
{
    public partial class change_suppliermode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupplierCategory",
                table: "Suppliers");

            migrationBuilder.AddColumn<Guid>(
                name: "SupplierCategoryCategoryId",
                table: "Suppliers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_SupplierCategoryCategoryId",
                table: "Suppliers",
                column: "SupplierCategoryCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_Categories_SupplierCategoryCategoryId",
                table: "Suppliers",
                column: "SupplierCategoryCategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_Categories_SupplierCategoryCategoryId",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_SupplierCategoryCategoryId",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "SupplierCategoryCategoryId",
                table: "Suppliers");

            migrationBuilder.AddColumn<string>(
                name: "SupplierCategory",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
