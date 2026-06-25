using eAgendaWeb.Compartilhado.Infra.Sql;

namespace eAgendaWeb.Compartilhado.Infra;

public static class InjecaoDependencia
{
    public static void AddInfraRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();

        // services.AddScoped<IRepositorioExemplo, RepositorioRequisicaoEmSql>();
    }
}
