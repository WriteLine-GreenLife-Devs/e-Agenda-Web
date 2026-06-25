CREATE TABLE [dbo].[TBCategoria] (
    [Id]     UNIQUEIDENTIFIER NOT NULL,
    [Titulo] NVARCHAR (100)   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE UNIQUE INDEX [IX_TBCategoria_Titulo] ON [dbo].[TBCategoria] ([Titulo]);
GO
