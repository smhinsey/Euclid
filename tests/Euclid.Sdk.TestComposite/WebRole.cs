using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Euclid.Common.Messaging;
using Euclid.Composites;
using Euclid.Composites.Mvc;
using Euclid.Sdk.TestAgent.Commands;
using Euclid.Sdk.TestComposite.Converters;

namespace Euclid.Sdk.TestComposite
{
    public class WebRole
    {
        private bool _initialized;
        public void Init()
        {
            if (_initialized) return;

            AreaRegistration.RegisterAllAreas();

            var container = new WindsorContainer();

            var composite = new MvcCompositeApp(container);

            var euclidCompositeConfiguration = new CompositeAppSettings();

            euclidCompositeConfiguration.OutputChannel.ApplyOverride(typeof(InMemoryMessageChannel));

            composite.Configure(euclidCompositeConfiguration);

            composite.AddAgent(typeof(TestCommand).Assembly);

            composite.RegisterInputModel(new TestInputModelToCommandConverter());

            container.Register(Component.For<ICompositeApp>().Instance(composite));

            _initialized = true;
        }

        private static WebRole _instance;

        public static WebRole GetInstance()
        {
            return _instance ?? (_instance = new WebRole());
        }

        private WebRole()
        {
            
        }
    }
}