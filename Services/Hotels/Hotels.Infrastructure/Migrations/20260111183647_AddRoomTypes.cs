using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotels.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                                name: "type_id",
                                table: "rooms");
            migrationBuilder.AddColumn<Guid>(
                                name: "type_id",
                                table: "rooms",
                                type: "uuid",
                                nullable: false);

            migrationBuilder.CreateTable(
                name: "room_types",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    hotel_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_room_types", x => x.id);
                    table.ForeignKey("fk_room_types_hotels", 
                                     x => x.hotel_id, 
                                     principalTable: "hotels", 
                                     principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_room_types_hotel_id",
                table: "room_types",
                column: "hotel_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "room_types");

            migrationBuilder.DropColumn(
                                name: "type_id",
                                table: "rooms");
            migrationBuilder.AddColumn<long>(
                                name: "type_id",
                                table: "rooms",
                                type: "bigint",
                                nullable: false,
                                defaultValue: 0L);
        }
    }
}
