using System;
using MySql.Data.MySqlClient;

namespace aspet.Utils
{
    public class MysqlDatabase : IDisposable
    {
        public MySqlConnection Connection;

        public MysqlDatabase()
        {
            MySqlBaseConnectionStringBuilder connBuilder = connString();
            Console.WriteLine("CSHOU DEBUG: " + connBuilder.ConnectionString);
            this.Connection = new MySqlConnection(connBuilder.ConnectionString);
            this.Connection.Open();
            this.init();
        }

        public void Dispose()
        {
            this.Connection.Close();
        }

        private void init()
        {
            using (var cmd = this.Connection.CreateCommand())
            {
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Tasks (
                        TaskId INT AUTO_INCREMENT,
                        Email TEXT NOT NULL,
                        Text TEXT NOT NULL,
                        Created DATE NOT NULL,
                        Completed DATE,
                        Archived DATE,
                        PRIMARY KEY(TaskId)
                    )";
                cmd.ExecuteNonQuery();
            }
        }

        private static MySqlConnectionStringBuilder connString()
        {
            var connStr = new MySqlConnectionStringBuilder()
            {
                SslMode = MySqlSslMode.None,
                Server = Environment.GetEnvironmentVariable("INSTANCE_CONNECTION_NAME"),
                UserID = Environment.GetEnvironmentVariable("DB_USER"),
                Password = Environment.GetEnvironmentVariable("DB_PASSWORD"),
                Database = Environment.GetEnvironmentVariable("DB_NAME"),
                ConnectionProtocol = MySqlConnectionProtocol.UnixSocket
            };
            connStr.Pooling = true;
            return connStr;
        }
    }
}
