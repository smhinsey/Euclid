/// <reference path="/Assets/Scripts/euclid-0.9.js" />
; (function ($) {
	var app = $.sammy(function () {

		this.get('/composite/new/#explorer', function () {
			
			// need to replace this with some mechanism for making sure the UI is entirely current
//			$("#inspectorMain").empty();

			WorkWithDataFromUrl("/composite/api", function (compositeMetadata) {

				Using(compositeMetadata).Fill("#inspectorDescription").With("/composite/js/inspector/templates/description.html");
				Using(compositeMetadata).Render("/composite/js/inspector/templates/globalNav.html").Manipulate(function(content) {
					$("#inspectorGlobalNav").replaceContent(content);
					$(".nav-pills li").removeClass("active");
					$("#nav-CompositeExplorer").addClass("active");
				});
				
				Using(compositeMetadata).Fill("#inspectorMain").With("/composite/js/inspector/templates/agents.html");
			});
		});
		
		this.get('/composite/new/#explorer/agent/:agentSystemName', function () {
			
			// need to replace this with some mechanism for making sure the UI is entirely current
//			$("#inspectorMain").empty();

			var agentSystemName = this.params['agentSystemName'];
			var agentSystemNameSlug = slugify(this.params['agentSystemName']);

			WorkWithDataFromUrl("/composite/api", function (compositeMetadata) {

				compositeMetadata.AgentSystemName = agentSystemName;

				Using(compositeMetadata).Fill("#inspectorDescription").With("/composite/js/inspector/templates/description.html");
				Using(compositeMetadata).Render("/composite/js/inspector/templates/globalNav.html").Manipulate(function(content) {
					$("#inspectorGlobalNav").replaceContent(content);
					$(".nav-pills li").removeClass("active");
					$("#nav-CompositeExplorer").addClass("active");
				});

				WorkWithDataFromUrl("/composite/api/agent/" + agentSystemName, function (agentMetadata) {

					var agentPartModel = {};
					agentPartModel.AgentSystemName = compositeMetadata.AgentSystemName;
					agentPartModel.Agents = compositeMetadata.Agents;
					agentPartModel.Commands = agentMetadata.Commands;
					agentPartModel.Queries = compositeMetadata.Queries;
					agentPartModel.ReadModels = agentMetadata.ReadModels;

					console.log(agentPartModel);

					Using(agentPartModel).Render("/composite/js/inspector/templates/agentWithParts.html").Manipulate(function(content) {
						$("#inspectorMain").replaceContent(content);
						$(".subNav").removeClass("active");
						$(".subNav-" + agentSystemNameSlug).addClass("active");
					});
				});

			});
			
		});
		
		this.get('/composite/new/#explorer/agent/:agentSystemName/:partType/:partName/form', function () {
			
			// need to replace this with some mechanism for making sure the UI is entirely current
//			$("#inspectorMain").empty();

			var selectedPartType = this.params['partType'];
			var agentPartName = this.params['partName'];
			var agentSystemName = this.params['agentSystemName'];
			var agentSystemNameSlug = slugify(this.params['agentSystemName']);

			var partNameSlug = slugify(this.params['partName']);

			WorkWithDataFromUrl("/composite/api", function (compositeMetadata) {

				compositeMetadata.AgentSystemName = agentSystemName;

				Using(compositeMetadata).Fill("#inspectorDescription").With("/composite/js/inspector/templates/description.html");
				Using(compositeMetadata).Render("/composite/js/inspector/templates/globalNav.html").Manipulate(function(content) {
					$("#inspectorGlobalNav").replaceContent(content);
					$(".nav-pills li").removeClass("active");
					$("#nav-CompositeExplorer").addClass("active");
				});

				WorkWithDataFromUrl("/composite/api/agent/" + agentSystemName, function (agentMetadata) {

					var agentPartModel = { };
					agentPartModel.AgentSystemName = compositeMetadata.AgentSystemName;
					agentPartModel.Agents = compositeMetadata.Agents;
					agentPartModel.Commands = agentMetadata.Commands;
					agentPartModel.Queries = compositeMetadata.Queries;	
					agentPartModel.ReadModels = agentMetadata.ReadModels;

					agentPartModel.SelectedPartType = selectedPartType;
					agentPartModel.IsCommand = selectedPartType == "command";
					agentPartModel.IsQuery = selectedPartType == "query";
					agentPartModel.IsReadModel = selectedPartType == "readModel";

					if(agentPartModel.IsCommand) {
						
						agentPartModel.Command = { };
						
						WorkWithDataFromUrl("/composite/api/command-is-supported/" + agentPartName, function (results) {

							console.log(results);

							if (results.Supported) {
								
								agentPartModel.InputModel = results.InputModel;

								agentPartModel.IsCommand = false;

								agentPartModel.IsInputModel = true;
								
								console.log(agentPartModel);
								
								Using(agentPartModel).Render("/composite/js/inspector/templates/agentPartForm.html").Manipulate(function(content) {
									
									console.log("Rendering input model");
									
									$("#inspectorMain").replaceContent(content);
									$(".subNav").removeClass("active");
									$(".subNav-" + agentSystemNameSlug).addClass("active");
									console.log("secondary nav selector: " + ".subNav-" + agentSystemNameSlug);
						
									$(".terNav").removeClass("active");
									$(".terNav-" + partNameSlug).addClass("active");
									console.log("tertiary nav selector: " + ".terNav-" + partNameSlug);
									console.log(agentPartModel);
								});

							} else {
								agentPartModel.Command = results.Command;
						
								Using(agentPartModel).Render("/composite/js/inspector/templates/agentPartForm.html").Manipulate(function(content) {
									$("#inspectorMain").replaceContent(content);
									$(".subNav").removeClass("active");
									$(".subNav-" + agentSystemNameSlug).addClass("active");
									console.log("secondary nav selector: " + ".subNav-" + agentSystemNameSlug);
						
									$(".terNav").removeClass("active");
									$(".terNav-" + partNameSlug).addClass("active");
									console.log("tertiary nav selector: " + ".terNav-" + partNameSlug);
									console.log(agentPartModel);
								});
							}
								
						});
					
					}
					else if(agentPartModel.IsReadModel) {
						
						agentPartModel.ReadModel = { };

						WorkWithDataFromUrl("/composite/api/read-model/" + agentSystemName + "/" + agentPartName, function(readModel) {

							agentPartModel.ReadModel = readModel;
							agentPartModel.ReadModel.Name = agentPartName;

							Using(agentPartModel).Render("/composite/js/inspector/templates/agentPartForm.html").Manipulate(function(content) {
								$("#inspectorMain").replaceContent(content);
								$(".subNav").removeClass("active");
								$(".subNav-" + agentSystemNameSlug).addClass("active");

								$(".terNav").removeClass("active");
								$(".terNav-" + partNameSlug).addClass("active");
								console.log("partNameSlug: " + partNameSlug);
								console.log(agentPartModel);
							});

						});
					
					} 
					else if(agentPartModel.IsQuery) {
						
						agentPartModel.Query = { };

						WorkWithDataFromUrl("/composite/api/queries/" + agentPartName, function(query) {

							console.log(query);

							agentPartModel.Name = agentPartName;
							agentPartModel.Query = query;
							agentPartModel.Query.Name = agentPartName;

							Using(agentPartModel).Render("/composite/js/inspector/templates/agentPartForm.html").Manipulate(function(content) {
								$("#inspectorMain").replaceContent(content);
								$(".subNav").removeClass("active");
								$(".subNav-" + agentSystemNameSlug).addClass("active");

								$(".terNav").removeClass("active");
								$(".terNav-" + partNameSlug).addClass("active");
								console.log("partNameSlug: " + partNameSlug);
								console.log(agentPartModel);
							});

						});
					
					}
				});

			});
			
		});
		
		this.get('/composite/new/#command-registry', function () {

			$("#inspectorMain").empty();

			WorkWithDataFromUrl("/composite/api", function(compositeMetadata) {

				Using(compositeMetadata).Fill("#inspectorDescription").With("/composite/js/inspector/templates/description.html");
				Using(compositeMetadata).Render("/composite/js/inspector/templates/globalNav.html").Manipulate(function(content) {
					$("#inspectorGlobalNav").replaceContent(content);
					$(".nav-pills li").removeClass("active");
					$("#nav-CommandRegistry").addClass("active");
				});

			});

			getPublicationRecords(1);

		});
		
		this.get('/composite/new/#system-logs', function () {

			WorkWithDataFromUrl("/composite/api", function(compositeMetadata) {

				Using(compositeMetadata).Fill("#inspectorDescription").With("/composite/js/inspector/templates/description.html");
				Using(compositeMetadata).Render("/composite/js/inspector/templates/globalNav.html").Manipulate(function(content) {
					$("#inspectorGlobalNav").replaceContent(content);
					$(".nav-pills li").removeClass("active");
					$("#nav-SystemLogs").addClass("active");
				});

			});

			getLogEntries(1);

			});

		this.get('/composite/new', function () {
			$("#inspectorMain").empty();
			
			this.redirect('/composite/new/#explorer');
		});

	});
	
	$(function () {
		$("#compositeInspector").hide();
		$("#loading").show();
		Handlebars.registerPartialsFromUrl(
			[
				{name: "command", url:"/composite/js/inspector/templates/forms/command.html"},
				{name: "query", url:"/composite/js/inspector/templates/forms/query.html"},
				{name: "inputModel", url:"/composite/js/inspector/templates/forms/inputModel.html"},
				{name: "readModel", url:"/composite/js/inspector/templates/forms/readModel.html"},
				{name: "agentLeftCol", url:"/composite/js/inspector/templates/agent/leftCol.html"},
				{name: "agentCenterCol", url:"/composite/js/inspector/templates/agent/centerCol.html"}
			], 
			function() {
				$("#compositeInspector").show();
				$("#loading").hide();
				app.run();
			}
		);
	});
	
	function getLogEntries(pageNumber) {
		var offset = (pageNumber - 1) * 10;
		RunQuery(
				"LogQueries.GetLogEntries",
				{ offset: offset, pageSize: 10 },
				function (results) {
					var pagerData = results;
					pagerData["pageHandler"] = "getLogEntries";
					Using(results).Render("/composite/ui/template/log-entry-table").Manipulate(function (content) {
						$("#inspectorMain").replaceContent(content);
						Using(pagerData).Render("/composite/ui/template/pager").Manipulate(function (pager) { $(display).append(pager); });
					});
				}
			);
	}
	
	function getPublicationRecords(pageNumber) {
		console.log("getPublicationRecords(" + pageNumber + ")");
		var offset = (pageNumber - 1) * 10;
		RunQuery(
				"CommandRegistryQueries.GetPublicationRecords",
				{ offset: offset, recordsPerPage: 10 },
				function (results) {
					var pagerData = results;
					pagerData["pageHandler"] = "getPublicationRecords";
					Using(results).Render("/composite/ui/template/command-registry-table").Manipulate(function (content) {
						$("#inspectorMain").replaceContent(content);
						Using(pagerData).Render("/composite/ui/template/pager").Manipulate(function (pager) { $(display).append(pager); });
					});
				});
	}

})(jQuery);

