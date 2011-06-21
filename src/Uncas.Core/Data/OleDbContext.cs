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

        public OleDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable GetData(string commandText,
            params DbParameter[] parameters)
        {
            DataTable dt = new DataTable("uncas-data");
            dt.Locale = CultureInfo.InvariantCulture;
            using (OleDbConnection connection = new OleDbConnection(_connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(commandText, connection))
                {
                    foreach (DbParameter parameter in parameters)
                        command.Parameters.Add(parameter);
                    using (DbDataAdapter adapter = new OleDbDataAdapter(command))
                    {
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
            OleDbConnection connection = new OleDbConnection(_connectionString);
            DbCommand command = new OleDbCommand(commandText, connection);
            foreach (DbParameter parameter in parameters)
                command.Parameters.Add(parameter);
            connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public int ModifyData(string commandText,
            params DbParameter[] parameters)
        {
            int iOut = 0;
            using (OleDbConnection connection = new OleDbConnection(_connectionString))
            {
                using (DbCommand command = new OleDbCommand(commandText, connection))
                {
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
            using (OleDbConnection connection =
                new OleDbConnection(_connectionString))
            {
                using (DbCommand command =
                    new OleDbCommand(commandText, connection))
                {
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