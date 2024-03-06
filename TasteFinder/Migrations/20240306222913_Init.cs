using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasteFinder.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Keywords",
                columns: table => new
                {
                    Text = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keywords", x => x.Text);
                });

            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    OpenTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    CloseTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    OpenAir = table.Column<bool>(type: "bit", nullable: false),
                    KidsArea = table.Column<bool>(type: "bit", nullable: false),
                    Seats = table.Column<bool>(type: "bit", nullable: false),
                    Food = table.Column<bool>(type: "bit", nullable: false),
                    Drinks = table.Column<bool>(type: "bit", nullable: false),
                    Desserts = table.Column<bool>(type: "bit", nullable: false),
                    Delivery = table.Column<bool>(type: "bit", nullable: false),
                    Buffet = table.Column<bool>(type: "bit", nullable: false),
                    Vegan = table.Column<bool>(type: "bit", nullable: false),
                    Healthy = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contribution = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    ProfilePicture = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    PhotoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    PhotoData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    OwnerEmail = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.PhotoId);
                    table.ForeignKey(
                        name: "FK_Photos_Restaurants_OwnerEmail",
                        column: x => x.OwnerEmail,
                        principalTable: "Restaurants",
                        principalColumn: "Email");
                });

            migrationBuilder.CreateTable(
                name: "Possessions",
                columns: table => new
                {
                    PossessionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantEmail = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    KeyText = table.Column<string>(type: "nvarchar(450)", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Stars = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Contribution = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorEmail = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_AuthorEmail",
                        column: x => x.AuthorEmail,
                        principalTable: "Users",
                        principalColumn: "Email");
                });

            migrationBuilder.CreateTable(
                name: "Contributions",
                columns: table => new
                {
                    ContributionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewId = table.Column<int>(type: "int", nullable: true),
                    AuthorEmail = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contributions", x => x.ContributionId);
                    table.ForeignKey(
                        name: "FK_Contributions_Reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Reviews",
                        principalColumn: "ReviewId");
                    table.ForeignKey(
                        name: "FK_Contributions_Users_AuthorEmail",
                        column: x => x.AuthorEmail,
                        principalTable: "Users",
                        principalColumn: "Email");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contributions_AuthorEmail",
                table: "Contributions",
                column: "AuthorEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Contributions_ReviewId",
                table: "Contributions",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_OwnerEmail",
                table: "Photos",
                column: "OwnerEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Possessions_KeyText",
                table: "Possessions",
                column: "KeyText");

            migrationBuilder.CreateIndex(
                name: "IX_Possessions_RestaurantEmail",
                table: "Possessions",
                column: "RestaurantEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_AuthorEmail",
                table: "Reviews",
                column: "AuthorEmail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contributions");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "Possessions");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Keywords");

            migrationBuilder.DropTable(
                name: "Restaurants");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
