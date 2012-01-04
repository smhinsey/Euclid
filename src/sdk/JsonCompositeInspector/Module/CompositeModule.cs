using System;
using System.Linq;
using Euclid.Composites;
using JsonCompositeInspector.Models;
using Nancy;

namespace JsonCompositeInspector.Module
{
	public class CompositeModule : NancyModule
	{
		private readonly ICompositeApp _compositeApp;

		public CompositeModule(ICompositeApp compositeApp)
			: base("composite")
		{
			_compositeApp = compositeApp;

			Get[""] = _ =>
			          	{
			          		return View["Shared/Frameset.cshtml"];
			          	};

			Get["/home"] = _ =>
			               	{
			               		var model = new CompositeHome
			               		            	{
			               		            		Agents = _compositeApp.Agents,
			               		            		ConfigurationErrors = _compositeApp.IsValid() ? null : _compositeApp.GetConfigurationErrors(),
			               		            		IsValid = _compositeApp.IsValid(),
			               		            		CompositeDescription = _compositeApp.Description,
			               		            		CompositeName = _compositeApp.Name
			               		            	};
			               		return View["Composite/home.cshtml", model];
			               	};

			Get["/agent/{agentSystemName}"] = p =>
			                                  	{
			                                  		var agent =
			                                  			compositeApp.Agents.Where(
			                                  				a =>
			                                  				a.SystemName.Equals(p.agentSystemName,
			                                  				                    StringComparison.InvariantCultureIgnoreCase)).
			                                  				FirstOrDefault();

													if (agent == null) return HttpStatusCode.NotFound;

			                                  		var model = new AgentModel
			                                  		            	{
			                                  		            		AgentSystemName = p.agentSystemName,
																		Commands = agent.Commands,
																		Description = agent.Description,
																		DescriptiveName = agent.DescriptiveName,
																		Queries = agent.Queries,
																		ReadModels = agent.ReadModels
			                                  		            	};

			                                  		return View["Composite/view-agent.cshtml", model];
			                                  	};

			Get["/agent/{agentSystemName}.json"] = p => "Json details for agaent: " + p.agentSystemName;

			Get["/js/{file}"] = p =>
								{
									var resx = string.Format("CompositeInspector2.Assets.Scripts.{0}", ((string)p.file).Replace("/", "."));
									var s = GetType().Assembly.GetManifestResourceStream(resx);
									return Response.FromStream(s, "text/javascript");
								};

			Get["/css/{file}"] = p =>
									{
										var resx = string.Format("CompositeInspector2.Assets.Styles.{0}", ((string)p.file).Replace("/", "."));
										var s = GetType().Assembly.GetManifestResourceStream(resx);
										return Response.FromStream(s, "text/css");
									};

			Get["/image/{file}"] = p =>
									{
										var resx = string.Format("CompositeInspector2.Assets.Images.{0}", ((string)p.file).Replace("/", "."));
										var extension = resx.Substring(resx.Length - 3, 3);
										var contentType = string.Format("image/{0}", extension);
										var s = GetType().Assembly.GetManifestResourceStream(resx);
										return Response.FromStream(s, contentType);
									};
		}
	}
}