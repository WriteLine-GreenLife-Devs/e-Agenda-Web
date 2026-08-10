using Microsoft.Playwright.MSTest;

namespace eAgenda.Testes.E2E.Compartilhado;

public abstract class E2ETestsBase : PageTest
{
    private TestApplicationFactory aplicacao = null!;

    protected string UrlBase { get; set; } = string.Empty;

    [TestInitialize]
    public async Task InicializarAplicacao()
    {
        aplicacao = new TestApplicationFactory();

        UrlBase = aplicacao.UrlBase;
    }

    [TestCleanup]
    public async Task EncerrarAplicacao()
    {
        try
        {
            if (aplicacao is not null)
                await aplicacao.DisposeAsync();
        }
        finally
        {
            aplicacao = null!;
        }
    }

    protected void ExecutarComando(string comando, object? parametros = null)
    {
        aplicacao.ExecutarComando(comando, parametros);
    }

    protected T ExecutarConsulta<T>(string consulta, object? parametros = null)
    {
        return aplicacao.ExecutarConsulta<T>(consulta, parametros);
    }
}
