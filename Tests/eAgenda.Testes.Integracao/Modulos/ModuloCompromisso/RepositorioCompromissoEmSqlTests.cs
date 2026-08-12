using Dapper;
using eAgenda.Testes.Integracao.Compartilhado.Sql;
using eAgendaWeb.Modulos.ModuloCompromisso.Dominio;

namespace eAgenda.Testes.Integracao.Modulos.ModuloCompromisso;

[TestClass]
public sealed class RepositorioCompromissoEmSqlTests : RepositorioBaseEmSqlTests
{
    [TestMethod]
    public void CadastrarESelecionarPorId_ComDadosValidos_DeveCarregarCompromisso()
    {
        // Arranjo
        Compromisso compromisso = new(
            "Standup",
            DateTime.Today.AddDays(1),
            new TimeSpan(9, 0, 0),
            new TimeSpan(9, 30, 0),
            TipoCompromisso.Presencial,
            "Sala 1",
            string.Empty
        );

        // Ação
        repositorioCompromisso.Cadastrar(compromisso);
        Compromisso? selecionado = repositorioCompromisso.SelecionarPorId(compromisso.Id);

        // Asserção
        Assert.IsNotNull(selecionado);
        Assert.AreEqual(compromisso.Id, selecionado.Id);
        Assert.AreEqual("Standup", selecionado.Assunto);
        Assert.AreEqual(TipoCompromisso.Presencial, selecionado.TipoCompromisso);
    }

    [TestMethod]
    public void Editar_ComDadosValidos_DeveAtualizarRegistro()
    {
        // Arranjo
        Compromisso compromisso = new(
            "Reunião",
            DateTime.Today.AddDays(2),
            new TimeSpan(14, 0, 0),
            new TimeSpan(15, 0, 0),
            TipoCompromisso.Remoto,
            string.Empty,
            "https://meet.test"
        );

        repositorioCompromisso.Cadastrar(compromisso);

        Compromisso atualizado = new(
            "Reunião Atualizada",
            compromisso.DataOcorrencia,
            new TimeSpan(15, 0, 0),
            new TimeSpan(16, 0, 0),
            TipoCompromisso.Remoto,
            string.Empty,
            "https://meet.test/updated"
        );

        // Ação
        bool conseguiu = repositorioCompromisso.Editar(compromisso.Id, atualizado);
        Compromisso? carregado = repositorioCompromisso.SelecionarPorId(compromisso.Id);

        // Asserção
        Assert.IsTrue(conseguiu);
        Assert.IsNotNull(carregado);
        Assert.AreEqual("Reunião Atualizada", carregado.Assunto);
        Assert.AreEqual(new TimeSpan(15, 0, 0), carregado.HoraInicio);
    }

    [TestMethod]
    public void Excluir_ComRegistroExistente_DeveRemoverRegistro()
    {
        // Arranjo
        Compromisso compromisso = new(
            "Evento",
            DateTime.Today.AddDays(3),
            new TimeSpan(18, 0, 0),
            new TimeSpan(19, 0, 0),
            TipoCompromisso.Presencial,
            "Auditório",
            string.Empty
        );

        repositorioCompromisso.Cadastrar(compromisso);

        // Ação
        bool excluiu = repositorioCompromisso.Excluir(compromisso.Id);
        Compromisso? selecionado = repositorioCompromisso.SelecionarPorId(compromisso.Id);

        // Asserção
        Assert.IsTrue(excluiu);
        Assert.IsNull(selecionado);
    }

    [TestMethod]
    public void SelecionarTodos_ComRegistrosCadastrados_DeveRetornarListaOrdenada()
    {
        // Arranjo
        repositorioCompromisso.Cadastrar(new Compromisso("A", DateTime.Today.AddDays(5), new TimeSpan(9,0,0), new TimeSpan(10,0,0), TipoCompromisso.Presencial, "L", string.Empty));
        repositorioCompromisso.Cadastrar(new Compromisso("B", DateTime.Today.AddDays(4), new TimeSpan(9,0,0), new TimeSpan(10,0,0), TipoCompromisso.Presencial, "L", string.Empty));

        // Ação
        List<Compromisso> lista = repositorioCompromisso.SelecionarTodos();

        // Asserção
        Assert.HasCount(2, lista);
        Assert.IsTrue(lista[0].DataOcorrencia <= lista[1].DataOcorrencia);
    }
}
