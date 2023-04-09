using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlopClub.Migrations
{
    /// <inheritdoc />
    public partial class AddedAmmountToCallToPlayerModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isDealer",
                table: "Players",
                newName: "IsDealer");

            migrationBuilder.RenameColumn(
                name: "hasRaised",
                table: "Players",
                newName: "HasRaised");

            migrationBuilder.RenameColumn(
                name: "hasMove",
                table: "Players",
                newName: "HasMove");

            migrationBuilder.RenameColumn(
                name: "hasFolded",
                table: "Players",
                newName: "HasFolded");

            migrationBuilder.RenameColumn(
                name: "hasChecked",
                table: "Players",
                newName: "HasChecked");

            migrationBuilder.RenameColumn(
                name: "hasCalled",
                table: "Players",
                newName: "HasCalled");

            migrationBuilder.AddColumn<decimal>(
                name: "AmmountToCall",
                table: "Players",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmmountToCall",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "IsDealer",
                table: "Players",
                newName: "isDealer");

            migrationBuilder.RenameColumn(
                name: "HasRaised",
                table: "Players",
                newName: "hasRaised");

            migrationBuilder.RenameColumn(
                name: "HasMove",
                table: "Players",
                newName: "hasMove");

            migrationBuilder.RenameColumn(
                name: "HasFolded",
                table: "Players",
                newName: "hasFolded");

            migrationBuilder.RenameColumn(
                name: "HasChecked",
                table: "Players",
                newName: "hasChecked");

            migrationBuilder.RenameColumn(
                name: "HasCalled",
                table: "Players",
                newName: "hasCalled");
        }
    }
}
