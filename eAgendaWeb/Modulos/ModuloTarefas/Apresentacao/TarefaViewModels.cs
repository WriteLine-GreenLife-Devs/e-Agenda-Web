using System.ComponentModel.DataAnnotations;
using eAgendaWeb.Modulos.ModuloTarefas.Dominio;

namespace eAgendaWeb.Modulos.ModuloTarefas.Apresentacao;

public record ListarTarefasViewModel(
    Guid Id,
    string Titulo,
    PrioridadeTarefa Prioridade,
    DateTime DataCriacao,
    DateTime? DataConclusao,
    decimal PercentualConcluido
);

public record CadastrarTarefaViewModel(
    [Required(ErrorMessage = "O título da tarefa é obrigatório.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O título deve ter entre 2 e 100 caracteres.")]
    string Titulo,

    [Required(ErrorMessage = "A prioridade é obrigatória.")]
    PrioridadeTarefa Prioridade,

    List<ItemTarefaViewModel>? Itens = null
);

public record EditarTarefaViewModel(
    Guid Id,

    [Required(ErrorMessage = "O título da tarefa é obrigatório.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O título deve ter entre 2 e 100 caracteres.")]
    string Titulo,

    [Required(ErrorMessage = "A prioridade é obrigatória.")]
    PrioridadeTarefa Prioridade,

    List<ItemTarefaViewModel>? Itens = null
);

public record ItemTarefaViewModel(
    Guid Id,

    [Required(ErrorMessage = "O título do item é obrigatório.")]
    string Titulo,

    bool Concluido
);

public record ExcluirTarefaViewModel(
    Guid Id,
    string Titulo,
    PrioridadeTarefa Prioridade,
    decimal PercentualConcluido
);

public record AtualizarStatusItemViewModel(
    Guid TarefaId,
    Guid ItemId,
    bool Concluido
);