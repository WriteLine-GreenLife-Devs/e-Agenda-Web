using eAgendaWeb.Compartilhado.Dominio;

namespace eAgendaWeb.Modulos.ModuloContatos.Dominio;

public sealed class Contato : EntidadeBase<Contato>
{

    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public string Empresa { get; set; } = string.Empty;

    public Contato() { }

    public Contato(string nome, string email, string telefone, string cargo, string empresa)
    {
        Nome = nome;
        Email = email;
        Telefone = telefone;
        Cargo = cargo;
        Empresa = empresa;
    }
    public override void Atualizar(Contato entidadeAtualizada)
    {
        Nome = entidadeAtualizada.Nome;
        Email = entidadeAtualizada.Email;
        Telefone = entidadeAtualizada.Telefone;
        Cargo = entidadeAtualizada.Cargo;
        Empresa = entidadeAtualizada.Empresa;
    }

    public string VerificarTelefone(string telefone)
    {
        string apenasNumeros = System.Text.RegularExpressions.Regex.Replace(telefone ?? "", @"[^\d]", "");

        int tamanho = apenasNumeros.Length;

        if (tamanho == 10)
        {
            Telefone = long.Parse(apenasNumeros).ToString(@"(00) 0000-0000");
            return long.Parse(apenasNumeros).ToString(@"(00) 0000-0000");
        }
        else if (tamanho == 11)
        {
            Telefone = long.Parse(apenasNumeros).ToString(@"(00) 0 0000-0000");
            return long.Parse(apenasNumeros).ToString(@"(00) 0 0000-0000");
        }
        else
        {
            return "";
        }
    }

    public override List<string> Validar()
    {
        List<string> erros = new List<string>();

        if (string.IsNullOrWhiteSpace(Nome))
            erros.Add("O campo \"Nome\" deve ser preenchido.");

        else if (Nome.Length < 2 || Nome.Length > 100)
            erros.Add("O campo \"Nome\" deve conter entre 2 e 100 caracteres.");

        if (string.IsNullOrWhiteSpace(Telefone))
            erros.Add("O campo \"Telefone\" deve ser preenchido.");

        else if (VerificarTelefone(Telefone) == "")
            erros.Add("O campo \"Telefone\" é inválido.");

        if (string.IsNullOrWhiteSpace(Email))
            erros.Add("O campo \"Email\" deve ser preenchido.");

        else if (!Email.Contains("@"))
            erros.Add("O campo \"Email\" é inválido.");

        return erros;
    }
}
