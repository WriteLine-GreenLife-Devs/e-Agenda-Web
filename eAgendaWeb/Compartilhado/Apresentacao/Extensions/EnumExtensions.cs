using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace eAgendaWeb.Compartilhado.Apresentacao.Extensions;

public static class EnumExtensions
{
    public static string ObterDescricao(this Enum valor)
    {
        if (valor == null) return string.Empty;

        FieldInfo? field = valor.GetType().GetField(valor.ToString());
        if (field == null) return valor.ToString();

        DisplayAttribute? atributo = field.GetCustomAttribute<DisplayAttribute>();

        // Retorna o nome escolhido em [Display], se não houver, retornará o nome original do Enum.
        return atributo?.Name ?? valor.ToString();
        
        // Deve importar essa extensão nos arquivos cshtml para funcionar.
    }
}