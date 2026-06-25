using Microsoft.AspNetCore.Mvc;

namespace eAgendaWeb.Compartilhado.Apresentacao;

public class HomeController : Controller
{
    [HttpGet]
    public ActionResult Index()
    {
        return View();
    }
}
