using AutoMapper;
using eAgendaWeb.Modulos.ModuloContatos.Aplicacao;
using eAgendaWeb.Modulos.ModuloContatos.Dominio;

namespace eAgendaWeb.Modulos.ModuloContatos.Apresentacao;

public class ContatoProfile : Profile
{
    public ContatoProfile()
    {
        CreateMap<CadastrarContatoViewModel, CadastrarContatoDto>();
        CreateMap<EditarContatoViewModel, EditarContatoDto>();
        CreateMap<CadastrarContatoDto, Contato>();
        CreateMap<EditarContatoDto, Contato>();
        CreateMap<Contato, ListarContatosDto>();
        CreateMap<ListarContatosDto, ListarContatosViewModel>();
        CreateMap<ListarContatosDto, EditarContatoViewModel>();
        CreateMap<ListarContatosDto, ExcluirContatoViewModel>();
    }
}
