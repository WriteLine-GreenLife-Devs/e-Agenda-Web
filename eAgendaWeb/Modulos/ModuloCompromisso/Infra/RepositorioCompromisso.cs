using Dapper;
using eAgendaWeb.Compartilhado.Dominio;
using eAgendaWeb.Compartilhado.Infra.Sql;
using eAgendaWeb.Modulos.ModuloCompromisso.Dominio;
using Microsoft.Data.SqlClient;

namespace eAgendaWeb.Modulos.ModuloCompromisso.Infra;

public sealed class RepositorioCompromisso(ISqlConnectionFactory connectionFactory)
    : IRepositorioCompromisso
{
    private const string InserirSql = """
    INSERT INTO dbo.TBCompromisso (
        Id,
        Assunto,
        DataOcorrencia,
        HoraInicio,
        HoraTermino,
        TipoCompromisso,
        Local,
        Link,
        ContatoId
    )
    VALUES (
        @Id,
        @Assunto,
        @DataOcorrencia,
        @HoraInicio,
        @HoraTermino,
        @TipoCompromisso,
        NULLIF(@Local, ''),
        NULLIF(@Link, ''),
        @ContatoId
    );
""";

    private const string AtualizarSql = """
    UPDATE dbo.TBCompromisso
    SET
        Assunto = @Assunto,
        DataOcorrencia = @DataOcorrencia,
        HoraInicio = @HoraInicio,
        HoraTermino = @HoraTermino,
        TipoCompromisso = @TipoCompromisso,
        Local = NULLIF(@Local, ''),
        Link = NULLIF(@Link, ''),
        ContatoId = @ContatoId
    WHERE Id = @Id;
""";

    private const string ExcluirSql = """
    DELETE FROM dbo.TBCompromisso
    WHERE Id = @Id;
""";

    private const string SelecionarPorIdSql = """
    SELECT Id,
           Assunto,
           DataOcorrencia,
           HoraInicio,
           HoraTermino,
           TipoCompromisso,
           Local,
           Link,
           ContatoId
    FROM dbo.TBCompromisso
    WHERE Id = @Id;
""";

    private const string SelecionarTodosSql = """
    SELECT Id,
           Assunto,
           DataOcorrencia,
           HoraInicio,
           HoraTermino,
           TipoCompromisso,
           Local,
           Link,
           ContatoId
    FROM dbo.TBCompromisso
    ORDER BY DataOcorrencia, HoraInicio;
""";

    public void Cadastrar(Compromisso entidade)
    {
        var parametros = new
        {
            entidade.Id,
            entidade.Assunto,
            entidade.DataOcorrencia,
            entidade.HoraInicio,
            entidade.HoraTermino,
            TipoCompromisso = entidade.TipoCompromisso.ToString(),
            Local = string.IsNullOrWhiteSpace(entidade.Local) ? null : entidade.Local,
            Link = string.IsNullOrWhiteSpace(entidade.Link) ? null : entidade.Link,
            entidade.ContatoId
        };

        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        conexao.Execute(InserirSql, parametros);
    }

    public bool Editar(Guid idSelecionado, Compromisso entidadeAtualizada)
    {
        entidadeAtualizada.Id = idSelecionado;

        var parametros = new
        {
            entidadeAtualizada.Id,
            entidadeAtualizada.Assunto,
            entidadeAtualizada.DataOcorrencia,
            entidadeAtualizada.HoraInicio,
            entidadeAtualizada.HoraTermino,
            TipoCompromisso = entidadeAtualizada.TipoCompromisso.ToString(),
            Local = string.IsNullOrWhiteSpace(entidadeAtualizada.Local) ? null : entidadeAtualizada.Local,
            Link = string.IsNullOrWhiteSpace(entidadeAtualizada.Link) ? null : entidadeAtualizada.Link,
            entidadeAtualizada.ContatoId
        };

        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        return conexao.Execute(AtualizarSql, parametros) == 1;
    }

    public bool Excluir(Guid idSelecionado)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        return conexao.Execute(ExcluirSql, new { Id = idSelecionado }) == 1;
    }

    public Compromisso? SelecionarPorId(Guid idSelecionado)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        return conexao.QuerySingleOrDefault<Compromisso>(SelecionarPorIdSql, new { Id = idSelecionado });
    }

    public List<Compromisso> SelecionarTodos()
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        return conexao.Query<Compromisso>(SelecionarTodosSql).ToList();
    }

    public List<Compromisso> Filtrar(Predicate<Compromisso> filtro)
    {
        return SelecionarTodos().FindAll(filtro);
    }
}
