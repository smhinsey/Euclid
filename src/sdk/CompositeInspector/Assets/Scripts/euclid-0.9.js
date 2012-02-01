$.getScript("/composite/js/jquery/jquery.form.js");

if ($.validator == null || $.validator == undefined) {
	$.ajaxSetup({ "async": false });
		$.getScript("/composite/js/jquery/jquery.validate.min.js");
		$.getScript("/composite/js/jquery/jquery.validate.unobtrusive.min.js");
		$.ajaxSetup({ "async": true });
		
}

var EUCLID = function () {
	/* private methods */
	var _submitForm = (function (form, values) {
		var model = null;

		if (values != null && typeof values == "object") {
			for (valueName in values) {
				if (values.hasOwnProperty(valueName)) {
					var element = $(form).find("[name='" + valueName + "']");
					if ($(element).length > 0) {
						element.attr("value", values[valueName]);
					}
				}
			}
		}

		$.ajaxSetup({ async: false });
		$(form).ajaxSubmit({
			headers: {
				Accept: "application/json; charset=utf-8"
			},

			success: function (responseText, statusText, jqHxr, $form) {
				if (jqHxr.status == 500) {
					throw {
						name: model.name,
						message: model.message + "\n\n" + _model.callstack
					}
				} else {
					model = $.parseJSON(jqHxr.responseText);
				}
			},
			error: function (jqHxr, statusText) {
				throw $.parseJSON(jqHxr.responseText);
			}
		});
		$.ajaxSetup({ async: true });

		var re = new RegExp("\\/Date\\((-?\\d+)\\)\\/");
		for (property in model) {
			if (model.hasOwnProperty(property) && typeof property != "function") {
				var m = re.exec(model[property]);
				if (m != null) {
					model[property] = new Date(parseInt(m[1]));
				}
			}
		}

		return model;
	}); // end submitForm

	return {
		getQueryMetadata: (function (args) {
			/// <summary>
			/// Retrieve a JSON representation of a query
			///  &#10; .AgentSystemName
			///  &#10; .Namespace
			///  &#10; .Name
			///  &#10; .Methods (an array of method objects)
			///  &#10;  .Name
			///  &#10;  .ReturnType
			///  &#10;  .Arguments (an array of parameters to execute this query)
			///  &#10;    .ArgumentName
			///  &#10;    .ArgumentType, 
			///  &#10;    .Choices,
			///  &#10;    .MultiChoice (boolean indicates if multiple choices are allowed)
			/// </summary>
			/// <param name='args'>a JSON object with the following properties
			/// &#10; queryName: the name of the query object
			/// &#10; methodName: the name of the method that executes the query
			/// </param>
			throw { name: "Not Implemented Exception", message: "executeQuery is not implemented" };

			var model = null;
			_displayErrorWrapper({
				callback: function (args) {
					if (args === null || args === undefined || !args.hasOwnProperty("queryName")) {
						throw {
							name: "Invalid Argument Exception",
							message: "EUCLID.getQueryMethods expects an object with the properties: 'queryName'"
						};
					}

					var queryName = args.queryName;
					var getQueryMethodsUrl = "/composite/queries/" + queryName;
					model = _getJsonObject(getQueryMethodsUrl);
					if (args.hasOwnProperty("methodName")) {
						$.each(model.Methods, function (index, item) {
							if (item.Name.toLowerCase() == args.methodName.toLowerCase()) {
								model = item;
								return;
							}
						});
					}
				},
				callbackArgs: args
			});

			return model;
		}), // end getQueryMethods

		executeQuery: (function (args) {
			/// <summary>Executes an agent query and returns a JSON encoded representation of the results</summary>
			/// <param name='args'>a JSON object with the following properties
			/// &#10; queryName: the name of the query object
			/// &#10; methodName: the name of the method that executes the query
			/// &#10; parameters: an array of arguments represented as a JSON name value pair
			/// </param>
			throw { name: "Not Implemented Exception", message: "executeQuery is not implemented" };
			var model = null;
			_displayErrorWrapper({
				callback: function (args) {
					if (args == null || args === undefined || !args.hasOwnProperty("queryName") || !args.hasOwnProperty("methodName") || !args.hasOwnProperty("parameters")) {
						throw {
							name: "Invalid Argument Exception",
							message: "EUCLID.executeQuery expects an object with the properties: \nqueryName: the name of the query object\nmethodName: the name of the method to execute\nparameters: a JSON representation of the query parameters"
						}
					}

					var method = EUCLID.getQueryMetadata(args);
					var form = EUCLID.getQueryForm({ queryName: args.queryName, method: method });
					model = _submitForm(form, args.parameters);
				},
				callbackArgs: args
			});

			return model;
		}), // end executeQuery

		getId: (function () {
			///<summary>returns a pseudo-random GUID</summary>
			return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
		}),

		withFormResults: (function (form, onComplete, onError) {
			///<summary>submits a form</summary>
			///<param name='form'>a jquery form object</param>
			///<param name='onComplete'>callback for handling the results</param>
			///<param name='onError'>an optional error handler</param>
			var errorHandler = onError == null ? EUCLID.displayError : onError;

			$(form).ajaxSubmit({
				headers: {
					Accept: "application/json; charset=utf-8"
				},

				success: function (responseText, statusText, jqHxr, $form) {
					var data = $.parseJSON(jqHxr.responseText);

					if (jqHxr.status == 500) {
						errorHandler(data);
					} else {
						onComplete(data);
					}
				},
				error: function (jqHxr, statusText) {
					errorHandler($.parseJSON(jqHxr.responseText));
				}
			});
		}),

		getQueryForm: (function (args) {
			///<summary>retrieves a form object for collecting arguments for the specified query</summary>
			///<param name='args'>a JSON object containing the properties
			/// &#10; method - the name of the method to execute
			/// &#10; queryName - the name fo the query
			/// </param>
			throw { name: "Not Implemented Exception", message: "getQueryForm is not implemented" };

			if (args == null || args === undefined || !args.hasOwnProperty("method") || !args.hasOwnProperty("queryName")) {
				throw {
					name: "Invalid Argument Exception",
					message: "EUCLID.getQueryForm exepcts an object with the properties: 'method' and 'query'"
				}
			}

			var method = args.method;
			var queryName = args.queryName;
			var methodName = method.Name
			var form = $("<form action='/composite/queries/" + queryName + "/" + method.Name + "' method='post'><legend>" + method.Name + "</legend><fieldset></fieldset></form>");
			var fieldSet = $(form).children("fieldset");

			$.each(method.Arguments, function (index, item) {
				var forceShow = true; // methodName == "FindById" && item.ArgumentName == "id";
				_addElementToForm(item.ArgumentName, item.ArgumentType, "", item.Choices, item.MultiChoice, fieldSet, forceShow);
			});

			$(form).find(".input-date").datepicker();
			$(form).append("<input type='submit' value='" + method.Name + "' />");
			return form;
		}),

		withInputModel: (function (commandName, onSuccess, onError) {
			///<summary>retrieves a JSON representation of an input model and executes a callback</summary>
			///<param name='commandName'>the name of the command</param>
			///<param name='onSuccess'>a function that accepts an input model
			/// &#10; the input model object supports the following methods:
			/// &#10;  getForm() to retrieve a form for executing the command
			/// &#10;  publish() to publish the command
			/// </param>
			///<param name='onError'>optional error handler</param>
			/// </param>
			var errorHandler = (onError == null) ? EUCLID.displayError : onError;

			if (commandName == null) {
				var e = {
					name: "Invalid Argument Exception",
					message: "A command name must be specified"
				};

				errorHandler(e);
			}

			GetData("/composite/api/command/" + commandName,
				function (data) {
					try {
						var inputModel = GetInputModel(commandName, data);
						onSuccess(inputModel);
					} catch (e) {
						errorHandler(e);
					}
				});
		}), // with getInputModel

		pollForCommandStatus: (function (args) {
			///<summary>naviely polls the composite for the status of a given command
			/// &#10;  this implementation creates a new request for each poll, and is not appropriate for use in a high volume situation
			///</summary>
			///<param name='args'>a JSON object containing the properties
			/// &#10; publicationId- the id of the command
			/// &#10; pollMax - the maximum number of times to poll (optional, default = 100)
			/// &#10; pollInterval - the time between requests (optional, default 250ms)
			/// &#10; onCommandComplete - a callback function that is called if the command has completed succesfully (accepts a JSON representation of a CommandPublicationRecord)
			/// &#10; onCommandError - a callback function that is called if the command has errored (accepts a JSON representation of a CommandPublicationRecord)
			/// &#10; onPollError - a callback function that is called if an error occurs during the polling operation (accepts a JSON object with .name, .message & .callStack)
			/// </param>
			// publicationId, onOpportunityToCancelPolling, onCommandComplete, onCommandError, onPollError
			var _pollMax = 0;
			var _pollInterval = 0;
			var _pollCount = 0;
			var _pollerId = -1;

			if (!args.hasOwnProperty("publicationId") || !args.hasOwnProperty("onCommandComplete") || !args.hasOwnProperty("onCommandError")) {
				throw {
					name: "Invalid Argument Exception",
					message: "EUCLID.pollForCommandStatus expects an object that contains: publicationId and the functions onCommandCompleted & onCommandError.  Optionally you may specify the property Interval (number of ms between status requests) and the functions onOpportunityToCancelPolling (a function that accepts the number of times the registry has been polled, and returns false to stop polling), and the function onPollError (accepts a standard error object)"
				}
			}

			_pollMax = (args.pollMax == null || args.pollMax <= 0) ? 100 : args.pollMax;
			_pollInterval = (args.pollInterval == null || args.pollInterval <= 0) ? 250 : args.pollInterval;
			_pollerId = setInterval(doPoll, _pollInterval);

			function doPoll() {
				var errorHandler = args.hasOwnProperty("onPollError") ? args.onPollError : EUCLID.displayError;

				GetData(
					"/composite/commands/status/" + args.publicationId,
					function (result) {
						_pollCount++;
						complete = result.Completed;

						if (result.Error) {
							clearInterval(_pollerId);
							args.onCommandError(result);
						} else if (result.Completed) {
							clearInterval(_pollerId);
							args.onCommandComplete(result);
						} else {
							var e = { name: "", message: "" };
							if (args.hasOwnProperty("onOpportunityToCancelPolling")) {
								continuePolling = args.onOpportunityToCancelPolling(_pollCount);
								e.name = "Polling Aborted Exception";
								e.message = "Polling cancelled by caller";
							}

							if (continuePolling) {
								continuePolling = _pollCount < _pollMax;
								e.name = "Polling Max Reached";
								e.message = "Polled maximum number of time: " + _pollMax;
							}

							if (!continuePolling) {
								clearInterval(_pollerId);
								if (args.hasOwnProperty("onPollError")) {
									args.onPollError(e);
								}
							}
						}
					},
					errorHandler
				);
			} // doPoll
		}), // end pollForCommandStatus

		displayError: (function (e) {
			///<summary>displays an error</summary>
			///<param name='e'>an object containing the error information expected properties are .name, .message & .callstack</param>
			// publicationId, onOpportunityToCancelPolling, onCommandComplete, onCommandError, onPollError
			if ($("#euclid-error-display").length == 0) {
				$("body").prepend("<div id='euclid-error-display' style='z-index:auto'></div>");
			}

			var error = $("#euclid-error-display");

			EUCLID.populateTemplate({
				templateUrl: "/composite/ui/template/euclid-error",
				data: e,
				onComplete: function (content) {
					$(error).append($(content));
					$(window).scrollTop(0);
				}
			});

			return false;
		}), // end displayError

		showModalForm: (function (args) {
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
		}), // end showModalForm

		populateTemplate: (function (args) {
			///<summary>fetch a handlebars template, and populate with the provided data, returns a jquery object containing the content</summary>
			///<param name='args'>a JSON object containing the properties:
			/// &#10; templateUrl - the url from which to fetch the template
			/// &#10; dataUrl - the url from which to fetch the data (must be null if data is provided)
			/// &#10; data - the data to bind (must be null if dataUrl is provided)
			/// &#10; onComplete - a callback function that receives the populated template
			/// &#10; onError - (optional) to override the default error handling
			///<param>
			var onError = args.hasOwnProperty("onError") ? args.onError : EUCLID.displayError;

			// TODO: clean up this method
			if (!args.hasOwnProperty("onComplete") || typeof args.onComplete != 'function') {
				onError({
					name: "Invalid Argument Exception",
					message: "The parameter onComplete - a callback for receiving the populated template - must be provided"
				});
			}

			if (!args.hasOwnProperty("dataUrl") && !args.hasOwnProperty("data")) {
				onError({
					name: "Invalid Argument Exception",
					message: "Either dataUrl or data must be specified"
				});
			}

			if (args.hasOwnProperty("dataUrl") && args.hasOwnProperty("data")) {
				onError({
					name: "Invalid Argument Exception",
					message: "Both dataUrl and data contain values, only one can be specified"
				});
			}

			if (args.dataUrl != null) {
				GetData(args.DataUrl,
					function (data) {
						$.get(args.templateUrl, function (source) {
							var template = Handlebars.compile(source);
							args.onComplete($(template(args.data)));
						});
					}),
					onError;
			} else {
				$.get(args.templateUrl, function (source) {
					var template = Handlebars.compile(source);
					args.onComplete($(template(args.data)));
				});
			}
		}) // populateTemplate
	}
} ();

