using eAgendaWeb.Compartilhado.Dominio;

namespace eAgendaWeb.Modulos.ModuloCompromisso.Dominio;

public sealed class Compromisso : EntidadeBase<Compromisso>
{
    public string Assunto { get; set; } = string.Empty;
    public DateTime DataOcorrencia { get; set; } = DateTime.Now;
    public TimeSpan HoraInicio { get; set; } = TimeSpan.Zero;
    public TimeSpan HoraTermino { get; set; } = TimeSpan.Zero;
    public TipoCompromisso TipoCompromisso { get; set; } = TipoCompromisso.Presencial;
    public string Local { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public Guid? ContatoId { get; set; }

    public Compromisso() { }

    public Compromisso(string assunto, DateTime dataOcorrencia, TimeSpan horaInicio, TimeSpan horaTermino, TipoCompromisso tipoCompromisso, string local, string link, Guid? contatoId = null)
    {
        Assunto = assunto;
        DataOcorrencia = dataOcorrencia;
        HoraInicio = horaInicio;
        HoraTermino = horaTermino;
        TipoCompromisso = tipoCompromisso;
        Local = local;
        Link = link;
        ContatoId = contatoId;
    }
    public override void Atualizar(Compromisso entidadeAtualizada)
    {
        Assunto = entidadeAtualizada.Assunto;
        DataOcorrencia = entidadeAtualizada.DataOcorrencia;
        HoraInicio = entidadeAtualizada.HoraInicio;
        HoraTermino = entidadeAtualizada.HoraTermino;
        TipoCompromisso = entidadeAtualizada.TipoCompromisso;
        Local = entidadeAtualizada.Local;
        Link = entidadeAtualizada.Link;
        ContatoId = entidadeAtualizada.ContatoId;
    }

    public override List<string> Validar()
    {
        List<string> erros = new List<string>();

        if (string.IsNullOrWhiteSpace(Assunto))
            erros.Add("O campo \"Assunto\" deve ser preenchido.");

        else if (Assunto.Length < 2 || Assunto.Length > 100)
            erros.Add("O campo \"Assunto\" deve conter entre 2 e 100 caracteres.");

        if (string.IsNullOrWhiteSpace(DataOcorrencia.ToString()))
            erros.Add("O campo \"Data de Ocorrência\" deve ser preenchido.");

        else if (DataOcorrencia < DateTime.Now)
            erros.Add("O campo \"Data de Ocorrência\" deve ser uma data futura.");

        if (string.IsNullOrWhiteSpace(HoraInicio.ToString()))
            erros.Add("O campo \"Hora de Início\" deve ser preenchido.");

        if (string.IsNullOrWhiteSpace(HoraTermino.ToString()))
            erros.Add("O campo \"Hora de Término\" deve ser preenchido.");

        if (string.IsNullOrWhiteSpace(TipoCompromisso.ToString()))
            erros.Add("O campo \"Tipo de Compromisso\" deve ser preenchido.");

        if (string.IsNullOrWhiteSpace(Local) && TipoCompromisso == TipoCompromisso.Presencial)
            erros.Add("O campo \"Local\" deve ser preenchido para compromissos presenciais.");

        if (string.IsNullOrWhiteSpace(Link) && TipoCompromisso == TipoCompromisso.Remoto)
            erros.Add("O campo \"Link\" deve ser preenchido para compromissos remotos.");

        return erros;
    }
}
