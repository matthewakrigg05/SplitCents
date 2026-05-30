# SplitCents — Test Projects

## Overview
Tests are split into three projects that mirror the source projects they cover. Each project lives under `tests/` in the solution.

---

## SplitCents.Core.Tests

### Purpose
Fast, isolated unit tests for domain logic and business rules. No database, no HTTP, no I/O.

### What to test
- Budget period calculations (e.g. has a monthly budget been exceeded?)
- Split/share calculations
- Sync conflict resolution rules
- Any non-trivial logic on domain models or services defined in Core

### Tools
- xUnit
- FluentAssertions

---

## SplitCents.Infrastructure.Tests

### Purpose
Verify that repositories and the DbContext behave correctly against a real (or realistic) database.

### What to test
- Repository CRUD operations return correct results
- EF Core queries produce the expected SQL / results
- Migrations apply cleanly against a fresh database
- Sync batching logic correctly identifies Dirty entities

### Tools
- xUnit
- EF Core in-memory provider *or* Testcontainers (PostgreSQL) for higher fidelity
- FluentAssertions

---

## SplitCents.API.Tests

### Purpose
Integration tests that exercise the full HTTP pipeline — middleware, auth, routing, and response shape — without needing a live external database.

### What to test
- Endpoints return correct status codes and response bodies
- Authentication/authorization is enforced (401/403 on protected routes)
- Validation rejects malformed request bodies
- Sync push/pull endpoints handle edge cases correctly

### Tools
- xUnit
- `WebApplicationFactory<Program>` (ASP.NET Core test host)
- EF Core in-memory or SQLite provider to avoid needing a real PostgreSQL instance in CI
- FluentAssertions
