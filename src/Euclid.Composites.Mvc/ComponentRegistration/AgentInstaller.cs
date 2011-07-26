using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Euclid.Composites.Mvc.ComponentRegistration
{
    public class CommandInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            throw new NotImplementedException();
        }
    }

    public class QueryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            throw new NotImplementedException();
        }
    }
}
