using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.Modulos.ModuloTarefa;

public sealed class TarefaListarPage(IPage page, string urlBase)
{
    public string Url => $"{urlBase}/Tarefas/Listar";
    public string UrlPendentes => $"{urlBase}/Tarefas/ListarPendentes";
    public string UrlConcluidas => $"{urlBase}/Tarefas/ListarConcluidas";

    public ILocator EstadoVazio => page.GetByText(
        "Nenhuma tarefa cadastrada.",
        new() { Exact = true }
    );

    public ILocator TituloDaTarefa(string titulo) => page.GetByRole(
        AriaRole.Heading,
        new() { Name = titulo, Exact = true }
    );

    public ILocator DadosDaTarefa(string titulo, string dado) =>
        CardPorTitulo(titulo).GetByText(dado, new() { Exact = true });

    public async Task IrParaAsync()
    {
        await page.GotoAsync(Url);
    }

    public async Task IrParaPendentesAsync()
    {
        await page.GotoAsync(UrlPendentes);
    }

    public async Task IrParaConcluidasAsync()
    {
        await page.GotoAsync(UrlConcluidas);
    }

    public async Task EditarAsync(string titulo)
    {
        await CardPorTitulo(titulo).GetByRole(
            AriaRole.Link,
            new() { Name = "Editar", Exact = true }
        ).ClickAsync();
    }

    public async Task VisualizarDetalhesAsync(string titulo)
    {
        await CardPorTitulo(titulo).GetByRole(
            AriaRole.Link,
            new() { Name = "Detalhes", Exact = true }
        ).ClickAsync();
    }

    public async Task ExcluirAsync(string titulo)
    {
        await CardPorTitulo(titulo).GetByRole(
            AriaRole.Link,
            new() { Name = "Excluir", Exact = true }
        ).ClickAsync();
    }

    private ILocator CardPorTitulo(string titulo)
    {
        return page.Locator(".card").Filter(new() { Has = TituloDaTarefa(titulo) });
    }
}
