using eAgendaWeb.Modulos.ModuloContatos.Aplicacao;
using eAgendaWeb.Modulos.ModuloContatos.Dominio;
using FluentResults;
using Moq;

namespace eAgenda.Testes.Unidade.Modulos.ModuloContatos;

[TestClass]
public sealed class ServicoContatoTests
{
    [TestMethod]
    public void Excluir_ComCompromissosVinculados_DeveRetornarFalha()
    {
        // Arranjo
        Guid contatoId = Guid.NewGuid();
        Contato contato = new("João Silva", "joao@email.com", "11999999999", "", "")
        {
            Id = contatoId
        };
        Mock<IRepositorioContato> repositorioContato = new();

        repositorioContato
            .Setup(r => r.SelecionarPorId(contatoId))
            .Returns(contato);
        repositorioContato
            .Setup(r => r.PossuiCompromissosVinculados(contatoId))
            .Returns(true);

        ServicoContato servico = new(repositorioContato.Object);

        // Ação
        Result resultado = servico.Excluir(contatoId);

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.Contains("compromissos vinculados", resultado.Errors.Single().Message);
        repositorioContato.Verify(r => r.Excluir(It.IsAny<Guid>()), Times.Never);
    }
}
