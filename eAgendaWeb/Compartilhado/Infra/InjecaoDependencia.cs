using eAgendaWeb.Compartilhado.Infra.Sql;
using eAgendaWeb.Modulos.ModuloContatos.Dominio;
using eAgendaWeb.Modulos.ModuloContatos.Infra;

namespace eAgendaWeb.Compartilhado.Infra;

public static class InjecaoDependencia
{
    public static void AddInfraRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();

        // services.AddScoped<IRepositorioExemplo, RepositorioRequisicaoEmSql>();
        services.AddScoped<IRepositorioContato, RepositorioContato>();
    }
}
