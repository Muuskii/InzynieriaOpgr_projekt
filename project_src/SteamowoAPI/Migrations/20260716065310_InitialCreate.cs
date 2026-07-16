using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SteamowoAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tytul = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Producent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gatunek = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cena = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UrlOkladki = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataWydania = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataRejestracji = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SaldoPortfela = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: true),
                    Typ = table.Column<int>(type: "int", nullable: false),
                    Kwota = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    DataZakupu = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CenaZakupu = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CzasGry = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGames_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGames_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "Cena", "DataWydania", "Gatunek", "Opis", "Producent", "Tytul", "UrlOkladki" },
                values: new object[,]
                {
                    { 1, 49.99m, new DateTime(2015, 5, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "RPG", "Klasyk RPG od CD Projekt Red.", "CD Projekt Red", "Wiedzmin 3: Dziki Gon", "https://picsum.photos/seed/steamowo1/300/400" },
                    { 2, 0m, new DateTime(2023, 9, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "FPS", "Kultowa strzelanka druzynowa.", "Valve", "Counter Strike 2", "https://picsum.photos/seed/steamowo2/300/400" },
                    { 3, 29.99m, new DateTime(2016, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Symulator", "Relaksujaca gra farmerska.", "ConcernedApe", "Stardew Valley", "https://picsum.photos/seed/steamowo3/300/400" },
                    { 4, 199.99m, new DateTime(2022, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "RPG/Akcja", "Trudne souls-like w otwartym swiecie.", "FromSoftware", "Elden Ring", "https://picsum.photos/seed/steamowo4/300/400" },
                    { 5, 149.99m, new DateTime(2020, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "RPG/Akcja", "Futurystyczne RPG akcji w Night City.", "CD Projekt Red", "Cyberpunk 2077", "https://picsum.photos/seed/steamowo5/300/400" },
                    { 6, 99.99m, new DateTime(2013, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Akcja", "Otwarty swiat przestepczosci w Los Santos.", "Rockstar Games", "Grand Theft Auto V", "https://picsum.photos/seed/steamowo6/300/400" },
                    { 7, 89.99m, new DateTime(2011, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sandbox", "Buduj i eksploruj w swiecie z klockow.", "Mojang", "Minecraft", "https://picsum.photos/seed/steamowo7/300/400" },
                    { 8, 0m, new DateTime(2013, 7, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "MOBA", "Darmowa gra MOBA od Valve.", "Valve", "Dota 2", "https://picsum.photos/seed/steamowo8/300/400" },
                    { 9, 39.99m, new DateTime(2017, 2, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Platformowa", "Klimatyczna metroidvania w podziemnym swiecie.", "Team Cherry", "Hollow Knight", "https://picsum.photos/seed/steamowo9/300/400" },
                    { 10, 249.99m, new DateTime(2023, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sportowa", "Symulator pilki noznej.", "EA Sports", "FIFA 24", "https://picsum.photos/seed/steamowo10/300/400" },
                    { 11, 219.99m, new DateTime(2023, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "RPG", "Rozbudowane RPG oparte na D&D.", "Larian Studios", "Baldur's Gate 3", "https://picsum.photos/seed/steamowo11/300/400" },
                    { 12, 0m, new DateTime(2015, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sportowa/Akcja", "Pilka nozna samochodami.", "Psyonix", "Rocket League", "https://picsum.photos/seed/steamowo12/300/400" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_GameId",
                table: "Transactions",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGames_GameId",
                table: "UserGames",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGames_UserId_GameId",
                table: "UserGames",
                columns: new[] { "UserId", "GameId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "UserGames");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
