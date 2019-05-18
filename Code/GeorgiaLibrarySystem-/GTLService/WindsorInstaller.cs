using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Core;
using GTLService.Controller;
using GTLService.Controller.IController;
using GTLService.DataAccess.Database;
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
                Component.For<ILoginService, LoginService>().LifeStyle.Transient,
                Component.For<ILoginDm, LoginDm_Database>().LifeStyle.Transient,
                Component.For<LoginDa_Database, LoginDa_Database>().LifeStyle.Transient,

                Component.For<ILendingService, LendingService>().LifeStyle.Transient,
                Component.For<ILendingDm, LendingDm_Database>().LifeStyle.Transient,
                Component.For<LendingDa_Database, LendingDa_Database>().LifeStyle.Transient,

                Component.For<Context,Context>().LifeStyle.Transient);


        }
    }
}