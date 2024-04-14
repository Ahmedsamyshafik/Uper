using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Driver.Migrations
{
    /// <inheritdoc />
    public partial class edituserName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_trips_AspNetUsers_userID",
                table: "trips");

            migrationBuilder.RenameColumn(
                name: "userID",
                table: "trips",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_trips_userID",
                table: "trips",
                newName: "IX_trips_UserId");

            migrationBuilder.AddColumn<string>(
                name: "PassengerID",
                table: "trips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_trips_AspNetUsers_UserId",
                table: "trips",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_trips_AspNetUsers_UserId",
                table: "trips");

            migrationBuilder.DropColumn(
                name: "PassengerID",
                table: "trips");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "trips",
                newName: "userID");

            migrationBuilder.RenameIndex(
                name: "IX_trips_UserId",
                table: "trips",
                newName: "IX_trips_userID");

            migrationBuilder.AddForeignKey(
                name: "FK_trips_AspNetUsers_userID",
                table: "trips",
                column: "userID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
