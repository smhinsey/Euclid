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
						EUCLID.pollForCommandStatus(
							publicationRecord.Identifier,
							function (publicationRecord) { display(publicationRecord); },
							null,
							25,
							250,
							function (e) { displayError(e); },
							function (count) { onBeforePoll(count); }
						); // EUCLID.pollForCommandStatus
					}, 

					function (e) {
						EUCLID.displayError(e);
						$(modal).modal("hide");
					}
				); // EUCLID.withFormResults
			}); // Manipulate

	return false;
}

function display(publicationRecord) {
	var alertClass = "alert-success";
	if (publicationRecord.Error) {
		alertClass = "alert-error";
	} else if (!publicationRecord.Dispatched) {
		alertClass = "alert-block";
	}

	Using({ class: alertClass, record: publicationRecord })
		.Render("/composite/ui/template/modal/publication-record")
		.Manipulate(function (content) {
			setModalContent(content, false);
		}
	);
} // displayPublicationRecord

function displayError(e) {
	Using(e)
		.Render("/composite/ui/template/modal/euclid-error")
		.Manipulate(function (content) {
			var parent = $("<div></div>");
			$(parent).append(content);
			setModalContent(parent, false);
		}
	);
} // displayError

function onBeforePoll(count) {
	var msg = "Polled " + ((count == 1) ? "1 time" : (count + " times"));
	$(modal).find("#poll-status").text(msg);
	return continuePolling;
}