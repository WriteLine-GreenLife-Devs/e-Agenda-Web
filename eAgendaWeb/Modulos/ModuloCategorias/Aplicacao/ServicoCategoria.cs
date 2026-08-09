using eAgendaWeb.Modulos.ModuloCategorias.Dominio;
using eAgendaWeb.Modulos.ModuloDespesas.Dominio;
using FluentResults;

namespace eAgendaWeb.Modulos.ModuloCategorias.Aplicacao;

public class ServicoCategoria
{
    private readonly IRepositorioCategoria repositorioCategoria;
    private readonly IRepositorioDespesa repositorioDespesa;

    public ServicoCategoria(
        IRepositorioCategoria repositorioCategoria,
        IRepositorioDespesa repositorioDespesa)
    {
        this.repositorioCategoria = repositorioCategoria;
        this.repositorioDespesa = repositorioDespesa;
    }

    private static Result Falha(string campo, string mensagem)
    {
        return Result.Fail(new Error(mensagem).WithMetadata("Campo", campo));
    }

    private bool VerificarTituloExistente(string titulo)
    {
        return repositorioCategoria
            .SelecionarTodos()
            .Any(c => c.Titulo == titulo);
    }

    private bool VerificarTituloExistenteEditar(string titulo, Guid id)
    {
        return repositorioCategoria
            .SelecionarTodos()
            .Any(c => c.Titulo == titulo && c.Id != id);
    }

    private static Result ValidarEntidade(Categoria categoria)
    {
        List<string> erros = categoria.Validar();

        if (erros.Count == 0)
            return Result.Ok();


        string erro = erros.First();

        string campo = erro.Contains("Título")
            ? nameof(Categoria.Titulo)
            : string.Empty;


        return Result.Fail(
            new Error(erro)
                .WithMetadata("Campo", campo)
        );
    }

    public Result Cadastrar(CadastrarCategoriaDto dto)
    {
        if (VerificarTituloExistente(dto.Titulo))
            return Falha(
                nameof(CadastrarCategoriaDto.Titulo),
                "Já existe uma categoria cadastrada com este título."
            );


        Categoria novaCategoria = new(
            titulo: dto.Titulo
        );


        Result resultadoValidacao = ValidarEntidade(novaCategoria);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;


        repositorioCategoria.Cadastrar(novaCategoria);


        return Result.Ok();
    }

    public Result Editar(EditarCategoriaDto dto)
    {
        Categoria? categoria = repositorioCategoria.SelecionarPorId(dto.Id);

        if (categoria == null)
            return Result.Fail("Categoria não encontrada.");

        if (VerificarTituloExistenteEditar(dto.Titulo, dto.Id))
            return Falha(
                nameof(EditarCategoriaDto.Titulo),
                "Já existe uma categoria cadastrada com este título."
            );

        Categoria categoriaAtualizada = new(
            titulo: dto.Titulo
        );

        Result resultadoValidacao = ValidarEntidade(categoriaAtualizada);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioCategoria.Editar(dto.Id, categoriaAtualizada);

        return Result.Ok();
    }

    public Result Excluir(Guid id)
    {
        Categoria? categoria = repositorioCategoria.SelecionarPorId(id);

        if (categoria == null)
            return Result.Fail("Categoria não encontrada.");

        if (repositorioCategoria.PossuiDespesasVinculadas(id))
            return Falha(
                nameof(Categoria.Id),
                "Não é possível excluir uma categoria que possua despesas vinculadas."
            );

        repositorioCategoria.Excluir(id);

        return Result.Ok();
    }

    public List<ListarCategoriasDto> SelecionarTodos()
    {
        return repositorioCategoria
            .SelecionarTodos()
            .Select(c => new ListarCategoriasDto(
                c.Id,
                c.Titulo
            ))
            .ToList();
    }

    public Result<ListarCategoriasDto> SelecionarPorId(Guid id)
    {
        Categoria? categoria = repositorioCategoria.SelecionarPorId(id);

        if (categoria == null)
            return Result.Fail("Categoria não encontrada.");

        return Result.Ok(
            new ListarCategoriasDto(
                categoria.Id,
                categoria.Titulo
            )
        );
    }

    public Result<DetalhesCategoriaDto> SelecionarDetalhesPorId(Guid id)
    {
        Categoria? categoria = repositorioCategoria.SelecionarPorId(id);

        if (categoria == null)
            return Result.Fail("Categoria não encontrada.");

        List<ListarDespesaDaCategoriaDto> despesas = repositorioDespesa
            .SelecionarPorCategoria(id)
            .Select(d => new ListarDespesaDaCategoriaDto(
                d.Id,
                d.Descricao,
                d.DataOcorrencia,
                d.Valor
            ))
            .ToList();

        return Result.Ok(new DetalhesCategoriaDto(
            categoria.Id,
            categoria.Titulo,
            despesas
        ));
    }
}