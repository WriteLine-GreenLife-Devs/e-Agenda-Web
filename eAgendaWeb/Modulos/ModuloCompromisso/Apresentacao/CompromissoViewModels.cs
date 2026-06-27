using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using eAgendaWeb.Modulos.ModuloCompromisso.Dominio;

namespace eAgendaWeb.Modulos.ModuloCompromisso.Apresentacao;

public record ListarCompromissosViewModel(
    Guid Id,
    string Assunto,
    DateTime DataOcorrencia,
    TimeSpan HoraInicio,
    TimeSpan HoraTermino,
    TipoCompromisso TipoCompromisso,
    string? Local = null,
    string? Link = null,
    Guid? ContatoId = null,
    string? ContatoNome = null
);

public record CadastrarCompromissoViewModel(
    [Required(ErrorMessage = "O campo \"Assunto\" deve ser preenchido.")]
    [StringLength(150, MinimumLength = 3, ErrorMessage = "O campo \"Assunto\" deve conter entre 3 e 150 caracteres.")]
    string Assunto,

    [Required(ErrorMessage = "O campo \"Data de Ocorrência\" deve ser preenchido.")]
    [DataType(DataType.Date)]
    DateTime DataOcorrencia,

    [Required(ErrorMessage = "O campo \"Hora de Início\" deve ser preenchido.")]
    [DataType(DataType.Time)]
    TimeSpan HoraInicio,

    [Required(ErrorMessage = "O campo \"Hora de Término\" deve ser preenchido.")]
    [DataType(DataType.Time)]
    TimeSpan HoraTermino,

    [Required(ErrorMessage = "O campo \"Tipo de Compromisso\" deve ser preenchido.")]
    TipoCompromisso TipoCompromisso,

    string? Local = null,
    string? Link = null,
    Guid? ContatoId = null,
    IEnumerable<SelectListItem>? Contatos = null
);

public record EditarCompromissoViewModel(
    Guid Id,

    [Required(ErrorMessage = "O campo \"Assunto\" deve ser preenchido.")]
    [StringLength(150, MinimumLength = 3, ErrorMessage = "O campo \"Assunto\" deve conter entre 3 e 150 caracteres.")]
    string Assunto,

    [Required(ErrorMessage = "O campo \"Data de Ocorrência\" deve ser preenchido.")]
    [DataType(DataType.Date)]
    DateTime DataOcorrencia,

    [Required(ErrorMessage = "O campo \"Hora de Início\" deve ser preenchido.")]
    [DataType(DataType.Time)]
    TimeSpan HoraInicio,

    [Required(ErrorMessage = "O campo \"Hora de Término\" deve ser preenchido.")]
    [DataType(DataType.Time)]
    TimeSpan HoraTermino,

    [Required(ErrorMessage = "O campo \"Tipo de Compromisso\" deve ser preenchido.")]
    TipoCompromisso TipoCompromisso,

    string? Local = null,
    string? Link = null,
    Guid? ContatoId = null,
    IEnumerable<SelectListItem>? Contatos = null
);

public record ExcluirCompromissoViewModel(
    Guid Id,
    string Assunto,
    DateTime DataOcorrencia,
    TimeSpan HoraInicio,
    TimeSpan HoraTermino,
    TipoCompromisso TipoCompromisso,
    string? Local = null,
    string? Link = null,
    Guid? ContatoId = null,
    string? ContatoNome = null
);
