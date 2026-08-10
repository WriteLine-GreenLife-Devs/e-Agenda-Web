using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.Modulos.ModuloContato;

public sealed class ContatoFormPage(IPage page, string urlBase)
{
    public string UrlCadastrar => $"{urlBase}/Contatos/Cadastrar";

    public ILocator ErroDeValidacao(string mensagem) => page.GetByText(
        mensagem,
        new() { Exact = true }
    );

    public async Task IrParaCadastroAsync()
    {
        await page.GotoAsync(UrlCadastrar);
    }

    public async Task PreencherAsync(
        string nome,
        string telefone,
        string email,
        string cargo = "",
        string empresa = ""
    )
    {
        await page.GetByLabel("Nome").FillAsync(nome);
        await page.GetByLabel("Telefone").FillAsync(telefone);
        await page.GetByLabel("Email").FillAsync(email);
        await page.GetByLabel("Cargo (OPCIONAL)").FillAsync(cargo);
        await page.GetByLabel("Empresa (OPCIONAL)").FillAsync(empresa);
    }

    public async Task ConfirmarAsync()
    {
        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar", Exact = true })
            .ClickAsync();
    }
}
