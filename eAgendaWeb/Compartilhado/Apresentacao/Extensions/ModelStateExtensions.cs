using FluentResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace eAgendaWeb.Compartilhado.Apresentacao.Extensions;

public static class ModelStateExtensions
{
    public static void AddModelError(this ModelStateDictionary modelState, ResultBase result)
    {
        foreach (IError erro in result.Errors)
        {
            string campo = string.Empty;

            if (erro.Metadata != null && erro.Metadata.ContainsKey("Campo") && erro.Metadata["Campo"] is string)
                campo = erro.Metadata["Campo"].ToString()!;

            modelState.AddModelError(campo, erro.Message);
        }
    }
}
