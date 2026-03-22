# Implementation Notes

## Architecture Decisions

### Admin.WebApi Direct Database Access
The Admin.WebApi references Hotels.Infrastructure and accesses the Hotels PostgreSQL database directly, rather than through HTTP calls to Hotels.WebApi. This was chosen for demo simplicity - in production, you'd use either:
- HTTP calls via the existing HotelsClient with Polly resilience
- A shared read replica
- Event-driven projections in the Admin service's own database

### Shared Enums
RoomStatus, HousekeepingStatus, AmenityType, and CancellationPolicyType are defined in both Hotels.Domain (for the domain layer) and Shared.Contracts (for integration events). This avoids coupling services to each other's domain, while keeping the values in sync.

### Domain Model Simplifications
- **Hotel.CountryId** is kept as `int` from the original design, mapped to a simple Country entity
- **RatePlan.BasePrice** changed from `double` to `decimal` for proper monetary precision
- **Room.IsEnabled** renamed to **Room.IsActive** for consistency with other entities
- **SeasonPrice** retained from original design but not exposed through Admin UI

### Housekeeping vs Room Status (Domain Rule)
A room's HousekeepingStatus (Clean/Dirty/Inspected) is independent of its RoomStatus (Available/Occupied/Reserved/OutOfService/Maintenance). A dirty room can still be Available for booking - housekeeping is an operational concern, not a booking constraint. Only RoomStatus or IsActive flag determines booking availability.

## Known Limitations

1. **No EF Core migrations committed** - The seed service uses `Database.MigrateAsync()` but migrations need to be generated against a running PostgreSQL. Run `dotnet ef migrations add AdminExtensions -p Hotels.Infrastructure -s Hotels.WebApi` after starting PostgreSQL.

2. **No Transactional Outbox** - MassTransit is configured but the EF Core Transactional Outbox requires additional tables and configuration. Events are published in-process.

3. **No real reservation data** - The Reserve service is separate and doesn't share the Hotels database. Dashboard shows room statuses from seed data only.

4. **Authentication is dev-only** - The `DevAuth` login accepts hardcoded credentials from appsettings.Development.json. No real identity provider is integrated.

5. **Frontend CRUD forms** - Create/Edit dialogs for hotels, rooms, room types are not implemented. The UI focuses on viewing data, managing statuses, and the housekeeping board.

6. **No tests in this iteration** - Unit and integration tests were not added in this phase. The existing test projects (Hotels.xUnitTests, Admin.xUnitTests) need updates for the new entity fields.

7. **PgDbContextOptions pattern** - The Hotels service uses a custom `PgDbContextOptions` instead of standard DI-based DbContext registration. This works but is unconventional.

8. **No idempotent consumers** - MassTransit consumers for reservation events are not yet implemented. The infrastructure is in place (Shared.Contracts events defined, MassTransit configured).

## Tech Debt

- Align EF Core versions between Admin.Infrastructure (9.0.10) and Hotels.Infrastructure (9.0.10 via Npgsql)
- Migrate PgDbContextOptions to standard `AddDbContext` DI pattern
- Add input validation to Admin API endpoints (FluentValidation)
- Add response caching for dashboard queries
- Consider using MediatR for CQRS in Admin.WebApi controllers
