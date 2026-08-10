using eAgenda.Testes.Integracao.Compartilhado.Sql;
using eAgendaWeb.Modulos.ModuloCompromisso.Dominio;
using eAgendaWeb.Modulos.ModuloContatos.Aplicacao;
using eAgendaWeb.Modulos.ModuloContatos.Dominio;
using FluentResults;
using Microsoft.Data.SqlClient;

namespace eAgenda.Testes.Integracao.Modulos.ModuloContato;

[TestClass]
public sealed class RepositorioContatoEmSqlTests : RepositorioBaseEmSqlTests
{
    [TestMethod]
    public void CadastrarESelecionarPorId_ComTodosOsCampos_DeveCarregarContato()
    {
        // Arranjo
        Contato contato = CriarContato(
            "Ana Silva",
            "ana@email.com",
            "(49) 99999-0001",
            "Desenvolvedora",
            "Google"
        );

        // Ação
        repositorioContato.Cadastrar(contato);
        Contato? contatoSelecionado = repositorioContato.SelecionarPorId(contato.Id);

        // Asserção
        Assert.IsNotNull(contatoSelecionado);
        Assert.AreEqual(contato.Id, contatoSelecionado.Id);
        Assert.AreEqual("Ana Silva", contatoSelecionado.Nome);
        Assert.AreEqual("ana@email.com", contatoSelecionado.Email);
        Assert.AreEqual("(49) 99999-0001", contatoSelecionado.Telefone);
        Assert.AreEqual("Desenvolvedora", contatoSelecionado.Cargo);
        Assert.AreEqual("Google", contatoSelecionado.Empresa);
    }

    [TestMethod]
    public void Cadastrar_ApenasComCamposObrigatorios_DevePersistirContato()
    {
        // Arranjo
        Contato contato = CriarContato("Ana Silva", "ana@email.com", "(49) 99999-0001");

        // Ação
        repositorioContato.Cadastrar(contato);
        Contato? contatoSelecionado = repositorioContato.SelecionarPorId(contato.Id);

        // Asserção
        Assert.IsNotNull(contatoSelecionado);
        Assert.AreEqual(string.Empty, contatoSelecionado.Cargo);
        Assert.AreEqual(string.Empty, contatoSelecionado.Empresa);
    }

    [TestMethod]
    public void Cadastrar_ComEmailDuplicado_DeveRejeitarSegundoRegistro()
    {
        // Arranjo
        repositorioContato.Cadastrar(
            CriarContato("Ana Silva", "ana@email.com", "(49) 99999-0001")
        );
        Contato contatoDuplicado = CriarContato(
            "Ana Silva",
            "ana@email.com",
            "(49) 99999-0002"
        );

        // Ação
        Action acao = () => repositorioContato.Cadastrar(contatoDuplicado);

        // Asserção
        Assert.ThrowsExactly<SqlException>(acao);
        Assert.HasCount(1, repositorioContato.SelecionarTodos());
    }

    [TestMethod]
    public void Cadastrar_ComTelefoneDuplicado_DeveRejeitarSegundoRegistro()
    {
        // Arranjo
        repositorioContato.Cadastrar(
            CriarContato("Ana Silva", "ana@email.com", "(49) 99999-0001")
        );
        Contato contatoDuplicado = CriarContato(
            "Bruno Souza",
            "bruno@email.com",
            "(49) 99999-0001"
        );

        // Ação
        Action acao = () => repositorioContato.Cadastrar(contatoDuplicado);

        // Asserção
        Assert.ThrowsExactly<SqlException>(acao);
        Assert.HasCount(1, repositorioContato.SelecionarTodos());
    }

    [TestMethod]
    public void Editar_ComDadosValidos_DeveAtualizarContato()
    {
        // Arranjo
        Contato contato = CriarContato("Ana Silva", "ana@email.com", "(49) 99999-0001");
        repositorioContato.Cadastrar(contato);
        Contato contatoAtualizado = CriarContato(
            "Ana Silva Atualizada",
            "ana.souza@email.com",
            "(49) 99999-0002",
            "Analista",
            "Google"
        );

        // Ação
        bool conseguiuEditar = repositorioContato.Editar(contato.Id, contatoAtualizado);
        Contato? contatoSelecionado = repositorioContato.SelecionarPorId(contato.Id);

        // Asserção
        Assert.IsTrue(conseguiuEditar);
        Assert.IsNotNull(contatoSelecionado);
        Assert.AreEqual("Ana Silva Atualizada", contatoSelecionado.Nome);
        Assert.AreEqual("ana.souza@email.com", contatoSelecionado.Email);
        Assert.AreEqual("(49) 99999-0002", contatoSelecionado.Telefone);
        Assert.AreEqual("Analista", contatoSelecionado.Cargo);
        Assert.AreEqual("Google", contatoSelecionado.Empresa);
    }

