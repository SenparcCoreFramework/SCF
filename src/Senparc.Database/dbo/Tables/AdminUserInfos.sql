CREATE TABLE [dbo].[AdminUserInfos] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [UserName]      NVARCHAR (50)  NULL,
    [Password]      NVARCHAR (50)  NULL,
    [PasswordSalt]  VARCHAR (100)  NULL,
    [RealName]      NVARCHAR (50)  NULL,
    [Phone]         VARCHAR (20)   NULL,
    [Note]          NVARCHAR (MAX) NULL,
    [ThisLoginTime] DATETIME       NOT NULL,
    [ThisLoginIP]   VARCHAR (20)   NULL,
    [LastLoginTime] DATETIME       NOT NULL,
    [LastLoginIP]   VARCHAR (20)   NULL,
    [AddTime]       DATETIME       NOT NULL,
    CONSTRAINT [PK_AdminUserInfo] PRIMARY KEY CLUSTERED ([Id] ASC)
);



