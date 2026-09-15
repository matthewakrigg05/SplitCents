# Postman Testing for SplitCents

Postman is useful in this project because the API is not just a static code sample; it is a working ASP.NET Core service with real authentication, database migrations, and request validation. The current state of the project is centered on the API layer, and Postman gives a practical way to validate that the app behaves correctly from the outside in.

## Why Postman matters in this project

The current backend is organized around a few core flows:

- User registration and login
- JWT-based authentication
- Recurring transaction creation and retrieval
- Updating, deleting, and marking recurring transactions as paid or unpaid

These flows are implemented in the API controllers and are exactly the kinds of interactions that are easiest to validate in Postman. They also map cleanly to real-world testing scenarios such as:

- creating a new account
- receiving a token
- attaching the token to subsequent requests
- verifying that protected endpoints reject unauthenticated users
- confirming that database-backed operations return the expected results

Because the app runs migrations automatically on startup and uses a real database context, Postman testing is more than a convenience—it is a way to confirm that the API, database, and authentication system are working together correctly.

## Current project state reflected in API behavior

From the current implementation, the project is in an early but functional API stage. The main observable behavior is:

- `UsersController` exposes `POST /api/users/register` and `POST /api/users/login`
- `TransactionsController` exposes recurring transaction endpoints under `api/transactions`
- JWT authentication is enabled and enforced on the transaction routes
- `Program.cs` loads environment values, configures JWT validation, and runs database migrations when the app starts
- custom exception middleware is present, which means API errors are intentionally normalized and should be observed via Postman responses

This means Postman should be used to validate the system as it currently exists, rather than hypothetical future features. The most important current flows are registration, login, and recurring transactions.

## Postman workflow

### 1. Start the API

Before testing in Postman, run the API locally and confirm it starts without errors. The project loads environment variables from a `.env` file and then runs EF Core migrations automatically. If the app fails to start, Postman testing will not be meaningful because the backend itself is not yet healthy.

Test checklist:

- app starts successfully
- database initializes or migrates successfully
- JWT settings are loaded from configuration
- no startup exception is thrown from missing config or DB connectivity

### 2. Test the user registration flow

Use a Postman request to `POST /api/users/register`.

Expected flow:

- send a JSON payload containing email, password, display name, first name, and last name
- receive a `201 Created` response
- response includes a newly created user record and a JWT token

This is the most important first validation because it confirms:

- the API accepts input and maps it to the service layer
- the user service creates a user successfully
- token generation works
- the API returns the expected payload structure

A good regression check is to inspect the created response and then use the returned token for all subsequent requests.

### 3. Test login

Call `POST /api/users/login` using the same email and password.

Expected behavior:

- valid credentials return `200 OK`
- a valid token is returned
- the token should be rejected if the credentials are wrong

This helps confirm the login flow is consistent with registration and that JWT issuance is stable.

### 4. Configure authentication in Postman

Once a token is returned, configure the collection or request with a Bearer token.

For protected endpoints in `TransactionsController`, the API extracts the user ID from the JWT claims and uses it to scope data to the authenticated user. That means Postman should be set up to:

- use the token from login/register
- send `Authorization: Bearer <token>` on every protected request
- verify that requests without that header return `401 Unauthorized`

This is a critical part of validating the security model of the application.

### 5. Validate the recurring transaction endpoints

The current project exposes CRUD-style endpoints for recurring transactions.

Recommended sequence:

1. `POST /api/transactions/recurring` to create a recurring transaction
2. `GET /api/transactions/recurring` to list the current user’s recurring transactions
3. `GET /api/transactions/recurring/{transactionId}` to fetch one item
4. `PUT /api/transactions/recurring/{transactionId}` to update the item
5. `POST /api/transactions/recurring/{transactionId}/paid` to mark it paid
6. `DELETE /api/transactions/recurring/{transactionId}/paid` to mark it unpaid
7. `DELETE /api/transactions/recurring/{transactionId}` to delete it

These tests check that the middleware/auth pipeline, service layer, and database interaction all cooperate correctly. They also validate ID-based routing, entity lookup, and ownership scoping by authenticated user.

### 6. Validate upcoming recurring logic

The API supports `GET /api/transactions/recurring/upcoming` with optional query parameters such as:

- `from`
- `daysAhead`

This endpoint is especially useful in Postman because it exercises date logic and user-level filtering. It helps confirm that the API is computing upcoming recurring transactions correctly rather than just passing through raw data.

## Negative testing that matters right now

The most valuable negative tests for the current project are:

- login with bad credentials returns an error
- access protected routes without a token returns `401`
- use a non-existent or invalid recurring transaction ID and check the response behavior
- send invalid JSON or malformed request payloads to confirm validation middleware and exception handling work

The presence of `ExceptionMiddleware` suggests the project is intentionally designed to standardize failure responses, so Postman should be used to inspect the actual error payloads and confirm they are consistent and useful.

## How this relates to the current project stage

At this stage, the project is focused on the foundation of a budgeting application: users, authentication, and recurring financial entries. Postman testing is therefore not a final QA step; it is a core part of validating the system while the application is still being built.

That means Postman is especially useful for:

- proving that the API works from an end-user perspective
- validating each controller action against real HTTP requests
- confirming the JWT auth model before more features are layered on
- ensuring recurring financial workflows behave consistently with the current domain design

In short, Postman is acting as the validation layer for the current API implementation. It helps confirm that the app is not only compiling, but that it actually supports the user flows the project is designed around.

## Suggested test collection structure

A sensible Postman collection for this repo would include folders like:

- Auth
  - Register
  - Login
- Recurring Transactions
  - Create
  - List
  - Get by ID
  - Update
  - Mark paid
  - Mark unpaid
  - Upcoming
  - Delete
- Error/edge cases
  - Unauthorized request
  - Invalid credentials
  - Missing/invalid IDs

This structure mirrors the actual implementation and makes it easier to extend the collection as more endpoints are added.

## Bottom line

Postman testing is directly relevant to the current project because the API already contains the real flows that matter most: user identity and recurring transaction processing. The repository is not yet a fully complete finance platform, but it does have working HTTP endpoints that should be exercised through Postman to validate behavior, security, and data flow. That makes it an important tool for confirming the project’s current state and for guiding the next stages of development.
