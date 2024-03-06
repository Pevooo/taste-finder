using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasteFinder.Migrations
{
    public partial class SmallSmallFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RestaurantEmail",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_RestaurantEmail",
                table: "Reviews",
                column: "RestaurantEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Restaurants_RestaurantEmail",
                table: "Reviews",
                column: "RestaurantEmail",
                principalTable: "Restaurants",
                principalColumn: "Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Restaurants_RestaurantEmail",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_RestaurantEmail",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "RestaurantEmail",
                table: "Reviews");
        }
    }
}
