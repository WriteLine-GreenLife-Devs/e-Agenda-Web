using eAgendaWeb.Modulos.ModuloDespesas.Dominio;

namespace eAgenda.Testes.Unidade.Modulos.ModuloDespesas;

[TestClass]
public sealed class DespesaTests
{
    [TestMethod]
    public void Validar_ComValorZero_DeveRetornarErro()
    {
        // Arranjo
        Despesa despesa = new("Compra", DateTime.Now.Date, 0m, FormaPagamento.AVista, null);

        // Ação
        List<string> erros = despesa.Validar();

        // Asserção
        Assert.IsTrue(erros.Any());
        CollectionAssert.Contains(erros, "O valor da despesa deve ser maior que zero.");
    }

    [TestMethod]
    public void Validar_CreditoSemParcelas_DeveRetornarErro()
    {
        // Arranjo
        Despesa despesa = new("Compra", DateTime.Now.Date, 100m, FormaPagamento.Credito, null);

        // Ação
        List<string> erros = despesa.Validar();

        // Asserção
        Assert.IsTrue(erros.Any());
        CollectionAssert.Contains(erros, "Crédito exige parcelas.");
    }

    [TestMethod]
    public void Validar_ParcelasForaDoRange_DeveRetornarErro()
    {
        // Arranjo
        Despesa despesa = new("Compra", DateTime.Now.Date, 100m, FormaPagamento.Credito, 25);

        // Ação
        List<string> erros = despesa.Validar();

        // Asserção
        Assert.IsTrue(erros.Any());
        CollectionAssert.Contains(erros, "Parcelas devem estar entre 1 e 24.");
    }

    [TestMethod]
    public void Validar_NaoCreditoComParcelasMaiorQueUm_DeveRetornarErro()
    {
        // Arranjo
        Despesa despesa = new("Compra", DateTime.Now.Date, 100m, FormaPagamento.Debito, 2);

        // Ação
        List<string> erros = despesa.Validar();

        // Asserção
        Assert.IsTrue(erros.Any());
        CollectionAssert.Contains(erros, "Parcelamento permitido somente para crédito.");
    }

    [TestMethod]
    public void Validar_ComDadosValidos_NaoDeveRetornarErros()
    {
        // Arranjo
        Despesa despesa = new("Compra", DateTime.Now.Date, 150.50m, FormaPagamento.Credito, 3);

        // Ação
        List<string> erros = despesa.Validar();

        // Asserção
        Assert.HasCount(0, erros);
    }
}
