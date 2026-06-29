using AutoMapper;
using eAgendaWeb.Modulos.ModuloDespesas.Aplicacao;
using eAgendaWeb.Modulos.ModuloDespesas.Dominio;

namespace eAgendaWeb.Modulos.ModuloDespesas.Apresentacao;

public class DespesaProfile : Profile
{
    public DespesaProfile()
    {
        CreateMap<CadastrarDespesaViewModel, CadastrarDespesaDto>()
            .ForMember(
                destino => destino.FormaPagamento,
                origem => origem.MapFrom(x => x.FormaPagamento!.Value)
            );
        CreateMap<EditarDespesaViewModel, EditarDespesaDto>()
            .ForMember(
                destino => destino.FormaPagamento,
                origem => origem.MapFrom(x => x.FormaPagamento!.Value)
            );
        CreateMap<CadastrarDespesaDto, Despesa>();
        CreateMap<EditarDespesaDto, Despesa>();
        CreateMap<Despesa, ListarDespesasDto>();
        CreateMap<ListarDespesasDto, ListarDespesasViewModel>();
        CreateMap<ListarDespesasDto, EditarDespesaViewModel>();
        CreateMap<ListarDespesasDto, ExcluirDespesaViewModel>();
    }
}