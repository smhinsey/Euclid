(function ($) {
	var app = $.sammy(function () {

		this.get('/', function () {
			$('#app-content').text('Hello');
		});

	});

	$(function () {
		app.run();
	});
})(jQuery);