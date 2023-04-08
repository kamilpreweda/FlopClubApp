using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlopClub.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsDealerPropToPlayerModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isDealer",
                table: "Players",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDealer",
                table: "Players");
        }
    }
}
