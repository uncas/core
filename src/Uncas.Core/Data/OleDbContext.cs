using System;
using System.Data.OleDb;

namespace Uncas.Core.Data
{
    public class OleDbContext : DbContext
    {
        public OleDbContext(string connectionString)
            : base(OleDbFactory.Instance, connectionString)
        {
        }

        public static OleDbParameter GetInt32Parameter(string name, int? value)
        {
            OleDbParameter par = new OleDbParameter(name, OleDbType.Integer);
            if (value.HasValue)
            {
                par.Value = value;
            }
            else
            {
                par.Value = DBNull.Value;
            }

            return par;
        }

        public static OleDbParameter GetInt64Parameter(string name, long? value)
        {
            OleDbParameter par = new OleDbParameter(name, OleDbType.BigInt);
            if (value.HasValue)
            {
                par.Value = value;
            }
            else
            {
                par.Value = DBNull.Value;
            }

            return par;
        }

        public static OleDbParameter GetDateTimeParameter(string name, DateTime? value)
        {
            OleDbParameter par = new OleDbParameter(name, OleDbType.Date);
            if (value.HasValue)
            {
                par.Value = value.Value;
            }
            else
            {
                par.Value = DBNull.Value;
            }

            return par;
        }

        public static OleDbParameter GetBooleanParameter(string name, bool value)
        {
            OleDbParameter par = new OleDbParameter(name, OleDbType.Boolean);
            par.Value = value;
            return par;
        }

        public static OleDbParameter GetDecimalParameter(
            string name,
            decimal? value,
            byte precision,
            byte scale)
        {
            OleDbParameter par = new OleDbParameter(name, OleDbType.Decimal);
            par.Precision = precision;
            par.Scale = scale;
            if (value.HasValue)
            {
                par.Value = value.Value;
            }
            else
            {
                par.Value = DBNull.Value;
            }

            return par;
        }

        public static OleDbParameter GetSingleParameter(
            string name,
            float? value)
        {
            OleDbParameter par = new OleDbParameter(name, OleDbType.Single);
            if (value.HasValue)
            {
                par.Value = value;
            }
            else
            {
                par.Value = DBNull.Value;
            }

            return par;
        }

        public static OleDbParameter GetStringParameter(
            string name,
            string value)
        {
            return GetStringParameter(name, value, 50);
        }

        public static OleDbParameter GetStringParameter(
            string name,
            string value,
            int size)
        {
            OleDbParameter par = new OleDbParameter(name, OleDbType.VarChar);
            par.Size = size;
            if (!string.IsNullOrEmpty(value))
            {
                par.Value = value;
            }
            else
            {
                par.Value = DBNull.Value;
            }

            return par;
        }

        public static OleDbParameter GetNoteParameter(
            string name,
            string value)
        {
            OleDbParameter par = new OleDbParameter(name, OleDbType.LongVarChar);
            if (!string.IsNullOrEmpty(value))
            {
                par.Value = value;
            }
            else
            {
                par.Value = DBNull.Value;
            }

            return par;
        }
    }
}