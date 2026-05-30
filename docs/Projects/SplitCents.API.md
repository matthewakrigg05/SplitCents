# SplitCents.API

## Purpose
The HTTP entry point for the application. Receives requests from the React Native mobile client, delegates work to Core and Infrastructure, and returns responses. This layer is intentionally thin — no business logic lives here.

## Scope

### Endpoints
RESTful routes for each domain resource:
- `GET/POST /api/transactions`, `GET/PUT/DELETE /api/transactions/{id}`
- `GET/POST /api/accounts`, `GET/PUT/DELETE /api/accounts/{id}`
- `GET/POST /api/budgets`, `GET/PUT/DELETE /api/budgets/{id}`
- `GET/POST /api/categories`, `GET/PUT/DELETE /api/categories/{id}`
- `POST /api/auth/register`, `POST /api/auth/login`, `POST /api/auth/refresh`
- `POST /api/sync/push`, `GET /api/sync/pull` — mobile sync endpoints

### Request / Response Models
DTOs that are separate from the Core domain models. Validated on the way in (e.g. `[Required]`, `FluentValidation`). Never expose internal domain types directly to the client.

### Authentication & Authorization
- JWT bearer tokens
- Token issuance and refresh logic (or delegated to an auth library)
- `[Authorize]` attributes / policy-based auth

### Middleware & Cross-Cutting Concerns
- Global error handling / problem details responses
- Request logging
- CORS policy (for any web clients)
- Rate limiting *(future)*

### Program.cs / Startup
- Registers services from `AddInfrastructure()` and any API-specific services
- Configures the middleware pipeline
- Reads configuration (connection strings, JWT settings) from environment/secrets

## What Does NOT Belong Here
- Business rule calculations (belongs in Core)
- Direct database queries (belongs in Infrastructure)
- Mobile-side code
