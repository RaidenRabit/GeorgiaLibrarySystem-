﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GtlWebsite.LoaningServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="LoaningServiceReference.ILoaningService")]
    public interface ILoaningService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILoaningService/LoanBook", ReplyAction="http://tempuri.org/ILoaningService/LoanBookResponse")]
        bool LoanBook(int ssn, int copyId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILoaningService/LoanBook", ReplyAction="http://tempuri.org/ILoaningService/LoanBookResponse")]
        System.Threading.Tasks.Task<bool> LoanBookAsync(int ssn, int copyId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILoaningService/ReturnBook", ReplyAction="http://tempuri.org/ILoaningService/ReturnBookResponse")]
        bool ReturnBook(int copyId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILoaningService/ReturnBook", ReplyAction="http://tempuri.org/ILoaningService/ReturnBookResponse")]
        System.Threading.Tasks.Task<bool> ReturnBookAsync(int copyId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ILoaningServiceChannel : GtlWebsite.LoaningServiceReference.ILoaningService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LoaningServiceClient : System.ServiceModel.ClientBase<GtlWebsite.LoaningServiceReference.ILoaningService>, GtlWebsite.LoaningServiceReference.ILoaningService {
        
        public LoaningServiceClient() {
        }
        
        public LoaningServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public LoaningServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LoaningServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LoaningServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool LoanBook(int ssn, int copyId) {
            return base.Channel.LoanBook(ssn, copyId);
        }
        
        public System.Threading.Tasks.Task<bool> LoanBookAsync(int ssn, int copyId) {
            return base.Channel.LoanBookAsync(ssn, copyId);
        }
        
        public bool ReturnBook(int copyId) {
            return base.Channel.ReturnBook(copyId);
        }
        
        public System.Threading.Tasks.Task<bool> ReturnBookAsync(int copyId) {
            return base.Channel.ReturnBookAsync(copyId);
        }
    }
}
