using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using eAgendaWeb.Compartilhado.Infra.Sql;
using eAgendaWeb.Modulos.ModuloTarefas.Dominio;
using Microsoft.Data.SqlClient;

namespace eAgendaWeb.Modulos.ModuloTarefas.Infra;

public sealed class RepositorioTarefa(ISqlConnectionFactory connectionFactory) : IRepositorioTarefa
{
    private const string CadastrarSql = """
        INSERT INTO dbo.TBTarefa (
            Id,
            Titulo,
            Prioridade,
            DataCriacao,
            DataConclusao,
            StatusConclusao,
            Percentual
        )
        VALUES (
            @Id,
            @Titulo,
            @Prioridade,
            @DataCriacao,
            @DataConclusao,
            @StatusConclusao,
            @PercentualConcluido
        );
        """;

    private const string CadastrarItemSql = """
        INSERT INTO dbo.TBItemTarefa (
            Id,
            Titulo,
            Concluido,
            TarefaId
        )
        VALUES (
            @Id,
            @Titulo,
            @Concluido,
            @TarefaId
        );
        """;

    private const string EditarSql = """
        UPDATE dbo.TBTarefa 
        SET 
            Titulo = @Titulo, 
            Prioridade = @Prioridade, 
            DataConclusao = @DataConclusao,
            StatusConclusao = @StatusConclusao, 
            Percentual = @PercentualConcluido 
        WHERE 
            Id = @Id;
        """;

    private const string ExcluirItensSql = """
        DELETE FROM dbo.TBItemTarefa 
        WHERE TarefaId = @Id;
        """;

    private const string ExcluirSql = """
        DELETE FROM dbo.TBTarefa 
        WHERE Id = @Id;
        """;

    private const string SelecionarPorIdSql = """
        SELECT
            Id,
            Titulo,
            Prioridade,
            DataCriacao,
            DataConclusao,
            StatusConclusao,
            Percentual AS PercentualConcluido
        FROM dbo.TBTarefa 
        WHERE Id = @Id;
        """;

    private const string SelecionarItensSql = """
        SELECT * FROM dbo.TBItemTarefa 
        WHERE TarefaId = @Id;
        """;

    private const string SelecionarTodosSql = """
        SELECT
            Id,
            Titulo,
            Prioridade,
            DataCriacao,
            DataConclusao,
            StatusConclusao,
            Percentual AS PercentualConcluido
        FROM dbo.TBTarefa;
        """;

    public void Cadastrar(Tarefa entidade)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();
        conexao.Open();
        using SqlTransaction transacao = conexao.BeginTransaction();

        try
        {
            conexao.Execute(CadastrarSql, entidade, transacao);

            foreach (ItemTarefa item in entidade.Itens)
            {
                conexao.Execute(CadastrarItemSql, new
                {
                    item.Id,
                    item.Titulo,
                    item.Concluido,
                    TarefaId = entidade.Id
                }, transacao);
            }

            transacao.Commit();
        }
        catch
        {
            transacao.Rollback();
            throw;
        }
    }

    public bool Editar(Guid idSelecionado, Tarefa entidadeAtualizada)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();
        conexao.Open();
        using SqlTransaction transacao = conexao.BeginTransaction();

        try
        {
            entidadeAtualizada.Id = idSelecionado;
            int linhasAfetadas = conexao.Execute(EditarSql, entidadeAtualizada, transacao);

            if (linhasAfetadas == 0)
                return false;

            conexao.Execute(ExcluirItensSql, new { Id = idSelecionado }, transacao);

            foreach (ItemTarefa item in entidadeAtualizada.Itens)
            {
                conexao.Execute(CadastrarItemSql, new
                {
                    item.Id,
                    item.Titulo,
                    item.Concluido,
                    TarefaId = idSelecionado
                }, transacao);
            }

            transacao.Commit();
            return true;
        }
        catch
        {
            transacao.Rollback();
            throw;
        }
    }

    public bool Excluir(Guid idSelecionado)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();
        conexao.Open();
        using SqlTransaction transacao = conexao.BeginTransaction();

        try
        {
            conexao.Execute(ExcluirItensSql, new { Id = idSelecionado }, transacao);
            int linhasAfetadas = conexao.Execute(ExcluirSql, new { Id = idSelecionado }, transacao);

            transacao.Commit();
            return linhasAfetadas > 0;
        }
        catch
        {
            transacao.Rollback();
            throw;
        }
    }

    public Tarefa? SelecionarPorId(Guid idSelecionado)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();

        Tarefa? tarefa = conexao.QuerySingleOrDefault<Tarefa>(SelecionarPorIdSql, new { Id = idSelecionado });

        if (tarefa != null)
        {
            List<ItemTarefa> itens = conexao.Query<ItemTarefa>(SelecionarItensSql, new { Id = idSelecionado }).ToList();

            foreach (ItemTarefa item in itens)
            {
                tarefa.AdicionarItem(item);
            }
        }

        return tarefa;
    }

    public List<Tarefa> SelecionarTodos()
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();
        return conexao.Query<Tarefa>(SelecionarTodosSql).ToList();
    }

    public List<Tarefa> Filtrar(Predicate<Tarefa> filtro)
    {
        using SqlConnection conexao = connectionFactory.CreateConnection();
        return conexao.Query<Tarefa>(SelecionarTodosSql).Where(filtro.Invoke).ToList();
    }
}