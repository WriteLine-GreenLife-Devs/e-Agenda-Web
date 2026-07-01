using eAgendaWeb.Compartilhado.Dominio;

namespace eAgendaWeb.Modulos.ModuloDespesas.Dominio;

public interface IRepositorioDespesa : IRepositorio<Despesa>
{
    List<Guid> SelecionarCategorias(Guid despesaId);

    void AdicionarCategorias(Guid despesaId, List<Guid> categoriasIds);

    void RemoverCategorias(Guid despesaId);
}