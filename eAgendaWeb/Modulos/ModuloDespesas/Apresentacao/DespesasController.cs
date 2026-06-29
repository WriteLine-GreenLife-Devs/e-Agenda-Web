using AutoMapper;
using eAgendaWeb.Compartilhado.Apresentacao.Extensions;
using eAgendaWeb.Modulos.ModuloDespesas.Aplicacao;
using eAgendaWeb.Modulos.ModuloDespesas.Dominio;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace eAgendaWeb.Modulos.ModuloDespesas.Apresentacao;

public class DespesasController(ServicoDespesa servicoDespesa, IMapper mapeador) : Controller
{
    [HttpGet]
    public ActionResult Listar()
    {
        List<ListarDespesasDto> dtos = servicoDespesa.SelecionarTodos();

        List<ListarDespesasViewModel> vms =
            mapeador.Map<List<ListarDespesasViewModel>>(dtos);

        return View(vms);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        CadastrarDespesaViewModel vm = new(
            string.Empty,
            DateTime.Now,
            0,
            null,
            null,
            null
        );

        return View(vm);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarDespesaViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        CadastrarDespesaDto dto =
            mapeador.Map<CadastrarDespesaDto>(vm);

        Result resultado = servicoDespesa.Cadastrar(dto);

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
        Result<ListarDespesasDto> resultado =
            servicoDespesa.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData.AddErrorMessage(resultado);
            return RedirectToAction(nameof(Listar));
        }

        EditarDespesaViewModel vm =
            mapeador.Map<EditarDespesaViewModel>(resultado.Value);

        return View(vm);
    }

    [HttpPost]
    public ActionResult Editar(EditarDespesaViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        EditarDespesaDto dto =
            mapeador.Map<EditarDespesaDto>(vm);

        Result resultado = servicoDespesa.Editar(dto);

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
        Result<ListarDespesasDto> resultado =
            servicoDespesa.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData.AddErrorMessage(resultado);
            return RedirectToAction(nameof(Listar));
        }

        ExcluirDespesaViewModel vm =
            mapeador.Map<ExcluirDespesaViewModel>(resultado.Value);

        return View(vm);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirDespesaViewModel vm)
    {
        Result resultado = servicoDespesa.Excluir(vm.Id);

        if (resultado.IsFailed)
            TempData.AddErrorMessage(resultado);

        return RedirectToAction(nameof(Listar));
    }
}