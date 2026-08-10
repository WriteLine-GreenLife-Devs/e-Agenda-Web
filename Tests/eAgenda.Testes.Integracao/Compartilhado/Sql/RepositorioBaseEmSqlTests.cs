using System.Text.RegularExpressions;
using Dapper;
using eAgendaWeb.Modulos.ModuloCategorias.Infra;
using eAgendaWeb.Modulos.ModuloCompromisso.Infra;
using eAgendaWeb.Modulos.ModuloContatos.Infra;
using eAgendaWeb.Modulos.ModuloDespesas.Infra;
using eAgendaWeb.Modulos.ModuloTarefas.Infra;
using Microsoft.Data.SqlClient;

namespace eAgenda.Testes.Integracao.Compartilhado.Sql;

public abstract class RepositorioBaseEmSqlTests
{
    private const string ConnectionStringMaster =
        "Server=(localdb)\\MSSQLLocalDB;Database=master;Integrated Security=true;TrustServerCertificate=true";

    private string nomeBancoDados = null!;

    protected SqlConnectionFactoryTests connectionFactory = null!;
    protected RepositorioCategoria repositorioCategoria = null!;
    protected RepositorioDespesa repositorioDespesa = null!;
    protected RepositorioContato repositorioContato = null!;
    protected RepositorioCompromisso repositorioCompromisso = null!;
    protected RepositorioTarefa repositorioTarefa = null!;

    // Ganchos
    [TestInitialize]
    public void InicializarBancoDados()
    {
        nomeBancoDados = $"eAgendaTestes_{Guid.NewGuid():N}";

        using SqlConnection conexaoMaster = new(ConnectionStringMaster);
        conexaoMaster.Open();
        conexaoMaster.Execute($"CREATE DATABASE [{nomeBancoDados}]");

        string connectionString =
            $"Server=(localdb)\\MSSQLLocalDB;Database={nomeBancoDados};Integrated Security=true;TrustServerCertificate=true";

        connectionFactory = new SqlConnectionFactoryTests(connectionString);

        ExecutarScript("TBCategoria.sql");
        ExecutarScript("TBDespesa.sql");
        ExecutarScript("TBDespesaCategoria.sql");
        ExecutarScript("TBContato.sql");
        ExecutarScript("TBCompromisso.sql");
        ExecutarScript("TBTarefa.sql");
        ExecutarScript("TBItemTarefa.sql");

        repositorioCategoria = new RepositorioCategoria(connectionFactory);
        repositorioDespesa = new RepositorioDespesa(connectionFactory);
        repositorioContato = new RepositorioContato(connectionFactory);
        repositorioCompromisso = new RepositorioCompromisso(connectionFactory);
        repositorioTarefa = new RepositorioTarefa(connectionFactory);
    }

    [TestCleanup]
    public void DescartarBancoDados()
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

        using SqlConnection conexao = connectionFactory.CreateConnection();
        conexao.Open();

        foreach (string lote in lotes.Where(l => !string.IsNullOrWhiteSpace(l)))
            conexao.Execute(lote);
    }
}
