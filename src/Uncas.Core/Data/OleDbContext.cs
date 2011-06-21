using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Globalization;

namespace Uncas.Core.Data
{
    public class OleDbContext
    {
        private readonly string _connectionString;

        private readonly DbProviderFactory _factory;

        public OleDbContext(string connectionString)
        {
            _factory = OleDbFactory.Instance;
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
            using (DbConnection connection =_factory.CreateConnection())
            {
                connection.ConnectionString = _connectionString;
                using (DbCommand command =_factory.CreateCommand())
                {
                    command.CommandText= commandText;
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

        public static OleDbParameter GetInt32Parameter(string name, int? value)
        {
            OleDbParameter par = new OleDbParameter(name, OleDbType.Integer);
            if (value.HasValue)
                par.Value = value;
            else
                par.Value = DBNull.Value;
            return par;
        }

        public static OleDbParameter GetInt64Parameter(string name, long? value)
        {
            OleDbParameter par = new OleDbParameter(name, OleDbType.BigInt);
            if (value.HasValue)
                par.Value = value;
            else
                par.Value = DBNull.Value;
            return par;
        }

        public static OleDbParameter GetDateTimeParameter(string name, DateTime? value)
        {
            OleDbParameter par = new OleDbParameter(name, OleDbType.Date);
            if (value.HasValue)
                par.Value = value.Value;
            else
                par.Value = DBNull.Value;
            return par;
        }

        public static OleDbParameter GetBooleanParameter(string name, bool value)
        {
            OleDbParameter par = new OleDbParameter(name, OleDbType.Boolean);
            par.Value = value;
            return par;
        }

        public static OleDbParameter GetDecimalParameter(string name,
            decimal? value, byte precision, byte scale)
        {
            OleDbParameter par = new OleDbParameter(name, OleDbType.Decimal);
            par.Precision = precision;
            par.Scale = scale;
            if (value.HasValue)
                par.Value = value.Value;
            else
                par.Value = DBNull.Value;
            return par;
        }

        public static OleDbParameter GetSingleParameter(string name, Single? value)
        {
            OleDbParameter par = new OleDbParameter(name, OleDbType.Single);
            if (value.HasValue)
                par.Value = value;
            else
                par.Value = DBNull.Value;
            return par;
        }

        public static OleDbParameter GetStringParameter(string name,
            string value)
        {
            return GetStringParameter(name, value, 50);
        }

        public static OleDbParameter GetStringParameter(string name,
            string value, int size)
        {
            OleDbParameter par = new OleDbParameter(name, OleDbType.VarChar);
            par.Size = size;
            if (!string.IsNullOrEmpty(value))
                par.Value = value;
            else
                par.Value = DBNull.Value;
            return par;
        }

        public static OleDbParameter GetNoteParameter(string name,
            string value)
        {
            OleDbParameter par = new OleDbParameter(name, OleDbType.LongVarChar);
            if (!string.IsNullOrEmpty(value))
                par.Value = value;
            else
                par.Value = DBNull.Value;
            return par;
        }
    }
}