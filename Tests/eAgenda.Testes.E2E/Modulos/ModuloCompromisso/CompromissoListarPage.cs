using Microsoft.Playwright;
using System.Text.RegularExpressions;

namespace eAgenda.Testes.E2E.Modulos.ModuloCompromisso;

public sealed class CompromissoListarPage(IPage page, string urlBase)
{
    public string Url => $"{urlBase}/Compromissos/Listar";

    public ILocator EstadoVazio => page.GetByText(
        "Nenhum compromisso cadastrado.",
        new() { Exact = true }
    );

    public ILocator Assunto(string assunto) => page.GetByRole(
        AriaRole.Heading,
        new() { Name = assunto, Exact = true }
    );

    public ILocator DadosDoCompromisso(string assunto, string horarioOuTipo) => page.GetByText(horarioOuTipo);

    public async Task IrParaAsync()
    {
        await page.GotoAsync(Url);
    }

    public async Task EditarAsync(string assunto)
    {
        await CardPorAssunto(assunto).GetByRole(
            AriaRole.Link,
            new() { Name = "Editar", Exact = true }
        ).ClickAsync();
    }

    public async Task ExcluirAsync(string assunto)
    {
        await CardPorAssunto(assunto).GetByRole(
            AriaRole.Link,
            new() { Name = "Excluir", Exact = true }
        ).ClickAsync();
    }

    private ILocator CardPorAssunto(string assunto)
    {
        return page.Locator(".card").Filter(new() { Has = Assunto(assunto) });
    }
}
