namespace eAgendaWeb.Modulos.ModuloContatos.Aplicacao;

public record CadastrarContatoDto(
    string Nome,
    string Telefone,
    string Email,
    string? Cargo = null,
    string? Empresa = null
);

public record EditarContatoDto(
    Guid Id,
    string Nome,
    string Telefone,
    string Email,
    string? Cargo = null,
    string? Empresa = null
);

public record ListarContatosDto(
    Guid Id,
    string Nome,
    string Telefone,
    string Email,
    string? Cargo = null,
    string? Empresa = null
);
