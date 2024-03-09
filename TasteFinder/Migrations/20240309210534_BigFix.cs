using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasteFinder.Migrations
{
    public partial class BigFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contributions_Users_AuthorEmail",
                table: "Contributions");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorEmail",
                table: "Contributions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Contributions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contributions_UserEmail",
                table: "Contributions",
                column: "UserEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_Contributions_Users_AuthorEmail",
                table: "Contributions",
                column: "AuthorEmail",
                principalTable: "Users",
                principalColumn: "Email");

            migrationBuilder.AddForeignKey(
                name: "FK_Contributions_Users_UserEmail",
                table: "Contributions",
                column: "UserEmail",
                principalTable: "Users",
                principalColumn: "Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contributions_Users_AuthorEmail",
                table: "Contributions");

            migrationBuilder.DropForeignKey(
                name: "FK_Contributions_Users_UserEmail",
                table: "Contributions");

            migrationBuilder.DropIndex(
                name: "IX_Contributions_UserEmail",
                table: "Contributions");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Contributions");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorEmail",
                table: "Contributions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Contributions_Users_AuthorEmail",
                table: "Contributions",
                column: "AuthorEmail",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
