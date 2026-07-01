using AutoMapper;
using eAgendaWeb.Compartilhado.Apresentacao.Extensions;
using eAgendaWeb.Modulos.ModuloTarefas.Aplicacao;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace eAgendaWeb.Modulos.ModuloTarefas.Apresentacao;

public class TarefasController(
    ServicoTarefa servicoTarefa,
    IMapper mapeador
) : Controller
{
    [HttpGet]
    public ActionResult Listar()
    {
        List<ListarTarefasDto> dtos = servicoTarefa.SelecionarTodos();
        List<ListarTarefasViewModel> vms = mapeador.Map<List<ListarTarefasViewModel>>(dtos);

        return View(vms);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarTarefaViewModel vm)
    {
        vm.Itens?.RemoveAll(i => string.IsNullOrWhiteSpace(i.Titulo));

        if (!ModelState.IsValid)
            return View(vm);

        CadastrarTarefaDto dto = mapeador.Map<CadastrarTarefaDto>(vm);
        Result resultado = servicoTarefa.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            return View(vm);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(Guid id)
    {
        Result<DetalhesTarefaDto> resultado = servicoTarefa.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData.AddErrorMessage(resultado);
            return RedirectToAction(nameof(Listar));
        }

        EditarTarefaViewModel vm = mapeador.Map<EditarTarefaViewModel>(resultado.Value);

        return View(vm);
    }

    [HttpPost]
    public ActionResult Editar(EditarTarefaViewModel vm)
    {
        vm.Itens?.RemoveAll(i => string.IsNullOrWhiteSpace(i.Titulo));

        if (!ModelState.IsValid)
            return View(vm);

        EditarTarefaDto dto = mapeador.Map<EditarTarefaDto>(vm);
        Result resultado = servicoTarefa.Editar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            return View(vm);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(Guid id)
    {
        Result<DetalhesTarefaDto> resultado = servicoTarefa.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData.AddErrorMessage(resultado);
            return RedirectToAction(nameof(Listar));
        }

        ExcluirTarefaViewModel vm = mapeador.Map<ExcluirTarefaViewModel>(resultado.Value);

        return View(vm);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirTarefaViewModel vm)
    {
        Result resultado = servicoTarefa.Excluir(vm.Id);

        if (resultado.IsFailed)
            TempData.AddErrorMessage(resultado);

        return RedirectToAction(nameof(Listar));
    }

    [HttpPost]
    public IActionResult AtualizarStatusItem([FromBody] AtualizarStatusItemViewModel viewModel)
    {
        if (viewModel.TarefaId == Guid.Empty || viewModel.ItemId == Guid.Empty)
        {
            return BadRequest(new { erro = "IDs inválidos." });
        }

        Result resultado = servicoTarefa.AtualizarStatusItem(viewModel.TarefaId, viewModel.ItemId, viewModel.Concluido);

        if (resultado.IsFailed)
        {
            return BadRequest(new { erro = resultado.Errors[0].Message });
        }

        return Ok();
    }
}