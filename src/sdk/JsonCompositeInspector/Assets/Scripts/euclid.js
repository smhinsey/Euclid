var EUCLID = {};

EUCLID.getId = (function () {
	function S4() {
		return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
	}

	
	return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
});

EUCLID.publish = (function (inputModel) {
	if (!inputModel.isInputModel()) {
		throw new {
			name: "Invalid Argument Exception",
			message: "EUCLID.publish expects an input model as an argument"
		}
	}

	var form = $("<form></form>");
	for (prop in inputModel) {
		if (inputModel.hasOwnProperty(prop) && typeof inputModel[prop] !== 'function') {
			if (!inputModel.propertyNameIsValid(prop)) {
				throw {
					name: "Invalid Property Exception",
					message: "the input model for command '" + inputModel.PartName + "' does not contain a property named '" + prop + "'"
				}
			}

			$(form).append($("<input type='hidden' name='" + prop + "' value='" + inputModel[prop] + "' />"));
		}
	}

	$.ajaxSetup({ "async": false });
	var jqHxr = $.post($(form).attr("action"), $(form).serialize());
	var result = $.parseJSON(jqHxr.responseText);
	$.ajaxSetup({ "async": true });

	if (jqHxr.status == 500) {
		throw {
			name: result.name,
			message: result.message + "\n\n" + result.callstack
		};
	}

	return result.publicationId;
});

EUCLID.getQueryMethods = (function (args) {
	if (args === null || args === undefined || !args.hasOwnProperty("queryName")) {
		throw {
			name: "Invalid Argument Exception",
			message: "EUCLID.getQueryMethods expects an object with the properties: 'queryName'"
		};
	}

	var _queryName = args.queryName;
	var _getQueryMethodsUrl = "/composite/queries/" + _queryName + ".json";

	$.ajaxSetup({ "async": false });
	var jqHxr = jQuery.getJSON(_getQueryMethodsUrl);
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
	}

	_model.getForm = (function () {
		var form = $("<form action='/composite/queries/execute' method='POST'><legend>" + _queryName + "</legend><fieldset></fieldset></form>");
		var fieldSet = $(form).children("fieldset");

		$.each(_model.Methods, function (index, item) {

		});
	});

	$("body").append($("<pre>" + jqHxr.responseText + "</pre>"));

	return _model;
});

