CREATE TABLE [dbo].[TBTarefa] (
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [Titulo]          NVARCHAR (100)   NOT NULL,
    [Prioridade]      INT              NOT NULL,
    [DataCriacao]     DATE             NOT NULL,
    [DataConclusao]   DATE             NULL,
    [StatusConclusao] BIT              NOT NULL,
    [Percentual]      DECIMAL (5, 2)   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_TBTarefa_Prioridade] CHECK ([Prioridade] IN (0, 1, 2)),
    CONSTRAINT [CK_TBTarefa_Percentual] CHECK ([Percentual] BETWEEN 0 AND 100)
);
GO
