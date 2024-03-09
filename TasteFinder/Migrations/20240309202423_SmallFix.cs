using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasteFinder.Migrations
{
    public partial class SmallFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Keyword_Restaurants_RestaurantEmail",
                table: "Keyword");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Keyword",
                table: "Keyword");

            migrationBuilder.RenameTable(
                name: "Keyword",
                newName: "Possessions");

            migrationBuilder.RenameIndex(
                name: "IX_Keyword_RestaurantEmail",
                table: "Possessions",
                newName: "IX_Possessions_RestaurantEmail");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Possessions",
                table: "Possessions",
                column: "PossessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Possessions_Restaurants_RestaurantEmail",
                table: "Possessions",
                column: "RestaurantEmail",
                principalTable: "Restaurants",
                principalColumn: "Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Possessions_Restaurants_RestaurantEmail",
                table: "Possessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Possessions",
                table: "Possessions");

            migrationBuilder.RenameTable(
                name: "Possessions",
                newName: "Keyword");

            migrationBuilder.RenameIndex(
                name: "IX_Possessions_RestaurantEmail",
                table: "Keyword",
                newName: "IX_Keyword_RestaurantEmail");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Keyword",
                table: "Keyword",
                column: "PossessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Keyword_Restaurants_RestaurantEmail",
                table: "Keyword",
                column: "RestaurantEmail",
                principalTable: "Restaurants",
                principalColumn: "Email");
        }
    }
}
