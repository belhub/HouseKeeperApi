using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseKeeperApi.Migrations
{
    /// <inheritdoc />
    public partial class tenant_house_connection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "CreationDate",
                table: "Issues",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateTable(
                name: "HouseTenant",
                columns: table => new
                {
                    HouseId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseTenant", x => new { x.HouseId, x.TenantId });
                    table.ForeignKey(
                        name: "FK_HouseTenant_Houses_HouseId",
                        column: x => x.HouseId,
                        principalTable: "Houses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HouseTenant_Users_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HouseTenant_TenantId",
                table: "HouseTenant",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HouseTenant");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Issues");
        }
    }
}