EUCLID.getInputModel = (function (args) {
	if (args === null || args === undefined || !args.hasOwnProperty("commandName") || !args.hasOwnProperty("agentSystemName")) {
		throw {
			name: "Invalid Argument Exception",
			message: "EUCLID.getInputModel expects an an object with the properties: 'commandName' and 'agentSystemName'"
		};
	}

	var _commandName = args.commandName;
	var _agentSystemName = args.agentSystemName;
	var _getInputModelUrl = "/composite/commands/" + _agentSystemName + "/" + _commandName + ".json";
	
	$.ajaxSetup({ "async": false });
	var jqHxr = jQuery.getJSON(_getInputModelUrl);
	var _model = $.parseJSON(jqHxr.responseText);
	$.ajaxSetup({ "async": true });

	if (_model === null) {
		throw {
			name: "Invalid CommandName Exception",
			message: "Could not retrieve the input model for command '" + _commandName + "'"
		};
	} else if (jqHxr.status == 500) {
		throw {
			name: _model.name,
			message: _model.message + "\n\n" + _model.callstack
		}
	}

	var _returnModel = {};

	_returnModel.isInputModel = (function () {
		return true;
	});

	_returnModel.propertyNameIsValid = (function (name) {
		var found = ($.inArray(name, _propertyNames) > -1 || name === "PartName");
		return found;
	});

	_returnModel.getPropertyType = (function (propertyName) {
		var type = null;
		for (i = 0; i < _model.Properties.length; i++) {
			var obj = _model.Properties[i];
			if (obj.Name.toLowerCase() == propertyName.toLowerCase()) {
				type = obj.Type;
				break;
			}
		}

		if (type == null) {
			throw {
				name: "Invalid Property Exception",
				message: "The property '" + propertyName + "' does not exist on the inputModel"
			};
		}

		return type;
	});

	_returnModel.getChoices = (function (propertyName) {
		var prop = getProperty(propertyName);

		return (prop == null || prop.Choices === null) ? null : { Values: prop.Choices, MultiChoice: prop.MultiChoice };
	});

	function getProperty(propertyName) {
		for (i = 0; i < _model.Properties.length; i++) {
			var obj = _model.Properties[i];
			if (obj.Name.toLowerCase() == propertyName.toLowerCase()) {
				return obj;
			}
		}
	}

	_returnModel.getForm = (function () {
		var form = $("<form method='POST' action='/composite/commands/publish'><legend>" + _returnModel.PartName + "</legend><fieldset></fieldset></form>");
		var fieldSet = $(form).children("fieldset");
		for (prop in _returnModel) {
			if (_returnModel.hasOwnProperty(prop) && typeof _returnModel[prop] !== 'function') {
				if (!_returnModel.propertyNameIsValid(prop)) {
					throw {
						name: "Invalid Property Exception",
						message: "the input model for command '" + _returnModel.PartName + "' does not contain a property named '" + prop + "'"
					}
				}

				var type = (prop == "AgentSystemName" || prop == "PartName") ? "hidden" : "text";
				var inputClass = "xlarge";
				var choices = _returnModel.getChoices(prop);

				if (type != "hidden") {
					var propertyType = _returnModel.getPropertyType(prop).toLowerCase();
					switch (propertyType) {
						case "httppostedfilebase":
							$(form).attr("enctype", "multipart/form-data");
							type = "file";
							inputClass = "input-file";
							break;
						case "boolean":
						case "bool":
							type = "checkbox";
							break;
						case "guid":
						case "type":
							type = "hidden";
							break;
						default:
							type = "text";
							break;
					}

					if (propertyType == "string" && prop.toLowerCase().endsWith("url")) {
						var filePropertyName = prop.substr(0, prop.toLowerCase().lastIndexOf("url"));
						if (_returnModel.hasOwnProperty(filePropertyName) && _returnModel.getPropertyType(filePropertyName).toLowerCase() == "httppostedfilebase") {
							type = "hidden";
						}
					}
				}

				var inputId = EUCLID.getId();
				var inputElementHtml = "<input name='" + prop + "' type='" + type + "' class='" + inputClass + "' id='" + inputId + "' value='" + _returnModel[prop] + "'/>";
				if (choices != null) {
					//todo: multiple choice support (_returnModel.MultiChoice == true)
					var options = "";
					$.each(choices.Values, function (index, item) {
						options += "<option value='" + item + "'";
						if (_returnModel[prop] == item) {
							options += " selected='selected'";
						}
						
						options += ">" + item + "</option>";
					});

					var html =
						"<div class='clearfix'>" +
							"<label for='" + inputId + "'>" + prop + "</label>" +
							"<div class='input'>" +
								"<select name='" + prop + "'>" +
									options +
								"</select>" +
							"</div>" +
						"</div>";

					$(fieldSet).append($(html));
				} else if (type != "hidden" && type != "checkbox") {
					var fieldContainer = $("<div class='clearfix'></div>");
					$(fieldContainer).append("<label for='" + inputId + "'>" + prop + "</label>");
					$(fieldContainer).append("<div class='input'>" + inputElementHtml + "</div>");

					$(fieldSet).append($(fieldContainer));
				} else if (type == "checkbox") {
					var html =
							"<div class='clearfix'>" +
								"<label for='" + inputId + "'>" + prop + "</label>" +
								"<div class='input'> " +
									"<div class='input-prepend'>" +
										"<label class='add-on'><input type='checkbox' name='" + prop + "' id='" + inputId + "' /></label>" +
										"<input type='text' value='' class='uneditable-input' disabled='disabled' />" +
									"</div>" +
								"</div>" +
							"</div>";

					$(fieldSet).append($(html));
				} else {
					$(fieldSet).append($(inputElementHtml));
				}
			}
		}

		$(form).append("<input type='submit' value='Publish " + _commandName + "' />");
		return form;
	});

	var _propertyNames = new Array();
	for (i = 0; i < _model.Properties.length; i++) {
		var prop = _model.Properties[i];
		_returnModel[prop.Name] = prop.Value;
		_propertyNames.push(prop.Name);
	}
	_returnModel["PartName"] = _commandName;

	return _returnModel;
});

EUCLID.displayError = (function (title, message) {
	if ($("#euclid-error-display").length == 0) {
		$("body").prepend("<div id='euclid-error-display' style='z-index:9999'></div>");
	}

	var error = $("#euclid-error-display");
	var message = $("<div class='error-message pinned'><a class='close' href='#'>×</a><p><strong>" + title.replace(/\n/g, "<br />") + "</strong> " + message.replace(/\n/g, "<br />") + ".</p></div>");

	$(error).append(message);
	$(window).scrollTop(0);
});


EUCLID.showModalForm = (function (args) {
	if (args === null || args === undefined || !args.hasOwnProperty("Url")) {
		throw new {
			name: "Invalid Argument Exception",
			message: "The argument object must contain a property named 'Url'"
		};
	};

	var id = EUCLID.getId();
	var modal = $("<div id='" + id + "'></div>");
	$(modal).load(
			args.Url,
			function () {
				$.validator.unobtrusive.parse($("#" + id));
			})
		.modal({
			autoResize: true,
			autoPosition: true,
			dataCss: { backgroundColor: "#fff" },
			containerCss: { backgroundColor: "#fff" }
		});

	$(".simplemodal-container").css("height", "auto");
});

$(document).ready(function () {
	$(".close").live('click', function () {
		$(this).parent().remove();
		return false;
	});

	$(window).scroll(function () {
		$(".pinned").each(function (index, item) {
			var offset = parseInt($(item).attr("data-offset"));
			var top = ($("body").scrollTop() + offset) + "px";

			$(item).css("top", top);
		});
	});
});

String.prototype.endsWith = function(suffix) {
	return this.indexOf(suffix, this.length - suffix.length) !== -1;
};