using System;
using DryIoc;

namespace queryExecutor.CQRS.Query
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IContainer _container;

        public QueryDispatcher(IContainer container)
        {
            _container = container;
        }

        public TResult Dispatch<TParameter, TResult>(TParameter query) where TParameter : IQuery where TResult : IQueryResult
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var handler = _container.Resolve<IQueryHandler<TParameter, TResult>>();
            return handler.Execute(query);
        }
    }
}