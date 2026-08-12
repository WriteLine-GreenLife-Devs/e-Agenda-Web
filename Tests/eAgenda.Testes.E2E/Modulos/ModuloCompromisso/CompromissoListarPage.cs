using Microsoft.Playwright;
using System.Text.RegularExpressions;

namespace eAgenda.Testes.E2E.Modulos.ModuloCompromisso;

public sealed class CompromissoListarPage(IPage page, string urlBase)
{
    public string Url => $"{urlBase}/Compromisso/Listar";

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
        var editLink = CardPorAssunto(assunto).GetByRole(
            AriaRole.Link,
            new() { Name = "Editar", Exact = true }
        );
        await editLink.WaitForAsync();
        await editLink.ClickAsync();
    }

    public async Task ExcluirAsync(string assunto)
    {
        var deleteLink = CardPorAssunto(assunto).GetByRole(
            AriaRole.Link,
            new() { Name = "Excluir", Exact = true }
        );
        await deleteLink.WaitForAsync();
        await deleteLink.ClickAsync();
    }

    private ILocator CardPorAssunto(string assunto)
    {
        // Tenta localizar o container do compromisso a partir do heading
        var heading = Assunto(assunto);
        // Procurar pelo ancestor com classe 'card' para ser mais resiliente
        return heading.Locator($"xpath=ancestor::div[contains(@class,'card')]");
    }
}
