using eAgendaWeb.Modulos.ModuloTarefas.Dominio;
using FluentResults;

namespace eAgendaWeb.Modulos.ModuloTarefas.Aplicacao;

public class ServicoTarefa
{
    private readonly IRepositorioTarefa repositorioTarefa;

    public ServicoTarefa(IRepositorioTarefa repositorioTarefa)
    {
        this.repositorioTarefa = repositorioTarefa;
    }

    private static Result ValidarEntidade(Tarefa tarefa)
    {
        List<string> erros = tarefa.Validar();

        if (erros.Count == 0)
            return Result.Ok();

        string erro = erros.First();
        string campo = erro.Contains("título") ? nameof(Tarefa.Titulo) : string.Empty;

        return Result.Fail(new Error(erro).WithMetadata("Campo", campo));
    }

    public Result Cadastrar(CadastrarTarefaDto dto)
    {
        Tarefa tarefa = new(dto.Titulo, dto.Prioridade);

        if (dto.Itens != null)
        {
            foreach (var itemDto in dto.Itens)
            {
                tarefa.AdicionarItem(new ItemTarefa(itemDto.Titulo));
            }
        }

        Result validacao = ValidarEntidade(tarefa);
        if (validacao.IsFailed) return validacao;

        repositorioTarefa.Cadastrar(tarefa);

        if (tarefa.Itens.Count > 0)
            repositorioTarefa.AdicionarItens(tarefa.Id, tarefa.Itens.ToList());

        return Result.Ok();
    }

    public Result Editar(EditarTarefaDto dto)
    {
        Tarefa? existente = repositorioTarefa.SelecionarPorId(dto.Id);
        if (existente == null) return Result.Fail("Tarefa não encontrada.");

        Tarefa tarefa = new(dto.Titulo, dto.Prioridade);
        tarefa.Id = dto.Id;

        List<ItemTarefa> itensParaSalvar = [];

        if (dto.Itens != null)
        {
            foreach (var itemDto in dto.Itens)
            {
                ItemTarefa item = new(itemDto.Titulo);
                item.Id = itemDto.Id != Guid.Empty ? itemDto.Id : Guid.NewGuid();
                
                if (itemDto.Concluido) 
                    item.Concluir();
                
                itensParaSalvar.Add(item);
                
                tarefa.AdicionarItem(item); 
            }
        }

        Result validacao = ValidarEntidade(tarefa);
        if (validacao.IsFailed) return validacao;

        repositorioTarefa.RemoverItens(dto.Id);
        repositorioTarefa.Editar(dto.Id, tarefa);

        if (itensParaSalvar.Count > 0)
            repositorioTarefa.AdicionarItens(dto.Id, itensParaSalvar);

        return Result.Ok();
    }

    public Result AtualizarStatusItem(Guid tarefaId, Guid itemId, bool concluido)
    {
        Tarefa? tarefa = repositorioTarefa.SelecionarPorId(tarefaId);
        if (tarefa == null) return Result.Fail("Tarefa não encontrada.");

        List<ItemTarefa> itens = repositorioTarefa.SelecionarItens(tarefaId);
        
        Tarefa tarefaReconstruida = new(tarefa.Titulo, tarefa.Prioridade);
        tarefaReconstruida.Id = tarefa.Id;
        
        foreach (var item in itens)
        {
            tarefaReconstruida.AdicionarItem(item);
        }

        if (concluido)
            tarefaReconstruida.ConcluirItem(itemId);
        else
            tarefaReconstruida.DesmarcarItem(itemId);

        repositorioTarefa.Editar(tarefaId, tarefaReconstruida);
        repositorioTarefa.AtualizarStatusItem(itemId, concluido);

        return Result.Ok();
    }

    public Result Excluir(Guid id)
    {
        Tarefa? tarefa = repositorioTarefa.SelecionarPorId(id);
        if (tarefa == null) return Result.Fail("Tarefa não encontrada.");

        repositorioTarefa.RemoverItens(id);
        bool excluido = repositorioTarefa.Excluir(id);

        return excluido ? Result.Ok() : Result.Fail("Falha ao excluir a tarefa.");
    }

    public List<ListarTarefasDto> SelecionarTodos()
    {
        return repositorioTarefa.SelecionarTodos()
            .Select(t => new ListarTarefasDto(
                t.Id, 
                t.Titulo, 
                t.Prioridade, 
                t.DataCriacao, 
                t.DataConclusao, 
                t.PercentualConcluido
            )).ToList();
    }

    public Result<DetalhesTarefaDto> SelecionarPorId(Guid id)
    {
        Tarefa? tarefa = repositorioTarefa.SelecionarPorId(id);
        if (tarefa == null) return Result.Fail("Tarefa não encontrada.");

        List<ItemTarefa> itens = repositorioTarefa.SelecionarItens(id);
        
        List<ItemTarefaDto> itensDto = itens
            .Select(i => new ItemTarefaDto(i.Id, i.Titulo, i.Concluido))
            .ToList();

        return Result.Ok(new DetalhesTarefaDto(
            tarefa.Id, 
            tarefa.Titulo, 
            tarefa.Prioridade, 
            tarefa.DataCriacao, 
            tarefa.DataConclusao, 
            tarefa.PercentualConcluido, 
            itensDto
        ));
    }
}