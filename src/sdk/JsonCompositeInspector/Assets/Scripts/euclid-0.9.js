var EUCLID = function () {
	/* private methods */
	var getFormElement = function (propertyName, model) {
		alert("getting form element for property");
	}

	var getJsonObject = function (url) {
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
		}

		return _model;
	}

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
			return getJsonObject(_getQueryMethodsUrl);
		}) // end getQueryMethods
	}
} ();