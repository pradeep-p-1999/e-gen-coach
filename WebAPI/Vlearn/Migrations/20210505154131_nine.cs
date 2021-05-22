using Microsoft.EntityFrameworkCore.Migrations;

namespace VShop.Migrations
{
    public partial class nine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserModelUserName",
                table: "Courses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApplicationUserModels",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserModels", x => x.UserName);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_ApplicationUserModelUserName",
                table: "Courses",
                column: "ApplicationUserModelUserName");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_ApplicationUserModels_ApplicationUserModelUserName",
                table: "Courses",
                column: "ApplicationUserModelUserName",
                principalTable: "ApplicationUserModels",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_ApplicationUserModels_ApplicationUserModelUserName",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "ApplicationUserModels");

            migrationBuilder.DropIndex(
                name: "IX_Courses_ApplicationUserModelUserName",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ApplicationUserModelUserName",
                table: "Courses");
        }
    }
}
