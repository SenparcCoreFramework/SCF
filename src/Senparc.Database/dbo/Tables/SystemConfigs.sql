CREATE TABLE [dbo].[SystemConfigs] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [SystemName]  NVARCHAR (100) NOT NULL,
    [MchId]       VARCHAR (100)  NULL,
    [MchKey]      VARCHAR (300)  NULL,
    [TenPayAppId] VARCHAR (100)  NULL,
    CONSTRAINT [PK_SystemConfig] PRIMARY KEY CLUSTERED ([Id] ASC)
);







