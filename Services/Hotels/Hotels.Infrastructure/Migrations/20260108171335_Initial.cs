using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotels.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_countries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hotels",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    stars = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    description = table.Column<string>(type: "text", nullable: false),
                    country_id = table.Column<int>(type: "integer", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hotels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rooms",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    hotel_id = table.Column<Guid>(type: "uuid", nullable: false),
                    number = table.Column<string>(type: "text", nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    is_enabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    type_id = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rooms", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_rooms_hotel_id",
                table: "rooms",
                column: "hotel_id");

            FillCountries(migrationBuilder);
        }

        private void FillCountries(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("countries",
                ["id", "name"],
                ["643", "Российская федерация"]);

            migrationBuilder.InsertData("countries",
                ["id", "name"],
                ["112", "Беларусь"]);

            migrationBuilder.InsertData("countries",
                ["id", "name"],
                ["688", "Сербия"]);

            migrationBuilder.InsertData("countries",
                ["id", "name"],
                ["417", "Киргизия"]);

            migrationBuilder.InsertData("countries",
                ["id", "name"],
                ["156", "Китай"]);

            migrationBuilder.InsertData("countries",
                ["id", "name"],
                ["356", "Индия"]);

            migrationBuilder.InsertData("countries",
                ["id", "name"],
                ["499", "Черногория"]);

            migrationBuilder.InsertData("countries",
                ["id", "name"],
                ["462", "Мальдивы"]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "countries");

            migrationBuilder.DropTable(
                name: "hotels");

            migrationBuilder.DropTable(
                name: "rooms");
        }
    }
}
