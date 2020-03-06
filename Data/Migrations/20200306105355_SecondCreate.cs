using Microsoft.EntityFrameworkCore.Migrations;

namespace smalandscamping.Data.Migrations
{
    public partial class SecondCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Cottage_CottageId",
                table: "Booking");

            migrationBuilder.AlterColumn<int>(
                name: "CottageId",
                table: "Booking",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Cottage_CottageId",
                table: "Booking",
                column: "CottageId",
                principalTable: "Cottage",
                principalColumn: "CottageId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Cottage_CottageId",
                table: "Booking");

            migrationBuilder.AlterColumn<int>(
                name: "CottageId",
                table: "Booking",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Cottage_CottageId",
                table: "Booking",
                column: "CottageId",
                principalTable: "Cottage",
                principalColumn: "CottageId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