    [TestMethod]
    public void Editar_ComEmailUtilizadoPorOutroContato_DeveRejeitarAlteracao()
    {
        // Arranjo
        Contato ana = CriarContato("Ana Silva", "ana@email.com", "(49) 99999-0001");
        Contato bruno = CriarContato("Bruno Souza", "bruno@email.com", "(49) 99999-0002");
        repositorioContato.Cadastrar(ana);
        repositorioContato.Cadastrar(bruno);

        Contato contatoAtualizado = CriarContato(
            "Bruno Souza",
            "ana@email.com",
            "(49) 99999-0002"
        );

        // Ação
        Action acao = () => repositorioContato.Editar(bruno.Id, contatoAtualizado);

        // Asserção
        Assert.ThrowsExactly<SqlException>(acao);
        Assert.AreEqual("bruno@email.com", repositorioContato.SelecionarPorId(bruno.Id)?.Email);
    }

    [TestMethod]
    public void Editar_MantendoProprioEmailETelefone_DeveAtualizarContato()
    {
        // Arranjo
        Contato contato = CriarContato("Ana Silva", "ana@email.com", "(49) 99999-0001");
        repositorioContato.Cadastrar(contato);
        Contato contatoAtualizado = CriarContato(
            "Ana Silva Atualizada",
            "ana@email.com",
            "(49) 99999-0001"
        );

        // Ação
        bool conseguiuEditar = repositorioContato.Editar(contato.Id, contatoAtualizado);

        // Asserção
        Assert.IsTrue(conseguiuEditar);
        Assert.AreEqual("Ana Silva Atualizada", repositorioContato.SelecionarPorId(contato.Id)?.Nome);
    }

    [TestMethod]
    public void SelecionarPorId_ComContatoCadastrado_DeveCarregarSeusDados()
    {
        // Arranjo
        Contato contato = CriarContato(
            "Ana Silva",
            "ana@email.com",
            "(49) 99999-0001",
            "Desenvolvedora",
            "Google"
        );
        repositorioContato.Cadastrar(contato);

        // Ação
        Contato? contatoSelecionado = repositorioContato.SelecionarPorId(contato.Id);

        // Asserção
        Assert.IsNotNull(contatoSelecionado);
        Assert.AreEqual(contato.Id, contatoSelecionado.Id);
        Assert.AreEqual("Ana Silva", contatoSelecionado.Nome);
        Assert.AreEqual("ana@email.com", contatoSelecionado.Email);
        Assert.AreEqual("(49) 99999-0001", contatoSelecionado.Telefone);
    }

    [TestMethod]
    public void SelecionarTodos_ComContatosCadastrados_DeveCarregarTodosRegistros()
    {
        // Arranjo
        repositorioContato.Cadastrar(
            CriarContato("Ana Silva", "ana@email.com", "(49) 99999-0001")
        );
        repositorioContato.Cadastrar(
            CriarContato("Bruno Souza", "bruno@email.com", "(49) 99999-0002")
        );

        // Ação
        List<Contato> contatos = repositorioContato.SelecionarTodos();

        // Asserção
        Assert.HasCount(2, contatos);
        CollectionAssert.AreEquivalent(
            new[] { "Ana Silva", "Bruno Souza" },
            contatos.Select(c => c.Nome).ToArray()
        );
    }

    [TestMethod]
    public void Excluir_SemCompromissosVinculados_DeveRemoverContato()
    {
        // Arranjo
        Contato contato = CriarContato("Ana Silva", "ana@email.com", "(49) 99999-0001");
        repositorioContato.Cadastrar(contato);

        // Ação
        bool conseguiuExcluir = repositorioContato.Excluir(contato.Id);

        // Asserção
        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(repositorioContato.SelecionarPorId(contato.Id));
    }

    [TestMethod]
    public void Excluir_ComCompromissosVinculados_DeveBloquearExclusao()
    {
        // Arranjo
        Contato contato = CriarContato("Ana Silva", "ana@email.com", "(49) 99999-0001");
        repositorioContato.Cadastrar(contato);

        Compromisso compromisso = new(
            "Reunião",
            DateTime.Today.AddDays(1),
            new TimeSpan(10, 0, 0),
            new TimeSpan(11, 0, 0),
            TipoCompromisso.Presencial,
            "Sala 1",
            string.Empty,
            contato.Id
        );
        repositorioCompromisso.Cadastrar(compromisso);

        ServicoContato servico = new(repositorioContato);

        // Ação
        Result resultado = servico.Excluir(contato.Id);

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.Contains("compromissos vinculados", resultado.Errors.Single().Message);
        Assert.IsNotNull(repositorioContato.SelecionarPorId(contato.Id));
    }

    private static Contato CriarContato(
        string nome,
        string email,
        string telefone,
        string cargo = "",
        string empresa = "")
    {
        return new Contato(nome, email, telefone, cargo, empresa);
    }
}
