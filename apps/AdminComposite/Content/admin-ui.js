$(document).ready(function () {
	$("#forum-host").live("keyup", function () {
		setForumUrl();
	});

	$("#theme-selector").live("change", function () {
		var src = getPreviewImage($(this).val());

		if (src == "") {
			$("#unselected-preview").show();
			$("#theme-preview").hide();
		}
		else {
			$("#unselected-preview").hide();
			$("#theme-preview").show();
			$("#theme-preview").attr("src", src);
		}
	});

	$(".new-forum-button").click(function () {
		$("<div></div>")
			.load("/Forum/Create")
			.modal({
				autoResize: true,
				autoPosition: true,
				position: new Array(25, 300),
				dataCss: { backgroundColor: "#fff", overflow: "auto", height: 835 },
				containerCss: { height: 850 }
			});

		return false;
	});

	$(".vote-scheme").live("click", function () {
		var description = "";

		if ($(this).val() == "00000000-0000-0000-0000-000000000000") {
			description = "Voting disabled";
		} else if ($(this).val() == "9A52AFD8-196B-475D-94DE-EC15F9C8E367") {
			description = "Users can vote Up (+1) or Down (-1)";
		}

		$("#vote-scheme-description").text(description);
	});

	$(".activate-badge").click(function () {

	});

	$(".admin-delete, .block-user").live("click", function () {
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

	$("#generate-slug").live("click", function () {
		$("#forum-slug").val(slugify($("#forum-name").val()));
		setForumUrl();
	});

	$("#forum-name, #forum-slug").live("blur", function () {
		var slug = slugify($("#forum-slug").val());

		if (isNullOrEmpty(slug)) {
			slug = slugify($("#forum-name").val());
		}

		$("#forum-slug").val(slug);
		setForumUrl();
	});

	$("#forum-slug").live("focus", function () {
		$(this).selectRange(0, $(this).val().length);
	});

	$("#forum-slug").live("mouseup", function () {
		return false;
	});
});

function isNullOrEmpty(value) {
	return value == null || value == "";
}

function blockUser(userId) {
	alert("blocking user " + userId);
}

function setForumUrl() {
	var forumSlug = slugify($("#forum-slug").val());
	$("#forum-slug").val(forumSlug);
	
	var host = slugify($("#forum-host").val());
	$("#host-example").html(host);

	var url;
	if (host == "@Model.UrlHostName") {
		url = "http://" + host + "/{organization name}/forums/" + forumSlug;
	} else {
		url = "http://" + host + "/forums/" + forumSlug;
	}

	$("#forum-url").html(url);
}

function getPreviewImage(id) {
	var imageUrl;
	if (id == "44444444-4444-4444-4444-444444444444") {
		imageUrl = "http://www.vbstyles.com/vbulletin_styles/l_RedFox-4x-vBulletin-Theme-Forum-Home1.png";
	} else if (id == "55555555-5555-5555-5555-555555555555") {
		imageUrl = "http://themes.wordpress-deutschland.org/demoblog/wp-content/themes/insomniac/screenshot.png";
	} else {
		imageUrl = "";
	}
	return imageUrl;
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