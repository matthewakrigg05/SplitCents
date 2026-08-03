/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

[dbo].[UserAuth]
Stores authentication-related data for a user.
This table is kept separate from the main user profile table to isolate sensitive
credential data and apply stricter access controls.
Each row contains one auth record for exactly one user, identified by userId.
The hashedPassword is stored instead of the plain-text password.
A foreign key ensures every auth record references a valid user.

 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */



CREATE TABLE [dbo].[UserAuth]
(
    [userId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [hashedPassword] NVARCHAR(MAX) NOT NULL,

    CONSTRAINT [FK_UserAuth_Users] FOREIGN KEY ([userId]) REFERENCES [dbo].[Users]([id])
        ON DELETE CASCADE
);