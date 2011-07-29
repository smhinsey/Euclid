using System;
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
}