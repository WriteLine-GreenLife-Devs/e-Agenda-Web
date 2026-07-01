using eAgendaWeb.Modulos.ModuloTarefas.Dominio;

namespace eAgendaWeb.Modulos.ModuloTarefas.Aplicacao;

public record CadastrarTarefaDto(
    string Titulo, 
    PrioridadeTarefa Prioridade, 
    List<ItemTarefaDto> Itens
);

public record EditarTarefaDto(
    Guid Id, 
    string Titulo, 
    PrioridadeTarefa Prioridade, 
    List<ItemTarefaDto> Itens
);

public record ItemTarefaDto(
    Guid Id, 
    string Titulo, 
    bool Concluido
);

public record ListarTarefasDto(
    Guid Id, 
    string Titulo, 
    PrioridadeTarefa Prioridade, 
    DateTime DataCriacao, 
    DateTime? DataConclusao, 
    decimal PercentualConcluido
);

public record DetalhesTarefaDto(
    Guid Id, 
    string Titulo, 
    PrioridadeTarefa Prioridade, 
    DateTime DataCriacao, 
    DateTime? DataConclusao, 
    decimal PercentualConcluido,
    List<ItemTarefaDto> Itens
);