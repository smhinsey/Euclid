/// <reference path="/Assets/Scripts/euclid-0.9.js" />
var commandScript = true;
var modal = $("<div id='content' class='modal'></div>");
$(document).ready(function () {
	$(document).on("click", ".command", function () {
		var commandName = $(this).attr("data-command-name");

		EUCLID.withInputModel(
				commandName,
				function (inputModel) {
					var form = inputModel.getForm(false);
					$(form).attr("id", "the-form");
					$(form).attr("data-command-name", commandName);

					Using({ Name: commandName })
						.Render("/composite/ui/template/modal/form-container")
						.Manipulate(function (template) {
							$(template).find("#form-content").append($(form));
							setModalContent(template, true);
						});
				},

				function (e) {
					if (e.name == "CommandNotPresentInCompositeException") {
						WorkWithDataFromUrl("/composite/api/command-metadata/" + commandName,
							function (data) {
								data["PresentInComposite"] = false;
								Using(data)
									.Render("/composite/ui/template/modal/command")
									.Manipulate(function (content) {
										setModalContent($(content), true);
									});
							}
						);
					} else {
						EUCLID.displayError(e);
					}
				});
		return false;
	});

	$(document).on("click", "#dismiss-modal", function () {
		$(modal).modal("hide");
		return false;
	});

	$(document).on("click", "#submit-modal-form", function () {
		var form = $(modal).find("#the-form");

		if (form.length == 0) {
			EUCLID.displayError({ name: "Could not find form", message: "no form with id 'the-form' could be found in the modal form container" });
		} else {
			var commandName = $(form).attr("data-command-name");
			submitForm($(form).clone(), commandName);
		}

		return false;
	});

	$(document).on("click", "#stop-polling", function () {
		continuePolling = false;
		return false;
	});

});

function setModalContent(content, show) {
	$(modal).html("");
	$(modal).append($(content));

	if (show) {
		$(modal).modal("show");
		$(modal).on("hidden", function () { $(modal).remove(); });
	}
}

var continuePolling = true;
function submitForm(form, commandName) {
	continuePolling = true;
	Using({ Name: commandName })
			.Render("/composite/ui/template/modal/polling")
			.Manipulate(function (template) {
				setModalContent(template, false);
				EUCLID.withFormResults(form,
					function (publicationRecord) {
						pollForStatus(publicationRecord.Identifier);
					},

					function (e) {
						EUCLID.displayError(e);
						$(modal).modal("hide");
					}
				); // EUCLID.withFormResults
			}); // Manipulate

	return false;
}

function pollForStatus(publicationRecordIdentifier) {
	EUCLID.pollForCommandStatus({
		publicationId: publicationRecordIdentifier,
		pollMax: 25,

		onOpportunityToCancelPolling: function (pollCount) {
			var msg = "Polled " + ((pollCount == 1) ? "1 time" : (pollCount + " times"));
			$(modal).find("#poll-status").text(msg);

			return continuePolling;
		},

		onCommandComplete: function (result) {
			displayResult(result);
		},

		onCommandError: function (result) {
			displayResult(result);
		},

		onPollError: function (e) {
			Using(e)
				.Render("/composite/ui/template/modal/euclid-error")
				.Manipulate(function(content) {
					var parent = $("<div></div>");
					$(parent).append(content);
					setModalContent(parent, false);
				}
			); // using
		}
	});

	return false;
}

function displayResult(result) {
	var alertClass = "alert-success";
	if (result.Error) {
		alertClass = "alert-error";
	} else if (!result.Dispatched) {
		alertClass = "alert-block";
	}

	Using({ class: alertClass, record: result })
		.Render("/composite/ui/template/modal/publication-record")
		.Manipulate(function (content) {
			setModalContent(content, false);
		}
	); // using

}