using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GtlService.Controller.iController;
using GtlService.DataAccess;
using GtlService.DataManagement;
using GtlService.DataManagement.iDataManagement;
using GtlService.Model;

namespace GtlService.Controller
{
    public class LoginController : ILoginController
    {
        //dependency injection!
        private ILoginDM loginDM = new LoginDM(new LoginDA(new GTLEntities()));
        //public LoginController(LoginDM loginDm)
        //{
        //    loginDM = loginDm;
        //}

        public bool Login(int ssn, string password)
        {
            return loginDM.Login(ssn, password);
        }
    }
}
