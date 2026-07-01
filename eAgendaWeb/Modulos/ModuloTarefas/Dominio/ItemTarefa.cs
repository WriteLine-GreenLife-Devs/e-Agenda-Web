using eAgendaWeb.Compartilhado.Dominio;

namespace eAgendaWeb.Modulos.ModuloTarefas.Dominio;

public class ItemTarefa : EntidadeBase<ItemTarefa>
{
    public string Titulo { get; private set; } = string.Empty;
    public bool Concluido { get; private set; }
    public Guid TarefaId { get; private set; }

    private ItemTarefa() { }

    public ItemTarefa(string titulo)
    {
        Titulo = titulo;
        Concluido = false;
    }

    public void Concluir()
    {
        Concluido = true;
    }

    public void Desmarcar()
    {
        Concluido = false;
    }

    public void VincularTarefa(Guid tarefaId)
    {
        TarefaId = tarefaId;
    }

    public override List<string> Validar()
    {
        List<string> erros = [];

        if (string.IsNullOrWhiteSpace(Titulo))
            erros.Add("O título do item é obrigatório.");

        if (Titulo.Length < 2 || Titulo.Length > 100)
            erros.Add("O título do item deve possuir entre 2 e 100 caracteres.");

        return erros;
    }

    public override void Atualizar(ItemTarefa entidade)
    {
        Titulo = entidade.Titulo;
        Concluido = entidade.Concluido;
    }
}