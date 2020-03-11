using Microsoft.EntityFrameworkCore.Migrations;

namespace smalandscamping.Data.Migrations
{
    public partial class AddedIsBooked : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBooked",
                table: "Cottage",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBooked",
                table: "Cottage");
        }
    }
}
