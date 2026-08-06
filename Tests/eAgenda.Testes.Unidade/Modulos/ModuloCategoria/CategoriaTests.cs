using eAgendaWeb.Modulos.ModuloCategorias.Dominio;

namespace eAgenda.Testes.Unidade.Modulos.ModuloCategorias;

[TestClass]
public sealed class CategoriaTests
{
    [TestMethod]
    public void Validar_ComTituloVazio_DeveRetornarErros()
    {
        // Arranjo
        Categoria categoria = new Categoria(string.Empty);

        // Ação
        List<string> erros = categoria.Validar();

        // Asserção
        Assert.IsTrue(erros.Count > 0);
        CollectionAssert.Contains(erros, "O título da categoria é obrigatório.");
    }

    [TestMethod]
    public void Validar_ComTituloApenasEspacos_DeveRetornarErroObrigatoriedade()
    {
        // Arranjo
        Categoria categoria = new Categoria("   ");

        // Ação
        List<string> erros = categoria.Validar();

        // Asserção
        CollectionAssert.Contains(erros, "O título da categoria é obrigatório.");
    }

    [TestMethod]
    public void Validar_ComTituloCurto_DeveRetornarErroTamanho()
    {
        // Arranjo
        Categoria categoria = new Categoria("A");

        // Ação
        List<string> erros = categoria.Validar();

        // Asserção
        CollectionAssert.Contains(erros, "O título da categoria deve possuir entre 2 e 100 caracteres.");
    }

    [TestMethod]
    public void Validar_ComTituloNoLimiteMinimo_NaoDeveRetornarErros()
    {
        // Arranjo
        Categoria categoria = new Categoria("AB");

        // Ação
        List<string> erros = categoria.Validar();

        // Asserção
        Assert.AreEqual(0, erros.Count);
    }

    [TestMethod]
    public void Validar_ComTituloNoLimiteMaximo_NaoDeveRetornarErros()
    {
        // Arranjo
        string titulo = new string('A', 100);
        Categoria categoria = new Categoria(titulo);

        // Ação
        List<string> erros = categoria.Validar();

        // Asserção
        Assert.AreEqual(0, erros.Count);
    }

    [TestMethod]
    public void Validar_ComTituloAcimaDoMaximo_DeveRetornarErroTamanho()
    {
        // Arranjo
        string titulo = new string('A', 101);
        Categoria categoria = new Categoria(titulo);

        // Ação
        List<string> erros = categoria.Validar();

        // Asserção
        CollectionAssert.Contains(erros, "O título da categoria deve possuir entre 2 e 100 caracteres.");
    }

    [TestMethod]
    public void Validar_ComDadosValidos_NaoDeveRetornarErros()
    {
        // Arranjo
        Categoria categoria = new Categoria("Alimentação");

        // Ação
        List<string> erros = categoria.Validar();

        // Asserção
        Assert.AreEqual(0, erros.Count);
    }

    [TestMethod]
    public void Atualizar_DeveAtualizar_Titulo()
    {
        // Arranjo
        Categoria categoria = new Categoria("Alimentação");
        Categoria categoriaAtualizada = new Categoria("Transporte");

        // Ação
        categoria.Atualizar(categoriaAtualizada);

        // Asserção
        Assert.AreEqual("Transporte", categoria.Titulo);
    }
}