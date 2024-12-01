using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseKeeperApi.Migrations
{
    /// <inheritdoc />
    public partial class update_notification_dto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomNotification",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ScheduleNotification",
                table: "Notifications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RoomNotification",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ScheduleNotification",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
