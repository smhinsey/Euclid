var EUCLID = function () {
	/* private methods */
	var _getJsonObject = (function (url) {
		$.ajaxSetup({ "async": false });
		var jqHxr = jQuery.getJSON(url);
		var _model = $.parseJSON(jqHxr.responseText);
		$.ajaxSetup({ "async": true });

		if (_model === null) {
			throw {
				name: "Invalid QueryName Exception",
				message: "Could not retrieve the methods for query: " + _queryName
			};
		} else if (jqHxr.status == 500) {
			throw {
				name: _model.name,
				message: _model.message + "\n\n" + _model.callstack
			}
		} // end getJsonObject

		return _model;
	}); // end getFormElement

	var _getId = (function () {
		function S4() {
			return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
		}
		return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
	}); // end getId

	var _addElementToForm = (function (propertyName, propertyType, propertyValue, choices, allowMultiChoice, fieldSet, forceShow) {
		var inputClass = "xlarge";
		switch (propertyType.toLowerCase()) {
			case "httppostedfilebase":
			case "byte[]":
				$(form).attr("enctype", "multipart/form-data");
				type = "file";
				inputClass = "input-file";
				break;
			case "boolean":
			case "bool":
				type = "checkbox";
				break;
			case "date":
			case "datetime":
				type = "text";
				inputClass = "input-date";
				break;
			case "guid":
			case "type":
				type = "hidden";
				break;
			default:
				type = (propertyName.toLowerCase() == "agentsystemname" || propertyName.toLowerCase() == "partname")
						? "hidden"
						: "text";
				break;
		}

		if (forceShow && type == "hidden") {
			type = "text";
		}

		if (propertyType == "string" && propertyName.toLowerCase().endsWith("url")) {
			// check to see if there is an associated file upload
			var filePropertyName = propertyName.substr(0, propertyName.toLowerCase().lastIndexOf("url"));
			var element = $(form).find("input[name='" + filePropertyName + "']");

			// if so, hide it
			if ($(element).length > 0 && $(element).hasAttr("file")) {
				type = "hidden";
			}
		}

		var inputId = _getId();
		var html = "";
		if (choices != null) {
			var options = "";
			$.each(choices.Values, function (index, item) {
				options += "<option value='" + item + "'";
				if (propertyValue == item) {
					options += " selected='selected'";
				}

				options += ">" + item + "</option>";
			});

			html = "<div class='clearfix'>" +
						"<label for='" + inputId + "'>" + propertyName + "</label>" +
						"<div class='input'>" +
							"<select class='" + inputClass + "' name='" + propertyName + "'>" +
								options +
							"</select>" +
						"</div>" +
					"</div>";

		} else if (type == "checkbox") {
			var inputElement = "";
			if (propertyValue.toLowerCase() == "true" || propertyValue.toLowerCase() == "yes" || propertyValue.toLowerCase() == "on" || propertyValue.toLowerCase() == "1") {
				inputElement = "<input class='" + inputClass + "' type='text' value='" + propertyName + "' class='uneditable-input' disabled='disabled' checked='checked' />";
			} else {
				inputElement = "<input class='" + inputClass + "' type='text' value='" + propertyName + "' class='uneditable-input' disabled='disabled' />";
			}

			html = "<div class='clearfix'>" +
							"<div class='input-prepend'>" +
								"<label class='add-on'><input type='checkbox' id='" + inputId + "' name='" + propertyName + "' /></label>" +
								inputElement +
							"</div>" +
						"</div>" +
					"</div>";
		} else if (type == "hidden") {
			html = "<input type='hidden' name='" + propertyName + "' value='" + propertyValue + "' />";
		} else {
			html = "<div class='clearfix'>" +
						"<label for='" + inputId + "'>" + propertyName + "</label>" +
						"<div class='input'>" +
							"<input class='" + inputClass + "' type='" + type + "' id='" + inputId + "' name='" + propertyName + "' value='" + propertyValue + "' />" +
						"</div>" +
					"</div>";
		}

		$(fieldSet).append($(html));
	}); //end addElementToForm

	return {
		getQueryMetadata: (function (args) {
			if (args === null || args === undefined || !args.hasOwnProperty("queryName")) {
				throw {
					name: "Invalid Argument Exception",
					message: "EUCLID.getQueryMethods expects an object with the properties: 'queryName'"
				};
			}

			var _queryName = args.queryName;
			var _getQueryMethodsUrl = "/composite/queries/" + _queryName + ".json";
			return _getJsonObject(_getQueryMethodsUrl);
		}), // end getQueryMethods

		randomId: (function () {
			return _getId();
		}),

		getQueryForm: (function (args) {
			if (args == null || args === undefined || !args.hasOwnProperty("method") || !args.hasOwnProperty("queryName")) {
				throw {
					name: "Invalid Argument Exception",
					message: "EUCLID.getQueryForm exepcts an object with the properties: 'method' and 'queryName'"
				}
			}

			var method = args.method;
			var queryName = args.queryName;
			var methodName = method.Name
			var form = $("<form action='/composite/queries/" + queryName + "/" + method.Name + "' method='post'><legend>" + method.Name + "</legend><fieldset></fieldset></form>");
			var fieldSet = $(form).children("fieldset");

			$.each(method.Arguments, function (index, item) {
				var forceShow = methodName == "FindById" && item.ArgumentName == "id";
				_addElementToForm(item.ArgumentName, item.ArgumentType, "", item.Choices, item.MultiChoice, fieldSet, forceShow);
			});

			$(form).find(".input-date").datepicker();
			$(form).append("<input type='submit' value='" + method.Name + "' />");
			return form;
		}),

		getJsonObject: (function (url) {
			return _getJsonObject(url);
		})
	}
} ();


// extension methods
$.fn.hasAttr = function(name) {  
   return this.attr(name) !== undefined;
};

String.prototype.endsWith = function(suffix) {
	return this.indexOf(suffix, this.length - suffix.length) !== -1;
};