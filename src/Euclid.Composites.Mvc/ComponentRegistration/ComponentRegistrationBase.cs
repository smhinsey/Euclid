using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Euclid.Composites.Mvc.ComponentRegistration
{
	public abstract class ComponentRegistrationBase : IWindsorInstaller
	{
		public abstract void Install(IWindsorContainer container, IConfigurationStore store);

		protected IEnumerable<Type> GetTypesThatImplement<T>()
		{
			var listOfTypes = new List<Type>();

			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				var types = assembly
					.GetTypes()
					.Where(type => typeof (T).IsAssignableFrom(type) && !type.IsAbstract && type != typeof (T));

				listOfTypes.AddRange(types);
			}

			return listOfTypes;
		}
	}
}