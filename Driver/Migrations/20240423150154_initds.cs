using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Driver.Migrations
{
    /// <inheritdoc />
    public partial class initds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestDrive_trips_Tripid",
                table: "RequestDrive");

            migrationBuilder.DropIndex(
                name: "IX_RequestDrive_Tripid",
                table: "RequestDrive");

            migrationBuilder.DropColumn(
                name: "Tripid",
                table: "RequestDrive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Tripid",
                table: "RequestDrive",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RequestDrive_Tripid",
                table: "RequestDrive",
                column: "Tripid");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestDrive_trips_Tripid",
                table: "RequestDrive",
                column: "Tripid",
                principalTable: "trips",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
