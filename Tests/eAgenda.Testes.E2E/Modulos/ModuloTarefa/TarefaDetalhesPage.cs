using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.Modulos.ModuloTarefa;

public sealed class TarefaDetalhesPage(IPage page)
{
    public ILocator TituloDaTarefa(string titulo) => page.GetByRole(
        AriaRole.Heading,
        new() { Name = titulo, Exact = true }
    );

    public ILocator Dado(string dado) => page
        .Locator("dl")
        .GetByText(dado, new() { Exact = true });

    public ILocator ItemPorTitulo(string titulo) => page
        .Locator(".item-tarefa")
        .Filter(new() { HasText = titulo });

    public ILocator StatusDoItem(string titulo, string status) =>
        ItemPorTitulo(titulo).GetByText(status, new() { Exact = true });
}
