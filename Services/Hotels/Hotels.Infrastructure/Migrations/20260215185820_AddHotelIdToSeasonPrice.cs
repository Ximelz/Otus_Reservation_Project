using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotels.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelIdToSeasonPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "hotel_id",
                table: "season_prices",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.Sql("UPDATE season_prices sp " +
                                 "SET hotel_id = (SELECT rt.hotel_id " +
                                 "                FROM room_types rt " +
                                 "                WHERE rt.id = sp.room_type_id)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "hotel_id",
                table: "season_prices");
        }
    }
}