// extension methods
$.fn.hasAttr = function(name) {
	///<summary>easily check if an element contains an attribute<summary>
	///<param name='name'>The name of the attribute to check for</param>
	return this.attr(name) !== undefined;
};

String.prototype.endsWith = function(suffix) {
	///<summary>returns true if the string ends with the specified suffix (case sensitive)</summary>
	///<param name='suffix'>the string the check for</param>
	return this.indexOf(suffix, this.length - suffix.length) !== -1;
};

if (jQuery.validator != null) {
	jQuery.validator.addMethod('uniquevalue', function (value, element, params) {
		var query = $(element).attr("data-val-uniquevalue-query");
		var method = $(element).attr("data-val-uniquevalue-method");
		var argument = $(element).attr("data-val-uniquevalue-argument");
		var argumentObject = $.parseJSON("{ \"" + argument + "\": \"" + value + "\"}")
		var results = EUCLID.executeQuery({ queryName: query, methodName: method, parameters: argumentObject });

		return results == null;
	}, '');

	// and an unobtrusive adapter
	jQuery.validator.unobtrusive.adapters.add('uniquevalue', {}, function (options) {
		options.rules['uniquevalue'] = true;
		options.messages['uniquevalue'] = options.message;
	});
}

if (Handlebars) {
	Handlebars.registerHelper('convert-breaks', function (value) {
		var replaced = value.replace(/\n/g, "<br />");
		console.log("convert-breaks: " + Handlebars.SafeString(replaced));
		return new Handlebars.SafeString(replaced);
	});
}

