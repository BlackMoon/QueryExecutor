﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace queryExecutor.Tests.Utils {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://web.aquilon.ru", ConfigurationName="Utils.IUtils")]
    public interface IUtils {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://web.aquilon.ru/IUtils/GetColumns", ReplyAction="http://web.aquilon.ru/IUtils/GetColumnsResponse")]
        queryExecutor.Domain.DscQColumn.DscQColumn[] GetColumns(queryExecutor.Domain.DscQColumn.Query.DscQColumnQuery query);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://web.aquilon.ru/IUtils/GetColumns", ReplyAction="http://web.aquilon.ru/IUtils/GetColumnsResponse")]
        System.Threading.Tasks.Task<queryExecutor.Domain.DscQColumn.DscQColumn[]> GetColumnsAsync(queryExecutor.Domain.DscQColumn.Query.DscQColumnQuery query);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://web.aquilon.ru/IUtils/GetParameters", ReplyAction="http://web.aquilon.ru/IUtils/GetParametersResponse")]
        queryExecutor.Domain.DscQueryParameter.DscQParameter[] GetParameters(queryExecutor.Domain.DscQueryParameter.Query.DscQParameterQuery query);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://web.aquilon.ru/IUtils/GetParameters", ReplyAction="http://web.aquilon.ru/IUtils/GetParametersResponse")]
        System.Threading.Tasks.Task<queryExecutor.Domain.DscQueryParameter.DscQParameter[]> GetParametersAsync(queryExecutor.Domain.DscQueryParameter.Query.DscQParameterQuery query);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://web.aquilon.ru/IUtils/GetResults", ReplyAction="http://web.aquilon.ru/IUtils/GetResultsResponse")]
        queryExecutor.Domain.DscQueryData.DscQData[] GetResults(queryExecutor.Domain.DscQueryData.Query.DscQDataQuery query);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://web.aquilon.ru/IUtils/GetResults", ReplyAction="http://web.aquilon.ru/IUtils/GetResultsResponse")]
        System.Threading.Tasks.Task<queryExecutor.Domain.DscQueryData.DscQData[]> GetResultsAsync(queryExecutor.Domain.DscQueryData.Query.DscQDataQuery query);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IUtilsChannel : queryExecutor.Tests.Utils.IUtils, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class UtilsClient : System.ServiceModel.ClientBase<queryExecutor.Tests.Utils.IUtils>, queryExecutor.Tests.Utils.IUtils {
        
        public UtilsClient() {
        }
        
        public UtilsClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public UtilsClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UtilsClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UtilsClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public queryExecutor.Domain.DscQColumn.DscQColumn[] GetColumns(queryExecutor.Domain.DscQColumn.Query.DscQColumnQuery query) {
            return base.Channel.GetColumns(query);
        }
        
        public System.Threading.Tasks.Task<queryExecutor.Domain.DscQColumn.DscQColumn[]> GetColumnsAsync(queryExecutor.Domain.DscQColumn.Query.DscQColumnQuery query) {
            return base.Channel.GetColumnsAsync(query);
        }
        
        public queryExecutor.Domain.DscQueryParameter.DscQParameter[] GetParameters(queryExecutor.Domain.DscQueryParameter.Query.DscQParameterQuery query) {
            return base.Channel.GetParameters(query);
        }
        
        public System.Threading.Tasks.Task<queryExecutor.Domain.DscQueryParameter.DscQParameter[]> GetParametersAsync(queryExecutor.Domain.DscQueryParameter.Query.DscQParameterQuery query) {
            return base.Channel.GetParametersAsync(query);
        }
        
        public queryExecutor.Domain.DscQueryData.DscQData[] GetResults(queryExecutor.Domain.DscQueryData.Query.DscQDataQuery query) {
            return base.Channel.GetResults(query);
        }
        
        public System.Threading.Tasks.Task<queryExecutor.Domain.DscQueryData.DscQData[]> GetResultsAsync(queryExecutor.Domain.DscQueryData.Query.DscQDataQuery query) {
            return base.Channel.GetResultsAsync(query);
        }
    }
}
