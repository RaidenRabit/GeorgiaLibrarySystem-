using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Core;
using GtlService.DataAccess.Database;
using GtlService.DataAccess.IDataAccess;
using GtlService.DataManagement.Database;
using GtlService.DataManagement.iDataManagement;
using GTLService.Controller;
using GTLService.Controller.IController;
using GTLService.DataAccess.Database;
using GTLService.DataAccess.IDataAccess;
using GTLService.DataManagement.Database;
using GTLService.DataManagement.IDataManagement;

namespace GTLService
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //used: http://scotthannen.org/blog/2016/04/13/wcf-dependency-injection-in-5-minutes.html
            container.Register(
                Component.For<ILoginService, LoginService>(),
                Component.For<ILoginDm, LoginDm_Database>(),
                Component.For<ILoginDa, LoginDa_Database>(),
                Component.For<IMaterialService, MaterialService>(),
                Component.For<IMaterialsDm, MaterialsDm_Database>(),
                Component.For<IMaterialsDa, MaterialsDa_Database>(),
                Component.For<Context, Context>());
        }
    }
}

/*
 * using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GtlService.DataAccess.Database;
using GtlService.DataAccess.IDataAccess;
using GtlService.DataManagement.Database;
using GtlService.DataManagement.iDataManagement;
using GTLService.Controller.IController;
using Unity;
using Unity.Wcf;

namespace GTLService
{
    public class WcfServiceFactory: UnityServiceHostFactory
    {
        protected override void ConfigureContainer(IUnityContainer container)
        {
            container.RegisterType<ILoginDm,LoginDm_Database>();
            container.RegisterType<ILoginDa,LoginDa_Database>();
            container.RegisterType<ILoginService,ILoginService>();
        }
    }
}
 */