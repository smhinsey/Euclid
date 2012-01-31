﻿$.getScript("/composite/js/jquery/jquery.form.js");

if ($.validator == null || $.validator == undefined) {
	$.ajaxSetup({ "async": false });
		$.getScript("/composite/js/jquery/jquery.validate.min.js");
		$.getScript("/composite/js/jquery/jquery.validate.unobtrusive.min.js");
		$.ajaxSetup({ "async": true });
		
}

var EUCLID = function () {
	/* private methods */
	var _getJsonObject = (function (url) {
		$.ajaxSetup({ "async": false })

		var jqHxr = $.ajax({
						url: url,
						dataType: 'json',
						headers: {
							Accept : "application/json; charset=utf-8"
						}
					});
		var _model = $.parseJSON(jqHxr.responseText);
		$.ajaxSetup({ "async": true });

		if (jqHxr.status == 500) {
			throw _model;
		} 
		
		if (_model === null) {
			throw {
				name: "Invalid QueryName Exception",
				message: "Could not GET JSON from url : " + url
			};
		}

		return _model;
	}); // end getJsonObject

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
				Accept : "application/json; charset=utf-8"
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
				var model = $.parseJSON(jqHxr.responseText);
				throw {
					name: model.name,
					message: model.message + "\n\n" + _model.callstack
				}
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

		var inputId = _getId();
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

	var _displayErrorWrapper = (function (args) {
		if (args === null || args === undefined || !args.hasOwnProperty("callbackArgs") || !args.hasOwnProperty("callback")) {
			throw {
				name: "Invalid Argument Exception",
				message: "_displayErrorWrapper exepects the parameters 'callback' - the function to execute, and 'callbackArgs' - an object containing the arguments to pass into the function"
			}
		}

		try {
			args.callback(args.callbackArgs);
		} catch (e) {
			if (args.callbackArgs.hasOwnProperty("handleError")) {
				args.callbackArgs.handleError(e);
			} else {
				EUCLID.displayError(e);
			}
		}
	});

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
					var getQueryMethodsUrl = "/composite/queries/" + queryName + ".json";
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
			var id = "";
			_displayErrorWrapper({
				callback: function (args) {
					id = _getId();
				},
				callbackArgs: null
			});

			return id;
		}),

		getJsonObject: (function (url) {
			///<summary>synchrnously retrieves a JSON object from the specified URL</summary>
			///<param name='url'>the url that provides the required data</param>
			var object = null;
			_displayErrorWrapper({
				callback: function (args) {
					object = _getJsonObject(args);
				},
				callbackArgs: url
			});

			return object;
		}),

		submitForm: (function (form, namedArguments) {
			///<summary>submits a form</summary>
			///<param name='form'>a jquery form object</param>
			///<param name='namedArguments'>an array of JSON encoded name/value pairs</param>
			return _submitForm(form, namedArguments);
		}),

		getQueryForm: (function (args) {
			///<summary>retrieves a form object for collecting arguments for the specified query</summary>
			///<param name='args'>a JSON object containing the properties
			/// &#10; method - the name of the method to execute
			/// &#10; queryName - the name fo the query
			/// </param>
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

		getInputModel: (function (args) {
			///<summary>retrieves a JSON representation of an input model for a command
			/// &#10; the returned object supports the following methods:
			/// &#10;  getForm() to retrieve a form for executing the command
			/// &#10;  publish() to publish the command
			///</summary>
			///<param name='args'>a JSON object containing the properties
			/// &#10; commandName - the name of the command to execute
			/// &#10; agentSystemName - the name of the agent that supports the command
			/// </param>
			var _model = null;
			_displayErrorWrapper({
				callbackArgs: args,
				callback: function (args) {
					if (args === null || args === undefined || !args.hasOwnProperty("commandName") || !args.hasOwnProperty("agentSystemName")) {
						throw {
							name: "Invalid Argument Exception",
							message: "EUCLID.getInputModel expects an an object with the properties: 'commandName' and 'agentSystemName'"
						};
					}

					var _rawModel = _getJsonObject("/composite/commands/" + args.agentSystemName + "/" + args.commandName + ".json");
					var _propertyNames = new Array();
					_model = (function () {
						var _isInputModel = (function () {
							return true;
						});

						var _propertyNameIsValid = (function (name) {
							var found = ($.inArray(name, _propertyNames) > -1 || name === "PartName");
							return found;
						});

						var _getPropertyType = (function (propertyName) {
							if (propertyName.toLowerCase() == "partname") {
								return "String";
							}

							for (i = 0; i < _rawModel.Properties.length; i++) {
								var obj = _rawModel.Properties[i];
								if (obj.Name.toLowerCase() == propertyName.toLowerCase()) {
									return obj.Type;
								}
							}

							throw {
								name: "Invalid Property Exception",
								message: "The property '" + propertyName + "' does not exist on the inputModel"
							};
						});

						var _getChoices = (function (propertyName) {
							for (i = 0; i < _rawModel.Properties.length; i++) {
								var obj = _rawModel.Properties[i];
								if (obj.Name.toLowerCase() == propertyName.toLowerCase()) {
									return (obj.Choices === null) ? null : { Values: obj.Choices, MultiChoice: obj.MultiChoice };
								}
							}

							return null;
						});

						return {
							getForm: (function () {
								///<summary>returns a jquery object containing a form that can be used to collect data for this command</summary>
								var form = $("<form action='/composite/commands/publish' method='post'><legend>" + args.commandName + "</legend><fieldset></fieldset></form>");
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
								// $(form).append("<input type='submit' value='" + args.commandName + "' />");
								return form;
							}), // end getForm

							publish: (function () {
								/// <summary> publishes this command for processing</summary>
								var form = this.getForm();
								EUCLID.submitForm(form);
							}) // end publish
						}
					})();

					for (i = 0; i < _rawModel.Properties.length; i++) {
						var prop = _rawModel.Properties[i];
						_model[prop.Name] = prop.Value;
						_propertyNames.push(prop.Name);
					}
					_model["PartName"] = args.commandName;
				}
			});

			return _model;
		}), // end getInputModel

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

			_displayErrorWrapper({
				callbackArgs: args,
				callback: function() {
					if (!args.hasOwnProperty("publicationId") || !args.hasOwnProperty("onCommandComplete") || !args.hasOwnProperty("onCommandError")) {
						throw {
							name: "Invalid Argument Exception",
							message: "EUCLID.pollForCommandStatus expects an object that contains: publicationId and the functions onCommandCompleted & onCommandError.  Optionally you may specify the property Interval (number of ms between status requests) and the functions onOpportunityToCancelPolling (a function that accepts the number of times the registry has been polled, and returns false to stop polling), and the function onPollError (accepts a standard error object)"
						}
					}

					_pollMax = (args.pollMax == null || args.pollMax <= 0) ? 100 : args.pollMax;
					_pollInterval = (args.pollInterval == null || args.pollInterval <= 0) ? 250 : args.pollInterval;

					_pollerId = setInterval(doPoll, _pollInterval);
				}
			});

			function doPoll() {
				try {
					var result = _getJsonObject("/composite/commands/status/" + args.publicationId);
				} catch (e) {
					if (args.hasOwnProperty("onPollError")) {
						args.onPollError(e);
					} else {
						EUCLID.displayError(e);
					}
				}

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
			}
		}), // end pollForCommandStatus

		displayError: (function (e) {
			///<summary>displays an error</summary>
			///<param name='e'>an object containing the error information expected properties are .name, .message & .callstack</param>
			// publicationId, onOpportunityToCancelPolling, onCommandComplete, onCommandError, onPollError
			if ($("#euclid-error-display").length == 0) {
				$("body").prepend("<div id='euclid-error-display' style='z-index:auto'></div>");
			}

			var error = $("#euclid-error-display");

			var errorName = "";
			var errorMessage = "";
			var callStack = "";

			if (e.hasOwnProperty("name")){
				errorName = e.name.replace(/\n/g, "<br />");
			}

			if (e.hasOwnProperty("message")) {
				errorMessage = e.message.replace(/\n/g, "<br />");
			}

			if (e.hasOwnProperty("callStack")) {
				callStack = e.callStack.replace(/\n/g, "<br/>");
			}

			var message =  "<div class='alert alert-block alert-error' data-alert='alert'>" +
								"<a class='close' href='#' data-dismiss='alert'>×</a>" +
								"<h4 class='alert-heading'>" + errorName + "</h4>" +
								"<p>" + errorMessage + ".</p>" +
								"<p>" + callStack + "</p>" +
							"</div>";

			$(error).append($(message));
			$(window).scrollTop(0);

			return false;
		}), // end displayError

		showModalForm: (function (args) {
			if (args === null || args === undefined || !args.hasOwnProperty("Url")) {
				throw new {
					name: "Invalid Argument Exception",
					message: "The argument object must contain a property named 'Url'"
				};
			};

			var id = _getId();
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

$(document).ready(function () {
	$(window).scroll(function () {
		var top = $("body").scrollTop() + "px";
		$("#euclid-error-display").css("top", top);
	});

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