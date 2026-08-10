using eAgendaWeb.Modulos.ModuloContatos.Aplicacao;
using eAgendaWeb.Modulos.ModuloContatos.Dominio;
using FluentResults;
using Moq;

namespace eAgenda.Testes.Unidade.Modulos.ModuloContatos;

[TestClass]
public sealed class ServicoContatoTests
{
    [TestMethod]
    public void Cadastrar_ComTodosOsCamposPreenchidos_DeveCadastrarContato()
    {
        // Arranjo
        Mock<IRepositorioContato> repositorioContato = new();
        Contato? contatoCadastrado = null;
        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([]);
        repositorioContato.Setup(r => r.Cadastrar(It.IsAny<Contato>()))
            .Callback<Contato>(contato => contatoCadastrado = contato);
        ServicoContato servico = new(repositorioContato.Object);
        CadastrarContatoDto dto = new("João Silva", "49999990001", "joao@email.com", "Desenvolvedor", "Google");

        // Ação
        Result resultado = servico.Cadastrar(dto);

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(contatoCadastrado);
        Assert.AreEqual("João Silva", contatoCadastrado.Nome);
        Assert.AreEqual("(49) 99999-0001", contatoCadastrado.Telefone);
        Assert.AreEqual("joao@email.com", contatoCadastrado.Email);
        Assert.AreEqual("Desenvolvedor", contatoCadastrado.Cargo);
        Assert.AreEqual("Google", contatoCadastrado.Empresa);
        repositorioContato.Verify(r => r.Cadastrar(It.IsAny<Contato>()), Times.Once);
    }

    [TestMethod]
    public void Cadastrar_ApenasComCamposObrigatorios_DeveCadastrarContato()
    {
        // Arranjo
        Mock<IRepositorioContato> repositorioContato = new();
        Contato? contatoCadastrado = null;
        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([]);
        repositorioContato.Setup(r => r.Cadastrar(It.IsAny<Contato>()))
            .Callback<Contato>(contato => contatoCadastrado = contato);
        ServicoContato servico = new(repositorioContato.Object);
        CadastrarContatoDto dto = new("Maria Souza", "48999990002", "maria@email.com");

        // Ação
        Result resultado = servico.Cadastrar(dto);

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(contatoCadastrado);
        Assert.AreEqual(string.Empty, contatoCadastrado.Cargo);
        Assert.AreEqual(string.Empty, contatoCadastrado.Empresa);
        repositorioContato.Verify(r => r.Cadastrar(It.IsAny<Contato>()), Times.Once);
    }

    [TestMethod]
    public void Cadastrar_ComCamposObrigatoriosEmBranco_DeveRetornarFalha()
    {
        // Arranjo
        Mock<IRepositorioContato> repositorioContato = new();
        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([]);
        ServicoContato servico = new(repositorioContato.Object);
        CadastrarContatoDto dto = new("", "", "");

        // Ação
        Result resultado = servico.Cadastrar(dto);

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual(nameof(Contato.Nome), resultado.Errors.Single().Metadata["Campo"]);
        repositorioContato.Verify(r => r.Cadastrar(It.IsAny<Contato>()), Times.Never);
    }

    [TestMethod]
    public void Cadastrar_ComEmailDuplicado_DeveRetornarFalha()
    {
        // Arranjo
        Mock<IRepositorioContato> repositorioContato = new();
        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([
            CriarContato("Outro", "outro@email.com", "47999990000")
        ]);
        ServicoContato servico = new(repositorioContato.Object);
        CadastrarContatoDto dto = new("Novo", "48999990002", "OUTRO@email.com");

        // Ação
        Result resultado = servico.Cadastrar(dto);

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual(nameof(CadastrarContatoDto.Email), resultado.Errors.Single().Metadata["Campo"]);
        Assert.Contains("email", resultado.Errors.Single().Message);
        repositorioContato.Verify(r => r.Cadastrar(It.IsAny<Contato>()), Times.Never);
    }

