using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasteFinder.Migrations
{
    public partial class BigFix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contributions_Users_AuthorEmail",
                table: "Contributions");

            migrationBuilder.DropIndex(
                name: "IX_Contributions_AuthorEmail",
                table: "Contributions");

            migrationBuilder.DropColumn(
                name: "AuthorEmail",
                table: "Contributions");

            migrationBuilder.AlterColumn<string>(
                name: "UserEmail",
                table: "Contributions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserEmail1",
                table: "Contributions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contributions_UserEmail1",
                table: "Contributions",
                column: "UserEmail1");

            migrationBuilder.AddForeignKey(
                name: "FK_Contributions_Users_UserEmail1",
                table: "Contributions",
                column: "UserEmail1",
                principalTable: "Users",
                principalColumn: "Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contributions_Users_UserEmail1",
                table: "Contributions");

            migrationBuilder.DropIndex(
                name: "IX_Contributions_UserEmail1",
                table: "Contributions");

            migrationBuilder.DropColumn(
                name: "UserEmail1",
                table: "Contributions");

            migrationBuilder.AlterColumn<string>(
                name: "UserEmail",
                table: "Contributions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AuthorEmail",
                table: "Contributions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Contributions_AuthorEmail",
                table: "Contributions",
                column: "AuthorEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_Contributions_Users_AuthorEmail",
                table: "Contributions",
                column: "AuthorEmail",
                principalTable: "Users",
                principalColumn: "Email");
        }
    }
}
