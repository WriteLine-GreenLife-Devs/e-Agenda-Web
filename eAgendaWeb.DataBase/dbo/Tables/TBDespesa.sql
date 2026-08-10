CREATE TABLE [dbo].[TBDespesa] (
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [Descricao]       NVARCHAR (100)   NOT NULL,
    [DataOcorrencia]  DATE             NULL DEFAULT (CONVERT(date, GETDATE())),
    [Valor]           DECIMAL (18, 2)  NOT NULL,
    [FormaPagamento]  INT              NOT NULL,
    [QuantidadeParcelas] INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_TBDespesa_FormaPagamento] CHECK ([FormaPagamento] IN (1, 2, 3))
);
GO
