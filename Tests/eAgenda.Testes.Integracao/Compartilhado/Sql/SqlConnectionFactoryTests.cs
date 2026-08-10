using eAgendaWeb.Compartilhado.Infra.Sql;
using Microsoft.Data.SqlClient;

namespace eAgenda.Testes.Integracao.Compartilhado.Sql;

public sealed class SqlConnectionFactoryTests(string connectionString) : ISqlConnectionFactory
{
    public SqlConnection CreateConnection()
    {
        return new SqlConnection(connectionString);
    }
}
