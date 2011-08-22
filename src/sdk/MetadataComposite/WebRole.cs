using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Euclid.Composites;
using Euclid.Composites.Mvc;
using ForumAgent.Commands;

namespace AgentPanel
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

            /*
           * jt: this is how a composite developer would override the default settings for the mvc composite
           * euclidCompositeConfiguration.BlobStorage.ApplyOverride(typeof(SomeBlobStorageImplementation));
           */

            composite.Configure(euclidCompositeConfiguration);

            /*
            jt: this is going to be injected into composites adding this pacage
            */
            composite.AddAgent(typeof(PublishPost).Assembly);

            // composite.RegisterInputModel(new InputToFakeCommand4Converter());


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