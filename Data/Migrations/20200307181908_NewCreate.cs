using Microsoft.EntityFrameworkCore.Migrations;

namespace smalandscamping.Data.Migrations
{
    public partial class NewCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Booking",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Booking");
        }
    }
}
