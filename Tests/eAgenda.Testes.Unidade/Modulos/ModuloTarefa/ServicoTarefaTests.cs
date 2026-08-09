using eAgendaWeb.Modulos.ModuloTarefas.Aplicacao;
using eAgendaWeb.Modulos.ModuloTarefas.Dominio;
using FluentResults;
using Moq;

namespace eAgenda.Testes.Unidade.Modulos.ModuloTarefas;

[TestClass]
public sealed class ServicoTarefaTests
{
    [TestMethod]
    public void Cadastrar_ComDadosValidos_DevePersistirTarefa()
    {
        // Arranjo
        Mock<IRepositorioTarefa> repositorioTarefa = new();
        Tarefa? tarefaCadastrada = null;

        repositorioTarefa
            .Setup(r => r.Cadastrar(It.IsAny<Tarefa>()))
            .Callback<Tarefa>(tarefa => tarefaCadastrada = tarefa);

        ServicoTarefa servico = new(repositorioTarefa.Object);
        CadastrarTarefaDto dto = new("Estudar testes", PrioridadeTarefa.Alta, []);

        // Ação
        Result resultado = servico.Cadastrar(dto);

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(tarefaCadastrada);
        Assert.AreEqual("Estudar testes", tarefaCadastrada.Titulo);
        Assert.AreEqual(PrioridadeTarefa.Alta, tarefaCadastrada.Prioridade);
        repositorioTarefa.Verify(r => r.Cadastrar(It.IsAny<Tarefa>()), Times.Once);
    }

    [TestMethod]
    public void Cadastrar_TarefaNova_DeveNascerPendenteComZeroPorcentoESemDataConclusao()
    {
        // Arranjo
        Mock<IRepositorioTarefa> repositorioTarefa = new();
        Tarefa? tarefaCadastrada = null;

        repositorioTarefa
            .Setup(r => r.Cadastrar(It.IsAny<Tarefa>()))
            .Callback<Tarefa>(tarefa => tarefaCadastrada = tarefa);

        ServicoTarefa servico = new(repositorioTarefa.Object);

        // Ação
        Result resultado = servico.Cadastrar(
            new CadastrarTarefaDto("Estudar testes", PrioridadeTarefa.Normal, [])
        );

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(tarefaCadastrada);
        Assert.IsFalse(tarefaCadastrada.StatusConclusao);
        Assert.AreEqual(0m, tarefaCadastrada.PercentualConcluido);
        Assert.IsNull(tarefaCadastrada.DataConclusao);
        Assert.AreEqual(DateTime.Today, tarefaCadastrada.DataCriacao.Date);
    }

    [TestMethod]
    public void Cadastrar_SemItens_DevePersistirTarefaComListaVazia()
    {
        // Arranjo
        Mock<IRepositorioTarefa> repositorioTarefa = new();
        Tarefa? tarefaCadastrada = null;

        repositorioTarefa
            .Setup(r => r.Cadastrar(It.IsAny<Tarefa>()))
            .Callback<Tarefa>(tarefa => tarefaCadastrada = tarefa);

        ServicoTarefa servico = new(repositorioTarefa.Object);

        // Ação
        Result resultado = servico.Cadastrar(
            new CadastrarTarefaDto("Estudar testes", PrioridadeTarefa.Normal, [])
        );

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(tarefaCadastrada);
        Assert.IsEmpty(tarefaCadastrada.Itens);
        repositorioTarefa.Verify(r => r.Cadastrar(It.IsAny<Tarefa>()), Times.Once);
    }

