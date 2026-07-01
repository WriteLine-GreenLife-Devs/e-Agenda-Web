using eAgendaWeb.Compartilhado.Dominio;

namespace eAgendaWeb.Modulos.ModuloTarefas.Dominio;

public sealed class Tarefa : EntidadeBase<Tarefa>
{
    public string Titulo { get; private set; } = string.Empty;
    public PrioridadeTarefa Prioridade { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public DateTime? DataConclusao { get; private set; }
    public decimal PercentualConcluido { get; private set; }
    public bool StatusConclusao { get; private set; }
    
    private readonly List<ItemTarefa> itens = [];
    public IReadOnlyCollection<ItemTarefa> Itens => itens.AsReadOnly();

    private Tarefa() { }

    public Tarefa(string titulo, PrioridadeTarefa prioridade)
    {
        Titulo = titulo;
        Prioridade = prioridade;
        DataCriacao = DateTime.Now;
        PercentualConcluido = 0;
        StatusConclusao = false;

        Validar();
    }

    public void AdicionarItem(ItemTarefa item)
    {
        item.VincularTarefa(Id);
        itens.Add(item);
        CalcularPercentual();
    }

    public void ConcluirItem(Guid itemId)
    {
        ItemTarefa? item = itens.Find(i => i.Id == itemId);
        if (item == null) return;

        item.Concluir();
        CalcularPercentual();
    }

    public void DesmarcarItem(Guid itemId)
    {
        ItemTarefa? item = itens.Find(i => i.Id == itemId);
        if (item == null) return;

        item.Desmarcar();
        CalcularPercentual();
    }

    private void CalcularPercentual()
    {
        if (itens.Count == 0)
        {
            PercentualConcluido = 0;
            DataConclusao = null;
            StatusConclusao = false;
            return;
        }

        int qtdConcluidos = itens.Count(i => i.Concluido);
        decimal percentual = (decimal)qtdConcluidos / itens.Count * 100;
        
        PercentualConcluido = Math.Round(percentual, 2);

        if (PercentualConcluido == 100)
        {
            DataConclusao = DateTime.Now;
            StatusConclusao = true;
        }
        else
        {
            DataConclusao = null;
            StatusConclusao = false;
        }
    }

    public override List<string> Validar()
    {
        List<string> erros = [];

        if (string.IsNullOrWhiteSpace(Titulo))
            erros.Add("O título da tarefa é obrigatório.");

        if (Titulo.Length < 2 || Titulo.Length > 100)
            erros.Add("O título deve possuir entre 2 e 100 caracteres.");

        return erros;
    }

    public override void Atualizar(Tarefa entidade)
    {
        Titulo = entidade.Titulo;
        Prioridade = entidade.Prioridade;
    }
}