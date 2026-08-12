using Dapper;
using eAgenda.Testes.Integracao.Compartilhado.Sql;
using eAgendaWeb.Modulos.ModuloDespesas.Dominio;

namespace eAgenda.Testes.Integracao.Modulos.ModuloDespesas;

[TestClass]
public sealed class RepositorioDespesaEmSqlTests : RepositorioBaseEmSqlTests
{
    [TestMethod]
    public void CadastrarESelecionarPorId_ComDadosValidos_DeveCarregarDespesa()
    {
        // Arranjo
        Despesa despesa = new("Compra", DateTime.Today, 120.5m, FormaPagamento.AVista, null);

        // Ação
        repositorioDespesa.Cadastrar(despesa);
        Despesa? selecionada = repositorioDespesa.SelecionarPorId(despesa.Id);

        // Asserção
        Assert.IsNotNull(selecionada);
        Assert.AreEqual(despesa.Id, selecionada.Id);
        Assert.AreEqual(120.5m, selecionada.Valor);
    }

    [TestMethod]
    public void AdicionarCategoriasESelecionarPorCategoria_DeveRelacionarCategorias()
    {
        // Arranjo
        Despesa despesa = new("Compra", DateTime.Today, 200m, FormaPagamento.AVista, null);
        repositorioDespesa.Cadastrar(despesa);

        // Criar categoria diretamente via SQL
        Guid categoriaId = Guid.NewGuid();
        using var conexao = connectionFactory.CreateConnection();
        conexao.Open();
        conexao.Execute("INSERT INTO dbo.TBCategoria (Id, Titulo) VALUES (@Id, @Titulo)", new { Id = categoriaId, Titulo = "Alimentação" });

        // Ação
        repositorioDespesa.AdicionarCategorias(despesa.Id, new List<Guid> { categoriaId });
        List<Guid> categorias = repositorioDespesa.SelecionarCategorias(despesa.Id);
        List<Despesa> despesasPorCategoria = repositorioDespesa.SelecionarPorCategoria(categoriaId);

        // Asserção
        Assert.HasCount(1, categorias);
        Assert.AreEqual(categoriaId, categorias.Single());
        Assert.HasCount(1, despesasPorCategoria);
        Assert.AreEqual(despesa.Id, despesasPorCategoria.Single().Id);
    }

    [TestMethod]
    public void Editar_ComDadosValidos_DeveAtualizarDespesaECategorias()
    {
        // Arranjo
        Despesa despesa = new("Compra", DateTime.Today, 50m, FormaPagamento.AVista, null);
        repositorioDespesa.Cadastrar(despesa);

        Despesa atualizada = new("Compra Atualizada", despesa.DataOcorrencia, 75m, FormaPagamento.Debito, null);

        // Ação
        bool conseguiu = repositorioDespesa.Editar(despesa.Id, atualizada);
        Despesa? carregada = repositorioDespesa.SelecionarPorId(despesa.Id);

        // Asserção
        Assert.IsTrue(conseguiu);
        Assert.IsNotNull(carregada);
        Assert.AreEqual(75m, carregada.Valor);
    }

    [TestMethod]
    public void Excluir_ComCategoriasVinculadas_DeveRemoverDespesaEAssociacoes()
    {
        // Arranjo
        Despesa despesa = new("Compra", DateTime.Today, 300m, FormaPagamento.AVista, null);
        repositorioDespesa.Cadastrar(despesa);

        Guid categoriaId = Guid.NewGuid();
        using var conexao = connectionFactory.CreateConnection();
        conexao.Open();
        conexao.Execute("INSERT INTO dbo.TBCategoria (Id, Titulo) VALUES (@Id, @Titulo)", new { Id = categoriaId, Titulo = "Viagem" });
        repositorioDespesa.AdicionarCategorias(despesa.Id, new List<Guid> { categoriaId });

        // Ação
        bool excluiu = repositorioDespesa.Excluir(despesa.Id);

        int countAssociacoes = conexao.ExecuteScalar<int>("SELECT COUNT(*) FROM dbo.TBDespesaCategoria WHERE DespesaId = @DespesaId", new { DespesaId = despesa.Id });
        Despesa? selecionada = repositorioDespesa.SelecionarPorId(despesa.Id);

        // Asserção
        Assert.IsTrue(excluiu);
        Assert.AreEqual(0, countAssociacoes);
        Assert.IsNull(selecionada);
    }
}
