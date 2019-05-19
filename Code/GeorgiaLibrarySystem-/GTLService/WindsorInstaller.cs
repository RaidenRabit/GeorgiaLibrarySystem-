﻿using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Core;
using GTLService.Controller;
using GTLService.Controller.IController;
using GTLService.DataAccess.Code;
using GTLService.DataAccess.Database;
using GTLService.DataManagement.Code;
using GTLService.DataManagement.IDataManagement;
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
                Component.For<ILoginService, LoginService>(),
                Component.For<ILoginDm, LoginDm_Database>(),
                Component.For<ILoginDa, LoginDa_Database>(),
                Component.For<IMaterialService, MaterialService>(),
                Component.For<IMaterialsDm, MaterialDm_Code>(),
                Component.For<MaterialDa_Code, MaterialDa_Code>(),
                Component.For<PersonDa_Code, PersonDa_Code>(),
                Component.For<LibraryDa_Code, LibraryDa_Code>(),
                Component.For<MaterialsDa_Database, MaterialsDa_Database>(),
                Component.For<Context, Context>());
        }
    }
}
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