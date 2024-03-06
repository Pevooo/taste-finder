using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasteFinder.Migrations
{
    public partial class ContributionsAndKeywords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Healthy",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Vegan",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Keyword",
                columns: table => new
                {
                    Text = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keyword", x => x.Text);
                });

            migrationBuilder.CreateTable(
                name: "KeywordPossession",
                columns: table => new
                {
                    PossessionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantEmail = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    KeyText = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeywordPossession", x => x.PossessionId);
                    table.ForeignKey(
                        name: "FK_KeywordPossession_Keyword_KeyText",
                        column: x => x.KeyText,
                        principalTable: "Keyword",
                        principalColumn: "Text",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KeywordPossession_Restaurants_RestaurantEmail",
                        column: x => x.RestaurantEmail,
                        principalTable: "Restaurants",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KeywordPossession_KeyText",
                table: "KeywordPossession",
                column: "KeyText");

            migrationBuilder.CreateIndex(
                name: "IX_KeywordPossession_RestaurantEmail",
                table: "KeywordPossession",
                column: "RestaurantEmail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KeywordPossession");

            migrationBuilder.DropTable(
                name: "Keyword");

            migrationBuilder.DropColumn(
                name: "Healthy",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Vegan",
                table: "Restaurants");
        }
    }
}
