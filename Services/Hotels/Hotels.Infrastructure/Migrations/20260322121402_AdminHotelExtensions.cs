using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotels.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdminHotelExtensions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_season_prices_room_type_id",
                table: "season_prices",
                newName: "ix_season_prices_room_type_id");

            migrationBuilder.RenameColumn(
                name: "is_enabled",
                table: "rooms",
                newName: "is_active");

            migrationBuilder.RenameIndex(
                name: "IX_rooms_hotel_id",
                table: "rooms",
                newName: "ix_rooms_hotel_id");

            // ix_room_types_hotel_id already has lowercase name from AddRoomTypes migration

            migrationBuilder.RenameColumn(
                name: "price",
                table: "rate_plans",
                newName: "base_price");

            // ix_rate_plans_hotel_id already has lowercase name from AddRatePlans migration

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                table: "rooms",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<int>(
                name: "floor",
                table: "rooms",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "housekeeping_status",
                table: "rooms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "notes",
                table: "rooms",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "rooms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "updated_at",
                table: "rooms",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<string>(
                name: "view_type",
                table: "rooms",
                type: "varchar(64)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "base_area_sqm",
                table: "room_types",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "bed_configuration",
                table: "room_types",
                type: "varchar(128)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "capacity_adults",
                table: "room_types",
                type: "integer",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.AddColumn<int>(
                name: "capacity_children",
                table: "room_types",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "room_types",
                type: "varchar(32)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                table: "room_types",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "room_types",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "updated_at",
                table: "room_types",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<bool>(
                name: "breakfast_included",
                table: "rate_plans",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "cancellation_policy_type",
                table: "rate_plans",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "rate_plans",
                type: "varchar(32)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                table: "rate_plans",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<string>(
                name: "currency",
                table: "rate_plans",
                type: "varchar(3)",
                nullable: false,
                defaultValue: "RUB");

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "rate_plans",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_default",
                table: "rate_plans",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "prepayment_required",
                table: "rate_plans",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "updated_at",
                table: "rate_plans",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "check_in_time",
                table: "hotels",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "check_out_time",
                table: "hotels",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "hotels",
                type: "varchar(256)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                table: "hotels",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "hotels",
                type: "varchar(256)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "hotels",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "slug",
                table: "hotels",
                type: "varchar(128)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "timezone",
                table: "hotels",
                type: "varchar(64)",
                nullable: false,
                defaultValue: "Europe/Moscow");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "updated_at",
                table: "hotels",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "hotels",
                type: "varchar(256)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "hotel_amenities",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    hotel_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amenity_type = table.Column<int>(type: "integer", nullable: false),
                    custom_name = table.Column<string>(type: "varchar(128)", nullable: true),
                    is_available = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hotel_amenities", x => x.id);
                    table.ForeignKey(
                        name: "FK_hotel_amenities_hotels_hotel_id",
                        column: x => x.hotel_id,
                        principalTable: "hotels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "hotel_policies",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    hotel_id = table.Column<Guid>(type: "uuid", nullable: false),
                    check_in_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    check_out_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    early_check_in_note = table.Column<string>(type: "text", nullable: true),
                    late_check_out_note = table.Column<string>(type: "text", nullable: true),
                    cancellation_policy_text = table.Column<string>(type: "text", nullable: true),
                    confirmation_pending_enabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    auto_confirm_rules = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    terms_and_conditions_text = table.Column<string>(type: "text", nullable: true),
                    contact_instructions = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hotel_policies", x => x.id);
                    table.ForeignKey(
                        name: "FK_hotel_policies_hotels_hotel_id",
                        column: x => x.hotel_id,
                        principalTable: "hotels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "recent_activity_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    hotel_id = table.Column<Guid>(type: "uuid", nullable: true),
                    activity_type = table.Column<string>(type: "varchar(64)", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    entity_type = table.Column<string>(type: "varchar(64)", nullable: true),
                    entity_id = table.Column<Guid>(type: "uuid", nullable: true),
                    timestamp = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    performed_by = table.Column<string>(type: "varchar(256)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_recent_activity_logs", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_rooms_hotel_number",
                table: "rooms",
                columns: new[] { "hotel_id", "number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rooms_type_id",
                table: "rooms",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_rate_plans_room_type_id",
                table: "rate_plans",
                column: "room_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_hotels_is_active",
                table: "hotels",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "ix_hotels_slug",
                table: "hotels",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_hotel_amenities_hotel_id",
                table: "hotel_amenities",
                column: "hotel_id");

            migrationBuilder.CreateIndex(
                name: "ix_hotel_policies_hotel_id",
                table: "hotel_policies",
                column: "hotel_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_recent_activity_logs_hotel_id",
                table: "recent_activity_logs",
                column: "hotel_id");

            migrationBuilder.CreateIndex(
                name: "ix_recent_activity_logs_timestamp",
                table: "recent_activity_logs",
                column: "timestamp");

            migrationBuilder.AddForeignKey(
                name: "FK_rate_plans_hotels_hotel_id",
                table: "rate_plans",
                column: "hotel_id",
                principalTable: "hotels",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_rate_plans_room_types_room_type_id",
                table: "rate_plans",
                column: "room_type_id",
                principalTable: "room_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_room_types_hotels_hotel_id",
                table: "room_types",
                column: "hotel_id",
                principalTable: "hotels",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_rooms_hotels_hotel_id",
                table: "rooms",
                column: "hotel_id",
                principalTable: "hotels",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_rooms_room_types_type_id",
                table: "rooms",
                column: "type_id",
                principalTable: "room_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rate_plans_hotels_hotel_id",
                table: "rate_plans");

            migrationBuilder.DropForeignKey(
                name: "FK_rate_plans_room_types_room_type_id",
                table: "rate_plans");

            migrationBuilder.DropForeignKey(
                name: "FK_room_types_hotels_hotel_id",
                table: "room_types");

            migrationBuilder.DropForeignKey(
                name: "FK_rooms_hotels_hotel_id",
                table: "rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_rooms_room_types_type_id",
                table: "rooms");

            migrationBuilder.DropTable(
                name: "hotel_amenities");

            migrationBuilder.DropTable(
                name: "hotel_policies");

            migrationBuilder.DropTable(
                name: "recent_activity_logs");

            migrationBuilder.DropIndex(
                name: "ix_rooms_hotel_number",
                table: "rooms");

            migrationBuilder.DropIndex(
                name: "IX_rooms_type_id",
                table: "rooms");

            migrationBuilder.DropIndex(
                name: "IX_rate_plans_room_type_id",
                table: "rate_plans");

            migrationBuilder.DropIndex(
                name: "ix_hotels_is_active",
                table: "hotels");

            migrationBuilder.DropIndex(
                name: "ix_hotels_slug",
                table: "hotels");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "rooms");

            migrationBuilder.DropColumn(
                name: "floor",
                table: "rooms");

            migrationBuilder.DropColumn(
                name: "housekeeping_status",
                table: "rooms");

            migrationBuilder.DropColumn(
                name: "notes",
                table: "rooms");

            migrationBuilder.DropColumn(
                name: "status",
                table: "rooms");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "rooms");

            migrationBuilder.DropColumn(
                name: "view_type",
                table: "rooms");

            migrationBuilder.DropColumn(
                name: "base_area_sqm",
                table: "room_types");

            migrationBuilder.DropColumn(
                name: "bed_configuration",
                table: "room_types");

            migrationBuilder.DropColumn(
                name: "capacity_adults",
                table: "room_types");

            migrationBuilder.DropColumn(
                name: "capacity_children",
                table: "room_types");

            migrationBuilder.DropColumn(
                name: "code",
                table: "room_types");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "room_types");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "room_types");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "room_types");

            migrationBuilder.DropColumn(
                name: "breakfast_included",
                table: "rate_plans");

            migrationBuilder.DropColumn(
                name: "cancellation_policy_type",
                table: "rate_plans");

            migrationBuilder.DropColumn(
                name: "code",
                table: "rate_plans");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "rate_plans");

            migrationBuilder.DropColumn(
                name: "currency",
                table: "rate_plans");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "rate_plans");

            migrationBuilder.DropColumn(
                name: "is_default",
                table: "rate_plans");

            migrationBuilder.DropColumn(
                name: "prepayment_required",
                table: "rate_plans");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "rate_plans");

            migrationBuilder.DropColumn(
                name: "check_in_time",
                table: "hotels");

            migrationBuilder.DropColumn(
                name: "check_out_time",
                table: "hotels");

            migrationBuilder.DropColumn(
                name: "city",
                table: "hotels");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "hotels");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "hotels");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "hotels");

            migrationBuilder.DropColumn(
                name: "slug",
                table: "hotels");

            migrationBuilder.DropColumn(
                name: "timezone",
                table: "hotels");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "hotels");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "hotels");

            migrationBuilder.RenameIndex(
                name: "ix_season_prices_room_type_id",
                table: "season_prices",
                newName: "IX_season_prices_room_type_id");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "rooms",
                newName: "is_enabled");

            migrationBuilder.RenameIndex(
                name: "ix_rooms_hotel_id",
                table: "rooms",
                newName: "IX_rooms_hotel_id");

            // ix_room_types_hotel_id - no rename needed, was already lowercase

            migrationBuilder.RenameColumn(
                name: "base_price",
                table: "rate_plans",
                newName: "price");

            // ix_rate_plans_hotel_id - no rename needed, was already lowercase
        }
    }
}
