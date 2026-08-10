using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.Modulos.ModuloTarefa;

public sealed class TarefaAgruparPage(IPage page, string urlBase)
{
    public string Url => $"{urlBase}/Tarefas/AgruparPorPrioridade";

    public ILocator GrupoPorPrioridade(string prioridade) => page
        .Locator(".grupo-prioridade")
        .Filter(new()
        {
            Has = page.GetByRole(
                AriaRole.Heading,
                new() { Name = prioridade, Exact = true }
            )
        });

    public ILocator TarefaDoGrupo(string prioridade, string titulo) =>
        GrupoPorPrioridade(prioridade).GetByRole(
            AriaRole.Heading,
            new() { Name = titulo, Exact = true }
        );

    public async Task IrParaAsync()
    {
        await page.GotoAsync(Url);
    }
}
