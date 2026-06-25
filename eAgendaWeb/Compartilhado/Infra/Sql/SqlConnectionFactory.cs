using Microsoft.Data.SqlClient;

namespace eAgendaWeb.Compartilhado.Infra.Sql;

public sealed class SqlConnectionFactory(IConfiguration configuration) : ISqlConnectionFactory
{
    private const string NomeConnectionString = "ControleDeMedicamentosWeb";

    // ConnectionString = Endereço do banco de dados local/remoto que vamos usar
    public SqlConnection CreateConnection()
    {
        string? connectionString = configuration.GetConnectionString(NomeConnectionString);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                $"A connection string {NomeConnectionString} não foi encontrada."
            );
        }

        return new SqlConnection(connectionString);
    }
}
