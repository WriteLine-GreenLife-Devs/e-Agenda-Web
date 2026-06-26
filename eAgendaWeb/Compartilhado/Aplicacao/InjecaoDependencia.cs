using eAgendaWeb.Compartilhado.Aplicacao.Logging;
using eAgendaWeb.Modulos.ModuloContatos.Aplicacao;

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
    }
}
