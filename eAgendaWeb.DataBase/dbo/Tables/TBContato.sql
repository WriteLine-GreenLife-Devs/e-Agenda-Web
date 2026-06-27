CREATE TABLE [dbo].[TBContato] (
    [Id]       UNIQUEIDENTIFIER NOT NULL,
    [Nome]     NVARCHAR (100)    NOT NULL,
    [Email]    NVARCHAR (255)    NOT NULL,
    [Telefone] NVARCHAR (16)     NOT NULL,
    [Cargo]    NVARCHAR (100)    NULL,
    [Empresa]  NVARCHAR (100)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_TBContato_Email] CHECK ([Email] LIKE '%_@_%._%'),
    CONSTRAINT [CK_TBContato_Telefone] CHECK ([Telefone] LIKE '(__) ____-____' OR [Telefone] LIKE '(__) _ ____-____')
);
GO

CREATE UNIQUE INDEX [IX_TBContato_Email] ON [dbo].[TBContato] ([Email]);
GO

CREATE UNIQUE INDEX [IX_TBContato_Telefone] ON [dbo].[TBContato] ([Telefone]);
GO
