function slugify(text) {
	text = text.replace(/[^-a-zA-Z0-9,&\s]+/ig, '');
	text = text.replace(/-/gi, "_");
	text = text.replace(/\s/gi, "-");

	return text;
}

; (function ($) {

	Handlebars.registerHelper('slugify', function(text) {return slugify(text);});

	$.sammy(function () {

		this.bind('highlight-nav', function (e, data) {

			console.log("Building highlight selector");

			var navSelector = "#nav-" + data['current'];

			console.log("Deactivating current highlight");

			$(".nav-pills li").removeClass("active");

			console.log("Highlighting item at " + navSelector);

			$(navSelector).addClass("active");
		});
		
		this.bind('highlight-subnav', function (e, data) {

			console.log("Building highlight selector");

			var navSelector = "#subNav-" + data['current'];

			console.log("Deactivating current highlight");

			$(".nav-list li").removeClass("active");

			console.log("Highlighting item at " + navSelector);

			$(navSelector).addClass("active");
		});

	});

})(jQuery);