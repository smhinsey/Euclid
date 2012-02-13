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

if (EUCLID) {
	EUCLID.showModalForm = (function(args) {
		if (args === null || args === undefined || !args.hasOwnProperty("Url")) {
			throw new {
				name: "Invalid Argument Exception",
				message: "The argument object must contain a property named 'Url'"
			};
		}
		;

		var id = EUCLID.getId();
		var modal = $("<div id='" + id + "'></div>");
		$(modal).load(
			args.Url,
			function() {
				$.validator.unobtrusive.parse($("#" + id));
			})
			.modal({
				autoResize: true,
				autoPosition: true,
				dataCss: { backgroundColor: "#fff" },
				containerCss: { backgroundColor: "#fff" }
			});

		$(".simplemodal-container").css("height", "auto");
	}); // end showModalForm
}