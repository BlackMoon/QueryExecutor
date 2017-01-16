using System;
using System.ServiceModel;

namespace queryExecutor.Service.Factory
{
    public class DiServiceHost : ServiceHost
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiServiceHost"/> class.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="baseAddresses">The base addresses.</param>
        public DiServiceHost(Type serviceType, Uri[] baseAddresses) : base(serviceType, baseAddresses)
        {
            
        }

        /// <summary>
        /// Opens the channel dispatchers.
        /// </summary>
        /// <param name="timeout">The <see cref="T:System.Timespan"/> that specifies how long the on-open operation has to complete before timing out.</param>
        protected override void OnOpen(TimeSpan timeout)
        {
            Description.Behaviors.Add(new DiServiceBehavior());
            base.OnOpen(timeout);
        }
    }
}