/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

[dbo].[Users]
Stores the core profile information for a user.
Each user has a unique id, which acts as the primary key.
The email and displayName are unique so a user cannot register with the same email
or display name as another user.
First and last name are stored as profile fields and default to an empty string if not provided.
This table contains public or general account information and is intentionally separate
from authentication-related data.

 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

CREATE TABLE [dbo].[Users]
(
    [id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [email] NVARCHAR(254) NOT NULL UNIQUE,
    [displayName] NVARCHAR(30) NOT NULL UNIQUE,
    [firstName] NVARCHAR(100) NOT NULL DEFAULT N'',
    [lastName] NVARCHAR(100) NOT NULL DEFAULT N''
);

CREATE UNIQUE INDEX [IX_Users_Email] ON [dbo].[Users] ([email]);
CREATE UNIQUE INDEX [IX_Users_DisplayName] ON [dbo].[Users] ([displayName]);
