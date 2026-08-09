using eAgendaWeb.Modulos.ModuloContatos.Dominio;

namespace eAgenda.Testes.Unidade.Modulos.ModuloContatos;

[TestClass]
public sealed class ContatoTests
{
    [TestMethod]
    public void Validar_ComTodosCamposPreenchidos_NaoDeveRetornarErros()
    {
        // Arranjo
        Contato contato = new Contato("João Silva", "joao@email.com", "11999999999", "Desenvolvedor", "Google");

        // Ação
        List<string> erros = contato.Validar();

        // Asserção
        Assert.HasCount(0, erros);
    }

    [TestMethod]
    public void Validar_ComApenasCamposObrigatorios_NaoDeveRetornarErros()
    {
        // Arranjo
        Contato contato = new Contato("João Silva", "joao@email.com", "11999999999", "", "");

        // Ação
        List<string> erros = contato.Validar();

        // Asserção
        Assert.HasCount(0, erros);
    }

    [TestMethod]
    public void Validar_ComCamposObrigatoriosEmBranco_DeveRetornarErros()
    {
        // Arranjo
        Contato contato = new Contato("", "", "", "", "");

        // Ação
        List<string> erros = contato.Validar();

        // Asserção
        Assert.HasCount(3, erros);
        CollectionAssert.Contains(erros, "O campo \"Nome\" deve ser preenchido.");
        CollectionAssert.Contains(erros, "O campo \"Telefone\" deve ser preenchido.");
        CollectionAssert.Contains(erros, "O campo \"Email\" deve ser preenchido.");
    }

    [TestMethod]
    public void Validar_ComNomeAbaixoDoMinimo_DeveRetornarErroTamanho()
    {
        // Arranjo
        Contato contato = new Contato("A", "joao@email.com", "11999999999", "Dev", "Google");

        // Ação
        List<string> erros = contato.Validar();

        // Asserção
        Assert.HasCount(1, erros);
        CollectionAssert.Contains(erros, "O campo \"Nome\" deve conter entre 2 e 100 caracteres.");
    }

    [TestMethod]
    public void Validar_ComNomeNoLimiteMinimo_NaoDeveRetornarErros()
    {
        // Arranjo
        Contato contato = new Contato("AB", "joao@email.com", "11999999999", "Dev", "Google");

        // Ação
        List<string> erros = contato.Validar();

        // Asserção
        Assert.HasCount(0, erros);
    }

    [TestMethod]
    public void Validar_ComNomeNoLimiteMaximo_NaoDeveRetornarErros()
    {
        // Arranjo
        string nome = new string('A', 100);
        Contato contato = new Contato(nome, "joao@email.com", "11999999999", "Dev", "Google");

        // Ação
        List<string> erros = contato.Validar();

        // Asserção
        Assert.HasCount(0, erros);
    }

    [TestMethod]
    public void Validar_ComNomeAcimaDoMaximo_DeveRetornarErroTamanho()
    {
        // Arranjo
        string nome = new string('A', 101);
        Contato contato = new Contato(nome, "joao@email.com", "11999999999", "Dev", "Google");

        // Ação
        List<string> erros = contato.Validar();

        // Asserção
        Assert.HasCount(1, erros);
        CollectionAssert.Contains(erros, "O campo \"Nome\" deve conter entre 2 e 100 caracteres.");
    }

    [TestMethod]
    public void Validar_ComEmailFormatoInvalido_DeveRetornarErroEmail()
    {
        // Arranjo
        Contato contato = new Contato("João Silva", "abc", "11999999999", "Dev", "Google");

        // Ação
        List<string> erros = contato.Validar();

        // Asserção
        Assert.HasCount(1, erros);
        CollectionAssert.Contains(erros, "O campo \"Email\" é inválido.");
    }

    [TestMethod]
    public void Validar_ComEmailSemDominio_DeveRetornarErroEmail()
    {
        // Arranjo
        Contato contato = new Contato("João Silva", "joao@email", "11999999999", "Dev", "Google");

        // Ação
        List<string> erros = contato.Validar();

        // Asserção
        Assert.HasCount(1, erros);
        CollectionAssert.Contains(erros, "O campo \"Email\" é inválido.");
    }

    [TestMethod]
    public void VerificarTelefone_ComTelefoneFixo_DeveRetornarFormatado()
    {
        // Arranjo
        Contato contato = new Contato("João Silva", "joao@email.com", "1234567890", "", "");

        // Ação
        List<string> erros = contato.Validar();

        // Asserção
        Assert.HasCount(0, erros);
        Assert.AreEqual("(12) 3456-7890", contato.Telefone);
    }

    [TestMethod]
    public void VerificarTelefone_ComTelefoneCelular_DeveRetornarFormatado()
    {
        // Arranjo
        Contato contato = new Contato("João Silva", "joao@email.com", "12345678901", "", "");

        // Ação
        List<string> erros = contato.Validar();

        // Asserção
        Assert.HasCount(0, erros);
        Assert.AreEqual("(12) 34567-8901", contato.Telefone);
    }

    [TestMethod]
    public void Validar_ComTelefoneInvalido_DeveRetornarErroTelefone()
    {
        // Arranjo
        Contato contato = new Contato("João Silva", "joao@email.com", "123", "Dev", "Google");

        // Ação
        List<string> erros = contato.Validar();

        // Asserção
        Assert.HasCount(1, erros);
        CollectionAssert.Contains(erros, "O campo \"Telefone\" é inválido.");
    }

    [TestMethod]
    public void Atualizar_DeveAtualizarTodosCampos()
    {
        // Arranjo
        Contato contato = new Contato("João Silva", "joao@email.com", "11999999999", "Dev", "Google");
        Contato contatoAtualizado = new Contato("Maria Santos", "maria@email.com", "11888888888", "Gerente", "Tech Corp");

        // Ação
        contato.Atualizar(contatoAtualizado);

        // Asserção
        Assert.AreEqual("Maria Santos", contato.Nome);
        Assert.AreEqual("maria@email.com", contato.Email);
        Assert.AreEqual("11888888888", contato.Telefone);
        Assert.AreEqual("Gerente", contato.Cargo);
        Assert.AreEqual("Tech Corp", contato.Empresa);
    }
}