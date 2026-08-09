using eAgendaWeb.Compartilhado.Dominio;

namespace eAgendaWeb.Modulos.ModuloContatos.Dominio;

public interface IRepositorioContato : IRepositorio<Contato>
{
    bool PossuiCompromissosVinculados(Guid contatoId);
}
