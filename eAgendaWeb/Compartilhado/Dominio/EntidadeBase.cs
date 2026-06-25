namespace eAgendaWeb.Compartilhado.Dominio;

public abstract class EntidadeBase<T>
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public abstract List<string> Validar();
    public abstract void Atualizar(T entidadeAtualizada);
}
