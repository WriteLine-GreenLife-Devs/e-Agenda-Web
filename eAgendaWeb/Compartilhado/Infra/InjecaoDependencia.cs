using eAgendaWeb.Compartilhado.Infra.Sql;
using eAgendaWeb.Modulos.ModuloCompromisso.Dominio;
using eAgendaWeb.Modulos.ModuloCompromisso.Infra;
using eAgendaWeb.Modulos.ModuloContatos.Dominio;
using eAgendaWeb.Modulos.ModuloContatos.Infra;
using eAgendaWeb.Modulos.ModuloDespesas.Dominio;
using eAgendaWeb.Modulos.ModuloDespesas.Infra;

namespace eAgendaWeb.Compartilhado.Infra;

public static class InjecaoDependencia
{
    public static void AddInfraRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();

        // services.AddScoped<IRepositorioExemplo, RepositorioRequisicaoEmSql>();
        services.AddScoped<IRepositorioContato, RepositorioContato>();
        services.AddScoped<IRepositorioCompromisso, RepositorioCompromisso>();
        services.AddScoped<IRepositorioDespesa, RepositorioDespesa>();
    }
}
