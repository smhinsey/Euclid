//using System;
//using System.Web.Mvc;
//using Euclid.Composites.AgentResolution;
//using Euclid.Composites.Extensions;
//using Euclid.Composites.Mvc.Extensions;
//using Euclid.Framework.Agent.Metadata;

//namespace Euclid.Composites.Mvc.Binders
//{
//    public class AgentPartMetadataCollectionBinder : IEuclidModelBinder
//    {
//        private readonly IAgentResolver[] _resolvers;

//        public AgentPartMetadataCollectionBinder(IAgentResolver[] resolvers)
//        {
//            _resolvers = resolvers;
//        }

//        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
//        {
//            var systemName = controllerContext.GetAgentSystemName();

//            var partType = controllerContext.GetRouteValue<string>("partType");

//            if (string.IsNullOrEmpty(partType))
//            {
//                throw new AgentPartTypeNotSpecifiedException();
//            }

//            var metadata = _resolvers.GetAgentMetadata(systemName);

//            return metadata.Where(a=>a);
//        }

//        public bool IsMatch(Type modelType)
//        {
//            return typeof (IAgentPartMetadataFormatterCollection).IsAssignableFrom(modelType);
//        }
//    }
//}