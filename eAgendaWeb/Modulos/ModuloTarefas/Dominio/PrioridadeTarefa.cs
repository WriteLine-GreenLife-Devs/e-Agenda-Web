using System.ComponentModel.DataAnnotations;

namespace eAgendaWeb.Modulos.ModuloTarefas.Dominio;

public enum PrioridadeTarefa
{
    [Display(Name = "Baixa")]
    Baixa = 0,

    [Display(Name = "Normal")]
    Normal = 1,

    [Display(Name = "Alta")]
    Alta = 2
}