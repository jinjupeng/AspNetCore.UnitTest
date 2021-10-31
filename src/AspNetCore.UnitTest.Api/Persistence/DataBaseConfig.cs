using MySql.Data.MySqlClient;
using System.Data;

namespace AspNetCore.UnitTest.Api.Persistence
{
    public class DataBaseConfig
    {
        private static string MySqlConnectionString = @"Data Source=127.0.0.1;Initial Catalog=UnitTest;Charset=utf8mb4;User ID=root;Password=123456;";
        public static IDbConnection GetMySqlConnection(string sqlConnectionString = null)
        {
            if (string.IsNullOrWhiteSpace(sqlConnectionString))
            {
                sqlConnectionString = MySqlConnectionString;
            }
            IDbConnection conn = new MySqlConnection(sqlConnectionString);
            conn.Open();
            return conn;
        }
    }
}