var With = function (jsonObject) {
	///<sumary> Operate on a data</summary>
	///<param name='jsonObject'>A JSON data structure</param>
	var _data = jsonObject;
	return {
		Fill: function (elementId) {
			/// <summary> Add data to the element with the specified id</summary>
			/// <param name='elementId'> The id of the container element </param>
			if (elementId.substr(0, 1) != "#") {
				elementId = "#" + elementId;
			}

			var _element = $(elementId);

			return {
				From: function (templateUrl) {
					/// <summary> Get a template for rendering the JSON data structure</summary>
					/// <param name='templateUrl'> the Url of the template to render the data structure</param>
					EUCLID.populateTemplate({
						templateUrl: templateUrl,
						data: _data,
						onComplete: function (content) {
							$(_element).html("");
							$(_element).append($(content));
						}
					});
				}
			}
		}, // fill

		Render: function (templateUrl) {
			/// <summary> Renders a template for the data</summary>
			/// <param name='templateUrl'> the url of the template</param>
			var _templateUrl = templateUrl;

			return {
				Manipulate: function (callback) {
					/// <summary>Do something with the rendered template</summary>
					/// <param name='callback'>function that receives the completed template</param>
					EUCLID.populateTemplate({
						templateUrl: _templateUrl,
						data: _data,
						onComplete: function (content) {
							callback(content);
						}
					});
				}
			}
		} // in
	}
};

