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
		getQueryMethods: (function (args) {
			if (args === null || args === undefined || !args.hasOwnProperty("queryName")) {
				throw {
					name: "Invalid Argument Exception",
					message: "EUCLID.getQueryMethods expects an object with the properties: 'queryName'"
				};
			}

			var _queryName = args.queryName;
			var _getQueryMethodsUrl = "/composite/queries/" + _queryName + ".json";
			var _queryModel = getJsonObject(_getQueryMethodsUrl);

			var _methodList = new Array();
			$.each(_queryModel.Methods, function (index, item) {
				_methodList.push("<a href='/composite/queries/execute/" + _queryName + "/" + item.Name + "'>" + item.Name + "</a>");
			});

			return _methodList;
		}) // end getQueryMethods
	}
} ();