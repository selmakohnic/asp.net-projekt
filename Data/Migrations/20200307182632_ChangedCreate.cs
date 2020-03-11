using Microsoft.EntityFrameworkCore.Migrations;

namespace smalandscamping.Data.Migrations
{
    public partial class ChangedCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Booking");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Booking",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
