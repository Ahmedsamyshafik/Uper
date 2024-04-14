using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Driver.Migrations
{
    /// <inheritdoc />
    public partial class AddingTripssd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestDrive_AspNetUsers_UserId",
                table: "RequestDrive");

            migrationBuilder.DropIndex(
                name: "IX_RequestDrive_UserId",
                table: "RequestDrive");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RequestDrive");

            migrationBuilder.AddColumn<string>(
                name: "applicationUserId",
                table: "RequestDrive",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestDrive_applicationUserId",
                table: "RequestDrive",
                column: "applicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestDrive_AspNetUsers_applicationUserId",
                table: "RequestDrive",
                column: "applicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestDrive_AspNetUsers_applicationUserId",
                table: "RequestDrive");

            migrationBuilder.DropIndex(
                name: "IX_RequestDrive_applicationUserId",
                table: "RequestDrive");

            migrationBuilder.DropColumn(
                name: "applicationUserId",
                table: "RequestDrive");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "RequestDrive",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_RequestDrive_UserId",
                table: "RequestDrive",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestDrive_AspNetUsers_UserId",
                table: "RequestDrive",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
