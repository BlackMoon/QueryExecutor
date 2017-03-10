using System.Collections.Generic;
using DryIoc;

namespace queryExecutor.CQRS.Job
{
    public class JobDispatcher : IJobDispatcher
    {
        private readonly IContainer _container;

        public JobDispatcher(IContainer container)
        {
            _container = container;
        }

        public void Dispatch<TParameter>() where TParameter : IJob
        {
            IEnumerable<TParameter> jobs = _container.ResolveMany<TParameter>();

            foreach (TParameter j in jobs)
            {
                j.Run();
            }
        }
    }
}