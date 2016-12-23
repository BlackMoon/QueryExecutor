using System.Data;
using System.Data.Common;
using System.Data.Entity;
using queryExecutor.Domain.DscQueryParameter;

namespace queryExecutor.DbManager.Oracle
{
    [DbConfigurationType(typeof(OracleDbConfiguration))]

    public class OracleDbContext : DbContext
    {
        static OracleDbContext()
        {
            Database.SetInitializer<OracleDbContext>(null);
        }

        public DbSet<DscQParameter> DscQParameters { get; set; }

        public OracleDbContext(IDbConnection existingConnection, bool contextOwnsConnection)
            : base((DbConnection) existingConnection, contextOwnsConnection)
        {
#if DEBUG
            Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
#endif
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(string.Empty);
        }

    }
}