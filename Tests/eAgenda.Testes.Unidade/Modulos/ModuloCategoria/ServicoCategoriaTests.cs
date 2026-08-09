using eAgendaWeb.Modulos.ModuloCategorias.Aplicacao;
using eAgendaWeb.Modulos.ModuloCategorias.Dominio;
using FluentResults;
using Moq;

namespace eAgenda.Testes.Unidade.Modulos.ModuloCategorias;

[TestClass]
public sealed class ServicoCategoriaTests
{
    [TestMethod]
    public void Excluir_ComDespesasVinculadas_DeveRetornarFalha()
    {
        // Arranjo
        Guid categoriaId = Guid.NewGuid();
        Categoria categoria = new("Alimentação") { Id = categoriaId };
        Mock<IRepositorioCategoria> repositorioCategoria = new();

        repositorioCategoria
            .Setup(r => r.SelecionarPorId(categoriaId))
            .Returns(categoria);
        repositorioCategoria
            .Setup(r => r.PossuiDespesasVinculadas(categoriaId))
            .Returns(true);

        ServicoCategoria servico = new(repositorioCategoria.Object);

        // Ação
        Result resultado = servico.Excluir(categoriaId);

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.Contains("despesas vinculadas", resultado.Errors.Single().Message);
        repositorioCategoria.Verify(r => r.Excluir(It.IsAny<Guid>()), Times.Never);
    }
}