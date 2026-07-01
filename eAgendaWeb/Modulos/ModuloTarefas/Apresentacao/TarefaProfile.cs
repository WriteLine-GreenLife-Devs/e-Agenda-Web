using AutoMapper;
using eAgendaWeb.Modulos.ModuloTarefas.Aplicacao;
using eAgendaWeb.Modulos.ModuloTarefas.Dominio;

namespace eAgendaWeb.Modulos.ModuloTarefas.Apresentacao;

public class TarefaProfile : Profile
{
    public TarefaProfile()
    {
        CreateMap<CadastrarTarefaViewModel, CadastrarTarefaDto>();
        CreateMap<EditarTarefaViewModel, EditarTarefaDto>();

        CreateMap<ItemTarefaViewModel, ItemTarefaDto>().ReverseMap();

        CreateMap<CadastrarTarefaDto, Tarefa>();
        CreateMap<EditarTarefaDto, Tarefa>();

        CreateMap<ItemTarefaDto, ItemTarefa>().ReverseMap();

        CreateMap<Tarefa, ListarTarefasDto>();
        CreateMap<Tarefa, DetalhesTarefaDto>();

        CreateMap<ListarTarefasDto, ListarTarefasViewModel>();
        CreateMap<DetalhesTarefaDto, EditarTarefaViewModel>();
        CreateMap<DetalhesTarefaDto, ExcluirTarefaViewModel>();
    }
}