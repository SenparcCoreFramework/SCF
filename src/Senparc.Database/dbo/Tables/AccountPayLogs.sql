CREATE TABLE [dbo].[AccountPayLogs] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [AccountId]    INT             NOT NULL,
    [OrderNumber]  NVARCHAR (MAX)  NULL,
    [TotalPrice]   DECIMAL (18, 2) NOT NULL,
    [PayMoney]     DECIMAL (18, 2) NOT NULL,
    [UsedPoints]   DECIMAL (18, 2) NULL,
    [AddTime]      DATETIME2 (7)   NOT NULL,
    [CompleteTime] DATETIME2 (7)   NOT NULL,
    [AddIp]        NVARCHAR (MAX)  NULL,
    [GetPoints]    DECIMAL (18, 2) NOT NULL,
    [Status]       TINYINT         NOT NULL,
    [Description]  NVARCHAR (MAX)  NULL,
    [Type]         TINYINT         NULL,
    [TradeNumber]  NVARCHAR (MAX)  NULL,
    [PrepayId]     NVARCHAR (MAX)  NULL,
    [PayType]      INT             NOT NULL,
    [OrderType]    INT             NOT NULL,
    [PayParam]     NVARCHAR (MAX)  NULL,
    [Price]        DECIMAL (18, 2) NOT NULL,
    [Fee]          DECIMAL (18, 2) NOT NULL,
    CONSTRAINT [PK_AccountPayLogs] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AccountPayLogs_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Accounts] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_AccountPayLogs_AccountId]
    ON [dbo].[AccountPayLogs]([AccountId] ASC);

