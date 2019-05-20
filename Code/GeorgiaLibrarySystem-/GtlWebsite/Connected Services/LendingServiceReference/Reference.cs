﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GtlWebsite.LendingServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="LendingServiceReference.ILendingService")]
    public interface ILendingService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILendingService/LendBook", ReplyAction="http://tempuri.org/ILendingService/LendBookResponse")]
        bool LendBook(int ssn, int copyId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILendingService/LendBook", ReplyAction="http://tempuri.org/ILendingService/LendBookResponse")]
        System.Threading.Tasks.Task<bool> LendBookAsync(int ssn, int copyId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILendingService/ReturnBook", ReplyAction="http://tempuri.org/ILendingService/ReturnBookResponse")]
        bool ReturnBook(int copyId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILendingService/ReturnBook", ReplyAction="http://tempuri.org/ILendingService/ReturnBookResponse")]
        System.Threading.Tasks.Task<bool> ReturnBookAsync(int copyId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ILendingServiceChannel : GtlWebsite.LendingServiceReference.ILendingService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LendingServiceClient : System.ServiceModel.ClientBase<GtlWebsite.LendingServiceReference.ILendingService>, GtlWebsite.LendingServiceReference.ILendingService {
        
        public LendingServiceClient() {
        }
        
        public LendingServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public LendingServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LendingServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LendingServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool LendBook(int ssn, int copyId) {
            return base.Channel.LendBook(ssn, copyId);
        }
        
        public System.Threading.Tasks.Task<bool> LendBookAsync(int ssn, int copyId) {
            return base.Channel.LendBookAsync(ssn, copyId);
        }
        
        public bool ReturnBook(int copyId) {
            return base.Channel.ReturnBook(copyId);
        }
        
        public System.Threading.Tasks.Task<bool> ReturnBookAsync(int copyId) {
            return base.Channel.ReturnBookAsync(copyId);
        }
    }
}