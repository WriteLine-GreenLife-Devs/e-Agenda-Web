using AutoMapper;
using eAgendaWeb.Compartilhado.Apresentacao.Extensions;
using eAgendaWeb.Modulos.ModuloCompromisso.Aplicacao;
using eAgendaWeb.Modulos.ModuloCompromisso.Dominio;
using eAgendaWeb.Modulos.ModuloContatos.Aplicacao;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eAgendaWeb.Modulos.ModuloCompromisso.Apresentacao;

public class CompromissoController(ServicoCompromisso servicoCompromisso, ServicoContato servicoContato, IMapper mapeador) : Controller
{
    private IEnumerable<SelectListItem> ObterOpcoesContatos()
    {
        return servicoContato.SelecionarTodos()
            .Select(c => new SelectListItem(c.Nome, c.Id.ToString()))
            .ToList();
    }

    [HttpGet]
    public ActionResult Listar()
    {
        List<ListarCompromissosDto> dtos = servicoCompromisso.SelecionarTodos();
        List<ListarCompromissosViewModel> listarVms = mapeador.Map<List<ListarCompromissosViewModel>>(dtos);

        return View(listarVms);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        CadastrarCompromissoViewModel cadastrarVm = new(
            Assunto: string.Empty,
            DataOcorrencia: DateTime.Now.Date,
            HoraInicio: TimeSpan.Zero,
            HoraTermino: TimeSpan.Zero,
            TipoCompromisso: TipoCompromisso.Presencial,
            Contatos: ObterOpcoesContatos()
        );

        return View(cadastrarVm);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarCompromissoViewModel cadastrarVm)
    {
        cadastrarVm = cadastrarVm with { Contatos = ObterOpcoesContatos() };

        if (!ModelState.IsValid)
            return View(cadastrarVm);

        CadastrarCompromissoDto dto = mapeador.Map<CadastrarCompromissoDto>(cadastrarVm);

        Result resultado = servicoCompromisso.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            return View(cadastrarVm);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(Guid id)
    {
        Result<ListarCompromissosDto> resultado = servicoCompromisso.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData.AddErrorMessage(resultado);
            return RedirectToAction(nameof(Listar));
        }

        EditarCompromissoViewModel editarVm = mapeador.Map<EditarCompromissoViewModel>(resultado.Value) with
        {
            Contatos = ObterOpcoesContatos()
        };

        return View(editarVm);
    }

    [HttpPost]
    public ActionResult Editar(EditarCompromissoViewModel editarVm)
    {
        editarVm = editarVm with { Contatos = ObterOpcoesContatos() };

        if (!ModelState.IsValid)
            return View(editarVm);

        EditarCompromissoDto dto = mapeador.Map<EditarCompromissoDto>(editarVm);

        Result resultado = servicoCompromisso.Editar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);
            return View(editarVm);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(Guid id)
    {
        Result<ListarCompromissosDto> resultado = servicoCompromisso.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData.AddErrorMessage(resultado);
            return RedirectToAction(nameof(Listar));
        }

        ExcluirCompromissoViewModel excluirVm = mapeador.Map<ExcluirCompromissoViewModel>(resultado.Value);

        return View(excluirVm);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirCompromissoViewModel excluirVm)
    {
        Result resultado = servicoCompromisso.Excluir(excluirVm.Id);

        if (resultado.IsFailed)
            TempData.AddErrorMessage(resultado);

        return RedirectToAction(nameof(Listar));
    }
}
