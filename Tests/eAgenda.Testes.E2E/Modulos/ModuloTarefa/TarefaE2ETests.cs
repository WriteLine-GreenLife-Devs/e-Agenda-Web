using System.Text.RegularExpressions;
using eAgenda.Testes.E2E.Compartilhado;

namespace eAgenda.Testes.E2E.Modulos.ModuloTarefa;

[TestClass]
public sealed class TarefaE2ETests : E2ETestsBase
{
    [TestMethod]
    public async Task DeveCadastrar_Tarefa_ComDadosValidos()
    {
        // Arranjo
        TarefaFormPage formPage = new(Page, UrlBase);
        TarefaListarPage listarPage = new(Page, UrlBase);

        // Ação
        await formPage.IrParaCadastroAsync();
        await formPage.PreencherAsync("Preparar apresentação", "Normal");
        await formPage.ConfirmarAsync();

        // Asserção
        await Expect(Page).ToHaveURLAsync(listarPage.Url);
        await Expect(listarPage.TituloDaTarefa("Preparar apresentação")).ToBeVisibleAsync();
        await Expect(listarPage.DadosDaTarefa("Preparar apresentação", "Normal"))
            .ToBeVisibleAsync();
        await Expect(listarPage.DadosDaTarefa("Preparar apresentação", "Pendente"))
            .ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveCadastrar_Tarefa_ComItens()
    {
        // Arranjo
        TarefaFormPage formPage = new(Page, UrlBase);
        TarefaListarPage listarPage = new(Page, UrlBase);

        // Ação
        await formPage.IrParaCadastroAsync();
        await formPage.PreencherAsync("Preparar apresentação", "Alta");
        await formPage.AdicionarItemAsync("Criar os slides");
        await formPage.AdicionarItemAsync("Revisar o conteúdo");
        await formPage.ConfirmarAsync();

        // Asserção
        await Expect(Page).ToHaveURLAsync(listarPage.Url);
        await Expect(listarPage.DadosDaTarefa("Preparar apresentação", "0,00%"))
            .ToBeVisibleAsync();

        await listarPage.EditarAsync("Preparar apresentação");
        await Expect(formPage.ItemPorTitulo("Criar os slides")).ToBeVisibleAsync();
        await Expect(formPage.ItemPorTitulo("Revisar o conteúdo")).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveImpedir_Cadastro_DeTarefa_SemTitulo()
    {
        // Arranjo
        TarefaFormPage formPage = new(Page, UrlBase);

        // Ação
        await formPage.IrParaCadastroAsync();
        await formPage.ConfirmarAsync();

        // Asserção
        await Expect(Page).ToHaveURLAsync(formPage.UrlCadastrar);
        await Expect(formPage.ErroDeValidacao(
            "O título da tarefa é obrigatório."
        )).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveEditar_Tarefa_ComDadosValidos()
    {
        // Arranjo
        await CadastrarTarefaAsync("Preparar apresentação", "Normal");
        TarefaFormPage formPage = new(Page, UrlBase);
        TarefaListarPage listarPage = new(Page, UrlBase);
        await listarPage.EditarAsync("Preparar apresentação");

        // Ação
        await formPage.PreencherAsync("Apresentar resultados", "Alta");
        await formPage.SalvarAsync();

        // Asserção
        await Expect(Page).ToHaveURLAsync(listarPage.Url);
        await Expect(listarPage.TituloDaTarefa("Apresentar resultados")).ToBeVisibleAsync();
        await Expect(listarPage.TituloDaTarefa("Preparar apresentação")).Not.ToBeVisibleAsync();
        await Expect(listarPage.DadosDaTarefa("Apresentar resultados", "Alta"))
            .ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveConcluir_Tarefa_ERegistrarDataDeConclusao()
    {
        // Arranjo
        await CadastrarTarefaAsync(
            "Preparar apresentação",
            "Alta",
            "Criar os slides",
            "Revisar o conteúdo"
        );
        TarefaFormPage formPage = new(Page, UrlBase);
        TarefaListarPage listarPage = new(Page, UrlBase);
        await listarPage.EditarAsync("Preparar apresentação");

        // Ação
        await formPage.ConcluirItemAsync("Criar os slides");
        await formPage.ConcluirItemAsync("Revisar o conteúdo");
        await listarPage.IrParaAsync();

        // Asserção
        await Expect(listarPage.DadosDaTarefa("Preparar apresentação", "100,00%"))
            .ToBeVisibleAsync();
        await Expect(listarPage.DadosDaTarefa("Preparar apresentação", "Concluída"))
            .ToBeVisibleAsync();
        await Expect(listarPage.DadosDaTarefa(
            "Preparar apresentação",
            DateTime.Today.ToString("dd/MM/yyyy")
        )).ToHaveCountAsync(2);
    }

    [TestMethod]
    public async Task DeveListar_TodasAsTarefas_Cadastradas()
    {
        // Arranjo
        await CadastrarTarefaAsync("Organizar documentos", "Baixa");
        await CadastrarTarefaConcluidaAsync("Enviar relatório", "Alta");
        TarefaListarPage listarPage = new(Page, UrlBase);

        // Ação
        await listarPage.IrParaAsync();

        // Asserção
        await Expect(Page).ToHaveURLAsync(listarPage.Url);
        await Expect(listarPage.TituloDaTarefa("Organizar documentos")).ToBeVisibleAsync();
        await Expect(listarPage.TituloDaTarefa("Enviar relatório")).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveListar_ApenasTarefas_Pendentes()
    {
        // Arranjo
        await CadastrarTarefaAsync("Organizar documentos", "Baixa");
        await CadastrarTarefaConcluidaAsync("Enviar relatório", "Alta");
        TarefaListarPage listarPage = new(Page, UrlBase);

        // Ação
        await listarPage.IrParaPendentesAsync();

        // Asserção
        await Expect(Page).ToHaveURLAsync(listarPage.UrlPendentes);
        await Expect(listarPage.TituloDaTarefa("Organizar documentos")).ToBeVisibleAsync();
        await Expect(listarPage.TituloDaTarefa("Enviar relatório")).Not.ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveListar_ApenasTarefas_Concluidas()
    {
        // Arranjo
        await CadastrarTarefaAsync("Organizar documentos", "Baixa");
        await CadastrarTarefaConcluidaAsync("Enviar relatório", "Alta");
        TarefaListarPage listarPage = new(Page, UrlBase);

        // Ação
        await listarPage.IrParaConcluidasAsync();

        // Asserção
        await Expect(Page).ToHaveURLAsync(listarPage.UrlConcluidas);
        await Expect(listarPage.TituloDaTarefa("Enviar relatório")).ToBeVisibleAsync();
        await Expect(listarPage.TituloDaTarefa("Organizar documentos")).Not.ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveVisualizar_Tarefas_AgrupadasPorPrioridade()
    {
        // Arranjo
        await CadastrarTarefaAsync("Organizar documentos", "Baixa");
        await CadastrarTarefaAsync("Responder mensagens", "Normal");
        await CadastrarTarefaAsync("Enviar relatório", "Alta");
        TarefaAgruparPage agruparPage = new(Page, UrlBase);

        // Ação
        await agruparPage.IrParaAsync();

        // Asserção
        await Expect(Page).ToHaveURLAsync(agruparPage.Url);
        await Expect(agruparPage.TarefaDoGrupo("Baixa", "Organizar documentos"))
            .ToBeVisibleAsync();
        await Expect(agruparPage.TarefaDoGrupo("Normal", "Responder mensagens"))
            .ToBeVisibleAsync();
        await Expect(agruparPage.TarefaDoGrupo("Alta", "Enviar relatório"))
            .ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveExcluir_Tarefa_ESeusItensVinculados()
    {
        // Arranjo
        await CadastrarTarefaAsync(
            "Preparar apresentação",
            "Alta",
            "Criar os slides",
            "Revisar o conteúdo"
        );
        TarefaListarPage listarPage = new(Page, UrlBase);
        TarefaExcluirPage excluirPage = new(Page);

        int itensAntesDaExclusao = ExecutarConsulta<int>(
            "SELECT COUNT(*) FROM TBItemTarefa"
        );

        await listarPage.ExcluirAsync("Preparar apresentação");

        // Ação
        await Expect(Page).ToHaveURLAsync(
            new Regex($"{Regex.Escape(UrlBase)}/Tarefas/Excluir/.*")
        );
        await Expect(excluirPage.MensagemConfirmacao).ToBeVisibleAsync();
        await excluirPage.ConfirmarAsync();

        // Asserção
        int itensDepoisDaExclusao = ExecutarConsulta<int>(
            "SELECT COUNT(*) FROM TBItemTarefa"
        );

        await Expect(Page).ToHaveURLAsync(listarPage.Url);
        await Expect(listarPage.TituloDaTarefa("Preparar apresentação"))
            .Not.ToBeVisibleAsync();
        await Expect(listarPage.EstadoVazio).ToBeVisibleAsync();
        Assert.AreEqual(2, itensAntesDaExclusao);
        Assert.AreEqual(0, itensDepoisDaExclusao);
    }

    private async Task CadastrarTarefaAsync(
        string titulo,
        string prioridade,
        params string[] itens
    )
    {
        TarefaFormPage formPage = new(Page, UrlBase);
        TarefaListarPage listarPage = new(Page, UrlBase);

        await formPage.IrParaCadastroAsync();
        await formPage.PreencherAsync(titulo, prioridade);

        foreach (string item in itens)
            await formPage.AdicionarItemAsync(item);

        await formPage.ConfirmarAsync();

        await Expect(Page).ToHaveURLAsync(listarPage.Url);
    }

    private async Task CadastrarTarefaConcluidaAsync(string titulo, string prioridade)
    {
        const string item = "Finalizar atividade";

        await CadastrarTarefaAsync(titulo, prioridade, item);

        TarefaFormPage formPage = new(Page, UrlBase);
        TarefaListarPage listarPage = new(Page, UrlBase);

        await listarPage.EditarAsync(titulo);
        await formPage.ConcluirItemAsync(item);
        await listarPage.IrParaAsync();
    }
}
