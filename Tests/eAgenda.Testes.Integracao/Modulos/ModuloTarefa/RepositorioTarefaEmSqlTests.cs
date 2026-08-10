using Dapper;
using eAgenda.Testes.Integracao.Compartilhado.Sql;
using eAgendaWeb.Modulos.ModuloTarefas.Aplicacao;
using eAgendaWeb.Modulos.ModuloTarefas.Dominio;

namespace eAgenda.Testes.Integracao.Modulos.ModuloTarefa;

[TestClass]
public sealed class RepositorioTarefaEmSqlTests : RepositorioBaseEmSqlTests
{
    [TestMethod]
    public void CadastrarESelecionarPorId_ComDadosValidos_DeveCarregarTarefa()
    {
        // Arranjo
        Tarefa tarefa = new("Estudar integração", PrioridadeTarefa.Alta);

        // Ação
        repositorioTarefa.Cadastrar(tarefa);
        Tarefa? tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefa.Id);

        // Asserção
        Assert.IsNotNull(tarefaSelecionada);
        Assert.AreEqual(tarefa.Id, tarefaSelecionada.Id);
        Assert.AreEqual("Estudar integração", tarefaSelecionada.Titulo);
        Assert.AreEqual(PrioridadeTarefa.Alta, tarefaSelecionada.Prioridade);
    }

    [TestMethod]
    public void Cadastrar_TarefaNova_DevePersistirEstadoInicial()
    {
        // Arranjo
        Tarefa tarefa = new("Estudar integração", PrioridadeTarefa.Normal);

        // Ação
        repositorioTarefa.Cadastrar(tarefa);
        Tarefa? tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefa.Id);

        // Asserção
        Assert.IsNotNull(tarefaSelecionada);
        Assert.IsFalse(tarefaSelecionada.StatusConclusao);
        Assert.AreEqual(0m, tarefaSelecionada.PercentualConcluido);
        Assert.IsNull(tarefaSelecionada.DataConclusao);
        Assert.AreEqual(DateTime.Today, tarefaSelecionada.DataCriacao.Date);
    }

    [TestMethod]
    public void Cadastrar_SemItens_DevePersistirTarefaComListaVazia()
    {
        // Arranjo
        Tarefa tarefa = new("Estudar integração", PrioridadeTarefa.Normal);

        // Ação
        repositorioTarefa.Cadastrar(tarefa);
        Tarefa? tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefa.Id);

        // Asserção
        Assert.IsNotNull(tarefaSelecionada);
        Assert.IsEmpty(tarefaSelecionada.Itens);
    }

    [TestMethod]
    public void Cadastrar_ComItens_DevePersistirTarefaEItensVinculados()
    {
        // Arranjo
        Tarefa tarefa = new("Estudar integração", PrioridadeTarefa.Alta);
        tarefa.AdicionarItem(new ItemTarefa("Revisar conteúdo"));
        tarefa.AdicionarItem(new ItemTarefa("Resolver exercícios"));

        // Ação
        repositorioTarefa.Cadastrar(tarefa);
        Tarefa? tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefa.Id);

        // Asserção
        Assert.IsNotNull(tarefaSelecionada);
        Assert.HasCount(2, tarefaSelecionada.Itens);
        Assert.IsTrue(tarefaSelecionada.Itens.All(i => i.TarefaId == tarefa.Id));
        Assert.AreEqual(0m, tarefaSelecionada.PercentualConcluido);
    }

    [TestMethod]
    public void Editar_ComDadosValidos_DeveAtualizarTarefaEItens()
    {
        // Arranjo
        Tarefa tarefa = new("Estudar integração", PrioridadeTarefa.Normal);
        tarefa.AdicionarItem(new ItemTarefa("Item original"));
        repositorioTarefa.Cadastrar(tarefa);

        Tarefa tarefaAtualizada = new("Revisar integração", PrioridadeTarefa.Alta);
        tarefaAtualizada.AdicionarItem(new ItemTarefa("Novo item"));

        // Ação
        bool conseguiuEditar = repositorioTarefa.Editar(tarefa.Id, tarefaAtualizada);
        Tarefa? tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefa.Id);

        // Asserção
        Assert.IsTrue(conseguiuEditar);
        Assert.IsNotNull(tarefaSelecionada);
        Assert.AreEqual("Revisar integração", tarefaSelecionada.Titulo);
        Assert.AreEqual(PrioridadeTarefa.Alta, tarefaSelecionada.Prioridade);
        Assert.HasCount(1, tarefaSelecionada.Itens);
        Assert.AreEqual("Novo item", tarefaSelecionada.Itens.Single().Titulo);
    }

    [TestMethod]
    public void Editar_ConcluindoUltimoItem_DevePersistirConclusaoDaTarefa()
    {
        // Arranjo
        Tarefa tarefa = CriarTarefaComItem(false);
        repositorioTarefa.Cadastrar(tarefa);

        Tarefa tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefa.Id)!;
        tarefaSelecionada.ConcluirItem(tarefaSelecionada.Itens.Single().Id);

        // Ação
        bool conseguiuEditar = repositorioTarefa.Editar(tarefa.Id, tarefaSelecionada);
        Tarefa? tarefaRecarregada = repositorioTarefa.SelecionarPorId(tarefa.Id);

        // Asserção
        Assert.IsTrue(conseguiuEditar);
        Assert.IsNotNull(tarefaRecarregada);
        Assert.IsTrue(tarefaRecarregada.StatusConclusao);
        Assert.AreEqual(100m, tarefaRecarregada.PercentualConcluido);
        Assert.IsNotNull(tarefaRecarregada.DataConclusao);
    }

    [TestMethod]
    public void Editar_DesmarcandoItem_DevePersistirReaberturaDaTarefa()
    {
        // Arranjo
        Tarefa tarefa = CriarTarefaComItem(true);
        repositorioTarefa.Cadastrar(tarefa);

        Tarefa tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefa.Id)!;
        tarefaSelecionada.DesmarcarItem(tarefaSelecionada.Itens.Single().Id);

        // Ação
        bool conseguiuEditar = repositorioTarefa.Editar(tarefa.Id, tarefaSelecionada);
        Tarefa? tarefaRecarregada = repositorioTarefa.SelecionarPorId(tarefa.Id);

        // Asserção
        Assert.IsTrue(conseguiuEditar);
        Assert.IsNotNull(tarefaRecarregada);
        Assert.IsFalse(tarefaRecarregada.StatusConclusao);
        Assert.AreEqual(0m, tarefaRecarregada.PercentualConcluido);
        Assert.IsNull(tarefaRecarregada.DataConclusao);
    }

    [TestMethod]
    public void SelecionarTodos_ComTarefasCadastradas_DeveCarregarTodosRegistros()
    {
        // Arranjo
        repositorioTarefa.Cadastrar(new Tarefa("Tarefa pendente", PrioridadeTarefa.Normal));
        repositorioTarefa.Cadastrar(CriarTarefaComItem(true));

        // Ação
        List<Tarefa> tarefas = repositorioTarefa.SelecionarTodos();

        // Asserção
        Assert.HasCount(2, tarefas);
    }

    [TestMethod]
    public void Filtrar_ComTarefasCadastradas_DeveCarregarSomentePendentes()
    {
        // Arranjo
        Tarefa pendente = new("Tarefa pendente", PrioridadeTarefa.Normal);
        repositorioTarefa.Cadastrar(pendente);
        repositorioTarefa.Cadastrar(CriarTarefaComItem(true));

        // Ação
        List<Tarefa> tarefas = repositorioTarefa.Filtrar(t => !t.StatusConclusao);

        // Asserção
        Assert.HasCount(1, tarefas);
        Assert.AreEqual(pendente.Id, tarefas.Single().Id);
    }

    [TestMethod]
    public void Filtrar_ComTarefasCadastradas_DeveCarregarSomenteConcluidas()
    {
        // Arranjo
        repositorioTarefa.Cadastrar(new Tarefa("Tarefa pendente", PrioridadeTarefa.Normal));
        Tarefa concluida = CriarTarefaComItem(true);
        repositorioTarefa.Cadastrar(concluida);

        // Ação
        List<Tarefa> tarefas = repositorioTarefa.Filtrar(t => t.StatusConclusao);

        // Asserção
        Assert.HasCount(1, tarefas);
        Assert.AreEqual(concluida.Id, tarefas.Single().Id);
    }

    [TestMethod]
    public void SelecionarAgrupadasPorPrioridade_ComTarefasCadastradas_DeveAgruparTarefas()
    {
        // Arranjo
        repositorioTarefa.Cadastrar(new Tarefa("Tarefa baixa", PrioridadeTarefa.Baixa));
        repositorioTarefa.Cadastrar(new Tarefa("Tarefa normal", PrioridadeTarefa.Normal));
        repositorioTarefa.Cadastrar(new Tarefa("Tarefa alta", PrioridadeTarefa.Alta));
        ServicoTarefa servico = new(repositorioTarefa);

        // Ação
        List<TarefasPorPrioridadeDto> grupos = servico.SelecionarAgrupadasPorPrioridade();

        // Asserção
        Assert.HasCount(3, grupos);
        Assert.AreEqual("Tarefa baixa", grupos.Single(g => g.Prioridade == PrioridadeTarefa.Baixa).Tarefas.Single().Titulo);
        Assert.AreEqual("Tarefa normal", grupos.Single(g => g.Prioridade == PrioridadeTarefa.Normal).Tarefas.Single().Titulo);
        Assert.AreEqual("Tarefa alta", grupos.Single(g => g.Prioridade == PrioridadeTarefa.Alta).Tarefas.Single().Titulo);
    }

    [TestMethod]
    public void SelecionarPorId_ComTarefaEItensCadastrados_DeveCarregarSeusDados()
    {
        // Arranjo
        Tarefa tarefa = new("Estudar integração", PrioridadeTarefa.Alta);
        tarefa.AdicionarItem(new ItemTarefa("Revisar conteúdo"));
        tarefa.AdicionarItem(new ItemTarefa("Resolver exercícios"));
        repositorioTarefa.Cadastrar(tarefa);

        // Ação
        Tarefa? tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefa.Id);

        // Asserção
        Assert.IsNotNull(tarefaSelecionada);
        Assert.AreEqual("Estudar integração", tarefaSelecionada.Titulo);
        Assert.AreEqual(PrioridadeTarefa.Alta, tarefaSelecionada.Prioridade);
        Assert.HasCount(2, tarefaSelecionada.Itens);
        CollectionAssert.AreEquivalent(
            new[] { "Revisar conteúdo", "Resolver exercícios" },
            tarefaSelecionada.Itens.Select(i => i.Titulo).ToArray()
        );
    }

    [TestMethod]
    public void Excluir_ComItensVinculados_DeveRemoverTarefaEItens()
    {
        // Arranjo
        Tarefa tarefa = CriarTarefaComItem(false);
        repositorioTarefa.Cadastrar(tarefa);

        // Ação
        bool conseguiuExcluir = repositorioTarefa.Excluir(tarefa.Id);

        using var conexao = connectionFactory.CreateConnection();
        int quantidadeItens = conexao.ExecuteScalar<int>(
            "SELECT COUNT(*) FROM dbo.TBItemTarefa WHERE TarefaId = @TarefaId",
            new { TarefaId = tarefa.Id }
        );

        // Asserção
        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(repositorioTarefa.SelecionarPorId(tarefa.Id));
        Assert.AreEqual(0, quantidadeItens);
    }

    private static Tarefa CriarTarefaComItem(bool concluido)
    {
        Tarefa tarefa = new("Estudar integração", PrioridadeTarefa.Normal);
        ItemTarefa item = new("Criar testes");

        if (concluido)
            item.Concluir();

        tarefa.AdicionarItem(item);

        return tarefa;
    }
}
