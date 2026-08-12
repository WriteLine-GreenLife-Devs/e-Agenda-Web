using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.Modulos.ModuloCompromisso;

public sealed class CompromissoFormPage(IPage page, string urlBase)
{
    public string UrlCadastrar => $"{urlBase}/Compromissos/Cadastrar";

    public ILocator ErroDeValidacao(string mensagem) => page.GetByText(
        mensagem,
        new() { Exact = true }
    );

    public async Task IrParaCadastroAsync()
    {
        await page.GotoAsync(UrlCadastrar);
    }

    public async Task PreencherAsync(
        string assunto,
        DateTime dataOcorrencia,
        TimeSpan horaInicio,
        TimeSpan horaTermino,
        string tipo,
        string local = "",
        string link = ""
    )
    {
        await page.GetByLabel("Assunto").FillAsync(assunto);
        await page.GetByLabel("Data de Ocorrência").FillAsync(dataOcorrencia.ToString("yyyy-MM-dd"));
        await page.GetByLabel("Hora de Início").FillAsync(horaInicio.ToString(@"hh\:mm"));
        await page.GetByLabel("Hora de Término").FillAsync(horaTermino.ToString(@"hh\:mm"));
        await page.GetByLabel("Tipo de Compromisso").SelectOptionAsync(tipo);

        if (tipo == "Presencial")
            await page.GetByLabel("Local").FillAsync(local);
        else
            await page.GetByLabel("Link").FillAsync(link);
    }

    public async Task ConfirmarAsync()
    {
        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar", Exact = true })
            .ClickAsync();
    }
}
