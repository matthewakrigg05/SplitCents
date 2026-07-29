-- 01_create_users_table.sql
-- Initial schema for the SplitCents database.

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
