﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlopClub.Migrations
{
    /// <inheritdoc />
    public partial class ChangedUserLobbyRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Lobbies_LobbyId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_LobbyId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LobbyId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "LobbyUser",
                columns: table => new
                {
                    LobbiesId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LobbyUser", x => new { x.LobbiesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_LobbyUser_Lobbies_LobbiesId",
                        column: x => x.LobbiesId,
                        principalTable: "Lobbies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LobbyUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LobbyUser_UsersId",
                table: "LobbyUser",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LobbyUser");

            migrationBuilder.AddColumn<int>(
                name: "LobbyId",
                table: "Users",
                type: "int",
                nullable: true);

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
    }
}
