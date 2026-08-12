using Microsoft.Playwright;
using System.Globalization;

namespace eAgenda.Testes.E2E.Modulos.ModuloDespesas;

public sealed class DespesaFormPage(IPage page, string urlBase)
{
    public string UrlCadastrar => $"{urlBase}/Despesas/Cadastrar";

    public ILocator ErroDeValidacao(string mensagem) => page.GetByText(
        mensagem,
        new() { Exact = true }
    );

    public async Task IrParaCadastroAsync()
    {
        await page.GotoAsync(UrlCadastrar);
    }

    public async Task PreencherAsync(string descricao, DateTime data, decimal valor, string formaPagamento, int? parcelas = null)
    {
        await page.GetByLabel("Descrição").FillAsync(descricao);
        await page.GetByLabel("Data").FillAsync(data.ToString("yyyy-MM-dd"));
        await page.GetByLabel("Valor").FillAsync(valor.ToString(CultureInfo.InvariantCulture));
        await page.Locator("#formaPagamentoSelect").SelectOptionAsync(new SelectOptionValue { Value = formaPagamento });

        if (parcelas.HasValue)
            await page.GetByLabel("Parcelas").FillAsync(parcelas.Value.ToString());

        // garante ao menos uma categoria selecionada (select2 substitui UI, seleciona o original)
        var categoriasCount = await page.Locator("#listaCategorias option").CountAsync();
        if (categoriasCount > 1)
        {
            var primeira = await page.Locator("#listaCategorias option").Nth(1).GetAttributeAsync("value");
            if (!string.IsNullOrEmpty(primeira))
                await page.Locator("#listaCategorias").SelectOptionAsync(primeira);
        }
    }

    public async Task ConfirmarAsync()
    {
        await page.GetByRole(AriaRole.Button, new() { Name = "Salvar", Exact = true }).ClickAsync();
    }
}
