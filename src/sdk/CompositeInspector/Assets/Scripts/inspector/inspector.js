	/// <reference path="/Assets/Scripts/euclid-0.9.js" />
$.getScript("/composite/js/inspector/commonSammyEvents.js");

; (function ($) {
	var app = $.sammy(function () {

		this.get('/composite/new/#agents', function () {

			var eventContext = this;
			
			WorkWithDataFromUrl("/composite/api", function (compositeMetadata) {
				eventContext.trigger('render-model', {templateName: 'globalNav', model: compositeMetadata, targetSelector: "#inspectorGlobalNav"});
				eventContext.trigger('render-model', {templateName: 'home', model: compositeMetadata, targetSelector: "#inspectorMain"});
				eventContext.trigger('render-model', {templateName: 'description', model: compositeMetadata, targetSelector: "#inspectorDescription"});
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
			this.redirect('/composite/new/#agents');
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

