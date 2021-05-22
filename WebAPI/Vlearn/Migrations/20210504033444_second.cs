using Microsoft.EntityFrameworkCore.Migrations;

namespace VShop.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UplodeDate",
                table: "Courses",
                newName: "UplodedDate");

            migrationBuilder.RenameColumn(
                name: "Size",
                table: "Courses",
                newName: "Duration");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UplodedDate",
                table: "Courses",
                newName: "UplodeDate");

            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "Courses",
                newName: "Size");
        }
    }
}
