using eAgendaWeb.Modulos.ModuloCompromisso.Aplicacao;
using eAgendaWeb.Modulos.ModuloCompromisso.Dominio;
using eAgendaWeb.Modulos.ModuloContatos.Dominio;
using FluentResults;
using Moq;

namespace eAgenda.Testes.Unidade.Modulos.ModuloCompromisso;

[TestClass]
public sealed class ServicoCompromissoTests
{
    [TestMethod]
    public void Cadastrar_ComContatoInexistente_DeveRetornarFalha()
    {
        // Arranjo
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();
        Mock<IRepositorioContato> repositorioContato = new();
        repositorioContato.Setup(r => r.SelecionarTodos()).Returns(new List<Contato>());
        ServicoCompromisso servico = new(repositorioCompromisso.Object, repositorioContato.Object);
        CadastrarCompromissoDto dto = new("Reunião", DateTime.Now.Date.AddDays(1), new TimeSpan(10,0,0), new TimeSpan(11,0,0), TipoCompromisso.Presencial, "Sala", null, Guid.NewGuid());

        // Ação
        Result resultado = servico.Cadastrar(dto);

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual(nameof(CadastrarCompromissoDto.ContatoId), resultado.Errors.Single().Metadata["Campo"]);
        repositorioCompromisso.Verify(r => r.Cadastrar(It.IsAny<Compromisso>()), Times.Never);
    }

    [TestMethod]
    public void Cadastrar_ComHorarioConflitante_DeveRetornarFalha()
    {
        // Arranjo
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();
        Mock<IRepositorioContato> repositorioContato = new();
        Guid contatoId = Guid.NewGuid();
        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([ new Contato("X","x@x.com","47999990000", "", "") { Id = contatoId } ]);

        Compromisso existente = new("Existente", DateTime.Now.Date.AddDays(1), new TimeSpan(10,30,0), new TimeSpan(11,30,0), TipoCompromisso.Presencial, "Sala", string.Empty);
        repositorioCompromisso.Setup(r => r.SelecionarTodos()).Returns(new List<Compromisso> { existente });

        ServicoCompromisso servico = new(repositorioCompromisso.Object, repositorioContato.Object);
        CadastrarCompromissoDto dto = new("Novo", DateTime.Now.Date.AddDays(1), new TimeSpan(11,0,0), new TimeSpan(12,0,0), TipoCompromisso.Presencial, "Sala", null, contatoId);

        // Ação
        Result resultado = servico.Cadastrar(dto);

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual(nameof(CadastrarCompromissoDto.HoraInicio), resultado.Errors.Single().Metadata["Campo"]);
        repositorioCompromisso.Verify(r => r.Cadastrar(It.IsAny<Compromisso>()), Times.Never);
    }

    [TestMethod]
    public void Cadastrar_ComDadosValidos_DeveCadastrarCompromisso()
    {
        // Arranjo
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();
        Mock<IRepositorioContato> repositorioContato = new();
        Guid contatoId = Guid.NewGuid();
        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([ new Contato("X","x@x.com","47999990000", "", "") { Id = contatoId } ]);
        Compromisso? compromissoCadastrado = null;
        repositorioCompromisso.Setup(r => r.SelecionarTodos()).Returns(new List<Compromisso>());
        repositorioCompromisso.Setup(r => r.Cadastrar(It.IsAny<Compromisso>())).Callback<Compromisso>(c => compromissoCadastrado = c);

        ServicoCompromisso servico = new(repositorioCompromisso.Object, repositorioContato.Object);
        CadastrarCompromissoDto dto = new("Novo", DateTime.Now.Date.AddDays(1), new TimeSpan(9,0,0), new TimeSpan(10,0,0), TipoCompromisso.Remoto, null, "https://meet.test", contatoId);

        // Ação
        Result resultado = servico.Cadastrar(dto);

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(compromissoCadastrado);
        Assert.AreEqual("Novo", compromissoCadastrado.Assunto);
        Assert.AreEqual(TipoCompromisso.Remoto, compromissoCadastrado.TipoCompromisso);
        repositorioCompromisso.Verify(r => r.Cadastrar(It.IsAny<Compromisso>()), Times.Once);
    }
}
