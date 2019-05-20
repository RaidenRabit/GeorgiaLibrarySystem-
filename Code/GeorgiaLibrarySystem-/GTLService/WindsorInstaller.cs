using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Core;
using GTLService.Controller;
using GTLService.Controller.IController;
using GTLService.DataAccess.Code;
using GTLService.DataAccess.Database;
using GTLService.DataManagement.Code;
using GTLService.DataManagement.IDataManagement;
using GTLService.DataManagement.Database;

namespace GTLService
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            string approach = "Database";
            //used: http://scotthannen.org/blog/2016/04/13/wcf-dependency-injection-in-5-minutes.html
            container.Register(
                Component.For<IMaterialService, MaterialService>().LifeStyle.Transient,
                Component.For<ICopyService, CopyService>().LifeStyle.Transient,
                Component.For<ILoginService, LoginService>().LifeStyle.Transient,
                Component.For<ILendingService, LendingService>().LifeStyle.Transient,

                Component.For<MaterialDa_Code, MaterialDa_Code>().LifeStyle.Transient,
                Component.For<PersonDa_Code, PersonDa_Code>().LifeStyle.Transient,
                Component.For<MemberDa_Code, MemberDa_Code>().LifeStyle.Transient,
                Component.For<LibraryDa_Code, LibraryDa_Code>().LifeStyle.Transient,
                Component.For<LendingDa_Code, LendingDa_Code>().LifeStyle.Transient,
                Component.For<CopyDa_Code, CopyDa_Code>().LifeStyle.Transient,
                Component.For<LoginDa_Code, LoginDa_Code>().LifeStyle.Transient,

                Component.For<LendingDa_Database, LendingDa_Database>().LifeStyle.Transient,
                Component.For<MaterialsDa_Database, MaterialsDa_Database>().LifeStyle.Transient,
                Component.For<LoginDa_Database, LoginDa_Database>().LifeStyle.Transient,

                Component.For<Context,Context>().LifeStyle.Transient);

            switch (approach)
            {
                case "Database":
                    container.Register(
                        Component.For<ILoginDm, LoginDm_Database>().LifeStyle.Transient,
                        Component.For<IMaterialsDm, MaterialsDm_Database>(),
                        Component.For<ICopyDm, CopyDm_Database>(),
                        Component.For<ILendingDm, LendingDm_Database>().LifeStyle.Transient);
                    break;
                case "Code":
                    container.Register(
                        Component.For<ILoginDm, LoginDm_Code>().LifeStyle.Transient,
                        Component.For<IMaterialsDm, MaterialDm_Code>(),
                        Component.For<ICopyDm, CopyDm_Code>(),
                        Component.For<ILendingDm, LendingDm_Code>().LifeStyle.Transient);
                    break;
            }
        }
    }
}