namespace Uncas.Core.Data
{
    using System;
    using System.Data.OleDb;

    /// <summary>
    /// Represents a database context for Ole Db access.
    /// </summary>
    public class OleDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OleDbContext"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public OleDbContext(string connectionString)
            : base(OleDbFactory.Instance, connectionString)
        {
        }

        /// <summary>
        /// Gets the boolean parameter.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">If set to <c>true</c> [value].</param>
        /// <returns>The boolean parameter.</returns>
        public static OleDbParameter GetBooleanParameter(string name, bool value)
        {
            var par = new OleDbParameter(name, OleDbType.Boolean);
            par.Value = value;
            return par;
        }

        /// <summary>
        /// Gets the date time parameter.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>The date time parameter.</returns>
        public static OleDbParameter GetDateTimeParameter(string name, DateTime? value)
        {
            var par = new OleDbParameter(name, OleDbType.Date);
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

        /// <summary>
        /// Gets the decimal parameter.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="precision">The precision.</param>
        /// <param name="scale">The scale of the parameter.</param>
        /// <returns>The decimal parameter.</returns>
        public static OleDbParameter GetDecimalParameter(
            string name,
            decimal? value,
            byte precision,
            byte scale)
        {
            var par = new OleDbParameter(name, OleDbType.Decimal);
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

        /// <summary>
        /// Gets the int32 parameter.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>The integer parameter.</returns>
        public static OleDbParameter GetInt32Parameter(string name, int? value)
        {
            var par = new OleDbParameter(name, OleDbType.Integer);
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

        /// <summary>
        /// Gets the int64 parameter.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>The 64 bit integer parameter.</returns>
        public static OleDbParameter GetInt64Parameter(string name, long? value)
        {
            var par = new OleDbParameter(name, OleDbType.BigInt);
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

        /// <summary>
        /// Gets the note parameter.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>The note parameter.</returns>
        public static OleDbParameter GetNoteParameter(
            string name,
            string value)
        {
            var par = new OleDbParameter(name, OleDbType.LongVarChar);
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

        /// <summary>
        /// Gets the single parameter.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>The single precision parameter.</returns>
        public static OleDbParameter GetSingleParameter(
            string name,
            float? value)
        {
            var par = new OleDbParameter(name, OleDbType.Single);
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

        /// <summary>
        /// Gets the string parameter.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>The string parameter.</returns>
        public static OleDbParameter GetStringParameter(
            string name,
            string value)
        {
            return GetStringParameter(name, value, 50);
        }

        /// <summary>
        /// Gets the string parameter.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="size">The size of the parameter.</param>
        /// <returns>The string parameter.</returns>
        public static OleDbParameter GetStringParameter(
            string name,
            string value,
            int size)
        {
            var par = new OleDbParameter(name, OleDbType.VarChar);
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
    }
}