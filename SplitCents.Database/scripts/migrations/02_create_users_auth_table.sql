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
    [userId] UNIQUEIDENTIFIER NOT NULL,
    [hashedPassword] NVARCHAR(MAX) NOT NULL,
    CONSTRAINT [PK_UserAuth] PRIMARY KEY CLUSTERED ([userId]),
    CONSTRAINT [FK_UserAuth_UserId_Users_Id] FOREIGN KEY ([userId]) REFERENCES [dbo].[Users]([id])
        ON DELETE CASCADE
);