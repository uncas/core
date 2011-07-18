namespace Uncas.Core.Tests.Logging
{
    using System.Data.Common;
    using System.Data.SQLite;
    using Uncas.Core.Logging;

    public class SQLiteLogDbContext : ILogDbContext
    {
        public string ConnectionString
        {
            get { return "Data Source=Test.db;Version=3;"; }
        }

        public DbProviderFactory Factory
        {
            get { return SQLiteFactory.Instance; }
        }
    }
}
