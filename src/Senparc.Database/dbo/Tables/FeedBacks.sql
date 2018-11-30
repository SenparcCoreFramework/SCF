CREATE TABLE [dbo].[FeedBacks] (
    [Flag]      BIT            NOT NULL,
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [AccountId] INT            NOT NULL,
    [Content]   NVARCHAR (MAX) NULL,
    [Module]    INT            NOT NULL,
    [AddTime]   DATETIME       NOT NULL,
    CONSTRAINT [PK_FeedBacks] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FeedBacks_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Accounts] ([Id]) ON DELETE CASCADE
);




GO
CREATE NONCLUSTERED INDEX [IX_FeedBacks_AccountId]
    ON [dbo].[FeedBacks]([AccountId] ASC);

