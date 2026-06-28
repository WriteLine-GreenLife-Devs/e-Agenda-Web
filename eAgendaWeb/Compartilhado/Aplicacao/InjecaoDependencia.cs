using eAgendaWeb.Compartilhado.Aplicacao.Logging;
using eAgendaWeb.Modulos.ModuloCompromisso.Aplicacao;
using eAgendaWeb.Modulos.ModuloContatos.Aplicacao;
using eAgendaWeb.Modulos.ModuloDespesas.Aplicacao;

namespace eAgendaWeb.Compartilhado.Aplicacao;

public static class InjecaoDependencia
{
    public static void AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration,
        ILoggingBuilder logging
    )
    {
        services.AddSerilogLogger(configuration, logging);

        // services.AddScoped<ServicoExemplo>();
        services.AddScoped<ServicoContato>();
        services.AddScoped<ServicoCompromisso>();
        services.AddScoped<ServicoDespesa>();
    }
}
