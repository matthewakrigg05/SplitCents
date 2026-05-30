# SplitCents.Infrastructure

## Purpose
Implements the interfaces defined in Core. Handles all I/O: database access, migrations, and any external service integrations. Nothing in this layer invents business rules — it only persists and retrieves data.

## Scope

### Database
- **EF Core DbContext** (`SplitCentsDbContext`) configured for PostgreSQL
- `DbSet<T>` for each domain model
- Fluent API configuration (table names, indexes, constraints)

### Migrations
- All EF Core migrations live here
- Run via `dotnet ef migrations add` targeting this project

### Repository Implementations
Concrete classes that fulfil the interfaces from Core:
- `TransactionRepository`, `AccountRepository`, `BudgetRepository`, `CategoryRepository`, `UserRepository`
- All queries written against the DbContext; no raw SQL unless necessary

### Sync Infrastructure
- Logic that reads `SyncStatus` on entities and batches changes for upload
- Handles conflict resolution (last-write-wins initially)

### Dependency Registration
- Extension method(s) (e.g. `AddInfrastructure()`) that register DbContext, repositories, and services into the DI container — called from `SplitCents.API`'s `Program.cs`

## What Does NOT Belong Here
- Business rules or domain logic (belongs in Core)
- HTTP controllers or middleware (belongs in API)
- Anything the mobile client calls directly
