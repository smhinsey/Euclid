/// <reference path="handlebars-1.0.0.beta.6.js"/>

if (Handlebars) {
	Handlebars.registerHelper("convert-breaks", function(value) {
		if (!value) return "";

		var replaced = value.replace( /\n/g , "<br />");

		return new Handlebars.SafeString(replaced);
	});

	Handlebars.registerHelper("bold-selected", function(current, selected) {
		var handlebarValue = new Handlebars.SafeString(current);

		if (selected && current.toLowerCase() == selected.toLowerCase()) {
			handlebarValue = new Handlebars.SafeString("<strong>" + current + "</strong>");
		}

		return handlebarValue;
	});

	Handlebars.registerHelper("format-query-results", function(items, options) {
		var rows = "";
		for (var i = 0; i < items.length; i++) {
			rows += "<tr>";

			var cells = "";
			for (property in items[i]) {
				if (items[i][property] instanceof Array) {
					cells += "<td>An array of " + items[i][property].length + " items</td>";
				} else if (items[i][property] instanceof Date) {
					cells += "<td>" + items[i][property].format("mm/dd/yyyy [HH:MM:ss]") + "</td>";
				} else {
					cells += "<td>" + items[i][property] + "</td>";
				}
			}


			rows += cells + "</tr>";
		}

		return rows;
	});

	Handlebars.registerHelper("get-argument-count", function(items, options) {
		return items.length.toString();
	});

	Handlebars.registerHelper("comma-delimited-list", function(items, propertyName, selected) {
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

	Handlebars.registerHelper("format-date", function (date, formatString) {
		// supported formats here: http://blog.stevenlevithan.com/archives/date-time-format
		formatString = (typeof formatString == 'string') ? formatString : "mm/dd/yyyy [HH:MM:ss]";
		return (date instanceof Date)
			? date.format(formatString)
			: date;
	});

	Handlebars.registerHelper("format-bool", function (value, displayWhenTrue, displayWhenFalse) { return (value) ? displayWhenTrue : displayWhenFalse; });

	Handlebars.registerHelper("begin-input-model-form", function (inputModel) {
		var form = "<form action='/composite/api/publish' method='post' enctype='multipart/form-data'>";
		form += "<input type='hidden' name='partName' value='" + inputModel.CommandName + "'/>";

		return Handlebars.SafeString(form);
	});

	Handlebars["registerPartials"] = function (arrayOfKeyedTemplateUrls, callback, errorCallback) {
		errorCallback = errorCallback ? errorCallback : EUCLID.displayError;
		if (!(arrayOfKeyedTemplateUrls instanceof Array)) {
			throw {
				name: "Invalid Argument Exception",
				message: "The parameter 'arrayOfKeyedTemplateUrls' is not an array"
			};
		}

		var numberFetched = 0;
		for (var i = 0; i < arrayOfKeyedTemplateUrls.length; i++) {
			var key = arrayOfKeyedTemplateUrls[i].name;
			var url = arrayOfKeyedTemplateUrls[i].url;
			$.get(url, function (data) {
				numberFetched++;
				var template = Handlebars.compile(data);
				Handlebars.registerPartial(key, template);
				if (numberFetched == arrayOfKeyedTemplateUrls.length) {
					callback();
				}
			});
		}
	};
}
