	/// <reference path="/Assets/Scripts/euclid-0.9.js" />
; (function ($) {
	var app = $.sammy(function () {

		this.get('/composite/new/#explorer', function () {
			
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
					agentPartModel.Queries = agentMetadata.Queries;
					agentPartModel.ReadModels = agentMetadata.ReadModels;

					Using(agentPartModel).Render("/composite/js/inspector/templates/agentWithParts.html").Manipulate(function(content) {
						$("#inspectorMain").replaceContent(content);
						$(".subNav").removeClass("active");
						$(".subNav-" + agentSystemNameSlug).addClass("active");
					});
				});

			});
			
		});
		
		this.get('/composite/new/#explorer/agent/:agentSystemName/part/:partName/form', function () {
			
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

					var agentPartModel = {};
					agentPartModel.AgentSystemName = compositeMetadata.AgentSystemName;
					agentPartModel.Agents = compositeMetadata.Agents;
					agentPartModel.Commands = agentMetadata.Commands;
					agentPartModel.Queries = agentMetadata.Queries;
					agentPartModel.ReadModels = agentMetadata.ReadModels;

					Using(agentPartModel).Render("/composite/js/inspector/templates/agentPartForm.html").Manipulate(function(content) {
						$("#inspectorMain").replaceContent(content);
						$(".subNav").removeClass("active");
						$(".subNav-" + agentSystemNameSlug).addClass("active");
						
						$(".terNav").removeClass("active");
						$(".terNav-" + partNameSlug).addClass("active");
					});
				});

			});
			
		});
		
		this.get('/composite/new/#command-registry', function () {
			Using(compositeMetadata).Fill("#inspectorDescription").With("/composite/js/inspector/templates/description.html");

			getPublicationRecords(1);

			$(".nav-pills li").removeClass("active");
			$("#nav-CommandRegistry").addClass("active");			
		});
		
		this.get('/composite/new/#system-logs', function () {
			Using(compositeMetadata).Fill("#inspectorDescription").With("/composite/js/inspector/templates/description.html");

			getLogEntries(1);

			$(".nav-pills li").removeClass("active");
			$("#nav-SystemLogs").addClass("active");

		});

		this.get('/composite/new', function () {
			this.redirect('/composite/new/#explorer');
		});

	});
	
	$(function () {
		app.run();
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

