using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseKeeperApi.Migrations
{
    /// <inheritdoc />
    public partial class transactionDtoUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_SendFromId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_SendToId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "SendToId",
                table: "Transactions",
                newName: "ReceiverId");

            migrationBuilder.RenameColumn(
                name: "SendFromId",
                table: "Transactions",
                newName: "PayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_SendToId",
                table: "Transactions",
                newName: "IX_Transactions_ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_SendFromId",
                table: "Transactions",
                newName: "IX_Transactions_PayerId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "Transactions",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_PayerId",
                table: "Transactions",
                column: "PayerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_ReceiverId",
                table: "Transactions",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_PayerId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_ReceiverId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Transactions",
                newName: "SendToId");

            migrationBuilder.RenameColumn(
                name: "PayerId",
                table: "Transactions",
                newName: "SendFromId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_ReceiverId",
                table: "Transactions",
                newName: "IX_Transactions_SendToId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_PayerId",
                table: "Transactions",
                newName: "IX_Transactions_SendFromId");

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "Transactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_SendFromId",
                table: "Transactions",
                column: "SendFromId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_SendToId",
                table: "Transactions",
                column: "SendToId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
