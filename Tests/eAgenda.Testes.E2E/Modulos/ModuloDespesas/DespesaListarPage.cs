using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.Modulos.ModuloDespesas;

public sealed class DespesaListarPage(IPage page, string urlBase)
{
    public string Url => $"{urlBase}/Despesas/Listar";

    public ILocator EstadoVazio => page.GetByText("Nenhuma despesa cadastrada.", new() { Exact = true });

    public ILocator Descricao(string descricao) => page.GetByRole(AriaRole.Heading, new() { Name = descricao, Exact = true });

    public async Task IrParaAsync() => await page.GotoAsync(Url);

    public async Task EditarAsync(string descricao)
    {
        await CardPorDescricao(descricao).GetByRole(AriaRole.Link, new() { Name = "Editar", Exact = true }).ClickAsync();
    }

    public async Task ExcluirAsync(string descricao)
    {
        await CardPorDescricao(descricao).GetByRole(AriaRole.Link, new() { Name = "Excluir", Exact = true }).ClickAsync();
    }

    private ILocator CardPorDescricao(string descricao)
    {
        return page.Locator(".card").Filter(new() { Has = Descricao(descricao) });
    }
}
