using System;
using System.Data;
using System.Data.Common;
using System.Globalization;

namespace Uncas.Core.Data
{
    public abstract class DbContext
    {
        private readonly string _connectionString;

        private readonly DbProviderFactory _factory;

        protected DbContext(
            DbProviderFactory factory,
            string connectionString)
        {
            _factory = factory;
            _connectionString = connectionString;
        }

        public DataTable GetData(string commandText,
            params DbParameter[] parameters)
        {
            DataTable dt = new DataTable("uncas-data");
            dt.Locale = CultureInfo.InvariantCulture;
            using (DbConnection connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                using (DbCommand command = _factory.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.Connection = connection;
                    foreach (DbParameter parameter in parameters)
                        command.Parameters.Add(parameter);
                    using (DbDataAdapter adapter = _factory.CreateDataAdapter())
                    {
                        adapter.SelectCommand = command;
                        connection.Open();
                        adapter.Fill(dt);
                        connection.Close();
                    }
                }
            }

            return dt;
        }

        public DbDataReader GetReader(string commandText,
            params DbParameter[] parameters)
        {
            DbConnection connection = _factory.CreateConnection();
            connection.ConnectionString = _connectionString;
            DbCommand command = _factory.CreateCommand();
            command.CommandText = commandText;
            command.Connection = connection;
            foreach (DbParameter parameter in parameters)
                command.Parameters.Add(parameter);
            connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public int ModifyData(string commandText,
            params DbParameter[] parameters)
        {
            int iOut = 0;
            using (DbConnection connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                using (DbCommand command = _factory.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.Connection = connection;
                    foreach (DbParameter parameter in parameters)
                        command.Parameters.Add(parameter);
                    connection.Open();
                    iOut = command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            return iOut;
        }

        public int? GetInt32(string commandText,
            params DbParameter[] parameters)
        {
            return GetScalar<int?>(commandText, parameters);
        }

        public T GetScalar<T>(string commandText,
            params DbParameter[] parameters)
        {
            using (DbConnection connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                using (DbCommand command = _factory.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.Connection = connection;
                    foreach (DbParameter parameter in parameters)
                        command.Parameters.Add(parameter);
                    connection.Open();
                    object databaseValue = command.ExecuteScalar();
                    if (!(databaseValue is DBNull))
                        return (T)databaseValue;
                    return default(T);
                }
            }
        }
    }
}