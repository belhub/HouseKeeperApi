using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseKeeperApi.Migrations
{
    /// <inheritdoc />
    public partial class belongToHouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OwnersProperty",
                table: "Equipments",
                newName: "BelongToHouse");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BelongToHouse",
                table: "Equipments",
                newName: "OwnersProperty");
        }
    }
}
