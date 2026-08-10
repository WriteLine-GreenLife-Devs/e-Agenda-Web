using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.Modulos.ModuloCategoria;

public sealed class CategoriaDetalhesPage(IPage page)
{
    public ILocator TituloDaCategoria(string titulo) => page.GetByRole(
        AriaRole.Heading,
        new() { Name = titulo, Exact = true }
    );

    public ILocator DespesaPorDescricao(string descricao) => page
        .Locator(".despesa-categoria")
        .Filter(new()
        {
            Has = page.GetByRole(
                AriaRole.Heading,
                new() { Name = descricao, Exact = true }
            )
        });

    public ILocator DadosDaDespesa(string descricao, string dado) =>
        DespesaPorDescricao(descricao).GetByText(dado, new() { Exact = true });
}
