var EUCLID = {};

EUCLID.getModel = (function (args) {
	if (args === null || args === undefined || !args.hasOwnProperty("commandName") || !args.hasOwnProperty("agentSystemName")) {
		throw {
			name: "Invalid argument object",
			message: "EUCLID.getInputModelPublisher expects an an object with the properties: commandName, agentSystemName"
		};
	}

	var _agentSystemName = args.agentSystemName;
	var _commandName = args.commandName;
	var _getInputModelUrl = "/CompositeInspector/agents/" + _agentSystemName + "/ViewInputModelForCommand/" + _commandName + ".json";
	var _publishInputModelUrl = "/CompositeInspector/agents/Publish/";
	
	$.ajaxSetup({ "async": false });
	var jqHxr = jQuery.getJSON(_getInputModelUrl);
	var _model = $.parseJSON(jqHxr.responseText);
	$.ajaxSetup({ "async": true });

	if (_model === null) {
		throw {
			name: "Invalid input model",
			message: "Could not retrieve the input model for command '" + _commandName + "' from agent '" + _agentSystemName + "'"
		};
	}

	var _returnModel = {};
	for (i = 0; i < _model.Properties.length; i++) {
		var prop = _model.Properties[i];
		_returnModel[prop.Name] = prop.Value;
	}

	_returnModel.publish = function () {
		var form = $("<form method='POST' action='" + _publishInputModelUrl + "'></form>");
		$(form).append($("<input type='hidden' name='PartName' value='" + _commandName + "' />"));

		for (i = 0; i < _model.Properties.length; i++) {
			var prop = _model.Properties[i];
			$(form).append($("<input type='hidden' name='" + prop.Name + "' value='" + this[prop.Name] + "' />"));
		}

		$.post(_publishInputModelUrl, $(form).serialize());
	};

	return _returnModel;
});