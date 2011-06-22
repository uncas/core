using System;
using System.Data;
using System.Data.Common;
using System.Globalization;

namespace Uncas.Core.Data
{
    /// <summary>
    /// Represents a general db context.
    /// </summary>
    public abstract class DbContext
    {
        private readonly string _connectionString;

        private readonly DbProviderFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContext"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="connectionString">The connection string.</param>
        protected DbContext(
            DbProviderFactory factory,
            string connectionString)
        {
            _factory = factory;
            _connectionString = connectionString;
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public DataTable GetData(
            string commandText,
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
                    {
                        command.Parameters.Add(parameter);
                    }

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

        /// <summary>
        /// Gets the reader.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public DbDataReader GetReader(
            string commandText,
            params DbParameter[] parameters)
        {
            DbConnection connection = _factory.CreateConnection();
            connection.ConnectionString = _connectionString;
            DbCommand command = _factory.CreateCommand();
            command.CommandText = commandText;
            command.Connection = connection;
            foreach (DbParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Modifies the data.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public int ModifyData(
            string commandText,
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
                    {
                        command.Parameters.Add(parameter);
                    }

                    connection.Open();
                    iOut = command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            return iOut;
        }

        /// <summary>
        /// Gets the int32.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public int? GetInt32(
            string commandText,
            params DbParameter[] parameters)
        {
            return GetScalar<int?>(commandText, parameters);
        }

        /// <summary>
        /// Gets the scalar.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public T GetScalar<T>(
            string commandText,
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
                    {
                        command.Parameters.Add(parameter);
                    }

                    connection.Open();
                    object databaseValue = command.ExecuteScalar();
                    if (!(databaseValue is DBNull))
                    {
                        return (T)databaseValue;
                    }

                    return default(T);
                }
            }
        }
    }
}