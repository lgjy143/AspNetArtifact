using Oracle.ManagedDataAccess.Client;

namespace NetArtifact.Utils
{
    public class DapperUtils
    {
        public class DapperFactory
        {
            public static readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EASConnectionString"].ToString();

            public static OracleConnection CreateOracleConnection()
            {
                var connection = new OracleConnection(connectionString);
                connection.Open();
                return connection;
            }
        }
    }
}
