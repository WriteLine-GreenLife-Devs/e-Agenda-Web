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
        @Local,
        @Link,
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
        Local = @Local,
        Link = @Link,
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
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        conexao.Execute(InserirSql, entidade);
    }

    public bool Editar(Guid idSelecionado, Compromisso entidadeAtualizada)
    {
        entidadeAtualizada.Id = idSelecionado;

        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        return conexao.Execute(AtualizarSql, entidadeAtualizada) == 1;
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