    [TestMethod]
    public void Cadastrar_ComItens_DevePersistirTarefaComItensVinculados()
    {
        // Arranjo
        Mock<IRepositorioTarefa> repositorioTarefa = new();
        Tarefa? tarefaCadastrada = null;

        repositorioTarefa
            .Setup(r => r.Cadastrar(It.IsAny<Tarefa>()))
            .Callback<Tarefa>(tarefa => tarefaCadastrada = tarefa);

        ServicoTarefa servico = new(repositorioTarefa.Object);
        List<ItemTarefaDto> itens =
        [
            new(Guid.Empty, "Revisar conteúdo", false),
            new(Guid.Empty, "Resolver exercícios", false)
        ];

        // Ação
        Result resultado = servico.Cadastrar(
            new CadastrarTarefaDto("Estudar testes", PrioridadeTarefa.Alta, itens)
        );

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(tarefaCadastrada);
        Assert.HasCount(2, tarefaCadastrada.Itens);
        Assert.IsTrue(tarefaCadastrada.Itens.All(i => i.TarefaId == tarefaCadastrada.Id));
        Assert.AreEqual(0m, tarefaCadastrada.PercentualConcluido);
    }

    [TestMethod]
    public void Cadastrar_SemTitulo_DeveRetornarFalha()
    {
        // Arranjo
        Mock<IRepositorioTarefa> repositorioTarefa = new();
        ServicoTarefa servico = new(repositorioTarefa.Object);

        // Ação
        Result resultado = servico.Cadastrar(
            new CadastrarTarefaDto(string.Empty, PrioridadeTarefa.Normal, [])
        );

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual(nameof(Tarefa.Titulo), resultado.Errors.Single().Metadata["Campo"]);
        Assert.Contains("obrigatório", resultado.Errors.Single().Message);
        repositorioTarefa.Verify(r => r.Cadastrar(It.IsAny<Tarefa>()), Times.Never);
    }

    [TestMethod]
    public void Cadastrar_ComPrioridadeInvalida_DeveRetornarFalha()
    {
        // Arranjo
        Mock<IRepositorioTarefa> repositorioTarefa = new();
        ServicoTarefa servico = new(repositorioTarefa.Object);

        // Ação
        Result resultado = servico.Cadastrar(
            new CadastrarTarefaDto("Estudar testes", (PrioridadeTarefa)99, [])
        );

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual(nameof(Tarefa.Prioridade), resultado.Errors.Single().Metadata["Campo"]);
        Assert.Contains("Baixa, Normal ou Alta", resultado.Errors.Single().Message);
        repositorioTarefa.Verify(r => r.Cadastrar(It.IsAny<Tarefa>()), Times.Never);
    }

    [TestMethod]
    public void Editar_ComDadosValidos_DeveAtualizarTarefa()
    {
        // Arranjo
        Tarefa tarefa = new("Estudar testes", PrioridadeTarefa.Normal);
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        repositorioTarefa.Setup(r => r.SelecionarPorId(tarefa.Id)).Returns(tarefa);
        repositorioTarefa
            .Setup(r => r.Editar(tarefa.Id, It.IsAny<Tarefa>()))
            .Returns(true);

        ServicoTarefa servico = new(repositorioTarefa.Object);
        EditarTarefaDto dto = new(
            tarefa.Id,
            "Estudar serviços",
            PrioridadeTarefa.Alta,
            [new ItemTarefaDto(Guid.Empty, "Criar testes", false)]
        );

        // Ação
        Result resultado = servico.Editar(dto);

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        repositorioTarefa.Verify(
            r => r.Editar(
                tarefa.Id,
                It.Is<Tarefa>(t =>
                    t.Titulo == "Estudar serviços" &&
                    t.Prioridade == PrioridadeTarefa.Alta &&
                    t.Itens.Count == 1)
            ),
            Times.Once
        );
    }

