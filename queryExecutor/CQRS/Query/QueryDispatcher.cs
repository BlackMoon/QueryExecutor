using System;
using Microsoft.Extensions.DependencyInjection;

namespace queryExecutor.CQRS.Query
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TResult Dispatch<TParameter, TResult>(TParameter query) where TParameter : IQuery where TResult : IQueryResult
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            
            var handler = _serviceProvider.GetRequiredService<IQueryHandler<TParameter, TResult>>();
            return handler.Execute(query);
        }
    }
}