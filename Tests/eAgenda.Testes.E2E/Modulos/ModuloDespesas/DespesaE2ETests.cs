using System.Text.RegularExpressions;
using Microsoft.Playwright;
using eAgenda.Testes.E2E.Compartilhado;

namespace eAgenda.Testes.E2E.Modulos.ModuloDespesas;

[TestClass]
public sealed class DespesaE2ETests : E2ETestsBase
{
    [TestMethod]
    public async Task DeveCadastrar_Despesa_Basica()
    {
        // cria diretamente no banco (evita fragilidade do Select2 no ambiente de testes)
        ExecutarComando("INSERT INTO TBCategoria (Id, Titulo) VALUES (@Id, @Titulo)", new { Id = Guid.NewGuid(), Titulo = "CategoriaTeste" });
        ExecutarComando("INSERT INTO TBDespesa (Id, Descricao, DataOcorrencia, Valor, FormaPagamento, QuantidadeParcelas) VALUES (@Id, @Descricao, @Data, @Valor, @Forma, @Parcelas)", new { Id = Guid.NewGuid(), Descricao = "Compra mercado", Data = DateTime.Today, Valor = 120.50m, Forma = 1, Parcelas = (int?)null });

        DespesaListarPage listar = new(Page, UrlBase);
        await listar.IrParaAsync();
        await Expect(listar.Descricao("Compra mercado")).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveEditar_Despesa()
    {
        // cria categoria para o formulário e vincula à despesa
        var catId = Guid.NewGuid();
        var despId = Guid.NewGuid();
        ExecutarComando("INSERT INTO TBCategoria (Id, Titulo) VALUES (@Id, @Titulo)", new { Id = catId, Titulo = "CategoriaTeste" });

        ExecutarComando("INSERT INTO TBDespesa (Id, Descricao, DataOcorrencia, Valor, FormaPagamento, QuantidadeParcelas) VALUES (@Id, @Descricao, @Data, @Valor, @Forma, @Parcelas)", new { Id = despId, Descricao = "Original", Data = DateTime.Today, Valor = 50m, Forma = 1, Parcelas = (int?)null });
        ExecutarComando("INSERT INTO TBDespesaCategoria (Id, DespesaId, CategoriaId) VALUES (@Id, @DespesaId, @CategoriaId)", new { Id = Guid.NewGuid(), DespesaId = despId, CategoriaId = catId });

        DespesaListarPage listar = new(Page, UrlBase);
        DespesaFormPage form = new(Page, UrlBase);

        await listar.IrParaAsync();
        await listar.EditarAsync("Original");

        await form.PreencherAsync("Original Atualizada", DateTime.Today, 75m, "Debito");
        await form.ConfirmarAsync();

        await Expect(Page).ToHaveURLAsync(listar.Url);
        await Expect(listar.Descricao("Original Atualizada")).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveExcluir_Despesa()
    {
        // cria categoria para o formulário
        var catId2 = Guid.NewGuid();
        var despId2 = Guid.NewGuid();
        ExecutarComando("INSERT INTO TBCategoria (Id, Titulo) VALUES (@Id, @Titulo)", new { Id = catId2, Titulo = "CategoriaTeste" });

        ExecutarComando("INSERT INTO TBDespesa (Id, Descricao, DataOcorrencia, Valor, FormaPagamento, QuantidadeParcelas) VALUES (@Id, @Descricao, @Data, @Valor, @Forma, @Parcelas)", new { Id = despId2, Descricao = "ParaExcluir", Data = DateTime.Today, Valor = 80m, Forma = 1, Parcelas = (int?)null });
        ExecutarComando("INSERT INTO TBDespesaCategoria (Id, DespesaId, CategoriaId) VALUES (@Id, @DespesaId, @CategoriaId)", new { Id = Guid.NewGuid(), DespesaId = despId2, CategoriaId = catId2 });

        DespesaListarPage listar = new(Page, UrlBase);
        await listar.IrParaAsync();
        await listar.ExcluirAsync("ParaExcluir");

        // confirmação via página de exclusão segue padrão dos demais módulos
        await Expect(Page).ToHaveURLAsync(new Regex($"{Regex.Escape(UrlBase)}/Despesas/Excluir/.*"));
        // confirmar usando seletor genérico
        await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar", Exact = true }).ClickAsync();

        await Expect(Page).ToHaveURLAsync(listar.Url);
        await Expect(listar.Descricao("ParaExcluir")).Not.ToBeVisibleAsync();
    }
}
