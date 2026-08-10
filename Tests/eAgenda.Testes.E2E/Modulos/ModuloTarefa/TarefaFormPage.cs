using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.Modulos.ModuloTarefa;

public sealed class TarefaFormPage(IPage page, string urlBase)
{
    public string UrlCadastrar => $"{urlBase}/Tarefas/Cadastrar";

    public ILocator ErroDeValidacao(string mensagem) => page.GetByText(
        mensagem,
        new() { Exact = true }
    );

    public ILocator ItemPorTitulo(string titulo) => page
        .Locator(".item-linha")
        .Filter(new() { Has = page.Locator($"input[type='text'][value='{titulo}']") });

    public async Task IrParaCadastroAsync()
    {
        await page.GotoAsync(UrlCadastrar);
    }

    public async Task PreencherAsync(string titulo, string prioridade)
    {
        await page.GetByLabel("Título").FillAsync(titulo);
        await page.GetByLabel("Prioridade").SelectOptionAsync(prioridade);
    }

    public async Task AdicionarItemAsync(string titulo)
    {
        await page.GetByRole(
            AriaRole.Button,
            new() { NameRegex = new("Adicionar (Novo )?Item") }
        ).ClickAsync();

        await page.Locator(".item-linha").Last
            .Locator("input[type='text']")
            .FillAsync(titulo);
    }

    public async Task ConcluirItemAsync(string titulo)
    {
        await page.RunAndWaitForResponseAsync(
            async () =>
                await ItemPorTitulo(titulo).Locator("input[type='checkbox']").CheckAsync(),
            response => response.Url.EndsWith("/Tarefas/AtualizarStatusItem") && response.Ok
        );

        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task ConfirmarAsync()
    {
        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar", Exact = true })
            .ClickAsync();
    }

    public async Task SalvarAsync()
    {
        await page.GetByRole(AriaRole.Button, new() { Name = "Salvar", Exact = true })
            .ClickAsync();
    }
}