    [TestMethod]
    public void Cadastrar_ComEmailDuplicadoContendoEspacos_DeveRetornarFalha()
    {
        // Arranjo
        Mock<IRepositorioContato> repositorioContato = new();
        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([
            CriarContato("Outro", "outro@email.com", "47999990000")
        ]);
        ServicoContato servico = new(repositorioContato.Object);
        CadastrarContatoDto dto = new("Novo", "48999990002", "  outro@email.com  ");

        // Ação
        Result resultado = servico.Cadastrar(dto);

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual(nameof(CadastrarContatoDto.Email), resultado.Errors.Single().Metadata["Campo"]);
        Assert.Contains("email", resultado.Errors.Single().Message);
        repositorioContato.Verify(r => r.Cadastrar(It.IsAny<Contato>()), Times.Never);
    }

    [TestMethod]
    public void Cadastrar_ComTelefoneDuplicado_DeveRetornarFalha()
    {
        // Arranjo
        Mock<IRepositorioContato> repositorioContato = new();
        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([
            CriarContato("Outro", "outro@email.com", "(47) 99999-0000")
        ]);
        ServicoContato servico = new(repositorioContato.Object);
        CadastrarContatoDto dto = new("Novo", "47999990000", "novo@email.com");

        // Ação
        Result resultado = servico.Cadastrar(dto);

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual(nameof(CadastrarContatoDto.Telefone), resultado.Errors.Single().Metadata["Campo"]);
        Assert.Contains("telefone", resultado.Errors.Single().Message);
        repositorioContato.Verify(r => r.Cadastrar(It.IsAny<Contato>()), Times.Never);
    }

    [TestMethod]
    public void Editar_ComDadosValidos_DeveEditarContato()
    {
        // Arranjo
        Contato contato = CriarContato("João", "joao@email.com", "49999990001");
        Mock<IRepositorioContato> repositorioContato = new();
        Contato? contatoEditado = null;
        repositorioContato.Setup(r => r.SelecionarPorId(contato.Id)).Returns(contato);
        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([contato]);
        repositorioContato.Setup(r => r.Editar(contato.Id, It.IsAny<Contato>()))
            .Callback<Guid, Contato>((_, atualizado) => contatoEditado = atualizado);
        ServicoContato servico = new(repositorioContato.Object);
        EditarContatoDto dto = new(contato.Id, "João Atualizado", "49999990002", "atualizado@email.com", "Analista", "Google");

        // Ação
        Result resultado = servico.Editar(dto);

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(contatoEditado);
        Assert.AreEqual("João Atualizado", contatoEditado.Nome);
        Assert.AreEqual("(49) 99999-0002", contatoEditado.Telefone);
        Assert.AreEqual("atualizado@email.com", contatoEditado.Email);
        repositorioContato.Verify(r => r.Editar(contato.Id, It.IsAny<Contato>()), Times.Once);
    }

