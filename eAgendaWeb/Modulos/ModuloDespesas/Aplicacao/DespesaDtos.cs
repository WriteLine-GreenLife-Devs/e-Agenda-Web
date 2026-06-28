using eAgendaWeb.Modulos.ModuloDespesas.Dominio;

namespace eAgendaWeb.Modulos.ModuloDespesas.Aplicacao;

public record CadastrarDespesaDto(
    string Descricao,
    DateTime DataOcorrencia,
    decimal Valor,
    FormaPagamento FormaPagamento,
    int? QuantidadeParcelas,
    List<Guid>? CategoriasIds
);

public record EditarDespesaDto(
    Guid Id,
    string Descricao,
    DateTime DataOcorrencia,
    decimal Valor,
    FormaPagamento FormaPagamento,
    int? QuantidadeParcelas,
    List<Guid>? CategoriasIds
);

public record ListarDespesasDto(
    Guid Id,
    string Descricao,
    DateTime DataOcorrencia,
    decimal Valor,
    FormaPagamento FormaPagamento,
    int? QuantidadeParcelas,
    IReadOnlyList<string> Categorias
);