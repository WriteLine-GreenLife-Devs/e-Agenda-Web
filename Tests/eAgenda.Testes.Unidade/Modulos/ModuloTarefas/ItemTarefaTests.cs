using eAgendaWeb.Modulos.ModuloTarefas.Dominio;

namespace eAgenda.Testes.Unidade.Modulos.ModuloTarefas;

[TestClass]
public sealed class ItemTarefaTests
{
    [TestMethod]
    public void Concluir_DeveMarcarConcluido()
    {
        // Arranjo
        ItemTarefa item = new ItemTarefa("Ler aula");

        // Ação
        item.Concluir();

        // Asserção
        Assert.IsTrue(item.Concluido);
    }

    [TestMethod]
    public void Desmarcar_DeveMarcarNaoConcluido()
    {
        // Arranjo
        ItemTarefa item = new ItemTarefa("Ler aula");
        item.Concluir();

        // Ação
        item.Desmarcar();

        // Asserção
        Assert.IsFalse(item.Concluido);
    }

    [TestMethod]
    public void VincularTarefa_DeveAtribuirTarefaId()
    {
        // Arranjo
        ItemTarefa item = new ItemTarefa("Exercício");
        Guid tarefaId = Guid.NewGuid();

        // Ação
        item.VincularTarefa(tarefaId);

        // Asserção
        Assert.AreEqual(tarefaId, item.TarefaId);
    }

    [TestMethod]
    public void Validar_SemVinculo_DeveRetornarErro()
    {
        // Arranjo
        ItemTarefa item = new ItemTarefa("Exercício");

        // Ação
        List<string> erros = item.Validar();

        // Asserção
        Assert.IsTrue(erros.Any());
        CollectionAssert.Contains(erros, "O item deve estar vinculado a uma tarefa.");
    }
}
