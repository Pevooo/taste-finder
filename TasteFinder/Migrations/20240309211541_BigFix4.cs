﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasteFinder.Migrations
{
    public partial class BigFix4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
