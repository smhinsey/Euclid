using Euclid.Framework.Agent;
using StorefrontAgent.Commands;
using StorefrontAgent.Processors;
using StorefrontAgent.Queries;
using StorefrontAgent.ReadModels;

[assembly:
	AgentDescription(Value = "The Storefront Agent supports managing e-commerce sites.")]
[assembly: AgentName(Value = "NewCo Storefront Agent")]
[assembly: AgentSystemName(Value = "NewCo.StorefrontAgent")]
[assembly: LocationOfCommands(NamespaceOfType = typeof(RegisterNewCompany))]
[assembly: LocationOfQueries(NamespaceOfType = typeof(CompanyQueries))]
[assembly: LocationOfProcessors(NamespaceOfType = typeof(RegisterNewCompanyProcessor))]
[assembly: LocationOfReadModels(NamespaceOfType = typeof(Company))]