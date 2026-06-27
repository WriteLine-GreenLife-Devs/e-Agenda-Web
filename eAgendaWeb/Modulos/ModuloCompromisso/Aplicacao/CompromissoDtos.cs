using eAgendaWeb.Modulos.ModuloCompromisso.Dominio;

namespace eAgendaWeb.Modulos.ModuloCompromisso.Aplicacao;

public record CadastrarCompromissoDto(
    string Assunto,
    DateTime DataOcorrencia,
    TimeSpan HoraInicio,
    TimeSpan HoraTermino,
    TipoCompromisso TipoCompromisso,
    string? Local = null,
    string? Link = null,
    Guid? ContatoId = null
);

public record EditarCompromissoDto(
    Guid Id,
    string Assunto,
    DateTime DataOcorrencia,
    TimeSpan HoraInicio,
    TimeSpan HoraTermino,
    TipoCompromisso TipoCompromisso,
    string? Local = null,
    string? Link = null,
    Guid? ContatoId = null
);

public record ListarCompromissosDto(
    Guid Id,
    string Assunto,
    DateTime DataOcorrencia,
    TimeSpan HoraInicio,
    TimeSpan HoraTermino,
    TipoCompromisso TipoCompromisso,
    string? Local = null,
    string? Link = null,
    Guid? ContatoId = null
);
