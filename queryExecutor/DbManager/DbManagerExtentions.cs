using System.Data.Entity;

namespace queryExecutor.DbManager
{
    public static class DbManagerExtentions
    {
        public static T Cast<T> (this DbContext dbContext) where T : DbContext
        {
            return dbContext as T;
        }
    }
}