using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Driver.Migrations
{
    /// <inheritdoc />
    public partial class AddingTripss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "trips",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    userID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DriverID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Target = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trips", x => x.id);
                    table.ForeignKey(
                        name: "FK_trips_AspNetUsers_userID",
                        column: x => x.userID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_trips_userID",
                table: "trips",
                column: "userID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "trips");
        }
    }
}
