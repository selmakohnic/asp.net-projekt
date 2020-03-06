using Microsoft.EntityFrameworkCore.Migrations;

namespace smalandscamping.Data.Migrations
{
    public partial class FithCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Booking");

            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                table: "Booking",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Booking");

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Booking",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
