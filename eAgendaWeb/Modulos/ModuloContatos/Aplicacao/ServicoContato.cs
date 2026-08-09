using eAgendaWeb.Modulos.ModuloContatos.Dominio;
using FluentResults;

namespace eAgendaWeb.Modulos.ModuloContatos.Aplicacao;

public class ServicoContato
{
    private readonly IRepositorioContato repositorioContato;

    public ServicoContato(IRepositorioContato repositorioContato)
    {
        this.repositorioContato = repositorioContato;
    }

    private static Result Falha(string campo, string mensagem)
    {
        return Result.Fail(new Error(mensagem).WithMetadata("Campo", campo));
    }
    private bool VerificarEmailExistente(string email, Guid? contatoId = null)
    {
        return repositorioContato.SelecionarTodos().Any(c =>
            c.Id != contatoId &&
            string.Equals(c.Email.Trim(), email.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    private bool VerificarTelefoneExistente(string telefone, Guid? contatoId = null)
    {
        string telefoneNormalizado = NormalizarTelefone(telefone);

        return repositorioContato.SelecionarTodos().Any(c =>
            c.Id != contatoId &&
            NormalizarTelefone(c.Telefone) == telefoneNormalizado);
    }

    private static string NormalizarTelefone(string telefone)
    {
        return new string((telefone ?? string.Empty).Where(char.IsDigit).ToArray());
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
        if (VerificarEmailExistente(dto.Email))
            return Falha(nameof(CadastrarContatoDto.Email), "Já existe um contato cadastrado com este email.");

        if (VerificarTelefoneExistente(dto.Telefone))
            return Falha(nameof(CadastrarContatoDto.Telefone), "Já existe um contato cadastrado com este telefone.");

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

        if (repositorioContato.PossuiCompromissosVinculados(id))
            return Falha(nameof(Contato.Id), "Não é possível excluir um contato que possua compromissos vinculados.");

        repositorioContato.Excluir(id);

        return Result.Ok();
    }
    public Result Editar(EditarContatoDto dto)
    {
        Contato? contato = repositorioContato.SelecionarPorId(dto.Id);

        if (contato == null)
            return Result.Fail("Contato não encontrado.");

        if (VerificarEmailExistente(dto.Email, dto.Id))
            return Falha(nameof(EditarContatoDto.Email), "Já existe um contato cadastrado com este email.");

        if (VerificarTelefoneExistente(dto.Telefone, dto.Id))
            return Falha(nameof(EditarContatoDto.Telefone), "Já existe um contato cadastrado com este telefone.");

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
                c.Telefone,
                c.Email,
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