var GetData = function (url, onSuccess, onError) {
	/// <summary> fetches a json object and executes a callback </summary>
	/// <param name='url'>the url from which to fetch the data</param>
	/// <param name='onSuccess'>a callback that accepts the json object</param>
	/// <param name='onError'>optional error handler</param>
	$.ajax({
		url: url,
		dataType: 'json',
		headers: {
			Accept: "application/json; charset=utf-8"
		},
		success: function (obj) {
			var re = new RegExp("\\/Date\\((-?\\d+)\\)\\/");
			for (property in obj) {
				if (obj.hasOwnProperty(property) && typeof property != "function") {
					var m = re.exec(obj[property]);
					if (m != null) {
						obj[property] = new Date(parseInt(m[1]));
					}
				}
			}

			onSuccess(obj);
		},
		error: function (e) {
			if (onError != null) {
				onError(e);
			} else {
				EUCLID.displayError(e);
			}
		}
	});
}

$(document).ready(function () {
	$(".confirmation-dialog").live("click", function () {
		var msg = $(this).attr("data-confirmation-message");
		var confirmFunction = $(this).attr("data-confirm-function");
		var itemId = $(this).attr("data-item-id");
		var override = $(this).attr("data-override");

		if (override == true) return true;

		if (isNullOrEmpty(msg) || isNullOrEmpty(confirmFunction) || isNullOrEmpty(itemId)) {
			$("<div id='delete-usage-error'>" +
				"<div class='notification error' >" +
				"	<div><strong>Required Attributes Missing</strong></div>" +
				"	<p>" +
				"		<b>data-confirmation-message</b>: the message to display in the confirmation pop-up." +
				"	</p>" +
				"	<p>" +
				"		<b>data-confirm-function</b>: the name of the function to execute once the user confirms the deletion." +
				"	</p>" +
				"	<p>" +
				"		<b>data-item-id</b>: the id of the element to delete." +
				"	</p>" +
				"</div>" +
			"</div>").modal();
		} else {
			$("<div id='confirm' class='notification attention' style='height: 300px'> " +
				"<p> " +
				"	<strong>Confirmation</strong>" +
					msg +
				"</p> " +
				"<a href='#' class='yes button-link' style='float: right; margin-left: 10px'>Yes</a> " +
				"<a href='#' class='no simplemodal-close button-link' style='float: right'>No</a> " +
			"</div>")
				.modal({
					onShow: function (dialog) {
						var modal = this;

						// if the user clicks "yes"
						$('.yes', dialog.data[0]).live('click', function () {
							// call the callback
							eval(confirmFunction)(itemId);

							// close the dialog
							modal.close(); // or $.modal.close();
						});
					}
				});
		}

		return false;
	});
});

