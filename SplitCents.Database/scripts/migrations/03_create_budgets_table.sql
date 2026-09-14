-- 03_create_budgets_table.sql
-- Initial schema for the SplitCents database.

-- 03_create_budgets_table.sql
-- Initial schema additions: BudgetCategories and Budgets

CREATE TABLE [dbo].[BudgetCategories]
(
    [id] UNIQUEIDENTIFIER NOT NULL,
    [name] NVARCHAR(100) NOT NULL,
    [description] NVARCHAR(400) NULL,
    CONSTRAINT [PK_BudgetCategories] PRIMARY KEY CLUSTERED ([id])
);

CREATE TABLE [dbo].[Budgets]
(
    [id] UNIQUEIDENTIFIER NOT NULL,
    [userId] UNIQUEIDENTIFIER NOT NULL,
    [categoryId] UNIQUEIDENTIFIER NULL,
    [name] NVARCHAR(100) NOT NULL,
    [amount] DECIMAL(18,2) NOT NULL,
    [startDate] DATE NULL,
    [endDate] DATE NULL,
    [isActive] BIT NOT NULL DEFAULT (1),
    [createdAt] DATETIME2 NOT NULL DEFAULT (SYSUTCDATETIME()),
    [updatedAt] DATETIME2 NOT NULL DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT [PK_Budgets] PRIMARY KEY CLUSTERED ([id]),
    CONSTRAINT [FK_Budgets_UserId_Users_Id] FOREIGN KEY ([userId]) REFERENCES [dbo].[Users]([id]),
    CONSTRAINT [FK_Budgets_CategoryId_BudgetCategories_Id] FOREIGN KEY ([categoryId]) REFERENCES [dbo].[BudgetCategories]([id])
);

CREATE INDEX [IX_Budgets_UserId] ON [dbo].[Budgets] ([userId]);
CREATE INDEX [IX_Budgets_CategoryId] ON [dbo].[Budgets] ([categoryId]);
