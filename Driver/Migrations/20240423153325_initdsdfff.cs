using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Driver.Migrations
{
    /// <inheritdoc />
    public partial class initdsdfff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "driverRatings");

            migrationBuilder.AddColumn<int>(
                name: "UsersRating",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsersRating",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "driverRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_driverRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_driverRatings_AspNetUsers_DriverID",
                        column: x => x.DriverID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_driverRatings_DriverID",
                table: "driverRatings",
                column: "DriverID");
        }
    }
}
