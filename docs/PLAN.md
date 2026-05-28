# SplitCents — Project Plan

## Overview

This document outlines the phased development plan for SplitCents. Each phase builds on the last, starting with foundational setup and ending with cross-device sync. Because C# is new territory, each phase includes a list of topics to read about before starting it.

---

## Phase 0 — Project Setup

**Goal:** Working build scaffolding in place before writing any domain code.

### Steps
1. Expand `.gitignore` to cover .NET, Node.js, and OS-specific artifacts
2. Create `SplitCents.sln` at the repo root
3. Create `shared/SplitCents.Shared.csproj` (class library targeting `net8.0`)
4. Initialise React Native (TypeScript) app inside `mobile/`

### C# to read first
- C# project files (`.csproj`) and what they contain
- Solution files (`.sln`) and how they group projects
- `dotnet` CLI basics: `dotnet new`, `dotnet build`, `dotnet add reference`

### Done when
- `dotnet build` at the repo root succeeds
- `npx expo start` inside `mobile/` launches without error

---

## Phase 1 — Shared C# Library

**Goal:** All domain types (enums, models, DTOs, constants) compile cleanly. This library will later be referenced by the backend.

### Steps
1. **Enums** — `TransactionType`, `AccountType`, `SyncStatus`, `BudgetPeriod`
2. **Models** — `Transaction`, `Account`, `Category`, `Budget`
   - Include sync metadata fields: `LastSyncedAt`, `SyncStatus`, `IsDeleted`
3. **DTOs** — one per model, no sync fields exposed
4. **Constants** — `ApiRoutes`, `DatabaseConstants`, `SyncConstants`

### C# to read first
- Namespaces and how they map to folder structure
- Classes and auto-properties (`{ get; set; }`)
- `init`-only property setters
- Enums
- `Guid` — the standard ID type in .NET
- `decimal` — always use this for money, never `float` or `double`
- `DateTimeOffset` vs `DateTime`
- Nullable reference types (`string?`, `#nullable enable`)
- Access modifiers: `public`, `private`, `internal`, `protected`
- `const` vs `static readonly`
- Record types — a good fit for DTOs (immutable, value-based equality)

### Done when
- `dotnet build` compiles the shared library with zero errors and zero warnings

---

## Phase 2 — Mobile App (Local Only)

**Goal:** A fully working app with no backend. All data persists in SQLite on the device.

### Steps
1. React Navigation setup (stack + tab navigators)
2. TypeScript interfaces mirroring the C# models
3. `expo-sqlite` local database service with schema migrations
4. Transaction list screen
5. Add / Edit transaction screen
6. Accounts screen
7. Basic dashboard (totals, spending by category)

### C# to read first
Nothing new — this phase is TypeScript and React Native.

### Done when
- Add a transaction → force-close the app → reopen → transaction is still there

---

## Phase 3 — ASP.NET Core Backend

**Goal:** REST API backed by PostgreSQL with full CRUD and JWT authentication. Authentication is included here because SplitCents is designed for couples sharing data.

### Steps
1. `dotnet new webapi` inside `backend/`, reference the shared library
2. Add EF Core and the Npgsql (PostgreSQL) provider via NuGet
3. Create `User` model and JWT register / login endpoints
4. Create `AppDbContext` with all entities
5. Add Docker Compose for a local PostgreSQL instance
6. Run initial EF Core migration
7. Add controllers: `AuthController`, `TransactionsController`, `AccountsController`, `CategoriesController`, `BudgetsController`

### C# to read first
- Interfaces (`interface`, `IRepository<T>` pattern)
- `async` / `await` and `Task<T>`
- Dependency injection — `AddScoped`, `AddSingleton`, constructor injection
- EF Core — `DbContext`, `DbSet<T>`, setting up a connection string
- EF Core migrations — `dotnet ef migrations add`, `dotnet ef database update`
- LINQ — `Where`, `Select`, `FirstOrDefaultAsync`, `ToListAsync`
- ASP.NET Core controller attributes — `[HttpGet]`, `[HttpPost]`, `[Route]`, `[Authorize]`
- `IActionResult` and `ActionResult<T>`
- Middleware and the request pipeline
- Extension methods
- Generic types (`List<T>`, `Task<T>`, etc.)
- JWT bearer authentication setup in ASP.NET Core

### Done when
- All CRUD endpoints return correct responses (test with Postman or a `.http` file)
- Register + login flow issues a valid JWT
- EF migrations apply cleanly against a fresh PostgreSQL database

---

## Phase 4 — Sync Layer

**Goal:** Transactions created offline automatically sync to the server on reconnect, with no data loss.

### Steps
1. Backend sync endpoints
   - `GET /sync/pull?since={timestamp}` — return all records changed after a timestamp
   - `POST /sync/push` — accept a batch of dirty records
2. Mobile sync engine service
3. Mark records as `SyncStatus.Dirty` on every local write
4. Background sync loop: push dirty records → pull remote changes → resolve conflicts
5. Conflict resolution strategy: **last-write-wins** using `UpdatedAt`

### C# to read first
- `IHostedService` and `BackgroundService` — for background tasks in ASP.NET Core
- `HttpClient` and `IHttpClientFactory`
- `CancellationToken` — how to stop background work gracefully

### Done when
- Create a transaction with no network connection → restore connection → transaction appears in the server database

---

## Phase 5 — Desktop (Future)

Electron or Tauri wrapper reusing the mobile/web codebase. Scope will be defined after Phase 4 is complete.

---

## Quick Reference

| Phase | Focus | Key Tech |
|---|---|---|
| 0 | Scaffolding | dotnet CLI, Expo CLI |
| 1 | Shared domain types | C# class library |
| 2 | Local mobile app | React Native, expo-sqlite |
| 3 | Backend + auth | ASP.NET Core, EF Core, PostgreSQL, JWT |
| 4 | Sync | BackgroundService, HttpClient |
| 5 | Desktop | TBD |
