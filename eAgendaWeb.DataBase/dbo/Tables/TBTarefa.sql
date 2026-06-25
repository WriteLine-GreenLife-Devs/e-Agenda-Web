CREATE TABLE [dbo].[TBTarefa] (
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [Titulo]          NVARCHAR (100)   NOT NULL,
    [Prioridade]      NVARCHAR (10)    NOT NULL,
    [DataCriacao]     DATE             NOT NULL,
    [DataConclusao]   DATE             NOT NULL,
    [StatusConclusao] NVARCHAR (20)    NOT NULL,
    [Percentual]      TINYINT          NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_TBTarefa_Prioridade] CHECK ([Prioridade] IN ('Baixa', 'Normal', 'Alta')),
    CONSTRAINT [CK_TBTarefa_StatusConclusao] CHECK ([StatusConclusao] IN ('Pendente', 'Concluída', 'Em Andamento')),
    CONSTRAINT [CK_TBTarefa_Percentual] CHECK ([Percentual] BETWEEN 0 AND 100)
);
GO
