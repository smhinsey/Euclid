	/// <reference path="../Assets/Scripts/euclid-0.9.js" />
; (function ($) {
	var app = $.sammy(function () {

		this.get('/composite/new/#explorer', function () {
			
			WorkWithDataFromUrl("/composite/api", function (compositeMetadata) {

				Using(compositeMetadata).Fill("#inspectorDescription").With("/composite/js/inspector/templates/description.html");
				Using(compositeMetadata).Fill("#inspectorGlobalNav").With("/composite/js/inspector/templates/globalNav.html")
					.Then(function () {
						$(".nav-pills li").removeClass("active");
						$("#nav-Agents").addClass("active");
					});
				
				Using(compositeMetadata).Fill("#inspectorMain").With("/composite/js/inspector/templates/agents.html");
			});
		});
		
		this.get('/composite/new/#explorer/agent/:agentSystemName', function () {
			
			var agentSystemName = slugify(this.params['agentSystemName']);

			console.log("agentSystemName: " + agentSystemName);
			
			WorkWithDataFromUrl("/composite/api", function (compositeMetadata) {

				Using(compositeMetadata).Fill("#inspectorDescription").With("/composite/js/inspector/templates/description.html");
				
				Using(compositeMetadata).Fill("#inspectorGlobalNav").With("/composite/js/inspector/templates/globalNav.html")
					.Then(function () {
						$(".nav-pills li").removeClass("active");
						$("#nav-Agents").addClass("active");
					});


				Using(compositeMetadata).Render("/composite/js/inspector/templates/agentWithParts.html").Manipulate(function(content) {
						$(content).find(".subNav").removeClass("active");
						$(content).find(".subNav-" + agentSystemName).addClass("active");
						$("#inspectorMain").replaceContent(content);
						console.log($(".subNav"));
				});

//				Using(compositeMetadata).Fill("#inspectorMain").With("/composite/js/inspector/templates/agentWithParts.html")
//					.Then(function () {
//						$(".subNav").removeClass("active");
//						$(".subNav-" + agentSystemName).addClass("active");
//						console.log($(".subNav"));
//					});
			});
			
//			WorkWithDataFromUrl("/composite/api/agent/" + data['agentSystemName'], function (agentMetadata) {
//				var commandRenderData = { ListClass: "dropdown-menu", Commands: agentMetadata.Commands };
//				Using(commandRenderData).Render("/composite/ui/template/commands").ReplaceContentsOf("#agent-commands");

//				var queryRenderData = { ListClass: "dropdown-menu", Queries: agentMetadata.Queries };
//				Using(queryRenderData).Render("/composite/ui/template/queries").ReplaceContentsOf("#agent-queries");

//				var readModelRenderData = { ListClass: "dropdown-menu", SystemName: agentMetadata.SystemName, ReadModels: agentMetadata.ReadModels};
//				Using(readModelRenderData).Render("/composite/ui/template/read-models").ReplaceContentsOf("#agent-read-models");
//			});

		});
		
		this.get('/composite/new/#explorer/agent/:agentSystemName/form', function () {
			
			$(".nav-pills li").removeClass("active");
			$("#nav-Agents").addClass("active");
		});
		
		this.get('/composite/new/#command-registry', function () {

			getPublicationRecords(1);

			$(".nav-pills li").removeClass("active");
			$("#nav-CommandRegistry").addClass("active");			
		});
		
		this.get('/composite/new/#system-logs', function () {

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

