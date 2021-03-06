﻿namespace Uncas.Core.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    /// <summary>
    /// Represents a general db context.
    /// </summary>
    public abstract class DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbContext"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="connectionString">The connection string.</param>
        protected DbContext(
            DbProviderFactory factory,
            string connectionString)
        {
            Factory = factory;
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        protected string ConnectionString { get; private set; }

        /// <summary>
        /// Gets the factory.
        /// </summary>
        /// <value>The database provider factory.</value>
        protected DbProviderFactory Factory { get; private set; }

        /// <summary>
        /// Gets the data table for the given command text.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The data table.</returns>
        [Obsolete("Use overload that takes DbCommand instead.")]
        [SuppressMessage(
            "Microsoft.Reliability",
            "CA2000:Dispose objects before losing scope",
            Justification = "Is the return value...")]
        [SuppressMessage(
            "Microsoft.Security",
            "CA2100:Review SQL queries for security vulnerabilities",
            Justification = "This is marked as obsolete.")]
        public DataTable GetData(
            string commandText,
            params DbParameter[] parameters)
        {
            var dt = new DataTable("uncas-data") { Locale = CultureInfo.InvariantCulture };
            Action<DbCommand> commandAction =
                command =>
                    {
                        using (DbDataAdapter adapter = Factory.CreateDataAdapter())
                        {
                            adapter.SelectCommand = command;
                            adapter.Fill(dt);
                        }
                    };
            DbCommand command2 = Factory.CreateCommand();
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
        [SuppressMessage(
            "Microsoft.Reliability",
            "CA2000:Dispose objects before losing scope",
            Justification = "Is the return value...")]
        public DataTable GetData(
            DbCommand command,
            params DbParameter[] parameters)
        {
            var dt = new DataTable("uncas-data") { Locale = CultureInfo.InvariantCulture };
            Action<DbCommand> commandAction =
                command2 =>
                    {
                        using (DbDataAdapter adapter = Factory.CreateDataAdapter())
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
        /// Gets the int32.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The integer value.</returns>
        [Obsolete("Use overload that takes DbCommand instead.")]
        [SuppressMessage(
            "Microsoft.Security",
            "CA2100:Review SQL queries for security vulnerabilities",
            Justification = "This is marked as obsolete.")]
        public int? GetInt32(
            string commandText,
            params DbParameter[] parameters)
        {
            DbCommand command = Factory.CreateCommand();
            command.CommandText = commandText;
            return GetScalar<int?>(command, parameters);
        }

        /// <summary>
        /// Gets the int32.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The integer value.</returns>
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
        /// Gets the objects.
        /// </summary>
        /// <typeparam name="T">The type of the objects.</typeparam>
        /// <param name="command">The command.</param>
        /// <param name="conversion">The conversion.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A list of objects.</returns>
        public IEnumerable<T> GetObjects<T>(
            DbCommand command,
            Func<DbDataReader, T> conversion,
            params DbParameter[] parameters)
        {
            if (conversion == null)
            {
                throw new ArgumentNullException("conversion");
            }

            var result = new List<T>();
            using (DbDataReader reader = GetReader(command, parameters))
            {
                while (reader.Read())
                {
                    result.Add(conversion(reader));
                }
            }

            return result;
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

            DbConnection connection = Factory.CreateConnection();
            connection.ConnectionString = ConnectionString;
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
            DbConnection connection = Factory.CreateConnection();
            connection.ConnectionString = ConnectionString;
            DbCommand command = Factory.CreateCommand();
            command.CommandText = commandText;
            command.Connection = connection;
            AddParameters(command, parameters);
            connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Gets the scalar.
        /// </summary>
        /// <typeparam name="T">The type of the scalar.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The scalar value.</returns>
        [Obsolete("Use overload that takes DbCommand instead.")]
        [SuppressMessage(
            "Microsoft.Security",
            "CA2100:Review SQL queries for security vulnerabilities",
            Justification = "This is marked as obsolete.")]
        public T GetScalar<T>(
            string commandText,
            params DbParameter[] parameters)
        {
            using (DbCommand command = Factory.CreateCommand())
            {
                command.CommandText = commandText;
                return GetScalar<T>(command, parameters);
            }
        }

        /// <summary>
        /// Gets the scalar.
        /// </summary>
        /// <typeparam name="T">The type of the scalar.</typeparam>
        /// <param name="command">The command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The scalar value.</returns>
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
                command2 => databaseValue = command2.ExecuteScalar(),
                command,
                parameters);
            if (!(databaseValue is DBNull))
            {
                return (T)databaseValue;
            }

            return default(T);
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
            DbCommand command = Factory.CreateCommand();
            command.CommandText = commandText;
            OperateOnDbCommand(
                command2 => affectedRows = command2.ExecuteNonQuery(),
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
                command2 => affectedRows = command2.ExecuteNonQuery(),
                command,
                parameters);
            return affectedRows;
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="command">The command to add the parameter for.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        protected static void AddParameter(
            DbCommand command,
            string name,
            object value)
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
            parameter.Value = value ?? DBNull.Value;
            command.Parameters.Add(parameter);
        }

        /// <summary>
        /// Gets the date.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>The date time value.</returns>
        protected static DateTime GetDate(
            DbDataReader reader,
            string fieldName)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            object databaseValue = reader[fieldName];
            if (databaseValue is DBNull)
            {
                throw new InvalidOperationException(
                    "Database contains inconsistent data");
            }

            return (DateTime)databaseValue;
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>The string.</returns>
        protected static string GetString(
            DbDataReader reader,
            string fieldName)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            object databaseValue = reader[fieldName];
            if (databaseValue is DBNull)
            {
                return null;
            }

            return (string)databaseValue;
        }

        /// <summary>
        /// Creates the command.
        /// </summary>
        /// <returns>The database command.</returns>
        protected DbCommand CreateCommand()
        {
            return Factory.CreateCommand();
        }

        private static void AddParameters(
            DbCommand command,
            IEnumerable<DbParameter> parameters)
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
            using (DbConnection connection = Factory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                command.Connection = connection;
                AddParameters(command, parameters);
                connection.Open();
                commandAction(command);
            }
        }
    }
}