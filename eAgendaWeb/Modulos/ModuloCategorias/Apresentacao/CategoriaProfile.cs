using AutoMapper;
using eAgendaWeb.Modulos.ModuloCategorias.Aplicacao;
using eAgendaWeb.Modulos.ModuloCategorias.Dominio;

namespace eAgendaWeb.Modulos.ModuloCategorias.Apresentacao;

public class CategoriaProfile : Profile
{
    public CategoriaProfile()
    {
        CreateMap<CadastrarCategoriaViewModel, CadastrarCategoriaDto>();
        CreateMap<EditarCategoriaViewModel, EditarCategoriaDto>();
        CreateMap<CadastrarCategoriaDto, Categoria>();
        CreateMap<EditarCategoriaDto, Categoria>();
        CreateMap<Categoria, ListarCategoriasDto>();
        CreateMap<ListarCategoriasDto, ListarCategoriasViewModel>();
        CreateMap<ListarCategoriasDto, EditarCategoriaViewModel>();
        CreateMap<ListarCategoriasDto, ExcluirCategoriaViewModel>();
    }
}