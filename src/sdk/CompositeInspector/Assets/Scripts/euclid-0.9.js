$.getScript("/composite/js/jquery/jquery.form.js");

if ($.validator == null || $.validator == undefined) {
	$.getScript("/composite/js/jquery/jquery.validate.min.js");
	$.getScript("/composite/js/jquery/jquery.validate.unobtrusive.min.js");
}

var EUCLID = function () {
	/* private methods */
	var _getQueryForm = function (id) {
		var form = "<form ";
		if (id) {
			form += "id='" + id + "'";
		}
		form += "action='/composite/api/execute/query/" + this.queryName + "/" + this.Name + "' method='post'><legend style='display:none'>" + this.queryName + "." + this.Name + "</legend><fieldset></fieldset></form>";
		form = $(form);
		var fieldSet = $(form).children("fieldset");

		var method = this;
		$.each(this.Arguments, function (index, item) {
			var forceShow = true; // methodName == "FindById" && item.ArgumentName == "id";
			_addElementToForm(item.ArgumentName, item.ArgumentType, method[item.ArgumentName], item.Choices, item.MultiChoice, fieldSet, forceShow);
		});

		$(form).find(".input-date").datepicker();
		return form;
	} // end _getQueryForm

	var _getMethodByName = function (methodName, numberArguments) {
		var method;
		$.each(this.Methods, function (index, item) {
			if (item.Name == methodName && item.Arguments.length == numberArguments) {
				method = item;
				return;
			}
		});

		if (!method) {
			throw {
				name: "Method Not Found Exception",
				message: "There is not method named '" + methodName + "' with " + numberArguments + " on the query '" + this.Name + "'"
			};
		}

		$.each(method.Arguments, function (index, item) {
			method[item.ArgumentName] = "";
		});

		return method;
	}

	var _getInputModel = function (commandName, data) {
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
					var form = $("<form action='/composite/api/publish' method='post'><fieldset></fieldset></form>");
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

	var _parseDate = function (data) {
		//parese date
		var re = new RegExp("\\/Date\\((-?\\d+)\\)\\/");
		for (property in data) {
			if (data.hasOwnProperty(property) && typeof property != "function") {
				var m = re.exec(data[property]);
				if (m != null) {
					data[property] = new Date(parseInt(m[1]));
				}
			}

			if (data[property] instanceof Array) {
				$.each(data[property], function (index, item) {
					 _parseDate(item);
				});
			}
		}
	} // end _parseDAte

	return {
		getId: (function () {
			///<summary>returns a pseudo-random GUID</summary>
			return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
		}),

		withQuery: (function (query, onComplete, onError) {
			/// <summary>
			/// Retrieve a JSON representation of a query
			///  &#10; Properties:
			///  &#10;   .AgentSystemName
			///  &#10;   .Namespace
			///  &#10;   .Name
			///  &#10;   .Methods (an array of method objects)
			///  &#10;    .Name
			///  &#10;    .ReturnType
			///  &#10;    .Arguments (an array of parameters to execute this query)
			///  &#10;      .ArgumentName
			///  &#10;      .ArgumentType, 
			///  &#10;      .Choices,
			///  &#10;      .MultiChoice (boolean indicates if multiple choices are allowed)
			///  &#10; Methods:
			///  &#10;  .getForm
			/// </summary>
			/// <param name='query'>the name of the query object</param>
			/// <param name='onComplete'>a callback that accepts the query metadata</param>
			/// <param name='onError'>optional - error handler</param>
			var errorHandler = onError ? onError : EUCLID.displayError;

			if (query == null || onComplete == null) {
				errorHandler(new { name: "Invalid Argument Exception", message: "both 'query' and 'onComplete' are required parameters to EUCLID.withQueryMetadata" });
				return null;
			}

			WorkWithDataFromUrl("/composite/api/query-metadata/" + query,
				function (data) {
					data.getMethodNamed = _getMethodByName;
					$.each(data.Methods, function (idx, item) {
						item.queryName = query;
						item.getForm = _getQueryForm;
					});

					onComplete(data);
				},
				errorHandler);
		}), // end getQueryMethods

		withFormResults: (function (form, onComplete, onError) {
			///<summary>submits a form</summary>
			///<param name='form'>a jquery form object</param>
			///<param name='onComplete'>callback for handling the results</param>
			///<param name='onError'>an optional error handler</param>
			var errorHandler = onError == null ? EUCLID.displayError : onError;
			var innerParseDate = _parseDate;
			$(form).ajaxSubmit({
				headers: {
					Accept: "application/json; charset=utf-8"
				},

				success: function (responseText, statusText, jqHxr, $form) {
					var data = $.parseJSON(jqHxr.responseText);

					if (data instanceof Array) {
						$.each(data, function (index, item) {
							innerParseDate(item);
						});
					} else {
						innerParseDate(data);
					}

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
			var getInputModel = _getInputModel;

			if (commandName == null) {
				var e = {
					name: "Invalid Argument Exception",
					message: "A command name must be specified"
				};

				errorHandler(e);
			}

			WorkWithDataFromUrl("/composite/api/command/" + commandName,
				function (data) {
					try {
						var inputModel = getInputModel(commandName, data);
						onSuccess(inputModel);
					} catch (e) {
						errorHandler(e);
					}
				},
				errorHandler);
		}), // end withInputModel

		pollForCommandStatus: (function (publicationId, onCommandComplete, onCommandError, pollMax, pollInterval, onPollError, onBeforePoll) {
			///<summary>naviely polls the composite for the status of a given command
			/// &#10;  this implementation creates a new request for each poll, and is not appropriate for use in a high volume situation
			///</summary>
			///<param name='publicationId'>the id of the command</param>
			///<param name='onCommandComplete'>a callback function that is called if the command has completed succesfully (accepts a JSON representation of a CommandPublicationRecord)</param>
			///<param name='onCommandError'>optional - a callback function that is called if the command has errored, if none is specified, then onCommandComplete is called</param>
			///<param name='pollMax'>optional - the maximum number of times to poll (default = 100)</param>
			///<param name='pollInterval'>optional - the time between requests (default 250ms)</param>
			///<param name='onPollError'>optional - a callback function that is called if an error occurs during the polling operation</param>
			///<param name='onBeforePoll'>optional - a callback function that recieves the number of times polled, and can cancel polling by returning false</param>
			if (publicationId == null || onCommandComplete == null) {
				throw {
					name: "Invalid Argument Exception",
					message: "EUCLID.pollForCommandStatus expects an object that contains: publicationId and the functions onCommandCompleted & onCommandError.  Optionally you may specify the property Interval (number of ms between status requests) and the functions onOpportunityToCancelPolling (a function that accepts the number of times the registry has been polled, and returns false to stop polling), and the function onPollError (accepts a standard error object)"
				}
			}

			var _pollMax = pollMax ? pollMax : 100;
			var _pollInterval = pollInterval ? pollInterval : 250;
			var _pollCount = 0;
			var _errorHandler = onPollError ? onPollError : EUCLID.displayError;
			var _commandErrorHandler = onCommandError ? onCommandError : onCommandComplete;
			var _poll = function () {
				WorkWithDataFromUrl(
						"/composite/api/publicationRecord/" + publicationId,
						function (result) {
							_pollCount++;
							complete = result.Completed;

							if (result.Error) {
								_commandErrorHandler(result);
							} else if (result.Completed) {
								onCommandComplete(result);
							} else {
								var e = { name: "", message: "" };
								if (onBeforePoll) {
									continuePolling = onBeforePoll(_pollCount);
									e.name = "Polling Aborted Exception";
									e.message = "Polling cancelled by caller";
								}

								if (continuePolling) {
									continuePolling = _pollCount < _pollMax;
									e.name = "Polling Max Reached";
									e.message = "Polled maximum number of time: " + _pollMax;
								}

								if (!continuePolling) {
									complete = true;
									_errorHandler(e);
								}
							}

							if (complete) {
								clearInterval(_pollerId);
							}
						},
						_errorHandler
					);
			}; ; // _poll

			var _pollerId = setInterval(_poll, _pollInterval);
		}), // end pollForCommandStatus

		displayError: (function (e) {
			///<summary>displays an error</summary>
			///<param name='e'>an object containing the error information expected properties are .name, .message & .callstack</param>
			// publicationId, onOpportunityToCancelPolling, onCommandComplete, onCommandError, onPollError
			if ($("#euclid-error-display").length == 0) {
				$("body").prepend("<div id='euclid-error-display' style='z-index:auto'></div>");
			}

			var error = $("#euclid-error-display");

			Using(e)
				.Render("/composite/ui/template/euclid-error")
				.Manipulate(function (content) {
					$(error).append($(content));
					$(window).scrollTop(0);
				}
			);

			return false;
		}) // end displayError
	} // return
} ();       // EUCLID

var Using = function (jsonObject) {
	///<sumary>Use JSON data for UI elements</summary>
	///<param name='jsonObject'>A JSON data structure</param>

	var _populateTemplate = (function (templateUrl, data, onComplete, onError) {
		///<summary>fetch a handlebars template, and populate with the provided data, returns a jquery object containing the content</summary>
		///<param name='templateUrl'>the url from which to fetch the template</param>
		///<param name='data'>the data to bind (must be null if dataUrl is provided)</param>
		///<param name='onComplete'>a callback function that receives the populated template</parama>
		///<param name='onError'>(optional) to override the default error handling</param>
		var onError = onError ? onError : EUCLID.displayError;

		if (onComplete == null) {
			onError({
				name: "Invalid Argument Exception",
				message: "The parameter onComplete must be provided"
			});

			return false;
		}

		if (data == null) {
			onError({
				name: "Invalid Argument Exception",
				message: "_populateTemplate - the parameter data must be specified"
			});

			return false;
		}

		$.get(templateUrl, function (source) {
			var template = Handlebars.compile(source);
			onComplete($(template(data)));
		});
	}) // populateTemplate

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
				With: function (templateUrl) {
					/// <summary> Get a template for rendering the JSON data structure</summary>
					/// <param name='templateUrl'> the Url of the template to render the data structure</param>
					_populateTemplate(
						templateUrl,
						_data,
						function (content) {
							$(_element).html("");
							$(_element).append($(content));
						}
					);
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
					_populateTemplate(
						_templateUrl,
						_data,
						function (content) {
							callback(content);
						}
					);
				}
			}
		} // Render
	}
};

var WorkWithDataFromUrl = function (url, onSuccess, onError) {
	/// <summary> fetches a json object and executes a callback </summary>
	/// <param name='url'>the url from which to fetch the data</param>
	/// <param name='onSuccess'>a callback that accepts the json object</param>
	/// <param name='onError'>optional error handler</param>

	var _errorHandler = onError ? onError : EUCLID.displayError;

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
		error: function (err) {
			var e = err;
			if (e.hasOwnProperty("status") && e.status == 404) {
				_errorHandler({ name: "Not found", message: "Unable to fetch JSON from the url '" + url + "'" });
			} else if (err.hasOwnProperty("responseText")) {
				_errorHandler($.parseJSON(err.responseText));
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