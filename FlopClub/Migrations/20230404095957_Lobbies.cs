using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlopClub.Migrations
{
    /// <inheritdoc />
    public partial class Lobbies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LobbyId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActivePlayers",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxPlayers",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Lobbies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lobbies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lobbies_Games_Id",
                        column: x => x.Id,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_LobbyId",
                table: "Users",
                column: "LobbyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Lobbies_LobbyId",
                table: "Users",
                column: "LobbyId",
                principalTable: "Lobbies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Lobbies_LobbyId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Lobbies");

            migrationBuilder.DropIndex(
                name: "IX_Users_LobbyId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LobbyId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ActivePlayers",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "MaxPlayers",
                table: "Games");
        }
    }
}
