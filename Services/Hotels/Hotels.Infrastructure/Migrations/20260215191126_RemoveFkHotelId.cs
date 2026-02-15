using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotels.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFkHotelId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("fk_rate_plans_hotels", "rate_plans");
            migrationBuilder.AddForeignKey("fk_rooms_hotels", "rooms", "hotel_id", "hotels", onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("fk_rooms_hotels", "rooms");
            migrationBuilder.AddForeignKey("fk_rate_plans_hotels", "rate_plans", "hotel_id", "hotels", onDelete: ReferentialAction.Cascade);
        }
    }
}
