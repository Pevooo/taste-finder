using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasteFinder.Migrations
{
    public partial class RemoveUnimportantTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Possessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Keywords",
                table: "Keywords");

            migrationBuilder.RenameTable(
                name: "Keywords",
                newName: "Keyword");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Keyword",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "PossessionId",
                table: "Keyword",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "RestaurantEmail",
                table: "Keyword",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Keyword",
                table: "Keyword",
                column: "PossessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Keyword_RestaurantEmail",
                table: "Keyword",
                column: "RestaurantEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_Keyword_Restaurants_RestaurantEmail",
                table: "Keyword",
                column: "RestaurantEmail",
                principalTable: "Restaurants",
                principalColumn: "Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Keyword_Restaurants_RestaurantEmail",
                table: "Keyword");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Keyword",
                table: "Keyword");

            migrationBuilder.DropIndex(
                name: "IX_Keyword_RestaurantEmail",
                table: "Keyword");

            migrationBuilder.DropColumn(
                name: "PossessionId",
                table: "Keyword");

            migrationBuilder.DropColumn(
                name: "RestaurantEmail",
                table: "Keyword");

            migrationBuilder.RenameTable(
                name: "Keyword",
                newName: "Keywords");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Keywords",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Keywords",
                table: "Keywords",
                column: "Text");

            migrationBuilder.CreateTable(
                name: "Possessions",
                columns: table => new
                {
                    PossessionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KeyText = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RestaurantEmail = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Possessions", x => x.PossessionId);
                    table.ForeignKey(
                        name: "FK_Possessions_Keywords_KeyText",
                        column: x => x.KeyText,
                        principalTable: "Keywords",
                        principalColumn: "Text");
                    table.ForeignKey(
                        name: "FK_Possessions_Restaurants_RestaurantEmail",
                        column: x => x.RestaurantEmail,
                        principalTable: "Restaurants",
                        principalColumn: "Email");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Possessions_KeyText",
                table: "Possessions",
                column: "KeyText");

            migrationBuilder.CreateIndex(
                name: "IX_Possessions_RestaurantEmail",
                table: "Possessions",
                column: "RestaurantEmail");
        }
    }
}
