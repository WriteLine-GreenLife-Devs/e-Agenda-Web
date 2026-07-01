using System.ComponentModel.DataAnnotations;

namespace eAgendaWeb.Modulos.ModuloDespesas.Dominio;

public enum FormaPagamento
{
    [Display(Name = "À Vista")]
    AVista = 1,

    [Display(Name = "Crédito")]
    Credito = 2,
    
    [Display(Name = "Débito")]
    Debito = 3
}