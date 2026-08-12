using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.Modulos.ModuloCompromisso;

public sealed class CompromissoFormPage(IPage page, string urlBase)
{
    public string UrlCadastrar => $"{urlBase}/Compromisso/Cadastrar";

    public ILocator ErroDeValidacao(string mensagem) => page.GetByText(
        mensagem,
        new() { Exact = true }
    );

    public async Task IrParaCadastroAsync()
    {
        // Navega primeiro para a listagem e clica no link "Cadastrar Novo"
        // Navega diretamente para a página de cadastro garantindo que o carregamento de rede termine
        var response = await page.GotoAsync(UrlCadastrar, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });
        Console.WriteLine($"[DEBUG] Navegação para {UrlCadastrar} retornou status: {response?.Status}");

        // Aguarda o formulário principal estar presente no DOM
        // Em caso de falha, registramos o HTML retornado para debug
        try
        {
            await page.WaitForSelectorAsync("form.card");
        }
        catch (Exception)
        {
            string html = await page.ContentAsync();
            Console.WriteLine("[DEBUG] Conteúdo da página Cadastrar (início):");
            Console.WriteLine(html.Length > 2000 ? html.Substring(0, 2000) : html);
            throw;
        }
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
        var assuntoLocator = page.Locator("input[name=Assunto]");
        await assuntoLocator.WaitForAsync();
        await assuntoLocator.FillAsync(assunto);

        var dataLocator = page.Locator("input[name=DataOcorrencia]");
        await dataLocator.WaitForAsync();
        await dataLocator.FillAsync(dataOcorrencia.ToString("yyyy-MM-dd"));

        var horaInicioLocator = page.Locator("input[name=HoraInicio]");
        await horaInicioLocator.WaitForAsync();
        await horaInicioLocator.FillAsync(horaInicio.ToString(@"hh\:mm"));

        var horaTerminoLocator = page.Locator("input[name=HoraTermino]");
        await horaTerminoLocator.WaitForAsync();
        await horaTerminoLocator.FillAsync(horaTermino.ToString(@"hh\:mm"));

        var tipoLocator = page.Locator("select[name=TipoCompromisso]");
        await tipoLocator.WaitForAsync();
        await tipoLocator.SelectOptionAsync(new SelectOptionValue { Label = tipo });

        if (tipo == "Presencial")
        {
            var localLocator = page.Locator("input[name=Local]");
            await localLocator.WaitForAsync();
            await localLocator.FillAsync(local);
        }
        else
        {
            var linkLocator = page.Locator("input[name=Link]");
            await linkLocator.WaitForAsync();
            await linkLocator.FillAsync(link);
        }
    }

    public async Task ConfirmarAsync()
    {
        await page.GetByRole(AriaRole.Button, new() { Name = "Confirmar", Exact = true })
            .ClickAsync();
    }
}
