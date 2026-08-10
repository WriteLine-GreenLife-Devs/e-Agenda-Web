using System.Text.RegularExpressions;
using eAgenda.Testes.E2E.Compartilhado;

namespace eAgenda.Testes.E2E.Modulos.ModuloContato;

[TestClass]
public sealed class ContatoE2ETests : E2ETestsBase
{
    [TestMethod]
    public async Task DeveCadastrar_Contato_ComTodosOsCamposPreenchidos()
    {
        // Arranjo
        ContatoFormPage formPage = new(Page, UrlBase);
        ContatoListarPage listarPage = new(Page, UrlBase);

        // Ação
        await formPage.IrParaCadastroAsync();
        await formPage.PreencherAsync(
            "Ana Silva",
            "(49) 99999-0001",
            "ana@email.com",
            "Desenvolvedora",
            "Google"
        );
        await formPage.ConfirmarAsync();

        // Asserção
        await Expect(Page).ToHaveURLAsync(listarPage.Url);
        await Expect(listarPage.NomeDoContato("Ana Silva")).ToBeVisibleAsync();
        await Expect(listarPage.DadosDoContato("Ana Silva", "(49) 99999-0001"))
            .ToBeVisibleAsync();
        await Expect(listarPage.DadosDoContato("Ana Silva", "ana@email.com"))
            .ToBeVisibleAsync();
        await Expect(listarPage.DadosDoContato("Ana Silva", "Desenvolvedora"))
            .ToBeVisibleAsync();
        await Expect(listarPage.DadosDoContato("Ana Silva", "Google"))
            .ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveCadastrar_Contato_ComNomeNoLimiteMinimo()
    {
        // Arranjo
        ContatoFormPage formPage = new(Page, UrlBase);
        ContatoListarPage listarPage = new(Page, UrlBase);

        // Ação
        await formPage.IrParaCadastroAsync();
        await formPage.PreencherAsync(
            "Al",
            "(49) 99999-0001",
            "al@email.com"
        );
        await formPage.ConfirmarAsync();

        // Asserção
        await Expect(Page).ToHaveURLAsync(listarPage.Url);
        await Expect(listarPage.NomeDoContato("Al")).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveImpedir_Cadastro_DeContato_ComEmailDuplicado()
    {
        // Arranjo
        await CadastrarContatoAsync(
            "Ana Silva",
            "(49) 99999-0001",
            "ana@email.com"
        );
        ContatoFormPage formPage = new(Page, UrlBase);

        // Ação
        await formPage.IrParaCadastroAsync();
        await formPage.PreencherAsync(
            "Ana Souza",
            "(49) 99999-0002",
            "ana@email.com"
        );
        await formPage.ConfirmarAsync();

        // Asserção
        await Expect(Page).ToHaveURLAsync(formPage.UrlCadastrar);
        await Expect(formPage.ErroDeValidacao(
            "Já existe um contato cadastrado com este email."
        )).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveImpedir_Cadastro_DeContato_ComTelefoneInvalido()
    {
        // Arranjo
        ContatoFormPage formPage = new(Page, UrlBase);

        // Ação
        await formPage.IrParaCadastroAsync();
        await formPage.PreencherAsync(
            "Ana Silva",
            "123",
            "ana@email.com"
        );
        await formPage.ConfirmarAsync();

        // Asserção
        await Expect(Page).ToHaveURLAsync(formPage.UrlCadastrar);
        await Expect(formPage.ErroDeValidacao(
            "O campo \"Telefone\" deve conter entre 10 e 16 caracteres."
        )).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveEditar_Contato_ComDadosValidos()
    {
        // Arranjo
        await CadastrarContatoAsync(
            "Ana Silva",
            "(49) 99999-0001",
            "ana@email.com",
            "Desenvolvedora",
            "Google"
        );
        ContatoFormPage formPage = new(Page, UrlBase);
        ContatoListarPage listarPage = new(Page, UrlBase);
        await listarPage.EditarAsync("Ana Silva");

        // Ação
        await formPage.PreencherAsync(
            "Ana Silva Atualizada",
            "(49) 99999-0002",
            "ana.atualizada@email.com",
            "Engenheira de Software",
            "Meta"
        );
        await formPage.ConfirmarAsync();

        // Asserção
        await Expect(Page).ToHaveURLAsync(listarPage.Url);
        await Expect(listarPage.NomeDoContato("Ana Silva Atualizada")).ToBeVisibleAsync();
        await Expect(listarPage.NomeDoContato("Ana Silva")).Not.ToBeVisibleAsync();
        await Expect(listarPage.DadosDoContato(
            "Ana Silva Atualizada",
            "ana.atualizada@email.com"
        )).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveListar_TodosOsContatos_Cadastrados()
    {
        // Arranjo
        await CadastrarContatoAsync(
            "Ana Silva",
            "(49) 99999-0001",
            "ana@email.com"
        );
        await CadastrarContatoAsync(
            "Bruno Souza",
            "(49) 99999-0002",
            "bruno@email.com"
        );
        ContatoListarPage listarPage = new(Page, UrlBase);

        // Ação
        await listarPage.IrParaAsync();

        // Asserção
        await Expect(Page).ToHaveURLAsync(listarPage.Url);
        await Expect(listarPage.NomeDoContato("Ana Silva")).ToBeVisibleAsync();
        await Expect(listarPage.NomeDoContato("Bruno Souza")).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveExcluir_Contato_SemCompromissosVinculados()
    {
        // Arranjo
        await CadastrarContatoAsync(
            "Ana Silva",
            "(49) 99999-0001",
            "ana@email.com"
        );
        ContatoListarPage listarPage = new(Page, UrlBase);
        ContatoExcluirPage excluirPage = new(Page);
        await listarPage.ExcluirAsync("Ana Silva");

        // Ação
        await Expect(Page).ToHaveURLAsync(
            new Regex($"{Regex.Escape(UrlBase)}/Contatos/Excluir/.*")
        );
        await Expect(excluirPage.MensagemConfirmacao).ToBeVisibleAsync();
        await excluirPage.ConfirmarAsync();

        // Asserção
        await Expect(Page).ToHaveURLAsync(listarPage.Url);
        await Expect(listarPage.NomeDoContato("Ana Silva")).Not.ToBeVisibleAsync();
        await Expect(listarPage.EstadoVazio).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveImpedir_Exclusao_DeContato_ComCompromissoVinculado()
    {
        // Arranjo
        Guid contatoId = Guid.CreateVersion7();

        ExecutarComando(
            """
            INSERT INTO TBContato (Id, Nome, Email, Telefone, Cargo, Empresa)
            VALUES (@Id, @Nome, @Email, @Telefone, @Cargo, @Empresa)
            """,
            new
            {
                Id = contatoId,
                Nome = "Ana Silva",
                Email = "ana@email.com",
                Telefone = "(49) 99999-0001",
                Cargo = "Desenvolvedora",
                Empresa = "Google"
            }
        );
        ExecutarComando(
            """
            INSERT INTO TBCompromisso
                (Id, Assunto, DataOcorrencia, HoraInicio, HoraTermino,
                 TipoCompromisso, Local, Link, ContatoId)
            VALUES
                (@Id, @Assunto, @DataOcorrencia, @HoraInicio, @HoraTermino,
                 @TipoCompromisso, @Local, @Link, @ContatoId)
            """,
            new
            {
                Id = Guid.CreateVersion7(),
                Assunto = "Reunião de projeto",
                DataOcorrencia = DateTime.Today.AddDays(1),
                HoraInicio = new TimeSpan(9, 0, 0),
                HoraTermino = new TimeSpan(10, 0, 0),
                TipoCompromisso = "Presencial",
                Local = "Google",
                Link = (string?)null,
                ContatoId = contatoId
            }
        );

        ContatoListarPage listarPage = new(Page, UrlBase);
        ContatoExcluirPage excluirPage = new(Page);
        await listarPage.IrParaAsync();
        await listarPage.ExcluirAsync("Ana Silva");

        // Ação
        await Expect(Page).ToHaveURLAsync(
            new Regex($"{Regex.Escape(UrlBase)}/Contatos/Excluir/.*")
        );
        await excluirPage.ConfirmarAsync();

        // Asserção
        await Expect(Page).ToHaveURLAsync(listarPage.Url);
        await Expect(listarPage.MensagemDeErro(
            "Não é possível excluir um contato que possua compromissos vinculados."
        )).ToBeVisibleAsync();
        await Expect(listarPage.NomeDoContato("Ana Silva")).ToBeVisibleAsync();
    }

    private async Task CadastrarContatoAsync(
        string nome,
        string telefone,
        string email,
        string cargo = "",
        string empresa = ""
    )
    {
        ContatoFormPage formPage = new(Page, UrlBase);
        ContatoListarPage listarPage = new(Page, UrlBase);

        await formPage.IrParaCadastroAsync();
        await formPage.PreencherAsync(nome, telefone, email, cargo, empresa);
        await formPage.ConfirmarAsync();

        await Expect(Page).ToHaveURLAsync(listarPage.Url);
    }
}
