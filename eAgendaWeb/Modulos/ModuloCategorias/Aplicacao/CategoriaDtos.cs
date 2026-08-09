namespace eAgendaWeb.Modulos.ModuloCategorias.Aplicacao;

public record CadastrarCategoriaDto(
    string Titulo
);

public record EditarCategoriaDto(
    Guid Id,
    string Titulo
);

public record ListarCategoriasDto(
    Guid Id,
    string Titulo
);

public record ListarDespesaDaCategoriaDto(
    Guid Id,
    string Descricao,
    DateTime DataOcorrencia,
    decimal Valor
);

public record DetalhesCategoriaDto(
    Guid Id,
    string Titulo,
    IReadOnlyList<ListarDespesaDaCategoriaDto> Despesas
);