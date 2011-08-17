using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Euclid.Composites;
using Euclid.Composites.Mvc;
using ForumAgent.Commands;
namespace Euclid.Sdk.NugetTests
{
    public partial class MvcApplication
    {
      protected void Application_Start()
      {
          AreaRegistration.RegisterAllAreas();

          RegisterGlobalFilters(GlobalFilters.Filters);

          RegisterRoutes(RouteTable.Routes);

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

          Error += composite.LogUnhandledException;

          BeginRequest += composite.BeginPageRequest;
      }
  }
}