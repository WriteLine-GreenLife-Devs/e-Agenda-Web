using System.ComponentModel.DataAnnotations;
using eAgendaWeb.Modulos.ModuloDespesas.Dominio;

namespace eAgendaWeb.Modulos.ModuloDespesas.Apresentacao;

public record ListarDespesasViewModel(
    Guid Id,
    string Descricao,
    DateTime DataOcorrencia,
    decimal Valor,
    FormaPagamento? FormaPagamento,
    int? QuantidadeParcelas,
    IReadOnlyList<string> Categorias
);

public record CadastrarDespesaViewModel(

    [Required(ErrorMessage = "O campo \"Descrição\" deve ser preenchido.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "A descrição deve ter entre 2 e 100 caracteres.")]
    string Descricao,

    [Required(ErrorMessage = "O campo \"Data da Ocorrência\" deve ser preenchido.")]
    DateTime DataOcorrencia,

    [Required(ErrorMessage = "O campo \"Valor\" deve ser preenchido.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
    decimal Valor,

    [Required(ErrorMessage = "O campo \"Forma de Pagamento\" deve ser preenchido.")]
    FormaPagamento? FormaPagamento,

    [Range(1, 24, ErrorMessage = "A quantidade de parcelas deve ser entre 1 e 24.")]
    int? QuantidadeParcelas = null,

    List<Guid>? CategoriasIds = null
);

public record EditarDespesaViewModel(

    Guid Id,

    [Required(ErrorMessage = "O campo \"Descrição\" deve ser preenchido.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "A descrição deve ter entre 2 e 100 caracteres.")]
    string Descricao,

    [Required(ErrorMessage = "O campo \"Data da Ocorrência\" deve ser preenchido.")]
    DateTime DataOcorrencia,

    [Required(ErrorMessage = "O campo \"Valor\" deve ser preenchido.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
    decimal Valor,

    [Required(ErrorMessage = "O campo \"Forma de Pagamento\" deve ser preenchido.")]
    FormaPagamento? FormaPagamento,

    [Range(1, 24, ErrorMessage = "A quantidade de parcelas deve ser entre 1 e 24.")]
    int? QuantidadeParcelas = null,

    List<Guid>? CategoriasIds = null
);

public record ExcluirDespesaViewModel(
    Guid Id,
    string Descricao,
    DateTime DataOcorrencia,
    decimal Valor,
    FormaPagamento? FormaPagamento,
    int? QuantidadeParcelas,
    IReadOnlyList<string> Categorias
);