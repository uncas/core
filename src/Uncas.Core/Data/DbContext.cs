namespace Uncas.Core.Data
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

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
        /// Gets the data table for the given command text.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The data table.</returns>
        [Obsolete("Use overload that takes DbCommand instead.")]
        [SuppressMessage(
            "Microsoft.Security",
            "CA2100:Review SQL queries for security vulnerabilities",
            Justification = "This is marked as obsolete.")]
        public DataTable GetData(
            string commandText,
            params DbParameter[] parameters)
        {
            DataTable dt = new DataTable("uncas-data");
            dt.Locale = CultureInfo.InvariantCulture;
            Action<DbCommand> commandAction =
                (DbCommand command) =>
                {
                    using (DbDataAdapter adapter = _factory.CreateDataAdapter())
                    {
                        adapter.SelectCommand = command;
                        adapter.Fill(dt);
                    }
                };
            DbCommand command2 = _factory.CreateCommand();
            command2.CommandText = commandText;
            OperateOnDbCommand(
                commandAction,
                command2,
                parameters);
            return dt;
        }

        /// <summary>
        /// Gets the data table for the given command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The data table.</returns>
        public DataTable GetData(
            DbCommand command,
            params DbParameter[] parameters)
        {
            DataTable dt = new DataTable("uncas-data");
            dt.Locale = CultureInfo.InvariantCulture;
            Action<DbCommand> commandAction =
                (DbCommand command2) =>
                {
                    using (DbDataAdapter adapter = _factory.CreateDataAdapter())
                    {
                        adapter.SelectCommand = command2;
                        adapter.Fill(dt);
                    }
                };
            OperateOnDbCommand(
                commandAction,
                command,
                parameters);
            return dt;
        }

        /// <summary>
        /// Gets the reader.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The data reader.</returns>
        public DbDataReader GetReader(
            DbCommand command,
            params DbParameter[] parameters)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            DbConnection connection = _factory.CreateConnection();
            connection.ConnectionString = _connectionString;
            command.Connection = connection;
            AddParameters(command, parameters);
            connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Gets the reader.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The data reader.</returns>
        [SuppressMessage(
            "Microsoft.Security",
            "CA2100:Review SQL queries for security vulnerabilities",
            Justification = "This is marked as obsolete.")]
        [Obsolete("Use overload that takes a DbCommand instead.")]
        public DbDataReader GetReader(
            string commandText,
            params DbParameter[] parameters)
        {
            DbConnection connection = _factory.CreateConnection();
            connection.ConnectionString = _connectionString;
            DbCommand command = _factory.CreateCommand();
            command.CommandText = commandText;
            command.Connection = connection;
            AddParameters(command, parameters);
            connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Modifies the data.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The number of affected rows.</returns>
        [Obsolete("Use overload with DbCommand instead.")]
        [SuppressMessage(
            "Microsoft.Security",
            "CA2100:Review SQL queries for security vulnerabilities",
            Justification = "This is marked as obsolete.")]
        public int ModifyData(
            string commandText,
            params DbParameter[] parameters)
        {
            int affectedRows = 0;
            var command = _factory.CreateCommand();
            command.CommandText = commandText;
            OperateOnDbCommand(
                (DbCommand command2) => affectedRows = command2.ExecuteNonQuery(),
                command,
                parameters);
            return affectedRows;
        }

        /// <summary>
        /// Modifies the data.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The number of affected rows.</returns>
        public int ModifyData(
            DbCommand command,
            params DbParameter[] parameters)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            int affectedRows = 0;
            OperateOnDbCommand(
                (DbCommand command2) => affectedRows = command2.ExecuteNonQuery(),
                command,
                parameters);
            return affectedRows;
        }

        /// <summary>
        /// Gets the int32.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        [Obsolete("Use overload that takes DbCommand instead.")]
        [SuppressMessage(
            "Microsoft.Security",
            "CA2100:Review SQL queries for security vulnerabilities",
            Justification = "This is marked as obsolete.")]
        public int? GetInt32(
            string commandText,
            params DbParameter[] parameters)
        {
            DbCommand command = _factory.CreateCommand();
            command.CommandText = commandText;
            return GetScalar<int?>(command, parameters);
        }

        /// <summary>
        /// Gets the int32.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public int? GetInt32(
            DbCommand command,
            params DbParameter[] parameters)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            return GetScalar<int?>(command, parameters);
        }

        /// <summary>
        /// Gets the scalar.
        /// </summary>
        /// <typeparam name="T">The type of the scalar.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        [Obsolete("Use overload that takes DbCommand instead.")]
        [SuppressMessage(
            "Microsoft.Security",
            "CA2100:Review SQL queries for security vulnerabilities",
            Justification = "This is marked as obsolete.")]
        public T GetScalar<T>(
            string commandText,
            params DbParameter[] parameters)
        {
            object databaseValue = null;
            DbCommand command = _factory.CreateCommand();
            command.CommandText = commandText;
            OperateOnDbCommand(
                (DbCommand command2) => databaseValue = command2.ExecuteScalar(),
                command,
                parameters);
            if (!(databaseValue is DBNull))
            {
                return (T)databaseValue;
            }

            return default(T);
        }

        /// <summary>
        /// Gets the scalar.
        /// </summary>
        /// <typeparam name="T">The type of the scalar.</typeparam>
        /// <param name="command">The command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public T GetScalar<T>(
            DbCommand command,
            params DbParameter[] parameters)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            object databaseValue = null;
            OperateOnDbCommand(
                (DbCommand command2) => databaseValue = command2.ExecuteScalar(),
                command,
                parameters);
            if (!(databaseValue is DBNull))
            {
                return (T)databaseValue;
            }

            return default(T);
        }

        /// <summary>
        /// Creates the command.
        /// </summary>
        /// <returns>The database command.</returns>
        protected DbCommand CreateCommand()
        {
            return _factory.CreateCommand();
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        protected static void AddParameter(DbCommand command, string name, object value)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            DbParameter parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value != null ? value : DBNull.Value;
            command.Parameters.Add(parameter);
        }

        private static void AddParameters(
            DbCommand command,
            DbParameter[] parameters)
        {
            if (parameters == null)
            {
                return;
            }

            foreach (DbParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }
        }

        private void OperateOnDbCommand(
            Action<DbCommand> commandAction,
            DbCommand command,
            params DbParameter[] parameters)
        {
            using (DbConnection connection = _factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                command.Connection = connection;
                AddParameters(command, parameters);
                connection.Open();
                commandAction(command);
            }
        }
    }
}