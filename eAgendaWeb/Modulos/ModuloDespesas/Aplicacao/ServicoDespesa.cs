using eAgendaWeb.Modulos.ModuloDespesas.Dominio;
using FluentResults;

namespace eAgendaWeb.Modulos.ModuloDespesas.Aplicacao;

public class ServicoDespesa
{
    private readonly IRepositorioDespesa repositorioDespesa;

    public ServicoDespesa(IRepositorioDespesa repositorioDespesa)
    {
        this.repositorioDespesa = repositorioDespesa;
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

    public Result Cadastrar(CadastrarDespesaDto dto)
    {
        Despesa baseDespesa = new(
            dto.Descricao,
            dto.DataOcorrencia,
            dto.Valor,
            dto.FormaPagamento,
            dto.QuantidadeParcelas
        );

        Result validacao = ValidarEntidade(baseDespesa);

        if (validacao.IsFailed)
            return validacao;

        int parcelas = dto.QuantidadeParcelas ?? 1;
        decimal valorParcela = dto.Valor / parcelas;

        for (int i = 1; i <= parcelas; i++)
        {
            Despesa despesa = new(
                descricao: parcelas > 1
                    ? $"{dto.Descricao} ({i}/{parcelas})"
                    : dto.Descricao,

                dataOcorrencia: dto.DataOcorrencia.AddMonths(i - 1),

                valor: valorParcela,

                formaPagamento: dto.FormaPagamento,

                quantidadeParcelas: parcelas
            );

            Result val = ValidarEntidade(despesa);

            if (val.IsFailed)
                return val;

            repositorioDespesa.Cadastrar(despesa);
        }

        return Result.Ok();
    }

    public Result Editar(EditarDespesaDto dto)
    {
        Despesa? existente = repositorioDespesa.SelecionarPorId(dto.Id);

        if (existente == null)
            return Result.Fail("Despesa não encontrada.");

        Despesa atualizada = new(
            dto.Descricao,
            dto.DataOcorrencia,
            dto.Valor,
            dto.FormaPagamento,
            dto.QuantidadeParcelas
        );

        existente.Atualizar(atualizada);

        Result validacao = ValidarEntidade(existente);

        if (validacao.IsFailed)
            return validacao;

        bool ok = repositorioDespesa.Editar(dto.Id, existente);

        return ok ? Result.Ok() : Result.Fail("Falha ao atualizar a despesa.");
    }

    public List<ListarDespesasDto> SelecionarTodos()
    {
        return repositorioDespesa.SelecionarTodos()
            .Select(d => new ListarDespesasDto(
                d.Id,
                d.Descricao,
                d.DataOcorrencia,
                d.Valor,
                d.FormaPagamento,
                d.QuantidadeParcelas,
                new List<string>() // categorias ainda não existem
            ))
            .ToList();
    }

    public Result<ListarDespesasDto> SelecionarPorId(Guid id)
    {
        Despesa? despesa = repositorioDespesa.SelecionarPorId(id);

        if (despesa == null)
            return Result.Fail("Despesa não encontrada.");

        return Result.Ok(new ListarDespesasDto(
            despesa.Id,
            despesa.Descricao,
            despesa.DataOcorrencia,
            despesa.Valor,
            despesa.FormaPagamento,
            despesa.QuantidadeParcelas,
            new List<string>()
        ));
    }

    public Result Excluir(Guid id)
    {
        Despesa? despesa = repositorioDespesa.SelecionarPorId(id);

        if (despesa == null)
            return Result.Fail("Despesa não encontrada.");

        bool excluido = repositorioDespesa.Excluir(id);

        return excluido ? Result.Ok() : Result.Fail("Falha ao excluir a despesa.");
    }
}