using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.Modulos.ModuloContato;

public sealed class ContatoListarPage(IPage page, string urlBase)
{
    public string Url => $"{urlBase}/Contatos/Listar";

    public ILocator EstadoVazio => page.GetByText(
        "Nenhum contato cadastrado.",
        new() { Exact = true }
    );

    public ILocator NomeDoContato(string nome) => page.GetByRole(
        AriaRole.Heading,
        new() { Name = nome, Exact = true }
    );

    public ILocator DadosDoContato(string nome, string dado) =>
        CardPorNome(nome).GetByText(dado, new() { Exact = true });

    public ILocator MensagemDeErro(string mensagem) => page.GetByText(
        mensagem,
        new() { Exact = true }
    );

    public async Task IrParaAsync()
    {
        await page.GotoAsync(Url);
    }

    public async Task EditarAsync(string nome)
    {
        await CardPorNome(nome).GetByRole(
            AriaRole.Link,
            new() { Name = "Editar", Exact = true }
        ).ClickAsync();
    }

    public async Task ExcluirAsync(string nome)
    {
        await CardPorNome(nome).GetByRole(
            AriaRole.Link,
            new() { Name = "Excluir", Exact = true }
        ).ClickAsync();
    }

    private ILocator CardPorNome(string nome)
    {
        return page.Locator(".card").Filter(new() { Has = NomeDoContato(nome) });
    }
}
