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