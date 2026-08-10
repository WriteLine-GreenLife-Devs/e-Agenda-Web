using eAgenda.Testes.Integracao.Compartilhado.Sql;
using eAgendaWeb.Modulos.ModuloCategorias.Aplicacao;
using eAgendaWeb.Modulos.ModuloCategorias.Dominio;
using eAgendaWeb.Modulos.ModuloDespesas.Dominio;
using FluentResults;
using Microsoft.Data.SqlClient;

namespace eAgenda.Testes.Integracao.Modulos.ModuloCategoria;

[TestClass]
public sealed class RepositorioCategoriaEmSqlTests : RepositorioBaseEmSqlTests
{
    [TestMethod]
    public void CadastrarESelecionarPorId_ComDadosValidos_DeveCarregarCategoria()
    {
        // Arranjo
        Categoria categoria = new("Alimentação");

        // Ação
        repositorioCategoria.Cadastrar(categoria);
        Categoria? categoriaSelecionada = repositorioCategoria.SelecionarPorId(categoria.Id);

        // Asserção
        Assert.IsNotNull(categoriaSelecionada);
        Assert.AreEqual(categoria.Id, categoriaSelecionada.Id);
        Assert.AreEqual("Alimentação", categoriaSelecionada.Titulo);
    }

    [TestMethod]
    public void Cadastrar_ComTituloDuplicado_DeveRejeitarSegundoRegistro()
    {
        // Arranjo
        repositorioCategoria.Cadastrar(new Categoria("Alimentação"));
        Categoria categoriaDuplicada = new("Alimentação");

        // Ação
        Action acao = () => repositorioCategoria.Cadastrar(categoriaDuplicada);

        // Asserção
        Assert.ThrowsExactly<SqlException>(acao);
        Assert.HasCount(1, repositorioCategoria.SelecionarTodos());
    }

    [TestMethod]
    public void Editar_ComDadosValidos_DeveAtualizarCategoria()
    {
        // Arranjo
        Categoria categoria = new("Alimentação");
        repositorioCategoria.Cadastrar(categoria);
        Categoria categoriaAtualizada = new("Transporte");

        // Ação
        bool conseguiuEditar = repositorioCategoria.Editar(categoria.Id, categoriaAtualizada);
        Categoria? categoriaSelecionada = repositorioCategoria.SelecionarPorId(categoria.Id);

        // Asserção
        Assert.IsTrue(conseguiuEditar);
        Assert.IsNotNull(categoriaSelecionada);
        Assert.AreEqual("Transporte", categoriaSelecionada.Titulo);
    }

    [TestMethod]
    public void Editar_ComTituloDuplicado_DeveRejeitarAlteracao()
    {
        // Arranjo
        Categoria alimentacao = new("Alimentação");
        Categoria transporte = new("Transporte");
        repositorioCategoria.Cadastrar(alimentacao);
        repositorioCategoria.Cadastrar(transporte);

        // Ação
        Action acao = () => repositorioCategoria.Editar(
            transporte.Id,
            new Categoria("Alimentação")
        );

        // Asserção
        Assert.ThrowsExactly<SqlException>(acao);
        Assert.AreEqual("Transporte", repositorioCategoria.SelecionarPorId(transporte.Id)?.Titulo);
    }

    [TestMethod]
    public void SelecionarPorCategoria_ComDespesasVinculadas_DeveCarregarDespesas()
    {
        // Arranjo
        Categoria categoria = new("Alimentação");
        repositorioCategoria.Cadastrar(categoria);

        Despesa mercado = new("Mercado", DateTime.Today, 150m, FormaPagamento.Debito, 1);
        Despesa restaurante = new("Restaurante", DateTime.Today, 80m, FormaPagamento.AVista, 1);
        repositorioDespesa.Cadastrar(mercado);
        repositorioDespesa.Cadastrar(restaurante);
        repositorioDespesa.AdicionarCategorias(mercado.Id, [categoria.Id]);
        repositorioDespesa.AdicionarCategorias(restaurante.Id, [categoria.Id]);

        // Ação
        List<Despesa> despesas = repositorioDespesa.SelecionarPorCategoria(categoria.Id);

        // Asserção
        Assert.HasCount(2, despesas);
        CollectionAssert.AreEquivalent(
            new[] { "Mercado", "Restaurante" },
            despesas.Select(d => d.Descricao).ToArray()
        );
    }

    [TestMethod]
    public void SelecionarTodos_ComCategoriasCadastradas_DeveCarregarTodosRegistros()
    {
        // Arranjo
        repositorioCategoria.Cadastrar(new Categoria("Alimentação"));
        repositorioCategoria.Cadastrar(new Categoria("Transporte"));

        // Ação
        List<Categoria> categorias = repositorioCategoria.SelecionarTodos();

        // Asserção
        Assert.HasCount(2, categorias);
        CollectionAssert.AreEquivalent(
            new[] { "Alimentação", "Transporte" },
            categorias.Select(c => c.Titulo).ToArray()
        );
    }

    [TestMethod]
    public void Excluir_SemDespesasVinculadas_DeveRemoverCategoria()
    {
        // Arranjo
        Categoria categoria = new("Alimentação");
        repositorioCategoria.Cadastrar(categoria);

        // Ação
        bool conseguiuExcluir = repositorioCategoria.Excluir(categoria.Id);

        // Asserção
        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(repositorioCategoria.SelecionarPorId(categoria.Id));
    }

    [TestMethod]
    public void Excluir_ComDespesasVinculadas_DeveBloquearExclusao()
    {
        // Arranjo
        Categoria categoria = new("Alimentação");
        repositorioCategoria.Cadastrar(categoria);

        Despesa despesa = new("Mercado", DateTime.Today, 150m, FormaPagamento.Debito, 1);
        repositorioDespesa.Cadastrar(despesa);
        repositorioDespesa.AdicionarCategorias(despesa.Id, [categoria.Id]);

        ServicoCategoria servico = new(repositorioCategoria, repositorioDespesa);

        // Ação
        Result resultado = servico.Excluir(categoria.Id);

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.Contains("despesas vinculadas", resultado.Errors.Single().Message);
        Assert.IsNotNull(repositorioCategoria.SelecionarPorId(categoria.Id));
    }
}
