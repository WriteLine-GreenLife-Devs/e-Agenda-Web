using System.Text.RegularExpressions;
using eAgenda.Testes.E2E.Compartilhado;

namespace eAgenda.Testes.E2E.Modulos.ModuloCategoria;

[TestClass]
public sealed class CategoriaE2ETests : E2ETestsBase
{
    [TestMethod]
    public async Task DeveCadastrar_Categoria_ComDadosValidos()
    {
        // Arranjo
        CategoriaFormPage formPage = new(Page, UrlBase);
        CategoriaListarPage listarPage = new(Page, UrlBase);

        // Ação
        await formPage.IrParaCadastroAsync();
        await formPage.PreencherTituloAsync("Alimentação");
        await formPage.ConfirmarAsync();

        // Asserção
        await Expect(Page).ToHaveURLAsync(listarPage.Url);
        await Expect(listarPage.TituloDaCategoria("Alimentação")).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveImpedir_Cadastro_DeCategoria_SemTitulo()
    {
        // Arranjo
        CategoriaFormPage formPage = new(Page, UrlBase);

        // Ação
        await formPage.IrParaCadastroAsync();
        await formPage.ConfirmarAsync();

        // Asserção
        await Expect(Page).ToHaveURLAsync(formPage.UrlCadastrar);
        await Expect(formPage.ErroDeValidacao("O campo \"Título\" deve ser preenchido."))
            .ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveImpedir_Cadastro_DeCategoria_ComTituloDuplicado()
    {
        // Arranjo
        await CadastrarCategoriaAsync("Alimentação");
        CategoriaFormPage formPage = new(Page, UrlBase);

        // Ação
        await formPage.IrParaCadastroAsync();
        await formPage.PreencherTituloAsync("Alimentação");
        await formPage.ConfirmarAsync();

        // Asserção
        await Expect(Page).ToHaveURLAsync(formPage.UrlCadastrar);
        await Expect(formPage.ErroDeValidacao(
            "Já existe uma categoria cadastrada com este título."
        )).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveEditar_Categoria_ComDadosValidos()
    {
        // Arranjo
        await CadastrarCategoriaAsync("Alimentação");
        CategoriaFormPage formPage = new(Page, UrlBase);
        CategoriaListarPage listarPage = new(Page, UrlBase);
        await listarPage.EditarAsync("Alimentação");

        // Ação
        await formPage.PreencherTituloAsync("Transporte");
        await formPage.ConfirmarAsync();

        // Asserção
        await Expect(Page).ToHaveURLAsync(listarPage.Url);
        await Expect(listarPage.TituloDaCategoria("Transporte")).ToBeVisibleAsync();
        await Expect(listarPage.TituloDaCategoria("Alimentação")).Not.ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveExcluir_Categoria_SemDespesasVinculadas()
    {
        // Arranjo
        await CadastrarCategoriaAsync("Alimentação");
        CategoriaListarPage listarPage = new(Page, UrlBase);
        CategoriaExcluirPage excluirPage = new(Page);
        await listarPage.ExcluirAsync("Alimentação");

        // Ação
        await Expect(Page).ToHaveURLAsync(
            new Regex($"{Regex.Escape(UrlBase)}/Categorias/Excluir/.*")
        );
        await Expect(excluirPage.MensagemConfirmacao).ToBeVisibleAsync();
        await excluirPage.ConfirmarAsync();

        // Asserção
        await Expect(Page).ToHaveURLAsync(listarPage.Url);
        await Expect(listarPage.TituloDaCategoria("Alimentação")).Not.ToBeVisibleAsync();
        await Expect(listarPage.EstadoVazio).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveImpedir_Exclusao_DeCategoria_ComDespesaVinculada()
    {
        // Arranjo
        Guid categoriaId = Guid.CreateVersion7();
        Guid despesaId = Guid.CreateVersion7();

        ExecutarComando(
            "INSERT INTO TBCategoria (Id, Titulo) VALUES (@Id, @Titulo)",
            new { Id = categoriaId, Titulo = "Alimentação" }
        );
        ExecutarComando(
            """
            INSERT INTO TBDespesa
                (Id, Descricao, DataOcorrencia, Valor, FormaPagamento, QuantidadeParcelas)
            VALUES
                (@Id, @Descricao, @DataOcorrencia, @Valor, @FormaPagamento, @QuantidadeParcelas)
            """,
            new
            {
                Id = despesaId,
                Descricao = "Almoço",
                DataOcorrencia = DateTime.Today,
                Valor = 45.90m,
                FormaPagamento = 1,
                QuantidadeParcelas = (int?)null
            }
        );
        ExecutarComando(
            """
            INSERT INTO TBDespesaCategoria (Id, DespesaId, CategoriaId)
            VALUES (@Id, @DespesaId, @CategoriaId)
            """,
            new { Id = Guid.CreateVersion7(), DespesaId = despesaId, CategoriaId = categoriaId }
        );

        CategoriaListarPage listarPage = new(Page, UrlBase);
        CategoriaExcluirPage excluirPage = new(Page);
        await listarPage.IrParaAsync();
        await listarPage.ExcluirAsync("Alimentação");

        // Ação
        await Expect(Page).ToHaveURLAsync(
            new Regex($"{Regex.Escape(UrlBase)}/Categorias/Excluir/.*")
        );
        await excluirPage.ConfirmarAsync();

        // Asserção
        await Expect(Page).ToHaveURLAsync(listarPage.Url);
        await Expect(listarPage.MensagemDeErro(
            "Não é possível excluir uma categoria que possua despesas vinculadas."
        )).ToBeVisibleAsync();
        await Expect(listarPage.TituloDaCategoria("Alimentação")).ToBeVisibleAsync();
    }

    private async Task CadastrarCategoriaAsync(string titulo)
    {
        CategoriaFormPage formPage = new(Page, UrlBase);
        CategoriaListarPage listarPage = new(Page, UrlBase);

        await formPage.IrParaCadastroAsync();
        await formPage.PreencherTituloAsync(titulo);
        await formPage.ConfirmarAsync();

        await Expect(Page).ToHaveURLAsync(listarPage.Url);
    }
}
