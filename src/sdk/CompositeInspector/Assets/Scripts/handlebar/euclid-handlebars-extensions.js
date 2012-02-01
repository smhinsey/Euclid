if (Handlebars) {
	Handlebars.registerHelper('convert-breaks', function (value) {
		var replaced = value.replace(/\n/g, "<br />");
		console.log("convert-breaks: " + Handlebars.SafeString(replaced));
		return new Handlebars.SafeString(replaced);
	});

	Handlebars.registerHelper('bold-selected', function (current, selected) {
		var handlebarValue = new Handlebars.SafeString(current);

		if (selected && current.toLowerCase() == selected.toLowerCase()) {
			handlebarValue = new Handlebars.SafeString("<strong>" + current + "</strong>");
		}

		return handlebarValue;
	});
}
