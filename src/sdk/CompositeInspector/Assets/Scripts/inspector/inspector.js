$.getScript("/composite/js/inspector/commonSammyEvents.js");


; (function ($) {
	var app = $.sammy(function () {

		this.get('/composite/new/#agents', function () {

			var context = this;

			WorkWithDataFromUrl("/composite/api", function (compositeMetadata) {
				context.trigger('render-model', {templateName: 'globalNav', model: compositeMetadata, targetSelector: "#inspectorGlobalNav"});
				context.trigger('render-model', {templateName: 'home', model: compositeMetadata, targetSelector: "#inspectorMain"});
				context.trigger('render-model', {templateName: 'description', model: compositeMetadata, targetSelector: "#inspectorDescription"});
			});

			this.trigger('highlight-nav', { current: 'Composite'});
		});
		
		this.get('/composite/new/#command-registry', function () {
		
			this.trigger('highlight-nav', { current: 'CommandRegistry'});
		});
		
		this.get('/composite/new/#system-logs', function () {
		
			this.trigger('highlight-nav', { current: 'SystemLogs'});
		});

		this.get('/composite/new', function () {
			this.redirect('/composite/new/#agents');
		});

	});
	
	$(function () {
		app.run();
	});
})(jQuery);