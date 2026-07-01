using eAgendaWeb.Compartilhado.Infra.Sql;
using eAgendaWeb.Modulos.ModuloCategorias.Dominio;
using eAgendaWeb.Modulos.ModuloCategorias.Infra;
using eAgendaWeb.Modulos.ModuloCompromisso.Dominio;
using eAgendaWeb.Modulos.ModuloCompromisso.Infra;
using eAgendaWeb.Modulos.ModuloContatos.Dominio;
using eAgendaWeb.Modulos.ModuloContatos.Infra;
using eAgendaWeb.Modulos.ModuloDespesas.Dominio;
using eAgendaWeb.Modulos.ModuloDespesas.Infra;
using eAgendaWeb.Modulos.ModuloTarefas.Dominio;
using eAgendaWeb.Modulos.ModuloTarefas.Infra;

namespace eAgendaWeb.Compartilhado.Infra;

public static class InjecaoDependencia
{
    public static void AddInfraRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();

        // services.AddScoped<IRepositorioExemplo, RepositorioRequisicaoEmSql>();
        services.AddScoped<IRepositorioContato, RepositorioContato>();
        services.AddScoped<IRepositorioCompromisso, RepositorioCompromisso>();
        services.AddScoped<IRepositorioCategoria, RepositorioCategoria>();
        services.AddScoped<IRepositorioDespesa, RepositorioDespesa>();
        services.AddScoped<IRepositorioTarefa, RepositorioTarefa>();
    }
}
