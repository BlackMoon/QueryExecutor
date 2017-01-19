using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using DryIoc;
using Serilog;

namespace queryExecutor.Service
{
    /// <summary>
    /// Фабрика сервисов
    /// </summary>
    public class BehaviorFactory : ServiceHostFactory
    {
        /// <summary>
        /// Creates a <see cref="BehaviorHost"/> for a specified type of service with a specific base address. 
        /// </summary>
        /// <returns>
        /// A <see cref="BehaviorHost"/> for the type of service specified with a specific base address.
        /// </returns>
        /// <param name="serviceType">
        /// Specifies the type of service to host. 
        /// </param>
        /// <param name="baseAddresses">
        /// The <see cref="T:System.Array"/> of type <see cref="T:System.Uri"/> that contains the base addresses for the service hosted.
        /// </param>
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            //Register the service as a type so it can be found from the instance provider
            Startup.Container.Register(serviceType);

            try
            {
                return new BehaviorHost(serviceType, baseAddresses);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                throw;
            }
        }
    }
}