using eAgendaWeb.Modulos.ModuloContatos.Dominio;
using FluentResults;

namespace eAgendaWeb.Modulos.ModuloContatos.Aplicacao;

public class ServicoContato
{
    private readonly IRepositorioContato repositorioContato;
    private readonly IRepositorioCompromisso repositorioCompromisso;

    public ServicoContato(IRepositorioContato repositorioContato, IRepositorioCompromisso repositorioCompromisso = null)
    {
        this.repositorioContato = repositorioContato;
        this.repositorioCompromisso = repositorioCompromisso;
    }

    private static Result Falha(string campo, string mensagem)
    {
        return Result.Fail(new Error(mensagem).WithMetadata("Campo", campo));
    }
    private bool VerificarEmailAndTelefoneExistente(string Email, string Telefone)
    {
        return repositorioContato.SelecionarTodos().Any(c => c.Email == Email && c.Telefone == Telefone);
    }
    private bool VerificarEmailAndTelefoneExistenteEditar(string Email, string Telefone, Guid Id)
    {
        return repositorioContato.SelecionarTodos().Any(c => c.Email == Email && c.Telefone == Telefone && c.Id != Id);
    }
    private bool VerificarCompromissosVinculados(Guid contatoId)
    {
        if (repositorioCompromisso == null)
            return false;

        return repositorioCompromisso.SelecionarTodos().Any(c => c.ContatoId == contatoId);
    }
    private static Result ValidarEntidade(Contato contato)
    {
        List<string> erros = contato.Validar();

        if (erros.Count == 0)
            return Result.Ok();

        string erro = erros.First();
        string campo = erro.Contains("Nome") ? nameof(Contato.Nome)
            : erro.Contains("Telefone") ? nameof(Contato.Telefone)
            : erro.Contains("Email") ? nameof(Contato.Email)
            : string.Empty;

        return Result.Fail(new Error(erro).WithMetadata("Campo", campo));
    }

    public Result Cadastrar(CadastrarContatoDto dto)
    {
        if (VerificarEmailAndTelefoneExistente(dto.Email, dto.Telefone))
            return Falha(nameof(CadastrarContatoDto.Email), "Já existe um contato cadastrado com este email e telefone.");

        Contato novoContato = new(
            nome: dto.Nome,
            telefone: dto.Telefone,
            email: dto.Email,
            cargo: dto.Cargo ?? string.Empty,
            empresa: dto.Empresa ?? string.Empty
        );

        Result resultadoValidacao = ValidarEntidade(novoContato);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioContato.Cadastrar(novoContato);

        return Result.Ok();
    }
    public Result Excluir(Guid id)
    {
        Contato? contato = repositorioContato.SelecionarPorId(id);

        if (contato == null)
            return Result.Fail("Contato não encontrado.");

        if (VerificarCompromissosVinculados(id))
            return Falha(nameof(Contato.Id), "Não é possível excluir um contato que possua compromissos vinculados.");

        repositorioContato.Excluir(id);

        return Result.Ok();
    }
    public Result Editar(EditarContatoDto dto)
    {
        Contato? contato = repositorioContato.SelecionarPorId(dto.Id);

        if (contato == null)
            return Result.Fail("Contato não encontrado.");

        if (VerificarEmailAndTelefoneExistenteEditar(dto.Email, dto.Telefone, dto.Id))
            return Falha(nameof(EditarContatoDto.Email), "Já existe um contato cadastrado com este email e telefone.");

        Contato contatoAtualizado = new Contato(
            nome: dto.Nome,
            telefone: dto.Telefone,
            email: dto.Email,
            cargo: dto.Cargo ?? string.Empty,
            empresa: dto.Empresa ?? string.Empty
        );

        Result resultadoValidacao = ValidarEntidade(contatoAtualizado);

        if (resultadoValidacao.IsFailed)
            return resultadoValidacao;

        repositorioContato.Editar(dto.Id, contatoAtualizado);

        return Result.Ok();
    }
    public List<ListarContatosDto> SelecionarTodos()
    {
        return repositorioContato
            .SelecionarTodos()
            .Select(c => new ListarContatosDto(
                c.Id,
                c.Nome,
                string.IsNullOrWhiteSpace(c.Cargo) ? null : c.Cargo,
                string.IsNullOrWhiteSpace(c.Empresa) ? null : c.Empresa
            ))
            .ToList();
    }

    public Result<ListarContatosDto> SelecionarPorId(Guid id)
    {
        Contato? contato = repositorioContato.SelecionarPorId(id);

        if (contato == null)
            return Result.Fail("Contato não encontrado.");

        return Result.Ok(new ListarContatosDto(
            contato.Id,
            contato.Nome,
            contato.Telefone,
            contato.Email,
            string.IsNullOrWhiteSpace(contato.Cargo) ? null : contato.Cargo,
            string.IsNullOrWhiteSpace(contato.Empresa) ? null : contato.Empresa
        ));
    }
}
