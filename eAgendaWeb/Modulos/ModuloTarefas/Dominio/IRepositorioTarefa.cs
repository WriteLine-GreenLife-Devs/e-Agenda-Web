using eAgendaWeb.Compartilhado.Dominio;

namespace eAgendaWeb.Modulos.ModuloTarefas.Dominio;

public interface IRepositorioTarefa : IRepositorio<Tarefa>
{
    List<ItemTarefa> SelecionarItens(Guid tarefaId);
    void AdicionarItens(Guid tarefaId, List<ItemTarefa> itens);
    void AtualizarStatusItem(Guid itemId, bool concluido);
    void RemoverItens(Guid tarefaId);
}