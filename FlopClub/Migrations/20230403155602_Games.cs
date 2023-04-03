using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlopClub.Migrations
{
    /// <inheritdoc />
    public partial class Games : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "Cards",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GameId1",
                table: "Cards",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Pot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SmallBlindValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BigBlindValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Dealer = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_GameId",
                table: "Players",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_GameId",
                table: "Cards",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_GameId1",
                table: "Cards",
                column: "GameId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Games_GameId",
                table: "Cards",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Games_GameId1",
                table: "Cards",
                column: "GameId1",
                principalTable: "Games",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Games_GameId",
                table: "Players",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Games_GameId",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Games_GameId1",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Games_GameId",
                table: "Players");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Players_GameId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Cards_GameId",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_GameId1",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "GameId1",
                table: "Cards");
        }
    }
}
