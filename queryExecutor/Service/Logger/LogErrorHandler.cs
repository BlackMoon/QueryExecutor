using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Serilog;

namespace queryExecutor.Service.Logger
{
    /// <summary>
    /// Сбор ошибок
    /// </summary>
    public class LogErrorHandler : IErrorHandler
    {
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
           
        }

        public bool HandleError(Exception error)
        {
            Log.Error(error, string.Empty);
            return false; 
        }
    }
}