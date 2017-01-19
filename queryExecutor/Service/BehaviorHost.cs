using System;
using System.ServiceModel;
using queryExecutor.Service.DI;
using queryExecutor.Service.Logger;

namespace queryExecutor.Service
{
    /// <summary>
    /// Доп. поведения
    /// </summary>
    public class BehaviorHost : ServiceHost
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BehaviorHost"/> class.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="baseAddresses">The base addresses.</param>
        public BehaviorHost(Type serviceType, Uri[] baseAddresses) : base(serviceType, baseAddresses)
        {
            
        }

        /// <summary>
        /// Opens the channel dispatchers.
        /// </summary>
        /// <param name="timeout">The <see cref="T:System.Timespan"/> that specifies how long the on-open operation has to complete before timing out.</param>
        protected override void OnOpen(TimeSpan timeout)
        {
            Description.Behaviors.Add(new DiServiceBehavior());
            Description.Behaviors.Add(new LogErrorBehavior());
            base.OnOpen(timeout);
        }
    }
}