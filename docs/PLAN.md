# SplitCents — Project Plan

## Overview

Building the backend API first. Single ASP.NET Core Web API project (`SplitCents.API`) backed by PostgreSQL. Mobile app comes later.

---

## Phase 0 — Project Setup ✓

**Goal:** Solution and project files in place, repo clean.

- `SplitCentsAPI.sln` at repo root
- `backend/SplitCents.API/SplitCents.API.csproj` — Web API targeting .NET 8
- `.gitignore` covers .NET, VS, and OS artifacts

### Done when
- `dotnet build` at the repo root succeeds

---

## Phase 1 — Domain Models

**Goal:** Core C# models and enums compiling inside the API project.

- Enums: `TransactionType`, `AccountType`, `BudgetPeriod`
- Models: `Transaction`, `Account`, `Category`, `Budget`, `User`
- Use `Guid` for IDs, `decimal` for money, `DateTimeOffset` for timestamps

### Done when
- `dotnet build` succeeds with no errors

---

## Phase 2 — Database

**Goal:** PostgreSQL connected via EF Core, schema created from migrations.

- `AppDbContext` with a `DbSet` per model
- Docker Compose for a local PostgreSQL instance
- Initial migration applied

### Done when
- `dotnet ef database update` creates the schema on a fresh database

---

## Phase 3 — Auth

**Goal:** Register and login endpoints issuing JWT tokens.

- `User` table with hashed passwords
- `POST /auth/register` and `POST /auth/login`
- All other endpoints require a valid JWT

### Done when
- Login returns a token; calling a protected endpoint without it returns 401

---

## Phase 4 — API Endpoints

**Goal:** Full CRUD for all resources.

- Controllers: `TransactionsController`, `AccountsController`, `CategoriesController`, `BudgetsController`
- Each user only sees their own data

### Done when
- All endpoints tested and working in Postman or a `.http` file