var GetInputModel = function (commandName, data) {
	///<summary>gets an input model object</summary>
	/// <param name='commandName'>the command associated with this input model</param>
	/// <param name='data'>the json representation of the input model properties</param>
	if (data == null) {
		throw ({
			name: "Invalid Argument Exception",
			message: "The parameter data cannot be null"
		});
	}

	var _propertyNames = new Array();
	var _model = (function () {
		var _propertyNameIsValid = (function (name) {
			var found = ($.inArray(name, _propertyNames) > -1 || name === "PartName");
			return found;
		});

		var _getPropertyType = (function (propertyName) {
			if (propertyName.toLowerCase() == "partname") {
				return "String";
			}

			for (i = 0; i < data.Properties.length; i++) {
				var obj = data.Properties[i];
				if (obj.Name.toLowerCase() == propertyName.toLowerCase()) {
					return obj.Type;
				}
			}

			throw ({
				name: "Invalid Property Exception",
				message: "The property '" + propertyName + "' does not exist on the inputModel"
			});
		});

		var _getChoices = (function (propertyName) {
			for (i = 0; i < data.Properties.length; i++) {
				var obj = data.Properties[i];
				if (obj.Name.toLowerCase() == propertyName.toLowerCase()) {
					return (obj.Choices === null) ? null : { Values: obj.Choices, MultiChoice: obj.MultiChoice };
				}
			}

			return null;
		});

		return {
			getForm: (function () {
				///<summary>returns a jquery object containing a form that can be used to collect data for this command</summary>
				var form = $("<form action='/composite/commands/publish' method='post'><fieldset></fieldset></form>");
				var fieldSet = $(form).children("fieldset");

				for (propertyName in _model) {
					if (_model.hasOwnProperty(propertyName) && typeof _model[propertyName] !== 'function') {
						if (!_propertyNameIsValid(propertyName)) {
							throw {
								name: "Invalid Property Exception",
								message: "the input model for command '" + _model.PartName + "' does not contain a property named '" + propertyName + "'"
							}
						}

						var propertyType = _getPropertyType(propertyName);
						var choiceObject = _getChoices(propertyName);
						var propertyChoices = choiceObject == null ? null : choiceObject.Values;
						var multiChoice = choiceObject == null ? false : choiceObject.MultiChoice;
						var forceShow = propertyType.toLowerCase() == "guid";
						_addElementToForm(propertyName, propertyType, _model[propertyName], propertyChoices, multiChoice, fieldSet, forceShow);
					}
				}

				$(form).find(".input-date").datepicker();
				return form;
			}), // end getForm

			publish: (function () {
				/// <summary> publishes this command for processing</summary>
				var form = this.getForm();
				EUCLID.submitForm(form);
			}) // end publish
		}
	})(); // end model definiton

	for (i = 0; i < data.Properties.length; i++) {
		var prop = data.Properties[i];
		_model[prop.Name] = prop.Value;
		_propertyNames.push(prop.Name);
	}

	_model["PartName"] = commandName;
	return _model;
}

var _addElementToForm = (function (propertyName, propertyType, propertyValue, choices, allowMultiChoice, fieldSet, forceShow) {
	var inputClass = "xlarge";
	switch (propertyType.toLowerCase()) {
		case "httppostedfilebase":
		case "byte[]":
			$(fieldSet).parent().attr("enctype", "multipart/form-data");
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

	var inputId = EUCLID.getId();
	var html = "";
	if (choices != null) {
		var options = "";
		$.each(choices, function (index, item) {
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
		if (("" + propertyValue).toLowerCase() == "true" || ("" + propertyValue).toLowerCase() == "yes" || ("" + propertyValue).toLowerCase() == "on" || ("" + propertyValue).toLowerCase() == "1") {
			inputElement = "<input id='" + inputId + "' type='checkbox' name='" + propertyName + "'  checked='checked' />";
		} else {
			inputElement = "<input id='" + inputId + "' type='checkbox' name='" + propertyName + "' />";
		}

		html = "<div class='clearfix'>" +
								"<label for='" + inputId + "'>" + propertyName + "</label>" +
								"<div class='input'> " +
									"<div class='input-prepend'>" +
										"<label class='add-on'>" + inputElement + "</label>" +
										"<input type='text' disabled='disabled' />" +
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
