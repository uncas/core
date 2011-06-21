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
            params OleDbParameter[] parameters)
        {
            DataTable dt = new DataTable("uncas-data");
            dt.Locale = CultureInfo.InvariantCulture;
            using (OleDbConnection connection = new OleDbConnection(_connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(commandText, connection))
                {
                    foreach (OleDbParameter parameter in parameters)
                        command.Parameters.Add(parameter);
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
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
            params OleDbParameter[] parameters)
        {
            OleDbConnection connection = new OleDbConnection(_connectionString);
            OleDbCommand command = new OleDbCommand(commandText, connection);
            foreach (OleDbParameter parameter in parameters)
                command.Parameters.Add(parameter);
            connection.Open();
            OleDbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }

        public int ModifyData(string commandText,
            params OleDbParameter[] parameters)
        {
            int iOut = 0;
            using (OleDbConnection connection = new OleDbConnection(_connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(commandText, connection))
                {
                    foreach (OleDbParameter parameter in parameters)
                        command.Parameters.Add(parameter);
                    connection.Open();
                    iOut = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return iOut;
        }

        public int? GetInt32(string commandText,
            params OleDbParameter[] parameters)
        {
            int? iOut = null;
            using (OleDbConnection connection = new OleDbConnection(_connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(commandText, connection))
                {
                    foreach (OleDbParameter parameter in parameters)
                        command.Parameters.Add(parameter);
                    connection.Open();
                    object o = command.ExecuteScalar();
                    if (!(o is DBNull))
                        iOut = (int)o;
                    connection.Close();
                }
            }
            return iOut;
        }
    }
}