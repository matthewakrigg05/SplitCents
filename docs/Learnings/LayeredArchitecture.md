# Layered Architecture in SplitCents

> This document is an AI-generated summary of a conversation and research conducted while planning the SplitCents backend. It captures key concepts explored and conclusions reached, written up as a reference for future development.

---

## Overview

The SplitCents backend follows a **layered architecture** (also referred to as clean architecture), splitting code across three projects with clear, enforced boundaries:

```
SplitCents.Core          — domain models, interfaces, business logic
SplitCents.Infrastructure — data access, repository implementations
SplitCents.API            — HTTP endpoints, controllers, DI wiring
```

Dependencies flow in one direction only:

```
API  →  Infrastructure  →  Core
```

Core has no dependencies on anything. Infrastructure depends only on Core. API depends on both.

---

## The Layers Explained

### Models (Core)
Plain C# classes that represent the shape of the data — `User`, `Transaction`, `Budget`, etc. They live in Core because they are the language the entire application speaks. Every layer references them; no layer redefines them.

Models can also contain **domain methods** — logic that only needs the object's own data:

```csharp
public class Budget
{
    public decimal Amount { get; set; }
    public decimal Spent { get; set; }

    public bool IsExceeded() => Spent > Amount;
    public decimal Remaining() => Amount - Spent;
}
```

### Interfaces (Core)
Contracts that describe *what operations must exist* without saying *how* they are performed. There are two kinds:

- **Repository interfaces** — describe how data is fetched and persisted (`IUserRepository`, `ITransactionRepository`)
- **Service interfaces** — describe what the application can *do* (`IUserService`, `IBudgetService`)

Interfaces live in Core because Core services need to express a need for data without depending on Infrastructure. The interface is the plug socket; Infrastructure provides the plug.

### Services (Core)
The business logic layer. Services orchestrate the work: they ask repositories for data via interfaces, apply rules, and return results. They never touch a database directly.

```csharp
public class UserService : IUserService
{
    private readonly IUserRepository _users;

    public UserService(IUserRepository users) => _users = users;

    public async Task UpdateEmailAsync(Guid userId, string newEmail)
    {
        var user = await _users.GetByIdAsync(userId);
        user.Email = newEmail;
        await _users.UpdateAsync(user);
    }
}
```

Services are testable in complete isolation by passing in a fake repository — no database required.

### Repository Implementations (Infrastructure)
Concrete classes that fulfil the repository interfaces defined in Core. This is the only layer that knows about EF Core, PostgreSQL, and `DbContext`.

```csharp
public class UserRepository : IUserRepository
{
    private readonly SplitCentsDbContext _db;

    public UserRepository(SplitCentsDbContext db) => _db = db;

    public async Task<User?> GetByIdAsync(Guid id)
        => await _db.Users.FindAsync(id);
}
```

### Controllers (API)
Thin HTTP handlers. They receive requests, call a service, and return a response. No business logic lives here.

```csharp
[HttpPut("{id}/email")]
public async Task<IActionResult> UpdateEmail(Guid id, UpdateEmailRequest request)
{
    await _userService.UpdateEmailAsync(id, request.NewEmail);
    return Ok();
}
```

If a controller method is getting long, logic has likely leaked in from the service layer.

---

## Why Interfaces?

This was the central question explored in the conversation. The short answers:

1. **Architectural necessity** — Core cannot reference Infrastructure (that would reverse the dependency direction). The interface is the only mechanism that lets Core describe a need for data without depending on who provides it.

2. **Testability** — Services in Core depend on `IUserRepository`, not `UserRepository`. In tests, a fake implementation is passed in. No database is needed to test business logic.

3. **Swappability** — Any class that implements the interface can be substituted without changing the service. Today: PostgreSQL. Tomorrow: SQLite for offline. The service is unchanged.

4. **Compiler enforcement** — Any class that claims to implement an interface *must* have all its methods, or the project won't build. This is the guardrail.

---

## The Request Flow

For a `PUT /users/{id}/email` request:

```
1. API        receives the HTTP request
2. API        calls IUserService.UpdateEmailAsync(id, newEmail)
3. Service    calls IUserRepository.GetByIdAsync(id)
4. Infrastructure  fetches User from PostgreSQL
5. Service    applies business rules, updates user.Email
6. Service    calls IUserRepository.UpdateAsync(user)
7. Infrastructure  writes updated User back to PostgreSQL
8. API        returns 200 OK
```

Infrastructure is called twice — once to fetch, once to save — always in response to the service, never initiating on its own.

---

## File Count vs Clarity

This pattern produces many files, most of them short. That is intentional. Each file has one job and one reason to change:

| Something is wrong with... | Open... |
|---|---|
| Email update logic | `UserService.cs` |
| Wrong data from database | `UserRepository.cs` |
| Wrong HTTP status code | Controller |
| Shape of the data | `User.cs` |

Fewer, larger files feel simpler initially but become hard to navigate and change safely as the app grows.

---

## Does This Apply Beyond C#?

Yes — layered architecture is a universal pattern. What changes is how strictly the language enforces it:

| Language | How contracts are expressed |
|---|---|
| C# / Java / Kotlin | Interfaces — compiler enforced |
| TypeScript | Interfaces — compiler enforced |
| Python | Abstract base classes or type hints — convention enforced |
| Go | Implicit interfaces — structural typing |
| Ruby / PHP | Convention only |

In Python/FastAPI the same structure is used with `ABC` (abstract base classes) in place of interfaces. The compiler won't stop you from skipping the abstraction, but projects that grow beyond a small size almost always adopt it regardless.
