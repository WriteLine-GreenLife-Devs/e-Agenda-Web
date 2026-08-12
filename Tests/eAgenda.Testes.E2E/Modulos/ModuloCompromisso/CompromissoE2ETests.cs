using System.Text.RegularExpressions;
using eAgenda.Testes.E2E.Compartilhado;

namespace eAgenda.Testes.E2E.Modulos.ModuloCompromisso;

[TestClass]
public sealed class CompromissoE2ETests : E2ETestsBase
{
    [TestMethod]
    public async Task DeveCadastrar_Compromisso_Presencial()
    {
        CompromissoFormPage form = new(Page, UrlBase);
        CompromissoListarPage listar = new(Page, UrlBase);

        await form.IrParaCadastroAsync();
        await form.PreencherAsync("Almoço", DateTime.Today.AddDays(1), new TimeSpan(12,0,0), new TimeSpan(13,0,0), "Presencial", "Restaurante", string.Empty);
        await form.ConfirmarAsync();

        await Expect(Page).ToHaveURLAsync(listar.Url);
        await Expect(listar.Assunto("Almoço")).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveCadastrar_Compromisso_Remoto()
    {
        CompromissoFormPage form = new(Page, UrlBase);
        CompromissoListarPage listar = new(Page, UrlBase);

        await form.IrParaCadastroAsync();
        await form.PreencherAsync("Call", DateTime.Today.AddDays(1), new TimeSpan(15,0,0), new TimeSpan(16,0,0), "Remoto", string.Empty, "https://meet.test/abc");
        await form.ConfirmarAsync();

        await Expect(Page).ToHaveURLAsync(listar.Url);
        await Expect(listar.Assunto("Call")).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveEditar_Compromisso()
    {
        // Arranjo
        ExecutarComando("INSERT INTO TBCompromisso (Id, Assunto, DataOcorrencia, HoraInicio, HoraTermino, TipoCompromisso, Local, Link, ContatoId) VALUES (@Id, @Assunto, @Data, @Hi, @Ht, @Tipo, @Local, @Link, @Contato)", new { Id = Guid.NewGuid(), Assunto = "Original", Data = DateTime.Today.AddDays(2), Hi = new TimeSpan(10,0,0), Ht = new TimeSpan(11,0,0), Tipo = "Presencial", Local = "Sala", Link = (string?)null, Contato = (Guid?)null });

        CompromissoFormPage form = new(Page, UrlBase);
        CompromissoListarPage listar = new(Page, UrlBase);

        await listar.IrParaAsync();
        await listar.EditarAsync("Original");

        await form.PreencherAsync("Original Atualizado", DateTime.Today.AddDays(2), new TimeSpan(11,0,0), new TimeSpan(12,0,0), "Presencial", "Sala 2", string.Empty);
        await form.ConfirmarAsync();

        await Expect(Page).ToHaveURLAsync(listar.Url);
        await Expect(listar.Assunto("Original Atualizado")).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveExcluir_Compromisso()
    {
        ExecutarComando("INSERT INTO TBCompromisso (Id, Assunto, DataOcorrencia, HoraInicio, HoraTermino, TipoCompromisso, Local, Link, ContatoId) VALUES (@Id, @Assunto, @Data, @Hi, @Ht, @Tipo, @Local, @Link, @Contato)", new { Id = Guid.NewGuid(), Assunto = "ParaExcluir", Data = DateTime.Today.AddDays(3), Hi = new TimeSpan(9,0,0), Ht = new TimeSpan(10,0,0), Tipo = "Presencial", Local = "Sala", Link = (string?)null, Contato = (Guid?)null });

        CompromissoListarPage listar = new(Page, UrlBase);
        CompromissoExcluirPage excluir = new(Page);

        await listar.IrParaAsync();
        await listar.ExcluirAsync("ParaExcluir");

        await Expect(Page).ToHaveURLAsync(new Regex($"{Regex.Escape(UrlBase)}/Compromissos/Excluir/.*"));
        await Expect(excluir.MensagemConfirmacao).ToBeVisibleAsync();
        await excluir.ConfirmarAsync();

        await Expect(Page).ToHaveURLAsync(listar.Url);
        await Expect(listar.Assunto("ParaExcluir")).Not.ToBeVisibleAsync();
    }
}
