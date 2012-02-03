if (Handlebars) {
	Handlebars.registerHelper("convert-breaks", function (value) {
		var replaced = value.replace(/\n/g, "<br />");
		console.log("convert-breaks: " + Handlebars.SafeString(replaced));
		return new Handlebars.SafeString(replaced);
	});

	Handlebars.registerHelper("bold-selected", function (current, selected) {
		var handlebarValue = new Handlebars.SafeString(current);

		if (selected && current.toLowerCase() == selected.toLowerCase()) {
			handlebarValue = new Handlebars.SafeString("<strong>" + current + "</strong>");
		}

		return handlebarValue;
	});

	Handlebars.registerHelper("format-query-results", function (items, options) {
		var rows = "";
		for (var i = 0; i < items.length; i++) {
			rows += "<tr>";

			var cells = "";
			for (property in items[i]) {
				cells += "<td>" + items[i][property] + "</td>";
			}


			rows += cells + "</tr>"
		}

		return rows;
	});

	Handlebars.registerHelper("get-argument-count", function (items, options) {
		return items.length.toString();
	});
}