    [TestMethod]
    public void Editar_ComEmailUtilizadoPorOutroContato_DeveRetornarFalha()
    {
        // Arranjo
        Contato contato = CriarContato("João", "joao@email.com", "49999990001");
        Contato outroContato = CriarContato("Maria", "maria@email.com", "49999990002");
        Mock<IRepositorioContato> repositorioContato = new();
        repositorioContato.Setup(r => r.SelecionarPorId(contato.Id)).Returns(contato);
        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([contato, outroContato]);
        ServicoContato servico = new(repositorioContato.Object);
        EditarContatoDto dto = new(contato.Id, "João", "49999990001", "maria@email.com");

        // Ação
        Result resultado = servico.Editar(dto);

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual(nameof(EditarContatoDto.Email), resultado.Errors.Single().Metadata["Campo"]);
        repositorioContato.Verify(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Contato>()), Times.Never);
    }

    [TestMethod]
    public void Editar_ComEmailUtilizadoPorOutroContatoDiferindoMaiusculasMinusculas_DeveRetornarFalha()
    {
        // Arranjo
        Contato contato = CriarContato("João", "joao@email.com", "49999990001");
        Contato outroContato = CriarContato("Maria", "maria@email.com", "49999990002");
        Mock<IRepositorioContato> repositorioContato = new();
        repositorioContato.Setup(r => r.SelecionarPorId(contato.Id)).Returns(contato);
        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([contato, outroContato]);
        ServicoContato servico = new(repositorioContato.Object);
        EditarContatoDto dto = new(contato.Id, "João", "49999990001", "MARIA@email.com");

        // Ação
        Result resultado = servico.Editar(dto);

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual(nameof(EditarContatoDto.Email), resultado.Errors.Single().Metadata["Campo"]);
        repositorioContato.Verify(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Contato>()), Times.Never);
    }

    [TestMethod]
    public void Editar_ComTelefoneUtilizadoPorOutroContatoEmFormatoDiferente_DeveRetornarFalha()
    {
        // Arranjo
        Contato contato = CriarContato("João", "joao@email.com", "49999990001");
        Contato outroContato = CriarContato("Maria", "maria@email.com", "(49) 99999-0002");
        Mock<IRepositorioContato> repositorioContato = new();
        repositorioContato.Setup(r => r.SelecionarPorId(contato.Id)).Returns(contato);
        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([contato, outroContato]);
        ServicoContato servico = new(repositorioContato.Object);
        EditarContatoDto dto = new(contato.Id, "João", "49999990002", "joao@email.com");

        // Ação
        Result resultado = servico.Editar(dto);

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual(nameof(EditarContatoDto.Telefone), resultado.Errors.Single().Metadata["Campo"]);
        Assert.Contains("telefone", resultado.Errors.Single().Message);
        repositorioContato.Verify(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Contato>()), Times.Never);
    }

    [TestMethod]
    public void Editar_ComContatoInexistente_DeveRetornarFalha()
    {
        // Arranjo
        Guid contatoId = Guid.NewGuid();
        Mock<IRepositorioContato> repositorioContato = new();
        repositorioContato.Setup(r => r.SelecionarPorId(contatoId)).Returns((Contato?)null);
        ServicoContato servico = new(repositorioContato.Object);
        EditarContatoDto dto = new(contatoId, "João", "49999990001", "joao@email.com");

        // Ação
        Result resultado = servico.Editar(dto);

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.Contains("não encontrado", resultado.Errors.Single().Message);
        repositorioContato.Verify(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Contato>()), Times.Never);
    }

    [TestMethod]
    public void Editar_MantendoProprioEmailETelefone_DeveEditarContato()
    {
        // Arranjo
        Contato contato = CriarContato("João", "joao@email.com", "(49) 99999-0001");
        Mock<IRepositorioContato> repositorioContato = new();
        repositorioContato.Setup(r => r.SelecionarPorId(contato.Id)).Returns(contato);
        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([contato]);
        ServicoContato servico = new(repositorioContato.Object);
        EditarContatoDto dto = new(contato.Id, "João Atualizado", "49999990001", "JOAO@email.com");

        // Ação
        Result resultado = servico.Editar(dto);

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        repositorioContato.Verify(r => r.Editar(contato.Id, It.IsAny<Contato>()), Times.Once);
    }

    [TestMethod]
    public void SelecionarPorId_ComContatoExistente_DeveRetornarSeusDados()
    {
        // Arranjo
        Contato contato = CriarContato("João", "joao@email.com", "(49) 99999-0001", "Analista", "Google");
        Mock<IRepositorioContato> repositorioContato = new();
        repositorioContato.Setup(r => r.SelecionarPorId(contato.Id)).Returns(contato);
        ServicoContato servico = new(repositorioContato.Object);

        // Ação
        Result<ListarContatosDto> resultado = servico.SelecionarPorId(contato.Id);

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.AreEqual(contato.Id, resultado.Value.Id);
        Assert.AreEqual("João", resultado.Value.Nome);
        Assert.AreEqual("(49) 99999-0001", resultado.Value.Telefone);
        Assert.AreEqual("joao@email.com", resultado.Value.Email);
        Assert.AreEqual("Analista", resultado.Value.Cargo);
        Assert.AreEqual("Google", resultado.Value.Empresa);
    }

    [TestMethod]
    public void SelecionarPorId_ComContatoInexistente_DeveRetornarFalha()
    {
        // Arranjo
        Guid contatoId = Guid.NewGuid();
        Mock<IRepositorioContato> repositorioContato = new();
        repositorioContato.Setup(r => r.SelecionarPorId(contatoId)).Returns((Contato?)null);
        ServicoContato servico = new(repositorioContato.Object);

        // Ação
        Result<ListarContatosDto> resultado = servico.SelecionarPorId(contatoId);

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.Contains("não encontrado", resultado.Errors.Single().Message);
    }

    [TestMethod]
    public void SelecionarPorId_ComCamposOpcionaisApenasComEspacos_DeveRetornarCamposNulos()
    {
        // Arranjo
        Contato contato = CriarContato("João", "joao@email.com", "49999990001", "   ", "   ");
        Mock<IRepositorioContato> repositorioContato = new();
        repositorioContato.Setup(r => r.SelecionarPorId(contato.Id)).Returns(contato);
        ServicoContato servico = new(repositorioContato.Object);

        // Ação
        Result<ListarContatosDto> resultado = servico.SelecionarPorId(contato.Id);

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNull(resultado.Value.Cargo);
        Assert.IsNull(resultado.Value.Empresa);
    }

    [TestMethod]
    public void SelecionarTodos_ComContatosCadastrados_DeveRetornarTodos()
    {
        // Arranjo
        List<Contato> contatos = [
            CriarContato("João", "joao@email.com", "49999990001"),
            CriarContato("Maria", "maria@email.com", "49999990002")
        ];
        Mock<IRepositorioContato> repositorioContato = new();
        repositorioContato.Setup(r => r.SelecionarTodos()).Returns(contatos);
        ServicoContato servico = new(repositorioContato.Object);

        // Ação
        List<ListarContatosDto> resultado = servico.SelecionarTodos();

        // Asserção
        Assert.HasCount(2, resultado);
        Assert.IsTrue(resultado.Any(c => c.Id == contatos[0].Id));
        Assert.IsTrue(resultado.Any(c => c.Id == contatos[1].Id));
    }

    [TestMethod]
    public void SelecionarTodos_SemContatosCadastrados_DeveRetornarListaVazia()
    {
        // Arranjo
        Mock<IRepositorioContato> repositorioContato = new();
        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([]);
        ServicoContato servico = new(repositorioContato.Object);

        // Ação
        List<ListarContatosDto> resultado = servico.SelecionarTodos();

        // Asserção
        Assert.HasCount(0, resultado);
    }

    [TestMethod]
    public void Excluir_SemCompromissosVinculados_DeveExcluirContato()
    {
        // Arranjo
        Contato contato = CriarContato("João", "joao@email.com", "49999990001");
        Mock<IRepositorioContato> repositorioContato = new();
        repositorioContato.Setup(r => r.SelecionarPorId(contato.Id)).Returns(contato);
        repositorioContato.Setup(r => r.PossuiCompromissosVinculados(contato.Id)).Returns(false);
        ServicoContato servico = new(repositorioContato.Object);

        // Ação
        Result resultado = servico.Excluir(contato.Id);

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        repositorioContato.Verify(r => r.Excluir(contato.Id), Times.Once);
    }

    [TestMethod]
    public void Excluir_ComCompromissosVinculados_DeveRetornarFalha()
    {
        // Arranjo
        Contato contato = CriarContato("João", "joao@email.com", "49999990001");
        Mock<IRepositorioContato> repositorioContato = new();
        repositorioContato.Setup(r => r.SelecionarPorId(contato.Id)).Returns(contato);
        repositorioContato.Setup(r => r.PossuiCompromissosVinculados(contato.Id)).Returns(true);
        ServicoContato servico = new(repositorioContato.Object);

        // Ação
        Result resultado = servico.Excluir(contato.Id);

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.Contains("compromissos vinculados", resultado.Errors.Single().Message);
        repositorioContato.Verify(r => r.Excluir(It.IsAny<Guid>()), Times.Never);
    }

    [TestMethod]
    public void Excluir_ComContatoInexistente_DeveRetornarFalha()
    {
        // Arranjo
        Guid contatoId = Guid.NewGuid();
        Mock<IRepositorioContato> repositorioContato = new();
        repositorioContato.Setup(r => r.SelecionarPorId(contatoId)).Returns((Contato?)null);
        ServicoContato servico = new(repositorioContato.Object);

        // Ação
        Result resultado = servico.Excluir(contatoId);

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.Contains("não encontrado", resultado.Errors.Single().Message);
        repositorioContato.Verify(r => r.Excluir(It.IsAny<Guid>()), Times.Never);
    }

    private static Contato CriarContato(
        string nome,
        string email,
        string telefone,
        string cargo = "",
        string empresa = "")
    {
        return new Contato(nome, email, telefone, cargo, empresa) { Id = Guid.NewGuid() };
    }
}
