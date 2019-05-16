﻿using System.ServiceModel;
using GTLService.DataManagement.IDataManagement;
using GTLService.Controller.IController;

namespace GTLService.Controller
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class LoginService : ILoginService
    {
        private readonly ILoginDm _loginDm;
        public LoginService(ILoginDm loginDm)
        {
            _loginDm = loginDm;
        }
        
        public bool Login(int ssn, string password)
        {
            return _loginDm.Login(ssn, password);
        }
    }
}

//new file?
//public class LoginDmFactory
//{
//    public ILoginDm Get(int id)
//    {
//        switch (id)
//        {
//            case 0:
//                return new LoginDmDatabase(new LoginDaDatabase(new GTLEntities()));
//            case 1:
//                return new LoginDmCode(new LoginDa_Code(new GTLEntities()));;
//            default:
//                return null;            
//        }
//    }
//}