using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotels.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeasonPrices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "season_prices",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    date_from = table.Column<DateOnly>(type: "date", nullable: false),
                    date_to = table.Column<DateOnly>(type: "date", nullable: false),
                    multiplier = table.Column<float>(type: "numeric", nullable: false),
                    room_type_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_season_prices", x => x.id);
                    table.ForeignKey("fk_season_prices_room_type_id", x => x.room_type_id,
                                     principalTable: "room_types",
                                     principalColumn: "id",
                                     onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_season_prices_room_type_id",
                table: "season_prices",
                column: "room_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "season_prices");
        }
    }
}
