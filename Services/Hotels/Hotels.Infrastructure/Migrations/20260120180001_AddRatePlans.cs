using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotels.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRatePlans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rate_plans",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    hotel_id = table.Column<Guid>(type: "uuid", nullable: false),
                    room_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<double>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rate_plans", x => x.id);

                    table.ForeignKey("fk_rate_plans_hotels", x => x.hotel_id, 
                                     principalTable: "hotels", 
                                     principalColumn: "id", 
                                     onDelete: ReferentialAction.Cascade);

                    table.ForeignKey("fk_rate_plans_room_types", x => x.room_type_id, 
                                     principalTable: "room_types", 
                                     principalColumn: "id", 
                                     onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_rate_plans_hotel_id",
                table: "rate_plans",
                column: "hotel_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rate_plans");
        }
    }
}
