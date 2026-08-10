using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.Modulos.ModuloCategoria;

public sealed class CategoriaFormPage(IPage page, string urlBase)
{
    public string UrlCadastrar => $"{urlBase}/Categorias/Cadastrar";

    public ILocator ErroDeValidacao(string mensagem) => page.GetByText(
        mensagem,
        new() { Exact = true }
    );

    public async Task IrParaCadastroAsync()
    {
        await page.GotoAsync(UrlCadastrar);
    }

    public async Task PreencherTituloAsync(string titulo)
    {
        await page.GetByLabel("Título").FillAsync(titulo);
    }

    public async Task ConfirmarAsync()
    {
        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar", Exact = true })
            .ClickAsync();
    }
}
