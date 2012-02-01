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
						.Render("/composite/ui/template/form-container-modal")
						.Manipulate(function (template) {
							$(template).find("#form-content").append($(form));
							setModalContent(template, true);
						});
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
			.Render("/composite/ui/template/polling-modal")
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
			EUCLID.populateTemplate({
				templateUrl: "/composite/ui/template/euclid-error-modal",
				data: e,
				onComplete: function (content) {
					var parent = $("<div></div>");
					$(parent).append(content);
					setModalContent(parent, false);
				}
			});
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

	EUCLID.populateTemplate({
		templateUrl: "/composite/ui/template/publication-record-modal",
		data: { class: alertClass, record: result },
		onComplete: function (content) {
			setModalContent(content, false);
		}
	});
}