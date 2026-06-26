using Dapper;
using eAgendaWeb.Compartilhado.Dominio;
using eAgendaWeb.Compartilhado.Infra.Sql;
using eAgendaWeb.Modulos.ModuloContatos.Dominio;
using Microsoft.Data.SqlClient;

namespace eAgendaWeb.Modulos.ModuloContatos.Infra;

public sealed class RepositorioContato(ISqlConnectionFactory connectionFactory)
    : IRepositorioContato
{
    private const string InserirSql = """
    INSERT INTO dbo.TBContato (Id, Nome, Email, Telefone, Cargo, Empresa)
    VALUES (@Id, @Nome, @Email, @Telefone, @Cargo, @Empresa);
""";

    private const string AtualizarSql = """
    UPDATE dbo.TBContato
    SET
        Nome = @Nome,
        Email = @Email,
        Telefone = @Telefone,
        Cargo = @Cargo,
        Empresa = @Empresa
    WHERE Id = @Id;
""";

    private const string ExcluirSql = """
    DELETE FROM dbo.TBContato
    WHERE Id = @Id;
""";

    private const string SelecionarPorIdSql = """
    SELECT Id, Nome, Email, Telefone, Cargo, Empresa
    FROM dbo.TBContato
    WHERE Id = @Id;
""";

    private const string SelecionarTodosSql = """
    SELECT Id, Nome, Email, Telefone, Cargo, Empresa
    FROM dbo.TBContato
    ORDER BY Nome;
""";

    public void Cadastrar(Contato entidade)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        conexao.Execute(InserirSql, entidade);
    }

    public bool Editar(Guid idSelecionado, Contato entidadeAtualizada)
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

    public Contato? SelecionarPorId(Guid idSelecionado)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        return conexao.QuerySingleOrDefault<Contato>(SelecionarPorIdSql, new { Id = idSelecionado });
    }

    public List<Contato> SelecionarTodos()
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        return conexao.Query<Contato>(SelecionarTodosSql).ToList();
    }

    public List<Contato> Filtrar(Predicate<Contato> filtro)
    {
        return SelecionarTodos().FindAll(filtro);
    }
}
