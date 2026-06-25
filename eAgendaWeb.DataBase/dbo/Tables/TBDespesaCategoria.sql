CREATE TABLE [dbo].[TBDespesaCategoria] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [DespesaId]   UNIQUEIDENTIFIER NOT NULL,
    [CategoriaId] UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

ALTER TABLE [dbo].[TBDespesaCategoria]
    ADD CONSTRAINT [FK_TBDespesaCategoria_TBDespesa] FOREIGN KEY ([DespesaId]) REFERENCES [dbo].[TBDespesa] ([Id]);
GO

ALTER TABLE [dbo].[TBDespesaCategoria]
    ADD CONSTRAINT [FK_TBDespesaCategoria_TBCategoria] FOREIGN KEY ([CategoriaId]) REFERENCES [dbo].[TBCategoria] ([Id]);
GO
