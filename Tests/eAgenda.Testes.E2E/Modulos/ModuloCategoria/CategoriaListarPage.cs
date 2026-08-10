using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.Modulos.ModuloCategoria;

public sealed class CategoriaListarPage(IPage page, string urlBase)
{
    public string Url => $"{urlBase}/Categorias/Listar";

    public ILocator EstadoVazio => page.GetByText(
        "Nenhuma categoria cadastrada.",
        new() { Exact = true }
    );

    public ILocator TituloDaCategoria(string titulo) => page.GetByRole(
        AriaRole.Heading,
        new() { Name = titulo, Exact = true }
    );

    public ILocator MensagemDeErro(string mensagem) => page.GetByText(
        mensagem,
        new() { Exact = true }
    );

    public async Task IrParaAsync()
    {
        await page.GotoAsync(Url);
    }

    public async Task EditarAsync(string titulo)
    {
        await CardPorTitulo(titulo).GetByRole(
            AriaRole.Link,
            new() { Name = "Editar", Exact = true }
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
        return page.Locator(".card").Filter(new() { Has = TituloDaCategoria(titulo) });
    }
}
