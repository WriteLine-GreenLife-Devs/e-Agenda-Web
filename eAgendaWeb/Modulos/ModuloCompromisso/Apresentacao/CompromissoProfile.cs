using AutoMapper;
using eAgendaWeb.Modulos.ModuloCompromisso.Aplicacao;

namespace eAgendaWeb.Modulos.ModuloCompromisso.Apresentacao;

public class CompromissoProfile : Profile
{
    public CompromissoProfile()
    {
        CreateMap<CadastrarCompromissoViewModel, CadastrarCompromissoDto>();
        CreateMap<EditarCompromissoViewModel, EditarCompromissoDto>();
        CreateMap<ListarCompromissosDto, ListarCompromissosViewModel>();
        CreateMap<ListarCompromissosDto, EditarCompromissoViewModel>();
        CreateMap<ListarCompromissosDto, ExcluirCompromissoViewModel>();
    }
}