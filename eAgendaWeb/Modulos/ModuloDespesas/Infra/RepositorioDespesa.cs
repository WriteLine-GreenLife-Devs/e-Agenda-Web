using Dapper;
using eAgendaWeb.Compartilhado.Infra.Sql;
using eAgendaWeb.Modulos.ModuloDespesas.Dominio;
using Microsoft.Data.SqlClient;

namespace eAgendaWeb.Modulos.ModuloDespesas.Infra;

public sealed class RepositorioDespesa(ISqlConnectionFactory connectionFactory)
    : IRepositorioDespesa
{
    private const string InserirSql = """
    INSERT INTO dbo.TBDespesa (
        Id,
        Descricao,
        DataOcorrencia,
        Valor,
        FormaPagamento
    )
    VALUES (
        @Id,
        @Descricao,
        @DataOcorrencia,
        @Valor,
        @FormaPagamento
    );
""";

    private const string AtualizarSql = """
    UPDATE dbo.TBDespesa
    SET
        Descricao = @Descricao,
        DataOcorrencia = @DataOcorrencia,
        Valor = @Valor,
        FormaPagamento = @FormaPagamento
    WHERE Id = @Id;
""";

    private const string ExcluirSql = """
    DELETE FROM dbo.TBDespesa
    WHERE Id = @Id;
""";

    private const string SelecionarPorIdSql = """
    SELECT
        Id,
        Descricao,
        DataOcorrencia,
        Valor,
        FormaPagamento
    FROM dbo.TBDespesa
    WHERE Id = @Id;
""";

    private const string SelecionarTodosSql = """
    SELECT
        Id,
        Descricao,
        DataOcorrencia,
        Valor,
        FormaPagamento
    FROM dbo.TBDespesa
    ORDER BY DataOcorrencia DESC, Descricao;
""";

    public void Cadastrar(Despesa entidade)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        conexao.Execute(
            InserirSql,
            entidade
        );
    }

    public bool Editar(Guid idSelecionado, Despesa entidadeAtualizada)
    {
        entidadeAtualizada.Id = idSelecionado;

        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        int registrosAfetados = conexao.Execute(
            AtualizarSql,
            entidadeAtualizada
        );

        return registrosAfetados > 0;
    }

    public bool Excluir(Guid idSelecionado)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        int registrosAfetados = conexao.Execute(
            ExcluirSql,
            new { Id = idSelecionado }
        );

        return registrosAfetados > 0;
    }

    public Despesa? SelecionarPorId(Guid idSelecionado)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        return conexao.QueryFirstOrDefault<Despesa>(
            SelecionarPorIdSql,
            new { Id = idSelecionado }
        );
    }

    public List<Despesa> SelecionarTodos()
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        conexao.Open();

        return conexao.Query<Despesa>(
            SelecionarTodosSql
        ).ToList();
    }

    public List<Despesa> Filtrar(Predicate<Despesa> filtro)
    {
        return SelecionarTodos()
            .FindAll(filtro);
    }
}