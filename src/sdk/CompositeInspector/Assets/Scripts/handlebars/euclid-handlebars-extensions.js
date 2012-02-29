/// <reference path="handlebars-1.0.0.beta.6.js"/>

if (Handlebars) {

	Handlebars.registerHelper("space-queries", function (value) {
		value = value.replace("Queries", " Queries");
		value = value.replace("Query", " Queries");
		return value;
	});

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

	Handlebars.registerHelper("begin-input-model-form", function (inputModel, className) {
		console.log("begin-input-model-form");
		var form = "<form action='/composite/api/publish' method='post' enctype='multipart/form-data'";
		if (className) {
			form += " class='" + className + "'";
		}
		form += ">";

		form += "<input type='hidden' name='partName' value='" + inputModel.CommandName + "'/>";

		console.log(form);

		return new Handlebars.SafeString(form);
	});

	Handlebars.registerHelper("add", function (url, id) {
		console.log("Handlerbars.add-on: fetching template from " + url);
		Using(this).Render(url).Manipulate(function (template) {
			$(id.toJqueryId()).append(template);
		});
	});

	Handlebars.registerPartialsFromUrl = function (arrayOfTemplateUrls, onFinished) {
		var numberOfParitals = arrayOfTemplateUrls.length;

		var registerPartial = function (name, url) {
			$.get(url, function (data) {
				var template = Handlebars.compile(data);
				Handlebars.registerPartial(name, template);
				if (--numberOfParitals == 0) {
					onFinished();
				}
			});
		};

		for (var i = 0; i < arrayOfTemplateUrls.length; i++) {
			registerPartial(arrayOfTemplateUrls[i].name, arrayOfTemplateUrls[i].url);
		}
	};
}
