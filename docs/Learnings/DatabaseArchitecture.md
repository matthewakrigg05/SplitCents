# Database Architecture in SplitCents

## Overview

SplitCents is moving toward a database-first design using PostgreSQL. The database schema and SQL migrations are treated as the authoritative definition of the database. Entity Framework Core remains responsible for mapping the application models to PostgreSQL and accessing the database, but it does not implicitly define or alter the schema.

The database will remain in the SplitCents repository so application changes, SQL migrations, infrastructure mappings, and tests can be reviewed together.

## Current Application Database

The operational database is the source of truth for the application. It should be normalized and designed for reliable transactional operations such as registering users, recording transactions, and updating budgets.

A possible structure is:

```text
splitcents_app
├── auth
│   └── users
├── app
│   ├── user_profiles
│   ├── households
│   ├── household_members
│   ├── transactions
│   ├── transaction_types
│   ├── transaction_sources
│   └── budgets
└── audit
    └── events
```

PostgreSQL schemas provide namespaces and useful permission boundaries:

- `auth` contains authentication identity data.
- `app` contains application and domain data.
- `audit` contains security and operational audit records.
- `analytics` may contain reporting views or derived summaries later.

## Authentication and Personal Data

Passwords must never be stored directly. The application stores only a slow, salted password hash, such as a BCrypt or Argon2id hash. Password hashes remain sensitive and require restricted access, secure backups, and protection from logs and source control.

Authentication and profile data may be separated logically without requiring separate databases:

```text
auth.users
----------
id
email
password_hash
created_at

app.user_profiles
-----------------
user_id
first_name
last_name
display_name
```

The email address can remain in `auth.users` when it is used as a login identifier. Analytics should use a user ID or other pseudonymous identifier rather than exposing names and email addresses unnecessarily.

JWTs should contain only the claims needed by the API, such as the subject, issuer, audience, token ID, and expiry. JWT contents are encoded rather than encrypted, so sensitive information must not be placed in them.

## Normalization

The operational database should be normalized so that each important fact has one authoritative location. Foreign keys, unique constraints, appropriate data types, and indexes should enforce the relationships and rules in PostgreSQL.

For example, transactions should refer to a transaction type rather than storing a repeated type name on every row:

```text
app.transaction_types
---------------------
id
name

app.transactions
----------------
id
transaction_type_id
amount
occurred_at
```

Denormalization should be introduced only when there is a measured performance or reporting need. Derived summaries can be represented by views, materialized views, or rebuildable reporting tables.

## SQL Migrations

SQL migrations should be stored in the repository and applied in one globally ordered sequence, even if PostgreSQL schemas are used to separate responsibilities:

```text
database/
├── migrations/
│   ├── 001_create_auth_schema.sql
│   ├── 002_create_users_table.sql
│   ├── 003_create_app_schema.sql
│   └── 004_create_transaction_tables.sql
├── seeds/
│   └── development.sql
├── scripts/
│   ├── create-database.ps1
│   ├── migrate.ps1
│   ├── reset-database.ps1
│   └── verify-database.ps1
└── README.md
```

A migration is an immutable, versioned database change. Once it has been applied outside a disposable local database, it should not be edited; a new migration should be added instead.

The database setup scripts can ensure that the local PostgreSQL database exists, apply pending migrations, seed development data, reset a disposable database, and verify the expected schema. Credentials must come from environment variables, user secrets, or local configuration rather than committed files.

## Optional Warehouse

SplitCents does not currently require a separate data warehouse. A normalized PostgreSQL application database should be built first. A warehouse can be added later as a downstream system or as a separate learning project.

A warehouse becomes more useful when the application needs to combine multiple data sources, preserve large amounts of history, support many analytical queries, or keep reporting workloads away from the operational database.

## Optional Medallion Architecture

If a warehouse is introduced, a medallion architecture can separate processing stages:

```text
Bronze  -> source-shaped and traceable data
Silver  -> cleaned, standardized, and integrated data
Gold    -> curated analytical facts, dimensions, and summaries
```

For PostgreSQL, the medallion layers can be represented by separate databases:

```text
splitcents_bronze
splitcents_silver
splitcents_gold
```

This is different from the operational application database:

```text
splitcents_app
    source of truth for application operations

splitcents_bronze
splitcents_silver
splitcents_gold
    optional downstream analytical representations
```

Separate databases create a real operational boundary. PostgreSQL connections normally target one database at a time, so movement between warehouse layers requires an ETL process, scheduled job, foreign data wrapper, or similar mechanism. While learning locally, the layers could instead be represented by schemas in one database to reduce operational complexity.

## Warehouse Naming

In a warehouse, schemas can represent either data-processing stages or source systems, depending on the layer.

Bronze schemas may identify the source system:

```text
splitcents_bronze.core.trns01
splitcents_bronze.core.trns02
splitcents_bronze.bank.transactions
splitcents_bronze.card_provider.transactions
```

Here, `core` identifies the core system that produced the data. Other schemas identify external sources. Coded names such as `trns01` are acceptable for bronze objects when they preserve source-system identifiers and are documented in a data catalog.

Silver and gold objects should generally use descriptive names because they are cleaned and consumed by developers or analysts:

```text
splitcents_silver.transaction.transactions
splitcents_silver.transaction.transaction_types

splitcents_gold.transaction.fact_transaction
splitcents_gold.transaction.dim_transaction_type
```

The naming principle is:

- Bronze identifies where data came from.
- Silver identifies what the standardized data means.
- Gold identifies how the data is consumed analytically.

The `01` in `trns01` represents the source or group identifier in the catalog. It should not be confused with a migration version. Migration versions belong in migration filenames, while source identifiers belong in source catalogs and bronze object names.

## Data Flow

The optional downstream flow would be:

```text
splitcents_app
    |
    | extract, export, or event stream
    v
splitcents_bronze
├── core.trns01
├── core.trns02
└── external_source.transactions
    |
    | clean, standardize, validate, and combine
    v
splitcents_silver
└── transaction.transactions
    |
    | model for reporting
    v
splitcents_gold
├── transaction.fact_transaction
└── transaction.dim_transaction_type
```

The application should not depend on the warehouse for ordinary operations. The warehouse is derived and should be rebuildable from the operational source and its ingestion history.

## Current Recommendation

Build the normalized operational PostgreSQL database first and keep its migrations in the SplitCents repository. Use descriptive names in the application database, such as `app.transactions` and `app.transaction_types`.

Treat the bronze-to-silver-to-gold warehouse as optional. It is a useful future learning project and may become valuable if SplitCents later integrates bank, card, investment, or other external data sources. Until then, analytics views or materialized views inside the application database are likely sufficient for application dashboards.
