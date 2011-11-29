$(document).ready(function () {
	$(".confirmation-dialog").live("click", function () {
		var msg = $(this).attr("data-confirmation-message");
		var confirmFunction = $(this).attr("data-confirm-function");
		var itemId = $(this).attr("data-item-id");
		var override = $(this).attr("data-override");

		if (override == true) return true;

		if (isNullOrEmpty(msg) || isNullOrEmpty(confirmFunction) || isNullOrEmpty(itemId)) {
			$("<div></div>").load("/Dashboard/GetConfirmationMessageAttributesMissingMessage").modal();
		} else {
			$("<div></div>")
				.load("/Dashboard/GetConfirmationMessage?message=" + $.URLEncode(msg))
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

function setForumUrl(host, orgSlug, forumSlug) {
	var url = "http://" + host + "/org/" + orgSlug + "/forum/" + forumSlug;

	$("#forum-url").html(url);
}

function isNullOrEmpty(value) {
	return value == null || value == "";
}

function slugify(value) {
	return value.replace(/ /g, "-").toLowerCase();
}

$.fn.selectRange = function (start, end) {
	return this.each(function () {
		if (this.setSelectionRange) {
			this.focus();
			this.setSelectionRange(start, end);
		} else if (this.createTextRange) {
			var range = this.createTextRange();
			range.collapse(true);
			range.moveEnd('character', end);
			range.moveStart('character', start);
			range.select();
		}
	});
};