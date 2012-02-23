; (function ($) {

	$.sammy(function () {

		this.bind('render-model', function (e, data) {

			var templateName = data['templateName'];

			var model = data['model'];

			var targetSelector = data['targetSelector'];

			console.log("render-model rendering using template " + templateName);
			console.log(model);

			var templatePath = '/composite/js/inspector/templates/' + templateName + '.html';

			console.log(templatePath);

			fetchHandlebarsTemplate(templatePath, function (template) {
				var renderedOutput = template(model);
				$(targetSelector).html(renderedOutput);
			});

		});

		this.bind('highlight-nav', function (e, data) {
			var navSelector = "#nav-" + data['current'];

			$(".nav-pills li").removeClass("active");

			$(navSelector).addClass("active");
		});

	});

	function fetchHandlebarsTemplate(path, callback) {
		var source;
		var template;

		console.log("Loading handlebars template from " + path);

		$.ajax({
			url: path,
			success: function (data) {

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