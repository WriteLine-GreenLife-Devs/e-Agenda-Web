using eAgendaWeb.Compartilhado.Dominio;

namespace eAgendaWeb.Modulos.ModuloDespesas.Dominio;

public sealed class Despesa : EntidadeBase<Despesa>
{
    public string Descricao { get; private set; } = string.Empty;
    public DateTime DataOcorrencia { get; private set; }
    public decimal Valor { get; private set; }
    public FormaPagamento FormaPagamento { get; private set; }
    public int? QuantidadeParcelas { get; private set; }

    private Despesa() { }

    public Despesa(
        string descricao,
        DateTime dataOcorrencia,
        decimal valor,
        FormaPagamento formaPagamento,
        int? quantidadeParcelas
        )
    {
        Descricao = descricao;
        DataOcorrencia = dataOcorrencia;
        Valor = valor;
        FormaPagamento = formaPagamento;
        QuantidadeParcelas = quantidadeParcelas;

        Validar();
    }

    public override List<string> Validar()
    {
        List<string> erros = [];

        if (string.IsNullOrWhiteSpace(Descricao))
            erros.Add("A descrição da despesa é obrigatória.");

        if (Descricao.Length < 2 || Descricao.Length > 100)
            erros.Add("A descrição deve possuir entre 2 e 100 caracteres.");

        if (Valor <= 0)
            erros.Add("O valor da despesa deve ser maior que zero.");

        if (FormaPagamento == FormaPagamento.Credito &&
            (!QuantidadeParcelas.HasValue || QuantidadeParcelas <= 0))
        {
            erros.Add("Compras no crédito precisam informar a quantidade de parcelas.");
        }

        if (FormaPagamento != FormaPagamento.Credito &&
            QuantidadeParcelas.HasValue)
        {
            erros.Add("Parcelamento permitido somente para crédito.");
        }

        return erros;
    }

    public override void Atualizar(Despesa entidade)
    {
        Descricao = entidade.Descricao;
        DataOcorrencia = entidade.DataOcorrencia;
        Valor = entidade.Valor;
        FormaPagamento = entidade.FormaPagamento;
        QuantidadeParcelas = entidade.QuantidadeParcelas;
    }
}