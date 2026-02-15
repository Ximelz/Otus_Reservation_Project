using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotels.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomTypeHotelIdCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("fk_room_types_hotels", "room_types");
            migrationBuilder.AddForeignKey("fk_room_types_hotels", 
                                           "room_types", 
                                           "hotel_id", 
                                           "hotels", 
                                           principalColumn: "id", 
                                           onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("fk_room_types_hotels", "room_types");
            migrationBuilder.AddForeignKey("fk_room_types_hotels",
                                           "room_types",
                                           "hotel_id",
                                           "hotels",
                                           principalColumn: "id");
        }
    }
}
