using Dapper;
using eAgendaWeb.Compartilhado.Infra.Sql;
using eAgendaWeb.Modulos.ModuloCategorias.Dominio;
using Microsoft.Data.SqlClient;

namespace eAgendaWeb.Modulos.ModuloCategorias.Infra;

public sealed class RepositorioCategoria(ISqlConnectionFactory connectionFactory)
    : IRepositorioCategoria
{
    private const string InserirSql = """
    INSERT INTO dbo.TBCategoria (Id, Titulo)
    VALUES (@Id, @Titulo);
""";

    private const string AtualizarSql = """
    UPDATE dbo.TBCategoria
    SET
        Titulo = @Titulo
    WHERE Id = @Id;
""";

    private const string ExcluirSql = """
    DELETE FROM dbo.TBCategoria
    WHERE Id = @Id;
""";

    private const string SelecionarPorIdSql = """
    SELECT Id, Titulo
    FROM dbo.TBCategoria
    WHERE Id = @Id;
""";

    private const string SelecionarTodosSql = """
    SELECT Id, Titulo
    FROM dbo.TBCategoria
    ORDER BY Titulo;
""";

    public void Cadastrar(Categoria entidade)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        conexao.Execute(InserirSql, entidade);
    }

    public bool Editar(Guid idSelecionado, Categoria entidadeAtualizada)
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

    public Categoria? SelecionarPorId(Guid idSelecionado)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        return conexao.QuerySingleOrDefault<Categoria>(
            SelecionarPorIdSql,
            new { Id = idSelecionado });
    }

    public List<Categoria> SelecionarTodos()
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        return conexao.Query<Categoria>(SelecionarTodosSql).ToList();
    }

    public List<Categoria> Filtrar(Predicate<Categoria> filtro)
    {
        return SelecionarTodos().FindAll(filtro);
    }
}