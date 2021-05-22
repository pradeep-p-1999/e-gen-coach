using Microsoft.EntityFrameworkCore.Migrations;

namespace VShop.Migrations
{
    public partial class sixteen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserCourse_AspNetUsers_applicationUserId",
                table: "ApplicationUserCourse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserCourse",
                table: "ApplicationUserCourse");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserCourse_applicationUserId",
                table: "ApplicationUserCourse");

            migrationBuilder.RenameColumn(
                name: "applicationUserId",
                table: "ApplicationUserCourse",
                newName: "ApplicationUsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserCourse",
                table: "ApplicationUserCourse",
                columns: new[] { "ApplicationUsersId", "CoursesId" });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserCourse_CoursesId",
                table: "ApplicationUserCourse",
                column: "CoursesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserCourse_AspNetUsers_ApplicationUsersId",
                table: "ApplicationUserCourse",
                column: "ApplicationUsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserCourse_AspNetUsers_ApplicationUsersId",
                table: "ApplicationUserCourse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserCourse",
                table: "ApplicationUserCourse");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUserCourse_CoursesId",
                table: "ApplicationUserCourse");

            migrationBuilder.RenameColumn(
                name: "ApplicationUsersId",
                table: "ApplicationUserCourse",
                newName: "applicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserCourse",
                table: "ApplicationUserCourse",
                columns: new[] { "CoursesId", "applicationUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserCourse_applicationUserId",
                table: "ApplicationUserCourse",
                column: "applicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserCourse_AspNetUsers_applicationUserId",
                table: "ApplicationUserCourse",
                column: "applicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
