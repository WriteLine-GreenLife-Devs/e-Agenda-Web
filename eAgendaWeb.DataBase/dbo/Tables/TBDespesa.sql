CREATE TABLE [dbo].[TBDespesa] (
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [Descricao]       NVARCHAR (100)   NOT NULL,
    [DataOcorrencia]  DATE             NULL DEFAULT (CONVERT(date, GETDATE())),
    [Valor]           DECIMAL (18, 2)  NOT NULL,
    [FormaPagamento]  NVARCHAR (20)    NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_TBDespesa_FormaPagamento] CHECK ([FormaPagamento] IN (N'À Vista', N'Crédito', N'Débito'))
);
GO
