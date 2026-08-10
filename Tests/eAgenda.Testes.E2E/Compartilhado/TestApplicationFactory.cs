using System.Text.RegularExpressions;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace eAgenda.Testes.E2E.Compartilhado;

public sealed class TestApplicationFactory : WebApplicationFactory<Program>
{
    private const string ConnectionStringMaster =
        "Server=(localdb)\\MSSQLLocalDB;Database=master;Integrated Security=true;TrustServerCertificate=true";

    private readonly string nomeBancoDados;
    private readonly string connectionString;

    public string UrlBase { get; }

    public TestApplicationFactory()
    {
        nomeBancoDados = $"eAgendaE2E_{Guid.NewGuid():N}";
        connectionString =
            $"Server=(localdb)\\MSSQLLocalDB;Database={nomeBancoDados};Integrated Security=true;TrustServerCertificate=true";

        CriarBancoDados();

        UseKestrel(0);
        StartServer();

        UrlBase = ObterUrlKestrel();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting("ConnectionStrings:eAgendaWeb", connectionString);
        builder.UseSetting("NewRelic:EndpointUrl", "http://127.0.0.1");
        builder.UseSetting("NewRelic:ApplicationName", "eAgendaWeb-testes");
        builder.UseSetting("NewRelic:LicenseKey", "chave-inerte-para-testes");
    }

    public void ExecutarComando(string comando, object? parametros = null)
    {
        using SqlConnection conexao = new(connectionString);
        conexao.Execute(comando, parametros);
    }

    public T ExecutarConsulta<T>(string consulta, object? parametros = null)
    {
        using SqlConnection conexao = new(connectionString);
        return conexao.QuerySingle<T>(consulta, parametros);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
            ExcluirBancoDados();
    }

    private void CriarBancoDados()
    {
        using SqlConnection conexaoMaster = new(ConnectionStringMaster);
        conexaoMaster.Open();
        conexaoMaster.Execute($"CREATE DATABASE [{nomeBancoDados}]");

        foreach (string nomeArquivo in new[]
        {
            "TBCategoria.sql",
            "TBDespesa.sql",
            "TBDespesaCategoria.sql",
            "TBContato.sql",
            "TBCompromisso.sql",
            "TBTarefa.sql",
            "TBItemTarefa.sql"
        })
        {
            ExecutarScript(nomeArquivo);
        }
    }

    private void ExcluirBancoDados()
    {
        SqlConnection.ClearAllPools();

        using SqlConnection conexaoMaster = new(ConnectionStringMaster);
        conexaoMaster.Open();
        conexaoMaster.Execute($"""
            ALTER DATABASE [{nomeBancoDados}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
            DROP DATABASE [{nomeBancoDados}];
            """);
    }

    private void ExecutarScript(string nomeArquivo)
    {
        string caminho = Path.Combine(AppContext.BaseDirectory, "Scripts", nomeArquivo);
        string script = File.ReadAllText(caminho);

        string[] lotes = Regex.Split(
            script,
            @"^\s*GO\s*$",
            RegexOptions.Multiline | RegexOptions.IgnoreCase
        );

        using SqlConnection conexao = new(connectionString);
        conexao.Open();

        foreach (string lote in lotes.Where(lote => !string.IsNullOrWhiteSpace(lote)))
            conexao.Execute(lote);
    }

    private string ObterUrlKestrel()
    {
        IServer servidor = Services.GetRequiredService<IServer>();
        IServerAddressesFeature? enderecos = servidor.Features.Get<IServerAddressesFeature>();

        if (enderecos is null)
            throw new InvalidOperationException("Não foi possível obter a URL do servidor.");

        return enderecos.Addresses.Single();
    }
}
