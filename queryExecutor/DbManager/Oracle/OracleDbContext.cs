using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using EntityFramework.Functions;
using queryExecutor.Domain.DscQColumn;
using queryExecutor.Domain.DscQueryParameter;
using queryExecutor.Domain.TdfFlexField;

namespace queryExecutor.DbManager.Oracle
{

    [DbConfigurationType(typeof(OracleDbConfiguration))]

    public class OracleDbContext : DbContext
    {
        static OracleDbContext()
        {
            Database.SetInitializer<OracleDbContext>(null);
        }

        public DbSet<DscQColumn> DscQColumns { get; set; }

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

            modelBuilder.AddFunctions<OracleDbContext>();
        }

        [ComposableScalarValuedFunction("QUERY_FIND", Schema = "DSC$UTILS")]
        [return: Parameter(DbType = "number")]
        public decimal DscUtils_QueryFind([Parameter(DbType = "varchar2")] string queryPath) => Function.CallNotSupported<decimal>();
    }
}