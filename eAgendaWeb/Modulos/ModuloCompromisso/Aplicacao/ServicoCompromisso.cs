using eAgendaWeb.Modulos.ModuloCompromisso.Dominio;
using eAgendaWeb.Modulos.ModuloContatos.Dominio;
using FluentResults;

namespace eAgendaWeb.Modulos.ModuloCompromisso.Aplicacao;

public class ServicoCompromisso
{
    private readonly IRepositorioCompromisso repositorioCompromisso;
    private readonly IRepositorioContato? repositorioContato;

    public ServicoCompromisso(IRepositorioCompromisso repositorioCompromisso, IRepositorioContato? repositorioContato = null)
    {
        this.repositorioCompromisso = repositorioCompromisso;
        this.repositorioContato = repositorioContato;
    }

    private static Result Falha(string campo, string mensagem)
    {
        return Result.Fail(new Error(mensagem).WithMetadata("Campo", campo));
    }

    private static string? NormalizarCampoOpcional(string? valor)
    {
        return string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
    }

    private static void AjustarCamposPorTipo(TipoCompromisso tipoCompromisso, ref string? local, ref string? link)
    {
        if (tipoCompromisso == TipoCompromisso.Presencial)
            link = null;
        else if (tipoCompromisso == TipoCompromisso.Remoto)
            local = null;
    }

    private static Result ValidarEntidade(Compromisso compromisso)
    {
        List<string> erros = compromisso.Validar();

        if (erros.Count == 0)
            return Result.Ok();

        string erro = erros.First();
        string campo = erro.Contains("Assunto") ? nameof(Compromisso.Assunto)
            : erro.Contains("Ocorrência") ? nameof(Compromisso.DataOcorrencia)
            : erro.Contains("Início") ? nameof(Compromisso.HoraInicio)
            : erro.Contains("Término") ? nameof(Compromisso.HoraTermino)
            : erro.Contains("Tipo de Compromisso") ? nameof(Compromisso.TipoCompromisso)
            : erro.Contains("Local") ? nameof(Compromisso.Local)
            : erro.Contains("Link") ? nameof(Compromisso.Link)
            : string.Empty;

        return Result.Fail(new Error(erro).WithMetadata("Campo", campo));
    }

    private bool ContatoExiste(Guid? contatoId)
    {
        if (contatoId == null || repositorioContato == null)
            return true;

        return repositorioContato.SelecionarTodos().Any(c => c.Id == contatoId);
    }

    private string? ObterNomeContato(Guid? contatoId)
    {
        if (contatoId == null || repositorioContato == null)
            return null;

        return repositorioContato.SelecionarTodos().FirstOrDefault(c => c.Id == contatoId)?.Nome;
    }

    private bool VerificarConflitoHorario(Compromisso compromisso, Guid? idIgnorado = null)
    {
        return repositorioCompromisso.SelecionarTodos()
            .Where(c => idIgnorado == null || c.Id != idIgnorado)
            .Any(c => c.DataOcorrencia.Date == compromisso.DataOcorrencia.Date
                && compromisso.HoraInicio < c.HoraTermino
                && compromisso.HoraTermino > c.HoraInicio);
    }

    public Result Cadastrar(CadastrarCompromissoDto dto)
    {
        if (!ContatoExiste(dto.ContatoId))
            return Falha(nameof(CadastrarCompromissoDto.ContatoId), "Contato selecionado não existe.");

        string? local = NormalizarCampoOpcional(dto.Local);
        string? link = NormalizarCampoOpcional(dto.Link);
        AjustarCamposPorTipo(dto.TipoCompromisso, ref local, ref link);

        Compromisso novoCompromisso = new(
            assunto: dto.Assunto,
            dataOcorrencia: dto.DataOcorrencia,
            horaInicio: dto.HoraInicio,
            horaTermino: dto.HoraTermino,
            tipoCompromisso: dto.TipoCompromisso,
            local: local,
            link: link,
            contatoId: dto.ContatoId
        );

        Result resultadoValidacao = ValidarEntidade(novoCompromisso);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        if (VerificarConflitoHorario(novoCompromisso))
            return Falha(nameof(CadastrarCompromissoDto.HoraInicio), "Não é possível cadastrar um compromisso com horário conflitante.");

        repositorioCompromisso.Cadastrar(novoCompromisso);

        return Result.Ok();
    }

    public Result Editar(EditarCompromissoDto dto)
    {
        Compromisso? compromissoExistente = repositorioCompromisso.SelecionarPorId(dto.Id);

        if (compromissoExistente == null)
            return Result.Fail("Compromisso não encontrado.");

        if (!ContatoExiste(dto.ContatoId))
            return Falha(nameof(EditarCompromissoDto.ContatoId), "Contato selecionado não existe.");

        string? local = NormalizarCampoOpcional(dto.Local);
        string? link = NormalizarCampoOpcional(dto.Link);
        AjustarCamposPorTipo(dto.TipoCompromisso, ref local, ref link);

        Compromisso compromissoAtualizado = new(
            assunto: dto.Assunto,
            dataOcorrencia: dto.DataOcorrencia,
            horaInicio: dto.HoraInicio,
            horaTermino: dto.HoraTermino,
            tipoCompromisso: dto.TipoCompromisso,
            local: local,
            link: link,
            contatoId: dto.ContatoId
        );

        compromissoExistente.Atualizar(compromissoAtualizado);

        Result resultadoValidacao = ValidarEntidade(compromissoExistente);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        if (VerificarConflitoHorario(compromissoExistente, dto.Id))
            return Falha(nameof(EditarCompromissoDto.HoraInicio), "Não é possível editar para um horário que conflita com outro compromisso.");

        bool atualizado = repositorioCompromisso.Editar(dto.Id, compromissoExistente);

        return atualizado ? Result.Ok() : Result.Fail("Falha ao atualizar o compromisso.");
    }

    public List<ListarCompromissosDto> SelecionarTodos()
    {
        return repositorioCompromisso
            .SelecionarTodos()
            .Select(c => new ListarCompromissosDto(
                c.Id,
                c.Assunto,
                c.DataOcorrencia,
                c.HoraInicio,
                c.HoraTermino,
                c.TipoCompromisso,
                string.IsNullOrWhiteSpace(c.Local) ? null : c.Local,
                string.IsNullOrWhiteSpace(c.Link) ? null : c.Link,
                c.ContatoId,
                ObterNomeContato(c.ContatoId)
            ))
            .ToList();
    }

    public Result<ListarCompromissosDto> SelecionarPorId(Guid id)
    {
        Compromisso? compromisso = repositorioCompromisso.SelecionarPorId(id);

        if (compromisso == null)
            return Result.Fail("Compromisso não encontrado.");

        return Result.Ok(new ListarCompromissosDto(
            compromisso.Id,
            compromisso.Assunto,
            compromisso.DataOcorrencia,
            compromisso.HoraInicio,
            compromisso.HoraTermino,
            compromisso.TipoCompromisso,
            string.IsNullOrWhiteSpace(compromisso.Local) ? null : compromisso.Local,
            string.IsNullOrWhiteSpace(compromisso.Link) ? null : compromisso.Link,
            compromisso.ContatoId,
            ObterNomeContato(compromisso.ContatoId)
        ));
    }

    public Result Excluir(Guid id)
    {
        Compromisso? compromisso = repositorioCompromisso.SelecionarPorId(id);

        if (compromisso == null)
            return Result.Fail("Compromisso não encontrado.");

        bool excluido = repositorioCompromisso.Excluir(id);

        return excluido ? Result.Ok() : Result.Fail("Falha ao excluir o compromisso.");
    }
}