    [TestMethod]
    public void AtualizarStatusItem_ConcluindoUltimoItem_DeveConcluirTarefa()
    {
        // Arranjo
        Tarefa tarefa = CriarTarefaComItem(false);
        ItemTarefa item = tarefa.Itens.Single();
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        repositorioTarefa.Setup(r => r.SelecionarPorId(tarefa.Id)).Returns(tarefa);
        repositorioTarefa.Setup(r => r.Editar(tarefa.Id, tarefa)).Returns(true);

        ServicoTarefa servico = new(repositorioTarefa.Object);

        // Ação
        Result resultado = servico.AtualizarStatusItem(tarefa.Id, item.Id, true);

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsTrue(tarefa.StatusConclusao);
        Assert.AreEqual(100m, tarefa.PercentualConcluido);
        Assert.IsNotNull(tarefa.DataConclusao);
        repositorioTarefa.Verify(r => r.Editar(tarefa.Id, tarefa), Times.Once);
    }

    [TestMethod]
    public void AtualizarStatusItem_DesmarcandoItem_DeveReabrirTarefa()
    {
        // Arranjo
        Tarefa tarefa = CriarTarefaComItem(true);
        ItemTarefa item = tarefa.Itens.Single();
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        repositorioTarefa.Setup(r => r.SelecionarPorId(tarefa.Id)).Returns(tarefa);
        repositorioTarefa.Setup(r => r.Editar(tarefa.Id, tarefa)).Returns(true);

        ServicoTarefa servico = new(repositorioTarefa.Object);

        // Ação
        Result resultado = servico.AtualizarStatusItem(tarefa.Id, item.Id, false);

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsFalse(tarefa.StatusConclusao);
        Assert.AreEqual(0m, tarefa.PercentualConcluido);
        Assert.IsNull(tarefa.DataConclusao);
        repositorioTarefa.Verify(r => r.Editar(tarefa.Id, tarefa), Times.Once);
    }

    [TestMethod]
    public void SelecionarTodos_ComTarefasCadastradas_DeveRetornarTodas()
    {
        // Arranjo
        Tarefa pendente = new("Tarefa pendente", PrioridadeTarefa.Normal);
        Tarefa concluida = CriarTarefaComItem(true);
        Mock<IRepositorioTarefa> repositorioTarefa = new();
        repositorioTarefa.Setup(r => r.SelecionarTodos()).Returns([pendente, concluida]);
        ServicoTarefa servico = new(repositorioTarefa.Object);

        // Ação
        List<ListarTarefasDto> resultado = servico.SelecionarTodos();

        // Asserção
        Assert.HasCount(2, resultado);
        Assert.IsTrue(resultado.Any(t => t.Id == pendente.Id));
        Assert.IsTrue(resultado.Any(t => t.Id == concluida.Id));
    }

    [TestMethod]
    public void SelecionarPendentes_ComTarefasCadastradas_DeveRetornarSomentePendentes()
    {
        // Arranjo
        Tarefa pendente = new("Tarefa pendente", PrioridadeTarefa.Normal);
        Tarefa concluida = CriarTarefaComItem(true);
        Mock<IRepositorioTarefa> repositorioTarefa = new();
        repositorioTarefa.Setup(r => r.SelecionarTodos()).Returns([pendente, concluida]);
        ServicoTarefa servico = new(repositorioTarefa.Object);

        // Ação
        List<ListarTarefasDto> resultado = servico.SelecionarPendentes();

        // Asserção
        Assert.HasCount(1, resultado);
        Assert.AreEqual(pendente.Id, resultado.Single().Id);
    }

    [TestMethod]
    public void SelecionarConcluidas_ComTarefasCadastradas_DeveRetornarSomenteConcluidas()
    {
        // Arranjo
        Tarefa pendente = new("Tarefa pendente", PrioridadeTarefa.Normal);
        Tarefa concluida = CriarTarefaComItem(true);
        Mock<IRepositorioTarefa> repositorioTarefa = new();
        repositorioTarefa.Setup(r => r.SelecionarTodos()).Returns([pendente, concluida]);
        ServicoTarefa servico = new(repositorioTarefa.Object);

        // Ação
        List<ListarTarefasDto> resultado = servico.SelecionarConcluidas();

        // Asserção
        Assert.HasCount(1, resultado);
        Assert.AreEqual(concluida.Id, resultado.Single().Id);
    }

