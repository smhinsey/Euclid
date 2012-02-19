;(function ($) {
	$.sammy(function () {
			
			this.bind('render-model', function(e, data) {

			var templateName = data['templateName'];

			var model = data['model'];

			console.log("render-model rendering using template" + templateName);
			console.log(model);

			fetchHandlebarsTemplate('content/app/templates/' + templateName + '.html', function(template) {
				var renderedOutput = template(model);
				$('#app-content').html(renderedOutput);
			});
			
		});

		this.bind('highlight-nav', function(e, data) {
			var navSelector = "#nav-" + data['slug'] + data['current'];

			$(".nav-list li").removeClass("active");

			$(navSelector).addClass("active");
		});

	});
	
	function fetchHandlebarsTemplate(path, callback) {
		var source;
		var template;
 
		$.ajax({
			url: path,
			success: function(data) {

				data = "<script>" + data + "</script>";

				source = $(data).html();


				template = Handlebars.compile(source);
				
				if (callback) {
					callback(template);
				};
			}
		});
	}
})(jQuery);