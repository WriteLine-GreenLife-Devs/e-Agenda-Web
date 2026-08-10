using eAgendaWeb.Modulos.ModuloCategorias.Aplicacao;
using eAgendaWeb.Modulos.ModuloCategorias.Dominio;
using eAgendaWeb.Modulos.ModuloDespesas.Dominio;
using FluentResults;
using Moq;

namespace eAgenda.Testes.Unidade.Modulos.ModuloCategorias;

[TestClass]
public sealed class ServicoCategoriaTests
{
    [TestMethod]
    public void Cadastrar_ComDadosValidos_DevePersistirCategoria()
    {
        // Arranjo
        Mock<IRepositorioCategoria> repositorioCategoria = new();
        Mock<IRepositorioDespesa> repositorioDespesa = new();
        Categoria? categoriaCadastrada = null;

        repositorioCategoria.Setup(r => r.SelecionarTodos()).Returns([]);
        repositorioCategoria
            .Setup(r => r.Cadastrar(It.IsAny<Categoria>()))
            .Callback<Categoria>(categoria => categoriaCadastrada = categoria);

        ServicoCategoria servico = new(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        // Ação
        Result resultado = servico.Cadastrar(new CadastrarCategoriaDto("Alimentação"));

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(categoriaCadastrada);
        Assert.AreEqual("Alimentação", categoriaCadastrada.Titulo);
        repositorioCategoria.Verify(r => r.Cadastrar(It.IsAny<Categoria>()), Times.Once);
    }

    [TestMethod]
    public void Cadastrar_SemTitulo_DeveRetornarFalha()
    {
        // Arranjo
        Mock<IRepositorioCategoria> repositorioCategoria = new();
        Mock<IRepositorioDespesa> repositorioDespesa = new();
        repositorioCategoria.Setup(r => r.SelecionarTodos()).Returns([]);

        ServicoCategoria servico = new(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        // Ação
        Result resultado = servico.Cadastrar(new CadastrarCategoriaDto(string.Empty));

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.Contains("obrigatório", resultado.Errors.Single().Message);
        repositorioCategoria.Verify(r => r.Cadastrar(It.IsAny<Categoria>()), Times.Never);
    }

    [TestMethod]
    public void Cadastrar_ComTituloDuplicado_DeveRetornarFalha()
    {
        // Arranjo
        Mock<IRepositorioCategoria> repositorioCategoria = new();
        Mock<IRepositorioDespesa> repositorioDespesa = new();
        repositorioCategoria
            .Setup(r => r.SelecionarTodos())
            .Returns([new Categoria("Alimentação")]);

        ServicoCategoria servico = new(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        // Ação
        Result resultado = servico.Cadastrar(new CadastrarCategoriaDto("Alimentação"));

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual("Titulo", resultado.Errors.Single().Metadata["Campo"]);
        Assert.Contains("Já existe", resultado.Errors.Single().Message);
        repositorioCategoria.Verify(r => r.Cadastrar(It.IsAny<Categoria>()), Times.Never);
    }

    [TestMethod]
    public void Cadastrar_ComTituloDuplicadoDiferindoMaiusculasMinusculas_DeveRetornarFalha()
    {
        // Arranjo
        Mock<IRepositorioCategoria> repositorioCategoria = new();
        Mock<IRepositorioDespesa> repositorioDespesa = new();
        repositorioCategoria
            .Setup(r => r.SelecionarTodos())
            .Returns([new Categoria("Alimentação")]);

        ServicoCategoria servico = new(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        // Ação
        Result resultado = servico.Cadastrar(new CadastrarCategoriaDto("alimentação"));

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual("Titulo", resultado.Errors.Single().Metadata["Campo"]);
        Assert.Contains("Já existe", resultado.Errors.Single().Message);
        repositorioCategoria.Verify(r => r.Cadastrar(It.IsAny<Categoria>()), Times.Never);
    }

    [TestMethod]
    public void Editar_ComDadosValidos_DeveAtualizarCategoria()
    {
        // Arranjo
        Guid categoriaId = Guid.NewGuid();
        Categoria categoria = new("Alimentação") { Id = categoriaId };
        Mock<IRepositorioCategoria> repositorioCategoria = new();
        Mock<IRepositorioDespesa> repositorioDespesa = new();

        repositorioCategoria.Setup(r => r.SelecionarPorId(categoriaId)).Returns(categoria);
        repositorioCategoria.Setup(r => r.SelecionarTodos()).Returns([categoria]);

        ServicoCategoria servico = new(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        // Ação
        Result resultado = servico.Editar(new EditarCategoriaDto(categoriaId, "Transporte"));

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        repositorioCategoria.Verify(
            r => r.Editar(categoriaId, It.Is<Categoria>(c => c.Titulo == "Transporte")),
            Times.Once
        );
    }

    [TestMethod]
    public void Editar_ComTituloDuplicado_DeveRetornarFalha()
    {
        // Arranjo
        Guid categoriaId = Guid.NewGuid();
        Categoria categoria = new("Alimentação") { Id = categoriaId };
        Categoria categoriaExistente = new("Transporte");
        Mock<IRepositorioCategoria> repositorioCategoria = new();
        Mock<IRepositorioDespesa> repositorioDespesa = new();

        repositorioCategoria.Setup(r => r.SelecionarPorId(categoriaId)).Returns(categoria);
        repositorioCategoria
            .Setup(r => r.SelecionarTodos())
            .Returns([categoria, categoriaExistente]);

        ServicoCategoria servico = new(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        // Ação
        Result resultado = servico.Editar(new EditarCategoriaDto(categoriaId, "Transporte"));

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual("Titulo", resultado.Errors.Single().Metadata["Campo"]);
        Assert.Contains("Já existe", resultado.Errors.Single().Message);
        repositorioCategoria.Verify(
            r => r.Editar(It.IsAny<Guid>(), It.IsAny<Categoria>()),
            Times.Never
        );
    }

    [TestMethod]
    public void Editar_ComTituloDuplicadoDiferindoMaiusculasMinusculas_DeveRetornarFalha()
    {
        // Arranjo
        Guid categoriaId = Guid.NewGuid();
        Categoria categoria = new("Alimentação") { Id = categoriaId };
        Categoria categoriaExistente = new("Transporte");
        Mock<IRepositorioCategoria> repositorioCategoria = new();
        Mock<IRepositorioDespesa> repositorioDespesa = new();

        repositorioCategoria.Setup(r => r.SelecionarPorId(categoriaId)).Returns(categoria);
        repositorioCategoria
            .Setup(r => r.SelecionarTodos())
            .Returns([categoria, categoriaExistente]);

        ServicoCategoria servico = new(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        // Ação
        Result resultado = servico.Editar(new EditarCategoriaDto(categoriaId, "transporte"));

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual("Titulo", resultado.Errors.Single().Metadata["Campo"]);
        Assert.Contains("Já existe", resultado.Errors.Single().Message);
        repositorioCategoria.Verify(
            r => r.Editar(It.IsAny<Guid>(), It.IsAny<Categoria>()),
            Times.Never
        );
    }

    [TestMethod]
    public void SelecionarDetalhesPorId_ComDespesasVinculadas_DeveRetornarCategoriaEDespesas()
    {
        // Arranjo
        Guid categoriaId = Guid.NewGuid();
        Categoria categoria = new("Alimentação") { Id = categoriaId };
        List<Despesa> despesas =
        [
            new("Mercado", DateTime.Today, 150m, FormaPagamento.Debito, 1),
            new("Restaurante", DateTime.Today, 80m, FormaPagamento.AVista, 1)
        ];
        Mock<IRepositorioCategoria> repositorioCategoria = new();
        Mock<IRepositorioDespesa> repositorioDespesa = new();

        repositorioCategoria.Setup(r => r.SelecionarPorId(categoriaId)).Returns(categoria);
        repositorioDespesa.Setup(r => r.SelecionarPorCategoria(categoriaId)).Returns(despesas);

        ServicoCategoria servico = new(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        // Ação
        Result<DetalhesCategoriaDto> resultado = servico.SelecionarDetalhesPorId(categoriaId);

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.AreEqual("Alimentação", resultado.Value.Titulo);
        Assert.HasCount(2, resultado.Value.Despesas);
        CollectionAssert.AreEquivalent(
            new[] { "Mercado", "Restaurante" },
            resultado.Value.Despesas.Select(d => d.Descricao).ToArray()
        );
    }

    [TestMethod]
    public void SelecionarTodos_ComCategoriasCadastradas_DeveRetornarTodasCategorias()
    {
        // Arranjo
        Mock<IRepositorioCategoria> repositorioCategoria = new();
        Mock<IRepositorioDespesa> repositorioDespesa = new();
        repositorioCategoria
            .Setup(r => r.SelecionarTodos())
            .Returns([new Categoria("Alimentação"), new Categoria("Transporte")]);

        ServicoCategoria servico = new(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        // Ação
        List<ListarCategoriasDto> resultado = servico.SelecionarTodos();

        // Asserção
        Assert.HasCount(2, resultado);
        CollectionAssert.AreEquivalent(
            new[] { "Alimentação", "Transporte" },
            resultado.Select(c => c.Titulo).ToArray()
        );
    }

    [TestMethod]
    public void Excluir_SemDespesasVinculadas_DeveExcluirCategoria()
    {
        // Arranjo
        Guid categoriaId = Guid.NewGuid();
        Categoria categoria = new("Alimentação") { Id = categoriaId };
        Mock<IRepositorioCategoria> repositorioCategoria = new();
        Mock<IRepositorioDespesa> repositorioDespesa = new();

        repositorioCategoria.Setup(r => r.SelecionarPorId(categoriaId)).Returns(categoria);
        repositorioCategoria.Setup(r => r.PossuiDespesasVinculadas(categoriaId)).Returns(false);

        ServicoCategoria servico = new(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        // Ação
        Result resultado = servico.Excluir(categoriaId);

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        repositorioCategoria.Verify(r => r.Excluir(categoriaId), Times.Once);
    }

    [TestMethod]
    public void Excluir_ComDespesasVinculadas_DeveRetornarFalha()
    {
        // Arranjo
        Guid categoriaId = Guid.NewGuid();
        Categoria categoria = new("Alimentação") { Id = categoriaId };
        Mock<IRepositorioCategoria> repositorioCategoria = new();
        Mock<IRepositorioDespesa> repositorioDespesa = new();

        repositorioCategoria.Setup(r => r.SelecionarPorId(categoriaId)).Returns(categoria);
        repositorioCategoria.Setup(r => r.PossuiDespesasVinculadas(categoriaId)).Returns(true);

        ServicoCategoria servico = new(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        // Ação
        Result resultado = servico.Excluir(categoriaId);

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.Contains("despesas vinculadas", resultado.Errors.Single().Message);
        repositorioCategoria.Verify(r => r.Excluir(It.IsAny<Guid>()), Times.Never);
    }
}
