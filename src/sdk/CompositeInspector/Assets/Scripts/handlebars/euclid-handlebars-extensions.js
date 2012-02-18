if (Handlebars) {
	Handlebars.registerHelper("convert-breaks", function (value) {
		if (!value) return "";
		
		var replaced = value.replace(/\n/g, "<br />");
		
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

	Handlebars.registerHelper("comma-delimited-list", function (items, propertyName, selected) {
		var list = "";
		for (var i = 0; i < items.length; i++) {
			if (list.length > 0) {
				list += ", ";
			}

			var value = items[i];
			if (propertyName != null && propertyName.length > 0) {
				value = items[i][propertyName];
			}

			if (selected.length > 0 && value == selected) {
				list += "<strong>" + value + "</strong>";
			} else {
				list += value;
			}
		}

		return list;
	});

	Handlebars.registerHelper("format-date", function (date) {
		return date.getMonth() + "/" + date.getDate() + "/" + date.getFullYear() + " [" + date.getHours() + ":" + date.getMinutes() + ":" + date.getSeconds() + "]";
	});
}
