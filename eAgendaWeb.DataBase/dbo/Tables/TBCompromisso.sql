CREATE TABLE [dbo].[TBCompromisso] (
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [Assunto]         NVARCHAR (100)   NOT NULL,
    [DataOcorrencia]  DATE             NOT NULL,
    [HoraInicio]      TIME             NOT NULL,
    [HoraTermino]     TIME             NOT NULL,
    [TipoCompromisso] NVARCHAR (10)    NOT NULL,
    [Local]           NVARCHAR (255)   NULL,
    [Link]            NVARCHAR (255)   NULL,
    [ContatoId]       UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_TBCompromisso_TipoCompromisso] CHECK ([TipoCompromisso] IN ('Remoto', 'Presencial')),
    CONSTRAINT [CK_TBCompromisso_LocalLink] CHECK (
        ([TipoCompromisso] = 'Presencial' AND [Local] IS NOT NULL AND [Link] IS NULL) OR
        ([TipoCompromisso] = 'Remoto' AND [Link] IS NOT NULL AND [Local] IS NULL)
    )
);
GO

ALTER TABLE [dbo].[TBCompromisso]
    ADD CONSTRAINT [FK_TBCompromisso_TBContato] FOREIGN KEY ([ContatoId]) REFERENCES [dbo].[TBContato] ([Id]);
GO
