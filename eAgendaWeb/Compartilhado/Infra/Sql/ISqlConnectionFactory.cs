using Microsoft.Data.SqlClient;

namespace eAgendaWeb.Compartilhado.Infra.Sql;

public interface ISqlConnectionFactory
{
    SqlConnection CreateConnection();
}
