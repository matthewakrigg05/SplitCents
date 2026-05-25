# TODO — SplitCents

## 1 — Domain model classes (C# entities)

| Class | Purpose |
|---|---|
| `User` | Registered account (email, display name, password hash) |
| `Household` | The shared space for a couple |
| `HouseholdMember` | Links a User to a Household with a role |
| `HouseholdInvite` | Pending invite (shareable code or email-based) |
| `Category` | Expense category (system defaults + user-defined) |
| `Budget` | Monthly spending limit for a category |
| `Account` | Manual balance entry — an asset or liability |
| `Transaction` | A single income or expense entry |
| `TransactionSplit` | How a transaction is divided between partners |
| `RecurringTransaction` | Template for a repeating transaction |
| `SavingsGoal` | A savings target with an optional deadline |
| `SavingsContribution` | A payment towards a savings goal |
| `Notification` | An in-app notification for a user |
| `NotificationPreference` | Per-user toggles for each alert type |

---

## 2 — Scaffold backend
- [ ] Create ASP.NET Core Web API project in `backend/`
- [ ] Add EF Core, Npgsql, JWT Bearer, BCrypt packages
- [ ] Create `AppDbContext` with a `DbSet<T>` per entity
- [ ] Add PostgreSQL connection string, run initial migration

## 3 — Scaffold frontend
- [ ] Create Expo TypeScript app in `frontend/`
- [ ] Set up Expo Router with a basic tab shell (Home, Budgets, Goals, Profile)
- [ ] Define a `theme.ts` (colours, typography, spacing)

## 4 — Authentication
- [ ] Backend: register, login, JWT + refresh token, OAuth (Google & Apple)
- [ ] Frontend: login/register screens, secure token storage, auth context

## 5 — Couple linking
- [ ] Backend: create household, generate invite code, send invite email, accept invite
- [ ] Frontend: post-registration flow to create or join a household
