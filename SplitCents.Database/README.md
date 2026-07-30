# SplitCents.Database

This folder contains the standalone database repository for SplitCents.

The database is intentionally kept separate from the main application source so it can be versioned, deployed, and operated independently.

## Repository structure

- `scripts/create/` - base schema creation scripts for new database provisioning
- `scripts/migrations/` - incremental database migration scripts
- `scripts/seed/` - optional data seeding scripts

## Usage

1. Apply the `scripts/create/` scripts in numeric order to provision the initial schema.
2. Apply `scripts/migrations/` scripts in numeric order when evolving the schema.
3. Run `scripts/seed/` scripts only when sample or required initial data is needed.

## Current schema

- `scripts/create/01_create_users_table.sql` - creates the initial `Users` table based on the current Core model.

These migration files are draft SQL for a PostgreSQL-oriented schema and are intended to be refined as the domain model becomes more explicit.
