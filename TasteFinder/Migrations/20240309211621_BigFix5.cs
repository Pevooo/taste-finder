using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasteFinder.Migrations
{
    public partial class BigFix5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contributions_Users_UserEmail",
                table: "Contributions");

            migrationBuilder.RenameColumn(
                name: "UserEmail",
                table: "Contributions",
                newName: "AuthorEmail");

            migrationBuilder.RenameIndex(
                name: "IX_Contributions_UserEmail",
                table: "Contributions",
                newName: "IX_Contributions_AuthorEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_Contributions_Users_AuthorEmail",
                table: "Contributions",
                column: "AuthorEmail",
                principalTable: "Users",
                principalColumn: "Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contributions_Users_AuthorEmail",
                table: "Contributions");

            migrationBuilder.RenameColumn(
                name: "AuthorEmail",
                table: "Contributions",
                newName: "UserEmail");

            migrationBuilder.RenameIndex(
                name: "IX_Contributions_AuthorEmail",
                table: "Contributions",
                newName: "IX_Contributions_UserEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_Contributions_Users_UserEmail",
                table: "Contributions",
                column: "UserEmail",
                principalTable: "Users",
                principalColumn: "Email");
        }
    }
}
