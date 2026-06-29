namespace eAgenda.Dominio.Modulos.Categoria;

public class Categoria
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string? Descricao { get; set; }
    public bool Ativo { get; set; }

    public DateTime DataCriacao { get; set; }

    public Categoria()
    {
        Id = Guid.NewGuid();
        Ativo = true;
        DataCriacao = DateTime.Now;
    }

    public void Atualizar(string nome, string? descricao)
    {
        Nome = nome;
        Descricao = descricao;
    }

    public void Desativar()
    {
        Ativo = false;
    }
}