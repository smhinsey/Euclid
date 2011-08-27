using System;
using System.Reflection;
using Castle.MicroKernel.ComponentActivator;
using Castle.Windsor;

namespace Euclid.Composites.Mvc.Extensions
{
    public static class WindsorContainerExtensions
    {
        public static void InjectProperties(this IWindsorContainer container, object target)
        {
            var type = target.GetType();
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.CanWrite && container.Kernel.HasComponent(property.PropertyType) )
                {
                    var value = container.Resolve(property.PropertyType);
                    try
                    {
                        property.SetValue(target, value, null);
                    }
                    catch (Exception ex)
                    {
                        var message = string.Format("Error setting property {0} on type {1}, See inner exception for more information.", property.Name, type.FullName);
                        throw new ComponentActivatorException(message, ex);
                    }
                }
            }
        }
    }
}