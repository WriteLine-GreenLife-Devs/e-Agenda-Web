using eAgendaWeb.Modulos.ModuloTarefas.Dominio;

namespace eAgenda.Testes.Unidade.Modulos.ModuloTarefas;

[TestClass]
public sealed class TarefaTests
{
    [TestMethod]
    public void Validar_ComDadosValidos_NaoDeveRetornarErros()
    {
        // Arranjo
        Tarefa tarefa = new Tarefa("Estudar para prova", PrioridadeTarefa.Alta);

        // Ação
        List<string> erros = tarefa.Validar();

        // Asserção
        Assert.HasCount(0, erros);
    }

    [TestMethod]
    public void Construtor_DeveNascerPendenteComZeroPorcentoESemDataConclusao()
    {
        // Arranjo + Ação
        Tarefa tarefa = new Tarefa("Estudar para prova", PrioridadeTarefa.Normal);

        // Asserção
        Assert.IsFalse(tarefa.StatusConclusao);
        Assert.AreEqual(0m, tarefa.PercentualConcluido);
        Assert.IsNull(tarefa.DataConclusao);
    }

    [TestMethod]
    public void Construtor_SemItens_DeveTerListaVazia()
    {
        // Arranjo + Ação
        Tarefa tarefa = new Tarefa("Estudar para prova", PrioridadeTarefa.Baixa);

        // Asserção
        Assert.HasCount(0, tarefa.Itens);
    }

    [TestMethod]
    public void AdicionarItem_DeveVincularItemETerListaNaoVazia()
    {
        // Arranjo
        Tarefa tarefa = new Tarefa("Estudar para prova", PrioridadeTarefa.Normal);
        ItemTarefa item = new ItemTarefa("Ler capítulo 1");

        // Ação
        tarefa.AdicionarItem(item);

        // Asserção
        Assert.HasCount(1, tarefa.Itens);
        Assert.AreEqual(tarefa.Id, item.TarefaId);
    }

    [TestMethod]
    public void Validar_ComTituloVazio_DeveRetornarErros()
    {
        // Arranjo
        Tarefa tarefa = new Tarefa("", PrioridadeTarefa.Baixa);

        // Ação
        List<string> erros = tarefa.Validar();

        // Asserção
        Assert.HasCount(2, erros);
        CollectionAssert.Contains(erros, "O título da tarefa é obrigatório.");
        CollectionAssert.Contains(erros, "O título deve possuir entre 2 e 100 caracteres.");
    }

    [TestMethod]
    public void Validar_ComTituloCurto_DeveRetornarErroTamanho()
    {
        // Arranjo
        Tarefa tarefa = new Tarefa("A", PrioridadeTarefa.Baixa);

        // Ação
        List<string> erros = tarefa.Validar();

        // Asserção
        Assert.HasCount(1, erros);
        CollectionAssert.Contains(erros, "O título deve possuir entre 2 e 100 caracteres.");
    }

    [TestMethod]
    public void Validar_ComTituloNoLimiteMinimo_NaoDeveRetornarErros()
    {
        // Arranjo
        Tarefa tarefa = new Tarefa("AB", PrioridadeTarefa.Baixa);

        // Ação
        List<string> erros = tarefa.Validar();

        // Asserção
        Assert.HasCount(0, erros);
    }

    [TestMethod]
    public void Validar_ComTituloNoLimiteMaximo_NaoDeveRetornarErros()
    {
        // Arranjo
        string titulo = new string('A', 100);
        Tarefa tarefa = new Tarefa(titulo, PrioridadeTarefa.Alta);

        // Ação
        List<string> erros = tarefa.Validar();

        // Asserção
        Assert.HasCount(0, erros);
    }

    [TestMethod]
    public void Validar_ComTituloAcimaDoMaximo_DeveRetornarErroTamanho()
    {
        // Arranjo
        string titulo = new string('A', 101);
        Tarefa tarefa = new Tarefa(titulo, PrioridadeTarefa.Alta);

        // Ação
        List<string> erros = tarefa.Validar();

        // Asserção
        Assert.HasCount(1, erros);
        CollectionAssert.Contains(erros, "O título deve possuir entre 2 e 100 caracteres.");
    }

    [TestMethod]
    public void Atualizar_DeveAtualizarTituloEPrioridade()
    {
        // Arranjo
        Tarefa tarefa = new Tarefa("Estudar", PrioridadeTarefa.Baixa);
        Tarefa tarefaAtualizada = new Tarefa("Estudar e revisar", PrioridadeTarefa.Alta);

        // Ação
        tarefa.Atualizar(tarefaAtualizada);

        // Asserção
        Assert.AreEqual("Estudar e revisar", tarefa.Titulo);
        Assert.AreEqual(PrioridadeTarefa.Alta, tarefa.Prioridade);
    }

    [TestMethod]
    public void ConcluirItem_DeveRegistrarDataConclusaoEStatus()
    {
        // Arranjo
        Tarefa tarefa = new Tarefa("Estudar", PrioridadeTarefa.Normal);
        ItemTarefa item = new ItemTarefa("Ler capítulo 1");
        tarefa.AdicionarItem(item);

        // Ação
        tarefa.ConcluirItem(item.Id);

        // Asserção
        Assert.AreEqual(100m, tarefa.PercentualConcluido);
        Assert.IsTrue(tarefa.StatusConclusao);
        Assert.IsNotNull(tarefa.DataConclusao);
    }

    [TestMethod]
    public void DesmarcarItem_DeveReabrirTarefaConcluida()
    {
        // Arranjo
        Tarefa tarefa = new Tarefa("Estudar", PrioridadeTarefa.Normal);
        ItemTarefa item = new ItemTarefa("Ler capítulo 1");
        tarefa.AdicionarItem(item);
        tarefa.ConcluirItem(item.Id);

        // Ação
        tarefa.DesmarcarItem(item.Id);

        // Asserção
        Assert.AreEqual(0m, tarefa.PercentualConcluido);
        Assert.IsFalse(tarefa.StatusConclusao);
        Assert.IsNull(tarefa.DataConclusao);
    }
}