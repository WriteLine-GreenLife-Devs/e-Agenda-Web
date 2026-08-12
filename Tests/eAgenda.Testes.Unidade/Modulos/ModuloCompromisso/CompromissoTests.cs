using eAgendaWeb.Modulos.ModuloCompromisso.Dominio;

namespace eAgenda.Testes.Unidade.Modulos.ModuloCompromisso;

[TestClass]
public sealed class CompromissoTests
{
    [TestMethod]
    public void Validar_ComDadosValidos_NaoDeveRetornarErros()
    {
        // Arranjo
        Compromisso compromisso = new(
            "Reunião",
            DateTime.Now.Date.AddDays(1),
            new TimeSpan(10, 0, 0),
            new TimeSpan(11, 0, 0),
            TipoCompromisso.Presencial,
            "Sala 1",
            string.Empty
        );

        // Ação
        List<string> erros = compromisso.Validar();

        // Asserção
        Assert.HasCount(0, erros);
    }

    [TestMethod]
    public void Validar_PresencialSemLocal_DeveRetornarErroLocal()
    {
        // Arranjo
        Compromisso compromisso = new(
            "Reunião",
            DateTime.Now.Date.AddDays(1),
            new TimeSpan(10, 0, 0),
            new TimeSpan(11, 0, 0),
            TipoCompromisso.Presencial,
            "",
            string.Empty
        );

        // Ação
        List<string> erros = compromisso.Validar();

        // Asserção
        Assert.HasCount(1, erros);
        CollectionAssert.Contains(erros, "O campo \"Local\" deve ser preenchido para compromissos presenciais.");
    }

    [TestMethod]
    public void Validar_RemotoSemLink_DeveRetornarErroLink()
    {
        // Arranjo
        Compromisso compromisso = new(
            "Chamada",
            DateTime.Now.Date.AddDays(1),
            new TimeSpan(14, 0, 0),
            new TimeSpan(15, 0, 0),
            TipoCompromisso.Remoto,
            string.Empty,
            ""
        );

        // Ação
        List<string> erros = compromisso.Validar();

        // Asserção
        Assert.HasCount(1, erros);
        CollectionAssert.Contains(erros, "O campo \"Link\" deve ser preenchido para compromissos remotos.");
    }

    [TestMethod]
    public void Validar_HoraTerminoAnteriorOuIgual_DeveRetornarErro()
    {
        // Arranjo
        Compromisso compromisso = new(
            "Reunião",
            DateTime.Now.Date.AddDays(1),
            new TimeSpan(10, 0, 0),
            new TimeSpan(10, 0, 0),
            TipoCompromisso.Presencial,
            "Sala",
            string.Empty
        );

        // Ação
        List<string> erros = compromisso.Validar();

        // Asserção
        Assert.HasCount(1, erros);
        CollectionAssert.Contains(erros, "O campo \"Hora de Término\" deve ser posterior ao horário de início.");
    }
}