    [TestMethod]
    public void SelecionarAgrupadasPorPrioridade_ComTarefasCadastradas_DeveAgruparTarefas()
    {
        // Arranjo
        List<Tarefa> tarefas =
        [
            new("Tarefa baixa", PrioridadeTarefa.Baixa),
            new("Tarefa normal", PrioridadeTarefa.Normal),
            new("Tarefa alta", PrioridadeTarefa.Alta)
        ];
        Mock<IRepositorioTarefa> repositorioTarefa = new();
        repositorioTarefa.Setup(r => r.SelecionarTodos()).Returns(tarefas);
        ServicoTarefa servico = new(repositorioTarefa.Object);

        // Ação
        List<TarefasPorPrioridadeDto> resultado = servico.SelecionarAgrupadasPorPrioridade();

        // Asserção
        Assert.HasCount(3, resultado);
        Assert.AreEqual("Tarefa baixa", resultado.Single(g => g.Prioridade == PrioridadeTarefa.Baixa).Tarefas.Single().Titulo);
        Assert.AreEqual("Tarefa normal", resultado.Single(g => g.Prioridade == PrioridadeTarefa.Normal).Tarefas.Single().Titulo);
        Assert.AreEqual("Tarefa alta", resultado.Single(g => g.Prioridade == PrioridadeTarefa.Alta).Tarefas.Single().Titulo);
    }

    [TestMethod]
    public void SelecionarPorId_ComTarefaEItensCadastrados_DeveRetornarSeusDados()
    {
        // Arranjo
        Tarefa tarefa = new("Estudar testes", PrioridadeTarefa.Alta);
        tarefa.AdicionarItem(new ItemTarefa("Revisar conteúdo"));
        tarefa.AdicionarItem(new ItemTarefa("Resolver exercícios"));
        Mock<IRepositorioTarefa> repositorioTarefa = new();
        repositorioTarefa.Setup(r => r.SelecionarPorId(tarefa.Id)).Returns(tarefa);
        ServicoTarefa servico = new(repositorioTarefa.Object);

        // Ação
        Result<DetalhesTarefaDto> resultado = servico.SelecionarPorId(tarefa.Id);

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.AreEqual(tarefa.Id, resultado.Value.Id);
        Assert.AreEqual("Estudar testes", resultado.Value.Titulo);
        Assert.AreEqual(PrioridadeTarefa.Alta, resultado.Value.Prioridade);
        Assert.HasCount(2, resultado.Value.Itens);
        CollectionAssert.AreEquivalent(
            new[] { "Revisar conteúdo", "Resolver exercícios" },
            resultado.Value.Itens.Select(i => i.Titulo).ToArray()
        );
    }

    [TestMethod]
    public void Excluir_ComItensVinculados_DeveExcluirTarefa()
    {
        // Arranjo
        Tarefa tarefa = CriarTarefaComItem(false);
        Mock<IRepositorioTarefa> repositorioTarefa = new();
        repositorioTarefa.Setup(r => r.SelecionarPorId(tarefa.Id)).Returns(tarefa);
        repositorioTarefa.Setup(r => r.Excluir(tarefa.Id)).Returns(true);
        ServicoTarefa servico = new(repositorioTarefa.Object);

        // Ação
        Result resultado = servico.Excluir(tarefa.Id);

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        repositorioTarefa.Verify(r => r.Excluir(tarefa.Id), Times.Once);
    }

    private static Tarefa CriarTarefaComItem(bool concluido)
    {
        Tarefa tarefa = new("Estudar testes", PrioridadeTarefa.Normal);
        ItemTarefa item = new("Criar testes");

        if (concluido)
            item.Concluir();

        tarefa.AdicionarItem(item);

        return tarefa;
    }
}