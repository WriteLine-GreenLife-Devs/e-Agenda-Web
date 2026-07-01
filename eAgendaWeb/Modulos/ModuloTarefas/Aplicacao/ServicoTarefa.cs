using System;
using System.Collections.Generic;
using System.Linq;
using FluentResults;
using eAgendaWeb.Modulos.ModuloTarefas.Dominio;

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

        string campo =
            erro.Contains("título") ? nameof(Tarefa.Titulo)
            : string.Empty;

        return Result.Fail(new Error(erro).WithMetadata("Campo", campo));
    }

    public Result Cadastrar(CadastrarTarefaDto dto)
    {
        Tarefa tarefa = new Tarefa(dto.Titulo, dto.Prioridade);

        if (dto.Itens != null)
        {
            foreach (var itemDto in dto.Itens)
            {
                ItemTarefa item = new ItemTarefa(itemDto.Titulo);

                if (itemDto.Concluido)
                    item.Concluir();

                tarefa.AdicionarItem(item);
            }
        }

        Result validacao = ValidarEntidade(tarefa);

        if (validacao.IsFailed)
            return validacao;

        repositorioTarefa.Cadastrar(tarefa);

        return Result.Ok();
    }

    public Result Editar(EditarTarefaDto dto)
    {
        Tarefa? existente = repositorioTarefa.SelecionarPorId(dto.Id);

        if (existente == null)
            return Result.Fail("Tarefa não encontrada.");

        Tarefa atualizada = new Tarefa(dto.Titulo, dto.Prioridade);

        if (dto.Itens != null)
        {
            foreach (var itemDto in dto.Itens)
            {
                ItemTarefa item = new ItemTarefa(itemDto.Titulo);

                if (itemDto.Id != Guid.Empty)
                    item.Id = itemDto.Id;

                if (itemDto.Concluido)
                    item.Concluir();

                atualizada.AdicionarItem(item);
            }
        }

        existente.Atualizar(atualizada);

        Result validacao = ValidarEntidade(atualizada);

        if (validacao.IsFailed)
            return validacao;

        bool editado = repositorioTarefa.Editar(dto.Id, atualizada);

        if (!editado)
            return Result.Fail("Falha ao editar a tarefa.");

        return Result.Ok();
    }

    public Result Excluir(Guid id)
    {
        Tarefa? tarefa = repositorioTarefa.SelecionarPorId(id);

        if (tarefa == null)
            return Result.Fail("Tarefa não encontrada.");

        bool excluido = repositorioTarefa.Excluir(id);

        if (!excluido)
            return Result.Fail("Falha ao excluir a tarefa.");

        return Result.Ok();
    }

    public List<ListarTarefasDto> SelecionarTodos()
    {
        return repositorioTarefa
            .SelecionarTodos()
            .Select(t => new ListarTarefasDto(
                t.Id,
                t.Titulo,
                t.Prioridade,
                t.DataCriacao,
                t.DataConclusao,
                t.PercentualConcluido
            ))
            .ToList();
    }

    public Result<DetalhesTarefaDto> SelecionarPorId(Guid id)
    {
        Tarefa? tarefa = repositorioTarefa.SelecionarPorId(id);

        if (tarefa == null)
            return Result.Fail("Tarefa não encontrada.");

        var itensDto = tarefa.Itens
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

    public Result AtualizarStatusItem(Guid tarefaId, Guid itemId, bool concluido)
    {
        Tarefa? tarefa = repositorioTarefa.SelecionarPorId(tarefaId);

        if (tarefa == null)
            return Result.Fail("Tarefa não encontrada.");

        if (concluido)
            tarefa.ConcluirItem(itemId);
        else
            tarefa.DesmarcarItem(itemId);

        bool editado = repositorioTarefa.Editar(tarefaId, tarefa);

        if (!editado)
            return Result.Fail("Falha ao atualizar o status do item.");

        return Result.Ok();
    }
}