var EUCLID = {};

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
	var result = $.post(inputModel.getPublishUrl(), $(form).serialize());
	$.ajaxSetup({ "async": true });

	return $.parseJSON(result.responseText).publicationId;
});

EUCLID.getModel = (function (args) {
	if (args === null || args === undefined || !args.hasOwnProperty("commandName") || !args.hasOwnProperty("agentSystemName")) {
		throw {
			name: "Invalid Argument Exception",
			message: "EUCLID.getInputModelPublisher expects an an object with the properties: commandName, agentSystemName"
		};
	}

	var _agentSystemName = args.agentSystemName;
	var _commandName = args.commandName;
	var _getInputModelUrl = "/CompositeInspector/agents/ViewInputModelForCommand/" + _commandName + ".json";
	var _publishInputModelUrl = "/CompositeInspector/agents/Publish/";

	$.ajaxSetup({ "async": false });
	var jqHxr = jQuery.getJSON(_getInputModelUrl);
	var _model = $.parseJSON(jqHxr.responseText);
	$.ajaxSetup({ "async": true });

	if (_model === null) {
		throw {
			name: "Invalid InputModel Exception",
			message: "Could not retrieve the input model for command '" + _commandName + "' from agent '" + _agentSystemName + "'"
		};
	}

	var _returnModel = {};

	_returnModel.isInputModel = (function () {
		return true;
	});

	_returnModel.getPublishUrl = (function () {
		return _publishInputModelUrl;
	});

	_returnModel.propertyNameIsValid = (function (name) {
		var found = ($.inArray(name, _propertyNames) > -1 || name === "PartName");
		console.log("$.inArray(" + name + ", _propertyNames) = " + found);

		return found;
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
	var message = $("<div class='error-message pinned'><a class='close' href='#'>×</a><p><strong>" + title + "</strong> " + message + ".</p></div>");

	$(error).append(message);
	$(window).scrollTop(0);
});

EUCLID.getId = (function () {
	var now = new Date();
	return Date.UTC(now.getFullYear(), now.getMonth(), now.getDay(), now.getHours(), now.getMinutes(), now.getSeconds(), now.getMilliseconds());
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