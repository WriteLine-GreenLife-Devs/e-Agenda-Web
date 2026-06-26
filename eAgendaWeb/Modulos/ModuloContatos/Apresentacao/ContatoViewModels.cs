using System.ComponentModel.DataAnnotations;

namespace eAgendaWeb.Modulos.ModuloContatos.Apresentacao;

public record ListarContatosViewModel(
    Guid Id,
    string Nome,
    string Telefone,
    string Email,
    string? Cargo = null,
    string? Empresa = null
);

public record CadastrarContatoViewModel(
    [Required(ErrorMessage = "O campo \"Nome\" deve ser preenchido.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo \"Nome\" deve conter entre 3 e 100 caracteres.")]
    string Nome,

    [Required(ErrorMessage = "O campo \"Telefone\" deve ser preenchido.")]
    [StringLength(16, MinimumLength = 10, ErrorMessage = "O campo \"Telefone\" deve conter entre 10 e 16 caracteres.")]
    string Telefone,

    [Required(ErrorMessage = "O campo \"Email\" deve ser preenchido.")]
    [EmailAddress(ErrorMessage = "O campo \"Email\" deve ser um email válido.")]
    string Email,

    [StringLength(50, ErrorMessage = "O campo \"Cargo\" deve conter no máximo 50 caracteres.")]
    string? Cargo = null,

    [StringLength(100, ErrorMessage = "O campo \"Empresa\" deve conter no máximo 100 caracteres.")]
    string? Empresa = null
);

public record EditarContatoViewModel(
    Guid Id,

    [Required(ErrorMessage = "O campo \"Nome\" deve ser preenchido.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo \"Nome\" deve conter entre 3 e 100 caracteres.")]
    string Nome,

    [Required(ErrorMessage = "O campo \"Telefone\" deve ser preenchido.")]
    [StringLength(16, MinimumLength = 10, ErrorMessage = "O campo \"Telefone\" deve conter entre 10 e 16 caracteres.")]
    string Telefone,

    [Required(ErrorMessage = "O campo \"Email\" deve ser preenchido.")]
    [EmailAddress(ErrorMessage = "O campo \"Email\" deve ser um email válido.")]
    string Email,

    [StringLength(50, ErrorMessage = "O campo \"Cargo\" deve conter no máximo 50 caracteres.")]
    string? Cargo = null,

    [StringLength(100, ErrorMessage = "O campo \"Empresa\" deve conter no máximo 100 caracteres.")]
    string? Empresa = null
);

public record ExcluirContatoViewModel(
    Guid Id,
    string Nome,
    string Telefone,
    string Email,
    string? Cargo = null,
    string? Empresa = null
);
