# Project Deliverables — SplitCents

A budgeting app for couples. Shared visibility over household finances, no bank account access required. Targets iOS, Android, and web from a single codebase.

## Tech Stack

| Layer | Technology |
|---|---|
| Frontend | React Native + Expo (iOS, Android, Web) |
| Backend | C# + ASP.NET Core (REST API) |
| Cloud Database | PostgreSQL + EF Core |
| Local Database | SQLite via Expo SQLite (offline-first) |
| Authentication | Email/password (JWT) + OAuth (Google & Apple) |
| Couple Linking | Invite by shareable code or email |

---

## Foundation & Infrastructure
- [ ] Scaffold ASP.NET Core Web API in `backend/`
- [ ] Configure EF Core with PostgreSQL, set up migrations
- [ ] Scaffold Expo TypeScript app in `frontend/` with Expo Router
- [ ] Basic tab navigation shell, base theme, ESLint/Prettier
- [ ] `.gitignore` covering C# and Node/Expo artefacts, update `README.md`

## Authentication & Couple Linking
- [ ] Register, login, JWT + refresh token, OAuth (Google & Apple)
- [ ] Household model, invite by code and by email, accept/reject invite
- [ ] Auth screens (login, register, OAuth), couple linking flow, account settings

## Transactions & Categories
- [ ] Categories, Transactions, and Budgets CRUD
- [ ] Delta sync endpoint (records modified since a given timestamp)
- [ ] Add/edit/delete transaction screen, category management, budget setup
- [ ] Offline sync: local SQLite write queue, replay on reconnect

## Split Bills & Recurring Transactions
- [ ] Split logic (equal, percentage, custom) with settlement tracking
- [ ] Recurring transaction model and scheduled job for future instances
- [ ] Split bill UI, settlement summary, recurring transaction management

## Savings Goals & Net Worth
- [ ] Savings goals and contributions CRUD, manual account balance entries
- [ ] Savings goals screen (progress bar), net worth overview, account management

## Notifications & Alerts
- [ ] Alert rules engine (over-budget, goal milestone), Expo Push Notifications
- [ ] In-app notification centre, notification preferences screen

## Reports & Analytics
- [ ] Endpoints: spending by category, over time, partner comparison, budget vs. actual
- [ ] Charts: category breakdown, spending trend, budget vs. actual, partner comparison

## Testing & Deployment
- [ ] Unit and integration tests (xUnit, Jest)
- [ ] CI/CD with GitHub Actions
- [ ] Backend deployment, hosted PostgreSQL with backups
- [ ] Expo EAS Build for iOS and Android, App Store/Play Store submission


**Goal:** Ensure the application is stable, tested, and ready for real users.

### Testing
- [ ] Unit tests for backend business logic (xUnit)
- [ ] Integration tests for API endpoints (xUnit + `WebApplicationFactory`)
- [ ] Frontend unit tests (Jest + React Native Testing Library)
- [ ] End-to-end tests (Detox or Maestro)

### Deployment
- [ ] CI/CD pipeline with GitHub Actions (build, test, deploy on merge to main)
- [ ] Backend deployment to cloud provider (Railway, Render, or Azure — TBD)
- [ ] Hosted PostgreSQL instance configured with automated backups
- [ ] Expo EAS Build configured for iOS and Android
- [ ] Environment variable management (staging vs. production)

### Finalisation
- [ ] Performance audit (API response times, app bundle size)
- [ ] Accessibility review (screen reader support, colour contrast ratios)
- [ ] App Store and Google Play submission preparation
- [ ] Privacy policy and terms of service documents
