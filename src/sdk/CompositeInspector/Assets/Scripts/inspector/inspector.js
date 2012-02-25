	/// <reference path="/Assets/Scripts/euclid-0.9.js" />
$.getScript("/composite/js/inspector/common.js");

; (function ($) {
	var app = $.sammy(function () {

		this.get('/composite/new/#explorer', function () {
			
			WorkWithDataFromUrl("/composite/api", function (compositeMetadata) {

				Using(compositeMetadata).Fill("#inspectorGlobalNav").With("/composite/js/inspector/templates/globalNav.html");
				Using(compositeMetadata).Fill("#inspectorMain").With("/composite/js/inspector/templates/agents.html");
				Using(compositeMetadata).Fill("#inspectorDescription").With("/composite/js/inspector/templates/description.html");
				
			});
			
			this.trigger('highlight-nav', { current: 'Agents'});

		});
		
		this.get('/composite/new/#explorer/agent/:agentSystemName', function () {
			
			var agentSystemName = slugify(this.params['agentSystemName']);

			console.log("agentSystemName: " + agentSystemName);
			
			WorkWithDataFromUrl("/composite/api", function (compositeMetadata) {

				Using(compositeMetadata).Fill("#inspectorGlobalNav").With("/composite/js/inspector/templates/globalNav.html");
				Using(compositeMetadata).Fill("#inspectorMain").With("/composite/js/inspector/templates/agentWithParts.html");
				Using(compositeMetadata).Fill("#inspectorDescription").With("/composite/js/inspector/templates/description.html");

			});
			
//			WorkWithDataFromUrl("/composite/api/agent/" + data['agentSystemName'], function (agentMetadata) {
//				var commandRenderData = { ListClass: "dropdown-menu", Commands: agentMetadata.Commands };
//				Using(commandRenderData).Render("/composite/ui/template/commands").ReplaceContentsOf("#agent-commands");

//				var queryRenderData = { ListClass: "dropdown-menu", Queries: agentMetadata.Queries };
//				Using(queryRenderData).Render("/composite/ui/template/queries").ReplaceContentsOf("#agent-queries");

//				var readModelRenderData = { ListClass: "dropdown-menu", SystemName: agentMetadata.SystemName, ReadModels: agentMetadata.ReadModels};
//				Using(readModelRenderData).Render("/composite/ui/template/read-models").ReplaceContentsOf("#agent-read-models");
//			});

			this.trigger('highlight-nav', { current: 'Agents'});
			this.trigger('highlight-subnav', { current: agentSystemName});
		});
		
		this.get('/composite/new/#explorer/agent/:agentSystemName/form', function () {

			var eventContext = this;
			
			WorkWithDataFromUrl("/composite/api", function (compositeMetadata) {
				eventContext.trigger('render-model', {templateName: 'globalNav', model: compositeMetadata, targetSelector: "#inspectorGlobalNav"});
				eventContext.trigger('render-model', {templateName: 'agentPartForm', model: compositeMetadata, targetSelector: "#inspectorMain"});
				eventContext.trigger('render-model', {templateName: 'description', model: compositeMetadata, targetSelector: "#inspectorDescription"});
			});
			
			WorkWithDataFromUrl("/composite/api/agent/" + data['agentSystemName'], function (agentMetadata) {
				var commandRenderData = { ListClass: "dropdown-menu", Commands: agentMetadata.Commands };
				Using(commandRenderData).Render("/composite/ui/template/commands").ReplaceContentsOf("#agent-commands");

				var queryRenderData = { ListClass: "dropdown-menu", Queries: agentMetadata.Queries };
				Using(queryRenderData).Render("/composite/ui/template/queries").ReplaceContentsOf("#agent-queries");

				var readModelRenderData = { ListClass: "dropdown-menu", SystemName: agentMetadata.SystemName, ReadModels: agentMetadata.ReadModels};
				Using(readModelRenderData).Render("/composite/ui/template/read-models").ReplaceContentsOf("#agent-read-models");
			});

			this.trigger('highlight-nav', { current: 'Agents'});

		});
		
		this.get('/composite/new/#command-registry', function () {

			getPublicationRecords(1);

			this.trigger('highlight-nav', { current: 'CommandRegistry'});
		});
		
		this.get('/composite/new/#system-logs', function () {

			getLogEntries(1);

			this.trigger('highlight-nav', { current: 'SystemLogs'});
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

