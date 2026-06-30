using eAgendaWeb.Compartilhado.Dominio;

namespace eAgendaWeb.Modulos.ModuloCategorias.Dominio;

public sealed class Categoria : EntidadeBase<Categoria>
{
    public string Titulo { get; private set; } = string.Empty;

    private Categoria() { }

    public Categoria(string titulo)
    {
        Titulo = titulo;

        Validar();
    }

    public override List<string> Validar()
    {
        List<string> erros = [];

        if (string.IsNullOrWhiteSpace(Titulo))
            erros.Add("O título da categoria é obrigatório.");

        if (Titulo.Length < 2 || Titulo.Length > 100)
            erros.Add("O título da categoria deve possuir entre 2 e 100 caracteres.");

        return erros;
    }

    public override void Atualizar(Categoria entidade)
    {
        Titulo = entidade.Titulo;
    }
}