using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlopClub.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedPlayerModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BuyInCount",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyInCount",
                table: "Players");
        }
    }
}
