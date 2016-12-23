namespace queryExecutor.CQRS.Query
{
    /// <summary>
    /// Base interface for query handlers
    /// </summary>
    /// <typeparam name="TParameter">Query type</typeparam>
    /// <typeparam name="TResult">Query Result type</typeparam>
    public interface IQueryHandler<in TParameter, out TResult> where TResult : IQueryResult where TParameter : IQuery
    {
        /// <summary>
        /// Execute a query result from a query
        /// </summary>
        /// <param name="query">Query</param>
        /// <returns>Execute Query Result</returns>
        TResult Execute(TParameter query);
    }
}
