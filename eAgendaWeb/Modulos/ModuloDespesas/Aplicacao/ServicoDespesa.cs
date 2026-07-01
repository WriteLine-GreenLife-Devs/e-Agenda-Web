using eAgendaWeb.Modulos.ModuloCategorias.Dominio;
using eAgendaWeb.Modulos.ModuloDespesas.Dominio;
using FluentResults;

namespace eAgendaWeb.Modulos.ModuloDespesas.Aplicacao;

public class ServicoDespesa
{
    private readonly IRepositorioDespesa repositorioDespesa;
    private readonly IRepositorioCategoria repositorioCategoria;

    public ServicoDespesa(
        IRepositorioDespesa repositorioDespesa,
        IRepositorioCategoria repositorioCategoria)
    {
        this.repositorioDespesa = repositorioDespesa;
        this.repositorioCategoria = repositorioCategoria;
    }

    private static Result ValidarEntidade(Despesa despesa)
    {
        List<string> erros = despesa.Validar();

        if (erros.Count == 0)
            return Result.Ok();

        string erro = erros.First();

        string campo =
            erro.Contains("descrição") ? nameof(Despesa.Descricao)
            : erro.Contains("valor") ? nameof(Despesa.Valor)
            : erro.Contains("data") ? nameof(Despesa.DataOcorrencia)
            : erro.Contains("parcelas") ? nameof(Despesa.QuantidadeParcelas)
            : string.Empty;

        return Result.Fail(new Error(erro).WithMetadata("Campo", campo));
    }

    private bool CategoriasExistem(List<Guid>? categoriasIds)
    {
        if (categoriasIds == null || categoriasIds.Count == 0)
            return true;

        List<Guid> existentes = repositorioCategoria
            .SelecionarTodos()
            .Select(c => c.Id)
            .ToList();

        return categoriasIds.All(id => existentes.Contains(id));
    }

    private List<string> ObterCategorias(Guid despesaId)
    {
        List<Guid> categoriasIds =
            repositorioDespesa.SelecionarCategorias(despesaId);

        return repositorioCategoria
            .SelecionarTodos()
            .Where(c => categoriasIds.Contains(c.Id))
            .Select(c => c.Titulo)
            .ToList();
    }

    public Result Cadastrar(CadastrarDespesaDto dto)
    {
        if (dto.CategoriasIds == null || dto.CategoriasIds.Count == 0)
            return Result.Fail("A despesa deve conter pelo menos uma categoria vinculada.");

        if (!CategoriasExistem(dto.CategoriasIds))
            return Result.Fail("Uma ou mais categorias selecionadas não existem.");

        int parcelasTratadas = dto.FormaPagamento == FormaPagamento.Credito
            ? (dto.QuantidadeParcelas is null or <= 0 ? 1 : dto.QuantidadeParcelas.Value)
            : 1;

        Despesa baseDespesa = new(
            dto.Descricao,
            dto.DataOcorrencia,
            dto.Valor,
            dto.FormaPagamento,
            parcelasTratadas
        );

        Result validacao = ValidarEntidade(baseDespesa);

        if (validacao.IsFailed)
            return validacao;

        decimal valorParcela = dto.Valor / parcelasTratadas;

        for (int i = 1; i <= parcelasTratadas; i++)
        {
            Despesa despesa = new(
                descricao: parcelasTratadas > 1
                    ? $"{dto.Descricao} ({i}/{parcelasTratadas})"
                    : dto.Descricao,

                dataOcorrencia: dto.DataOcorrencia.AddMonths(i - 1),

                valor: valorParcela,

                formaPagamento: dto.FormaPagamento,

                quantidadeParcelas: parcelasTratadas
            );

            Result resultado = ValidarEntidade(despesa);

            if (resultado.IsFailed)
                return resultado;

            repositorioDespesa.Cadastrar(despesa);

            if (dto.CategoriasIds?.Count > 0)
                repositorioDespesa.AdicionarCategorias(
                    despesa.Id,
                    dto.CategoriasIds
                );
        }

        return Result.Ok();
    }

    public Result Editar(EditarDespesaDto dto)
    {
        if (dto.CategoriasIds == null || dto.CategoriasIds.Count == 0)
            return Result.Fail("A despesa deve conter pelo menos uma categoria vinculada.");

        if (!CategoriasExistem(dto.CategoriasIds))
            return Result.Fail("Uma ou mais categorias selecionadas não existem.");

        Despesa? existente =
            repositorioDespesa.SelecionarPorId(dto.Id);

        if (existente == null)
            return Result.Fail("Despesa não encontrada.");

        int parcelasTratadas = dto.FormaPagamento == FormaPagamento.Credito
            ? (dto.QuantidadeParcelas is null or <= 0 ? 1 : dto.QuantidadeParcelas.Value)
            : 1;

        Despesa atualizada = new(
            dto.Descricao,
            dto.DataOcorrencia,
            dto.Valor,
            dto.FormaPagamento,
            parcelasTratadas
        );

        existente.Atualizar(atualizada);

        Result validacao = ValidarEntidade(existente);

        if (validacao.IsFailed)
            return validacao;

        bool atualizado =
            repositorioDespesa.Editar(dto.Id, existente);

        if (!atualizado)
            return Result.Fail("Falha ao atualizar a despesa.");

        repositorioDespesa.RemoverCategorias(dto.Id);

        if (dto.CategoriasIds?.Count > 0)
            repositorioDespesa.AdicionarCategorias(
                dto.Id,
                dto.CategoriasIds
            );

        return Result.Ok();
    }

    public List<ListarDespesasDto> SelecionarTodos()
    {
        return repositorioDespesa
            .SelecionarTodos()
            .Select(d => new ListarDespesasDto(
                d.Id,
                d.Descricao,
                d.DataOcorrencia,
                d.Valor,
                d.FormaPagamento,
                d.QuantidadeParcelas,
                ObterCategorias(d.Id)
            ))
            .ToList();
    }

    public Result<ListarDespesasDto> SelecionarPorId(Guid id)
    {
        Despesa? despesa =
            repositorioDespesa.SelecionarPorId(id);

        if (despesa == null)
            return Result.Fail("Despesa não encontrada.");

        return Result.Ok(new ListarDespesasDto(
            despesa.Id,
            despesa.Descricao,
            despesa.DataOcorrencia,
            despesa.Valor,
            despesa.FormaPagamento,
            despesa.QuantidadeParcelas,
            ObterCategorias(despesa.Id)
        ));
    }

    public Result Excluir(Guid id)
    {
        Despesa? despesa =
            repositorioDespesa.SelecionarPorId(id);

        if (despesa == null)
            return Result.Fail("Despesa não encontrada.");

        repositorioDespesa.RemoverCategorias(id);

        bool excluido =
            repositorioDespesa.Excluir(id);

        return excluido
            ? Result.Ok()
            : Result.Fail("Falha ao excluir a despesa.");
    }
}