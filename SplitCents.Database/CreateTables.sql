-- CreateTables.sql
-- SQL script for the SplitCents database based on the current Core models.

CREATE TABLE [dbo].[Users]
(
    [id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [email] NVARCHAR(254) NOT NULL,
    [hashedPassword] NVARCHAR(MAX) NOT NULL,
    [displayName] NVARCHAR(30) NOT NULL,
    [firstName] NVARCHAR(100) NOT NULL DEFAULT N'',
    [lastName] NVARCHAR(100) NOT NULL DEFAULT N''
);

CREATE UNIQUE INDEX [IX_Users_Email] ON [dbo].[Users] ([email]);
CREATE UNIQUE INDEX [IX_Users_DisplayName] ON [dbo].[Users] ([displayName]);
